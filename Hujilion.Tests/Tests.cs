using System;
using System.IO;
using System.Linq;
using Hujilion.Console;
using NUnit.Framework;
using Shouldly;

namespace Hujilion.Tests
{
    public class Tests
    {
        [Test]
        public void Post()
        {
            Telegramer.Post(new Purchase
                            {
                                Title = "Выполнение работ по строительству объекта: «Строительство средней школы в квартале 2008 г. Барнаула»",
                                Price = 953941338,
                                Uri = new Uri("http://zakupki.gov.ru/epz/order/notice/ea44/view/common-info.html?regNumber=0302100013516000408")
                            },
                            debug: true);
        }

        [Test]
        public void FormatPriceInACoolWay()
        {
            Telegramer.FormatPrice(953941338)
                      .ShouldBe("953 941 338.00");
        }

        [Test]
        public void ParseXml()
        {
            Xmler.Deserialize(File.ReadAllText(@"C:\Users\vorou\code\Hujilion\Hujilion.Tests\tender.xml"))
                 .Single()
                 .Uri.ShouldBe(new Uri("http://zakupki.gov.ru/epz/order/notice/ea44/view/common-info.html?regNumber=0302100013516000408"));
        }
    }
}