using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Serilog;

namespace Hujilion.Console
{
    public static class Xmler
    {
        public static IEnumerable<Purchase> Deserialize(string xml)
        {
            var result = new List<Purchase>();
            try
            {
                var xdoc = StripNS(XElement.Parse(xml));
                foreach (var xpurchase in xdoc.Elements())
                {
                    var nameLocalName = xpurchase.Name.LocalName;
                    if (!nameLocalName.ToLower().Contains("notification") || nameLocalName.ToLower().Contains("cancel") || nameLocalName.ToLower().Contains("datechange"))
                    {
                        Log.Information($"skipping non-notification node: [{nameLocalName}]");
                        continue;
                    }
                    result.Add(new Purchase
                               {
                                   Title = xpurchase.Element("purchaseObjectInfo").Value,
                                   Number = xpurchase.Element("purchaseNumber").Value,
                                   Price = xpurchase.Descendants("lot")
                                                    .Sum(x => x.Elements("maxPrice").Sum(e => Decimal.Parse(e.Value.Trim()))),
                                   Uri = new Uri(xpurchase.Element("href").Value)
                               });
                }
            }
            catch (Exception e)
            {
                var dumpFile = Path.GetTempFileName();
                File.WriteAllText(dumpFile, xml);
                Log.Information($"Failed to deserialize xml; dump is at [{dumpFile}]");
                throw;
            }
            return result;
        }

        private static XElement StripNS(XElement root)
        {
            return new XElement(root.Name.LocalName,
                                root.HasElements ? root.Elements().Select(StripNS) : (object) root.Value);
        }
    }
}