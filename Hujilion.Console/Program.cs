using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace Hujilion.Console
{
    internal class Program
    {
        private const string ImportedFile = @"imported.txt";

        public static void Main(string[] args)
        {
            System.Console.Out.WriteLine("йо");
//            var purchases = Deserialize(File.ReadAllText(@"tender.xml")).ToArray();
//            return;

            System.Console.Out.WriteLine("hi.");

            if (!File.Exists(ImportedFile))
                File.WriteAllBytes(ImportedFile, new byte[0]);
            var imported = new HashSet<string>(File.ReadAllLines(ImportedFile));

            var listing = GetListing(new Uri("ftp://ftp.zakupki.gov.ru/fcs_regions"));
            var regionDirs = listing.Select(x => x.Split('/').Last().Trim()).Where(IsRegionDir);

            var newPurchases = new List<Purchase>();
            var scanned = 0;
            var newZips = 0;
            foreach (var regionDir in regionDirs.Take(10))
            {
                var requestUri = new Uri($"ftp://ftp.zakupki.gov.ru/fcs_regions/{regionDir}/notifications/currMonth/");
                var regionListing = GetListing(requestUri);
                foreach (var zip in regionListing.Take(10))
                {
                    var zipFullUri = new Uri($"ftp://ftp.zakupki.gov.ru/fcs_regions/{regionDir}/notifications/currMonth/{zip}");
                    if (!imported.Contains(zipFullUri.AbsoluteUri))
                    {
                        var toAdd = GetPurchases(zipFullUri).ToArray();
                        newPurchases.AddRange(toAdd);
                        System.Console.Out.WriteLine($"new zip: [{zip}], purchases=[{toAdd.Count()}]");
                        imported.Add(zipFullUri.AbsoluteUri);
                        newZips++;
                    }
                    scanned++;
                }
            }
//            File.WriteAllLines(ImportedFile, imported);

            System.Console.Out.WriteLine($"zips: new=[{newZips}], scanned=[{scanned}]");
            System.Console.Out.WriteLine($"purchases: new=[{newPurchases.Count}]");

            var mostExpensive = newPurchases.OrderByDescending(x => x.Price).FirstOrDefault();
            if (mostExpensive!=null)
            {
                System.Console.Out.WriteLine($"most expensive purchase is: title=[{mostExpensive.Title}], price=[{mostExpensive.Price}]");
            }
            else
            {
                System.Console.Out.WriteLine("no new purchases");
            }

            System.Console.Out.WriteLine("over.");
        }

        private static IEnumerable<Purchase> GetPurchases(Uri zip)
        {
            System.Console.Out.WriteLine($"GetPurchases: [{zip.AbsoluteUri}]");
            var zipBytes = Download(zip);
            var xmlNameToBytes = Unzip(zipBytes);
            System.Console.Out.WriteLine($"Unzipped [{xmlNameToBytes.Count}] files");
            foreach (var kv in xmlNameToBytes)
            {
                foreach (var purchase in Deserialize(kv.Value))
                {
                    purchase.Zip = zip;
                    purchase.Xml = kv.Key;
                    purchase.Seen = DateTimeOffset.Now;
                    System.Console.Out.WriteLine($"new purchase: [{purchase}]");
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
            var listing = new StreamReader(response.GetResponseStream()).ReadToEnd()
                                                                        .Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries)
                                                                        .Select(x => x.Trim());
            response.Close();
            return listing;
        }

        private static bool IsRegionDir(string x) => !x.Contains(".") && char.IsUpper(x[0]);

        private static IEnumerable<Purchase> Deserialize(string xml)
        {
            var result = new List<Purchase>();
            try
            {
                var xdoc = StripNS(XElement.Parse(xml));
                foreach (var xpurchase in xdoc.Elements())
                {
                    var nameLocalName = xpurchase.Name.LocalName;
                    if (!nameLocalName.ToLower().Contains("notification") || nameLocalName.ToLower().Contains("cancel"))
                    {
                        System.Console.Out.WriteLine($"skipping non-notification node: [{nameLocalName}]");
                        continue;
                    }
                    result.Add(new Purchase
                               {
                                   Title = xpurchase.Element("purchaseObjectInfo").Value,
                                   Number = xpurchase.Element("purchaseNumber").Value,
                                   Price = decimal.Parse(xpurchase.Descendants("lot").Single().Element("maxPrice").Value.Trim())
                               });
                }
            }
            catch (Exception e)
            {
                var dumpFile = Path.GetTempFileName();
                File.WriteAllText(dumpFile, xml);
                System.Console.Out.WriteLine($"Failed to deserialize xml; dump is at [{dumpFile}]");
                throw;
            }
            return result;
        }

        public static XElement StripNS(XElement root)
        {
            return new XElement(root.Name.LocalName,
                                root.HasElements ? root.Elements().Select(StripNS) : (object) root.Value);
        }
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