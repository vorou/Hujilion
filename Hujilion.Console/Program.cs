using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Polly;
using Polly.Timeout;
using Serilog;

namespace Hujilion.Console
{
    public static class Program
    {
        public const bool Debug = true;
        private const string ImportedFile = @"imported.txt";

        public static void Main()
        {
            InitLogging();
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                                                          {
                                                              Log.Fatal(args.ExceptionObject.ToString());
                                                          };

            Log.Information("hi.");

            var newMostExpensivePurchase = GetNewMostExpensivePurchase();
            if (newMostExpensivePurchase == null)
            {
                Log.Information("no purchase to publish");
                return;
            }

            Telegramer.Post(newMostExpensivePurchase);

            Log.Information("over.");
        }

        private static void InitLogging()
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                                                  .WriteTo.ColoredConsole()
                                                  .WriteTo.RollingFile(@"C:\logs\hujilion-{Date}.log")
                                                  .CreateLogger();
        }

        private static Purchase GetNewMostExpensivePurchase()
        {
            var imported = new HashSet<string>();

            if (!Debug)
            {
                if (!File.Exists(ImportedFile))
                    File.WriteAllBytes(ImportedFile, new byte[0]);
                else
                    File.Copy(ImportedFile, Path.ChangeExtension(ImportedFile, "bak"));
                imported = new HashSet<string>(File.ReadAllLines(ImportedFile));
            }
            Log.Information($"last time I saw [{imported.Count}] zips");

            var listing = GetListing(new Uri("ftp://ftp.zakupki.gov.ru/fcs_regions"));
            var regionDirs = listing.Select(x => x.Split('/').Last().Trim()).Where(IsRegionDir);

            var newPurchases = new List<Purchase>();
            var scanned = 0;
            var newZips = 0;
            foreach (var regionDir in regionDirs.Take(1))
            {
                var requestUri = new Uri($"ftp://ftp.zakupki.gov.ru/fcs_regions/{regionDir}/notifications/currMonth/");
                var regionListing = GetListing(requestUri);
                foreach (var zip in regionListing)
                {
                    // notification_Burjatija_Resp_2016123100_2017010100_001.xml.zip
                    var tillDateSubstring = Regex.Match(zip, @"^\D+?_(?:\d+)_(\d+)").Groups[1].Value.Substring(0, 8);
                    var tillDate = DateTime.ParseExact(tillDateSubstring, "yyyyMMdd", null, DateTimeStyles.AssumeUniversal);
                    if (tillDate < DateTime.UtcNow - TimeSpan.FromDays(2))
                    {
                        Log.Information($"too old, skipping: [{zip}], tillDate=[{tillDate}]");
                        continue;
                    }

                    var zipFullUri = new Uri($"ftp://ftp.zakupki.gov.ru/fcs_regions/{regionDir}/notifications/currMonth/{zip}");
                    if (!imported.Contains(zipFullUri.AbsoluteUri))
                    {
                        var toAdd = GetPurchases(zipFullUri).ToArray();
                        newPurchases.AddRange(toAdd);
                        Log.Information($"new zip: [{zip}], purchases=[{toAdd.Count()}]");
                        imported.Add(zipFullUri.AbsoluteUri);
                        newZips++;
                    }
                    else
                    {
                        Log.Information($"saw it last time, skipping: [{zip}]");
                    }
                    scanned++;
                }
            }
            if (!Debug)
            {
                File.WriteAllLines(ImportedFile, imported);
            }

            Log.Information($"zips: new=[{newZips}], scanned=[{scanned}]");
            Log.Information($"purchases: new=[{newPurchases.Count}]");

            var mostExpensive = newPurchases.OrderByDescending(x => x.Price).FirstOrDefault();
            if (mostExpensive != null)
            {
                Log.Information($"most expensive purchase is: title=[{mostExpensive.Title}], price=[{mostExpensive.Price}]");
                return mostExpensive;
            }
            else
            {
                Log.Information("no new purchases");
                return null;
            }
        }

        private static IEnumerable<Purchase> GetPurchases(Uri zip)
        {
            Log.Information($"GetPurchases: [{zip.AbsoluteUri}]");

            var timeout = Policy.Timeout<IEnumerable<byte>>(TimeSpan.FromSeconds(5),
                                                            onTimeout: (context, span, t) =>
                                                                       {
                                                                           Log.Information($"Timeout zip downloading: [{zip}]");
                                                                       });
            var retry = Policy.Handle<TimeoutRejectedException>()
                              .Retry(5,
                                     onRetry: (exception, i) =>
                                              {
                                                  Log.Information($"Timeout zip downloading: "+
                                                                               $"[{zip}] "+
                                                                               $"tryCount=[{i}] " +
                                                                               $"exception=[{exception.Message}]");
                                              });
            var retryOnTimeout = retry.Wrap(timeout);
            var zipBytes = retryOnTimeout.Execute(() => Download(zip));

            var xmlNameToBytes = Unzip(zipBytes);
            Log.Information($"Unzipped [{xmlNameToBytes.Count}] files");
            foreach (var kv in xmlNameToBytes)
            {
                foreach (var purchase in Xmler.Deserialize(kv.Value))
                {
                    purchase.Zip = zip;
                    purchase.Xml = kv.Key;
                    purchase.Seen = DateTimeOffset.Now;
                    Log.Information($"new purchase: [{purchase}]");
                    yield return purchase;
                }
            }
        }

        public static Dictionary<string, string> Unzip(IEnumerable<byte> zipBytes)
        {
            var result = new Dictionary<string, string>();
            var zipArchive = new ZipArchive(new MemoryStream(zipBytes.ToArray()));
            foreach (var entry in zipArchive.Entries)
            {
                result.Add(entry.Name, new StreamReader(entry.Open()).ReadToEnd());
            }
            return result;
        }

        private static IEnumerable<byte> Download(Uri zip)
        {
            Log.Information($"Download: [{zip}]");
            using (var webClient = new WebClient {Credentials = new NetworkCredential("free", "free")})
                return webClient.DownloadData(zip);
        }

        private static IEnumerable<string> GetListing(Uri requestUri)
        {
            Log.Information($"GetListing: [{requestUri.AbsoluteUri}]");
            var request = (FtpWebRequest) WebRequest.Create(requestUri);
            request.Credentials = new NetworkCredential("free", "free");
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            var response = (FtpWebResponse) request.GetResponse();
            Log.Information($"response: [{response.StatusCode}]");
            var listing = new StreamReader(response.GetResponseStream()).ReadToEnd()
                                                                        .Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries)
                                                                        .Select(x => x.Trim());
            response.Close();
            return listing;
        }

        private static bool IsRegionDir(string x) => !x.Contains(".") && char.IsUpper(x[0]);
    }

    public class Purchase
    {
        public override string ToString() => $"{Zip}//{Xml}";

        public Uri Zip { get; set; }
        public string Xml { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset Seen { get; set; }
        public Uri Uri { get; set; }
    }

    //ftp://free@ftp.zakupki.gov.ru/fcs_regions/Burjatija_Resp/notifications/currMonth/notification_Burjatija_Resp_2016123100_2017010100_001.xml.zip
    //<purchaseObjectInfo>Поставка технических средств реабилитации, а именно кресел-стульев с санитарным оснащением различной модификации,  для обеспечения  инвалидов в 2017 году</purchaseObjectInfo>
    //    <lot>
    //    <maxPrice>570416.67</maxPrice>
}