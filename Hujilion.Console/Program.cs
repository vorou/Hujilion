using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace Hujilion.Console
{
    internal class Program
    {
        private const string ImportedFile = @"imported.txt";

        public static void Main(string[] args)
        {
            System.Console.Out.WriteLine("engaged.");

            if (!File.Exists(ImportedFile))
                File.WriteAllBytes(ImportedFile, new byte[0]);
            var imported = new HashSet<string>(File.ReadAllLines(ImportedFile));

            var listing = GetListing(new Uri("ftp://ftp.zakupki.gov.ru/fcs_regions"));
            var regionDirs = listing.Select(x => x.Split('/').Last().Trim()).Where(IsRegionDir);

            var newPurchases = new List<Purchase>();
            var scanned = 0;
            var newZips = 0;
            foreach (var regionDir in regionDirs.Take(1))
            {
                var requestUri = new Uri($"ftp://ftp.zakupki.gov.ru/fcs_regions/{regionDir}/notifications/currMonth/");
                var regionListing = GetListing(requestUri);
                foreach (var zip in regionListing.Take(1))
                {
                    var zipFullUri = new Uri($"ftp://ftp.zakupki.gov.ru/fcs_regions/{regionDir}/notifications/currMonth/{zip}");
                    if (!imported.Contains(zipFullUri.AbsoluteUri))
                    {
                        var toAdd = GetPurchases(zipFullUri).ToArray();
                        newPurchases.AddRange(toAdd);
                        System.Console.Out.WriteLine($"new zip: [{zip}], files=[{toAdd.Count()}]");
                        imported.Add(zipFullUri.AbsoluteUri);
                        newZips++;
                    }
                    scanned++;
                }
            }
            File.WriteAllLines(ImportedFile, imported);

            System.Console.Out.WriteLine($"zips: new=[{newZips}], scanned=[{scanned}]");
            System.Console.Out.WriteLine($"purchases: new=[{newPurchases.Count}]");
            System.Console.Out.WriteLine("over.");
        }

        private static IEnumerable<Purchase> GetPurchases(Uri zip)
        {
            System.Console.Out.WriteLine($"GetPurchases: [{zip.AbsoluteUri}]");
            var zipBytes = Download(zip);
            var xmlNameToBytes = Unzip(zipBytes);
            foreach (var kv in xmlNameToBytes)
            {
                var newPurchase = new Purchase {Zip = zip, Xml = kv.Key, Seen = DateTimeOffset.Now};
                System.Console.Out.WriteLine($"new purchase: [{newPurchase}]");
                yield return newPurchase;
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
            System.Console.Out.WriteLine($"Download: [{zip}]");
            var request = new WebClient {Credentials = new NetworkCredential("free", "free")};
            byte[] bytes;
            try
            {
                bytes = request.DownloadData(zip);
            }
            catch (WebException e)
            {
                System.Console.Out.WriteLine($"Failed to download zip: [{zip}], [{e}]");
                throw;
            }
            return bytes;
        }

        private static IEnumerable<string> GetListing(Uri requestUri)
        {
            System.Console.Out.WriteLine($"GetListing: [{requestUri.AbsoluteUri}]");
            var request = (FtpWebRequest) WebRequest.Create(requestUri);
            request.Credentials = new NetworkCredential("free", "free");
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            var response = (FtpWebResponse) request.GetResponse();
            System.Console.Out.WriteLine($"response: [{response.StatusCode}]");
            var listing = new StreamReader(response.GetResponseStream())
                .ReadToEnd()
                .Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries)
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
    }

    //ftp://free@ftp.zakupki.gov.ru/fcs_regions/Burjatija_Resp/notifications/currMonth/notification_Burjatija_Resp_2016123100_2017010100_001.xml.zip
    //<purchaseObjectInfo>Поставка технических средств реабилитации, а именно кресел-стульев с санитарным оснащением различной модификации,  для обеспечения  инвалидов в 2017 году</purchaseObjectInfo>
    //    <lot>
    //    <maxPrice>570416.67</maxPrice>
}