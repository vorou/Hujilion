using System.Linq;
using Hujilion.Console;
using NUnit.Framework;
using Shouldly;

namespace Hujilion.Tests
{
    public class ShittyXmls
    {
        [Test]
        public void SkipDateChange()
        {
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<ns2:export xmlns=""http://zakupki.gov.ru/oos/types/1"" xmlns:ns2=""http://zakupki.gov.ru/oos/export/1"" xmlns:ns3=""http://zakupki.gov.ru/oos/printform/1"" xmlns:ns4=""http://zakupki.gov.ru/oos/control99/1"">
    <ns2:fcs_notificationEFDateChange schemeVersion=""7.0"">
        <id>7518</id>
        <purchaseNumber>0171300013516000048</purchaseNumber>
        <docNumber>№УИА1</docNumber>
        <docPublishDate>2017-01-23T08:56:03.283+03:00</docPublishDate>
        <purchaseResponsible>
            <responsibleOrg>
                <regNum>01713000135</regNum>
                <consRegistryNum>78300163</consRegistryNum>
                <fullName>АДМИНИСТРАЦИЯ ПЕРЕСЛАВСКОГО МУНИЦИПАЛЬНОГО РАЙОНА</fullName>
            </responsibleOrg>
            <responsibleRole>CU</responsibleRole>
        </purchaseResponsible>
        <purchaseObjectInfo>Организация видеонаблюдения  с целью дистанционного мониторинга  и раннего обнаружения лесных пожаров</purchaseObjectInfo>
        <placingWay>
            <code>EAP44</code>
            <name>Электронный аукцион</name>
        </placingWay>
        <auctionTime>2016-12-30T10:45:00+03:00</auctionTime>
        <newAuctionDate>2017-02-03T00:00:00+03:00</newAuctionDate>
        <reason>
            <authorityPrescription>
                <externalPrescription>
                    <authorityName>Управление федеральной анитимонопольной службы по Ярославской области</authorityName>
                    <authorityType>FA</authorityType>
                    <docName>Предписание</docName>
                    <docDate>2017-01-13T00:00:00+03:00</docDate>
                    <docNumber>05-02/417Ж-16</docNumber>
                </externalPrescription>
            </authorityPrescription>
            <addInfo></addInfo>
        </reason>
        <href>http://zakupki.gov.ru/epz/order/notice/ea44/view/documents.html?regNumber=0171300013516000048</href>
        <printForm>
            <url>http://zakupki.gov.ru/epz/order/notice/printForm/viewXml.html?noticeChangeAuctionDateId=7518</url>
            <signature type=""CAdES-BES"">TUlJTm1nWUpLb1pJaHZjTkFRY0NvSUlOaXpDQ0RZY0NBUUV4RERBS0JnWXFoUU1DQWdrRkFEQUxCZ2txaGtpRzl3MEJCd0dnZ2dscU1JSUpaakNDQ1JXZ0F3SUJBZ0lER09xck1BZ0dCaXFGQXdJQ0F6Q0NBVjB4R0RBV0Jna3Foa2lHOXcwQkNRSVRDVk5sY25abGNpQkRRVEVnTUI0R0NTcUdTSWIzRFFFSkFSWVJkV05mWm10QWNtOXphMkY2Ym1FdWNuVXhIREFhQmdOVkJBZ01FemMzSU5DekxpRFFuTkMrMFlIUXV0Q3kwTEF4R2pBWUJnZ3FoUU1EZ1FNQkFSSU1NREEzTnpFd05UWTROell3TVJnd0ZnWUZLb1VEWkFFU0RURXdORGMzT1Rjd01UazRNekF4TERBcUJnTlZCQWtNSTlHRDBMdlF1TkdHMExBZzBKalF1OUdNMExqUXZkQzYwTEFzSU5DMDBMN1F2Q0EzTVJVd0V3WURWUVFIREF6UW5OQyswWUhRdXRDeTBMQXhDekFKQmdOVkJBWVRBbEpWTVRnd05nWURWUVFLREMvUXBOQzEwTFRRdGRHQTBMRFF1OUdNMEwzUXZ0QzFJTkM2MExEUXQ5QzkwTERSaDlDMTBMblJnZEdDMExMUXZqRS9NRDBHQTFVRUF3dzIwS1BRcGlEUXBOQzEwTFRRdGRHQTBMRFF1OUdNMEwzUXZ0Q3owTDRnMExyUXNOQzMwTDNRc05HSDBMWFF1ZEdCMFlMUXN0Q3dNQjRYRFRFMk1EY3hPVEV6TVRReU1Gb1hEVEUzTVRBeE9URXpNVFF5TUZvd2dnSkhNUm93R0FZSUtvVURBNEVEQVFFU0REYzJNRGd3TVRRME5UUTNNVEVXTUJRR0JTcUZBMlFERWdzd05EVTRNREUwTVRnME1ERVlNQllHQlNxRkEyUUJFZzB4TURJM05qQXhNRFUwTkRnd01TUXdJZ1lKS29aSWh2Y05BUWtCRmhWdGRXNTZZV3RoZWtCd1pYSmxjMnhoZG13dWNuVXhDekFKQmdOVkJBWVRBbEpWTVRFd0x3WURWUVFJRENnM05pRFFyOUdBMEw3UmdkQzcwTERRc3RHQjBMclFzTkdQSU5DKzBMSFF1OUN3MFlIUmd0R01NVFF3TWdZRFZRUUhEQ3ZRc3k0ZzBKL1F0ZEdBMExYUmdkQzcwTERRc3RDNzBZd3QwSmZRc05DNzBMWFJnZEdCMExyUXVOQzVNV2d3WmdZRFZRUUtERi9Ra05DMDBMelF1TkM5MExqUmdkR0MwWURRc05HRzBMalJqeURRbjlDMTBZRFF0ZEdCMEx2UXNOQ3kwWUhRdXRDKzBMUFF2aURRdk5HRDBMM1F1TkdHMExqUXY5Q3cwTHZSak5DOTBMN1FzOUMrSU5HQTBMRFF1ZEMrMEwzUXNERTNNRFVHQTFVRUN3d3UwTDdSZ3RDMDBMWFF1eURRdXRDKzBMM1JndEdBMExEUXV0R0MwTDNRdnRDNUlOR0IwTHZSZzlDMjBMSFJpekV3TUM0R0ExVUVLZ3duMEtIUXN0QzEwWUxRdTlDdzBMM1FzQ0RRbXRDdzBML1F1TkdDMEw3UXZkQyswTExRdmRDd01Sa3dGd1lEVlFRRURCRFFtdEMrMExMUXNOQzcwTFhRc3RDd01TZ3dKZ1lEVlFRTURCL1F2ZEN3MFlmUXNOQzcwWXpRdmRDNDBMb2cwTDdSZ3RDMDBMWFF1OUN3TVVFd1B3WURWUVFERERqUW10QyswTExRc05DNzBMWFFzdEN3SU5DaDBMTFF0ZEdDMEx2UXNOQzkwTEFnMEpyUXNOQy8wTGpSZ3RDKzBMM1F2dEN5MEwzUXNEQmpNQndHQmlxRkF3SUNFekFTQmdjcWhRTUNBaVFBQmdjcWhRTUNBaDRCQTBNQUJFQkdBaXNhend3ZzBPWnUyTXVpT0t6anZjWklRanhVNlFJQXFUM1F2Y2t0cFF1cU0vR2Rpdm1GQmxOR2lHYmcrM0NablEvRW5jZE5iTmNCQnhuczFGNCtvNElFekRDQ0JNZ3dEQVlEVlIwVEFRSC9CQUl3QURBZEJnTlZIU0FFRmpBVU1BZ0dCaXFGQTJSeEFUQUlCZ1lxaFFOa2NRSXdXQVlEVlIwUkJGRXdUNkFTQmdOVkJBeWdDeE1KTnpFNU1Ea3dNVEl4b0JrR0NpcUZBd005bnRjMkFRZWdDeE1KTnpZeU1qQXhNREF4b0JzR0NpcUZBd005bnRjMkFRV2dEUk1MTURFM01UTXdNREF4TXpXR0FUQXdOZ1lGS29VRFpHOEVMUXdySXRDYTBZRFF1TkMvMFlMUXZ0Q2YwWURRdmlCRFUxQWlJQ2pRc3RDMTBZRFJnZEM0MFk4Z015NDJLVENDQVdFR0JTcUZBMlJ3QklJQlZqQ0NBVklNUkNMUW10R0EwTGpRdjlHQzBMN1FuOUdBMEw0Z1ExTlFJaUFvMExMUXRkR0EwWUhRdU5HUElETXVOaWtnS05DNDBZSFF2OUMrMEx2UXZkQzEwTDNRdU5DMUlESXBER2dpMEovUmdOQyswTFBSZ05DdzBMelF2TkM5MEw0dDBMRFF2OUMvMExEUmdOQ3cwWUxRdmRHTDBMa2cwTHJRdnRDODBML1F1OUMxMExyUmdTQWkwSzdRdmRDNDBZSFF0ZEdBMFlJdDBKUFFudENoMEtJaUxpRFFrdEMxMFlEUmdkQzQwWThnTWk0eElneFAwS0hRdGRHQTBZTFF1TkdFMExqUXV0Q3cwWUlnMFlIUXZ0QyswWUxRc3RDMTBZTFJnZEdDMExMUXVOR1BJT0tFbGlEUW9kQ2tMekV5TkMweU56TTRJTkMrMFlJZ01ERXVNRGN1TWpBeE5ReFAwS0hRdGRHQTBZTFF1TkdFMExqUXV0Q3cwWUlnMFlIUXZ0QyswWUxRc3RDMTBZTFJnZEdDMExMUXVOR1BJT0tFbGlEUW9kQ2tMekV5T0MweU9EYzRJTkMrMFlJZ01qQXVNRFl1TWpBeE5qQU9CZ05WSFE4QkFmOEVCQU1DQS9nd1V3WURWUjBsQkV3d1NnWUlLd1lCQlFVSEF3SUdEaXFGQXdNOW50YzJBUVlEQkFFQkJnNHFoUU1EUFo3WE5nRUdBd1FCQWdZT0tvVURBejJlMXpZQkJnTUVBUU1HRGlxRkF3TTludGMyQVFZREJBRUVNQ3NHQTFVZEVBUWtNQ0tBRHpJd01UWXdOekU1TVRNeE5ETTVXb0VQTWpBeE56RXdNVGt4TXpFME1qQmFNSUlCandZRFZSMGpCSUlCaGpDQ0FZS0FGSjV4RGcvYXRBRW9Yei9peTQ5bEZaY0NSNHlyb1lJQlphU0NBV0V3Z2dGZE1SZ3dGZ1lKS29aSWh2Y05BUWtDRXdsVFpYSjJaWElnUTBFeElEQWVCZ2txaGtpRzl3MEJDUUVXRVhWalgyWnJRSEp2YzJ0aGVtNWhMbkoxTVJ3d0dnWURWUVFJREJNM055RFFzeTRnMEp6UXZ0R0IwTHJRc3RDd01Sb3dHQVlJS29VREE0RURBUUVTRERBd056Y3hNRFUyT0RjMk1ERVlNQllHQlNxRkEyUUJFZzB4TURRM056azNNREU1T0RNd01Td3dLZ1lEVlFRSkRDUFJnOUM3MExqUmh0Q3dJTkNZMEx2UmpOQzQwTDNRdXRDd0xDRFF0TkMrMEx3Z056RVZNQk1HQTFVRUJ3d00wSnpRdnRHQjBMclFzdEN3TVFzd0NRWURWUVFHRXdKU1ZURTRNRFlHQTFVRUNnd3YwS1RRdGRDMDBMWFJnTkN3MEx2UmpOQzkwTDdRdFNEUXV0Q3cwTGZRdmRDdzBZZlF0ZEM1MFlIUmd0Q3kwTDR4UHpBOUJnTlZCQU1NTnRDajBLWWcwS1RRdGRDMDBMWFJnTkN3MEx2UmpOQzkwTDdRczlDK0lOQzYwTERRdDlDOTBMRFJoOUMxMExuUmdkR0MwTExRc0lJQkFUQmVCZ05WSFI4RVZ6QlZNQ21nSjZBbGhpTm9kSFJ3T2k4dlkzSnNMbkp2YzJ0aGVtNWhMbkoxTDJOeWJDOW1hekF4TG1OeWJEQW9vQ2FnSklZaWFIUjBjRG92TDJOeWJDNW1jMlpyTG14dlkyRnNMMk55YkM5bWF6QXhMbU55YkRBZEJnTlZIUTRFRmdRVXVBa0kxOXk2SWpuancxMDExM0RuNzEzdzdab3dDQVlHS29VREFnSURBMEVBOU5EUDM0V3RsYWRaSXpLYmxIN3hSUEh4a2FyeTl6MjJqMC81UnA1Q1F6cGp4S2FWalFydUtPVktJRXl5Q1I0YmxNRVlULzlPMnhlYVdTaGIrZ2lheURHQ0EvY3dnZ1B6QWdFQk1JSUJaakNDQVYweEdEQVdCZ2txaGtpRzl3MEJDUUlUQ1ZObGNuWmxjaUJEUVRFZ01CNEdDU3FHU0liM0RRRUpBUllSZFdOZlptdEFjbTl6YTJGNmJtRXVjblV4SERBYUJnTlZCQWdNRXpjM0lOQ3pMaURRbk5DKzBZSFF1dEN5MExBeEdqQVlCZ2dxaFFNRGdRTUJBUklNTURBM056RXdOVFk0TnpZd01SZ3dGZ1lGS29VRFpBRVNEVEV3TkRjM09UY3dNVGs0TXpBeExEQXFCZ05WQkFrTUk5R0QwTHZRdU5HRzBMQWcwSmpRdTlHTTBMalF2ZEM2MExBc0lOQzAwTDdRdkNBM01SVXdFd1lEVlFRSERBelFuTkMrMFlIUXV0Q3kwTEF4Q3pBSkJnTlZCQVlUQWxKVk1UZ3dOZ1lEVlFRS0RDL1FwTkMxMExUUXRkR0EwTERRdTlHTTBMM1F2dEMxSU5DNjBMRFF0OUM5MExEUmg5QzEwTG5SZ2RHQzBMTFF2akUvTUQwR0ExVUVBd3cyMEtQUXBpRFFwTkMxMExUUXRkR0EwTERRdTlHTTBMM1F2dEN6MEw0ZzBMclFzTkMzMEwzUXNOR0gwTFhRdWRHQjBZTFFzdEN3QWdNWTZxc3dDZ1lHS29VREFnSUpCUUNnZ2dJb01CZ0dDU3FHU0liM0RRRUpBekVMQmdrcWhraUc5dzBCQndFd0hBWUpLb1pJaHZjTkFRa0ZNUThYRFRFM01ERXlNekEwTlRVeU1sb3dMd1lKS29aSWh2Y05BUWtFTVNJRUlIT1pFUURPanQyNXhxU0ZqQklsSzFpcGpZSW9sUTRKMDZMSnFTcEpRZ21aTUlJQnV3WUxLb1pJaHZjTkFRa1FBaTh4Z2dHcU1JSUJwakNDQWFJd2dnR2VNQWdHQmlxRkF3SUNDUVFndGRqR2dHRkxjd3hXVGs5d3ozcVNadVZvOXdiYlUxQ3RjNm93NC9LNzFpa3dnZ0Z1TUlJQlphU0NBV0V3Z2dGZE1SZ3dGZ1lKS29aSWh2Y05BUWtDRXdsVFpYSjJaWElnUTBFeElEQWVCZ2txaGtpRzl3MEJDUUVXRVhWalgyWnJRSEp2YzJ0aGVtNWhMbkoxTVJ3d0dnWURWUVFJREJNM055RFFzeTRnMEp6UXZ0R0IwTHJRc3RDd01Sb3dHQVlJS29VREE0RURBUUVTRERBd056Y3hNRFUyT0RjMk1ERVlNQllHQlNxRkEyUUJFZzB4TURRM056azNNREU1T0RNd01Td3dLZ1lEVlFRSkRDUFJnOUM3MExqUmh0Q3dJTkNZMEx2UmpOQzQwTDNRdXRDd0xDRFF0TkMrMEx3Z056RVZNQk1HQTFVRUJ3d00wSnpRdnRHQjBMclFzdEN3TVFzd0NRWURWUVFHRXdKU1ZURTRNRFlHQTFVRUNnd3YwS1RRdGRDMDBMWFJnTkN3MEx2UmpOQzkwTDdRdFNEUXV0Q3cwTGZRdmRDdzBZZlF0ZEM1MFlIUmd0Q3kwTDR4UHpBOUJnTlZCQU1NTnRDajBLWWcwS1RRdGRDMDBMWFJnTkN3MEx2UmpOQzkwTDdRczlDK0lOQzYwTERRdDlDOTBMRFJoOUMxMExuUmdkR0MwTExRc0FJREdPcXJNQW9HQmlxRkF3SUNFd1VBQkVDNjUvTzc0S2gwUGdmbmV4VVVLdEM3N1BiV2I1UTdFbVRscTRNeDVUOTgvNnhoa3E5N1luSERSWGNCNk0vTExLQVc3WEJpSkdHbmVQd0NTYmEzN1JEZA==</signature>
        </printForm>
    </ns2:fcs_notificationEFDateChange>
</ns2:export>
";

            Xmler.Deserialize(xml);
        }

        [Test]
        public void MultipleLots()
        {
            Xmler.Deserialize(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<ns2:export xmlns=""http://zakupki.gov.ru/oos/types/1"" xmlns:ns2=""http://zakupki.gov.ru/oos/export/1"" xmlns:ns3=""http://zakupki.gov.ru/oos/printform/1"" xmlns:ns4=""http://zakupki.gov.ru/oos/control99/1"">
    <ns2:fcsNotificationOKD schemeVersion=""7.0"">
        <id>10899367</id>
        <purchaseNumber>9911111112417000011</purchaseNumber>
        <docPublishDate>2017-01-23T00:14:06.568+03:00</docPublishDate>
        <href>http://zakupki.gov.ru/epz/order/notice/okd44/view/common-info.html?regNumber=9911111112417000011</href>
        <printForm>
            <url>http://zakupki.gov.ru/epz/order/notice/printForm/viewXml.html?noticeId=10899367</url>
            <signature type=""CAdES-BES"">TUlJTk5nWUpLb1pJaHZjTkFRY0NvSUlOSnpDQ0RTTUNBUUV4RERBS0JnWXFoUU1DQWdrRkFEQUxCZ2txaGtpRzl3MEJCd0dnZ2dqbU1JSUk0akNDQ0pHZ0F3SUJBZ0lDQ0ZRd0NBWUdLb1VEQWdJRE1JSUJiakVZTUJZR0NTcUdTSWIzRFFFSkFoTUpVMlZ5ZG1WeUlFTkJNU0F3SGdZSktvWklodmNOQVFrQkZoRjFZMTltYTBCeWIzTnJZWHB1WVM1eWRURWNNQm9HQTFVRUNBd1ROemNnMExNdUlOQ2MwTDdSZ2RDNjBMTFFzREVhTUJnR0NDcUZBd09CQXdFQkVnd3dNRGMzTVRBMU5qZzNOakF4R0RBV0JnVXFoUU5rQVJJTk1UQTBOemM1TnpBeE9UZ3pNREVzTUNvR0ExVUVDUXdqMFlQUXU5QzQwWWJRc0NEUW1OQzcwWXpRdU5DOTBMclFzQ3dnMExUUXZ0QzhJRGN4RlRBVEJnTlZCQWNNRE5DYzBMN1JnZEM2MExMUXNERUxNQWtHQTFVRUJoTUNVbFV4T0RBMkJnTlZCQW9NTDlDazBMWFF0TkMxMFlEUXNOQzcwWXpRdmRDKzBMVWcwTHJRc05DMzBMM1FzTkdIMExYUXVkR0IwWUxRc3RDK01WQXdUZ1lEVlFRRERFZlFvdEMxMFlIUmd0QyswTExSaTlDNUlOQ2owS1lnMEtUUXRkQzAwTFhSZ05DdzBMdlJqTkM5MEw3UXM5QytJTkM2MExEUXQ5QzkwTERSaDlDMTBMblJnZEdDMExMUXNEQWVGdzB4TlRFeE1UQXhORE0zTURCYUZ3MHhOekF5TVRBeE5ETTNNREJhTUlJQnl6RWFNQmdHQ0NxRkF3T0JBd0VCRWd3d01EUTBORFEwTkRRME1qUXhGakFVQmdVcWhRTmtBeElMTVRFeE1URXhNVEV4TWpJeEdEQVdCZ1VxaFFOa0FSSU5PVGs1T1RrNU9UazVPVGt5TkRFbE1DTUdDU3FHU0liM0RRRUpBUllXY21GemMydGhlbTkyYTNsQWIyNXNZVzUwWVM1eWRURUxNQWtHQTFVRUJoTUNVbFV4SERBYUJnTlZCQWdNRXpjM0lOQ3pMaURRbk5DKzBZSFF1dEN5MExBeEZUQVRCZ05WQkFjTUROQ2MwTDdSZ2RDNjBMTFFzREV6TURFR0ExVUVDZ3dxMFlMUXRkR0IwWUxRdnRDeTBMRFJqeURRdnRHQTBMUFFzTkM5MExqUXQ5Q3cwWWJRdU5HUElESTBNUlF3RWdZRFZRUUxEQXZRcHRDYUlOQ2UwSjdRb1RFc01Db0dBMVVFS2d3ajBKclF2dEM5MFlIUmd0Q3cwTDNSZ3RDNDBMMGcwSzdSZ05HTTBMWFFzdEM0MFljeEpEQWlCZ05WQkFRTUc5Q2cwTERSZ2RHQjBMclFzTkMzMEw3UXNpM1FvdEMxMFlIUmdqRXBNQ2NHQTFVRURBd2cwWUhRdjlDMTBZYlF1TkN3MEx2UXVOR0IwWUlnMEtiUW1pRFFudENlMEtFeFNEQkdCZ05WQkFNTVA5Q2cwTERSZ2RHQjBMclFzTkMzMEw3UXNpM1FvdEMxMFlIUmdpRFFtdEMrMEwzUmdkR0MwTERRdmRHQzBMalF2U0RRcnRHQTBZelF0ZEN5MExqUmh6QmpNQndHQmlxRkF3SUNFekFTQmdjcWhRTUNBaVFBQmdjcWhRTUNBaDRCQTBNQUJFQW5HSlN2cmdhVzJ0UGlpSlQyMlA2RTlReXVUb0FwMVdndUNIOWhIbXdlNWxFY2NHUkFjUGRrV1VKVmxTUTYxbFc5VEdGWFd1T1orLy9VZmRINkt6NTNvNElFdERDQ0JMQXdEQVlEVlIwVEFRSC9CQUl3QURBZEJnTlZIU0FFRmpBVU1BZ0dCaXFGQTJSeEFUQUlCZ1lxaFFOa2NRSXdXUVlEVlIwUkJGSXdVS0FUQmdOVkJBeWdEQk1LTVRFeE1ERTJOVGd4TTZBWkJnb3FoUU1EUFo3WE5nRUhvQXNUQ1RRME5EUTBORFF5TktBYkJnb3FoUU1EUFo3WE5nRUZvQTBUQ3prNU1URXhNVEV4TVRJMGhnRXdNRFlHQlNxRkEyUnZCQzBNS3lMUW10R0EwTGpRdjlHQzBMN1FuOUdBMEw0Z1ExTlFJaUFvMExMUXRkR0EwWUhRdU5HUElETXVOaWt3Z2dGaEJnVXFoUU5rY0FTQ0FWWXdnZ0ZTREVRaTBKclJnTkM0MEwvUmd0QyswSi9SZ05DK0lFTlRVQ0lnS05DeTBMWFJnTkdCMExqUmp5QXpMallwSUNqUXVOR0IwTC9RdnRDNzBMM1F0ZEM5MExqUXRTQXlLUXhvSXRDZjBZRFF2dEN6MFlEUXNOQzgwTHpRdmRDK0xkQ3cwTC9RdjlDdzBZRFFzTkdDMEwzUmk5QzVJTkM2MEw3UXZOQy8wTHZRdGRDNjBZRWdJdEN1MEwzUXVOR0IwTFhSZ05HQ0xkQ1QwSjdRb2RDaUlpNGcwSkxRdGRHQTBZSFF1TkdQSURJdU1TSU1UOUNoMExYUmdOR0MwTGpSaE5DNDBMclFzTkdDSU5HQjBMN1F2dEdDMExMUXRkR0MwWUhSZ3RDeTBMalJqeURpaEpZZzBLSFFwQzh4TWpFdE1UZzFPU0RRdnRHQ0lERTNMakEyTGpJd01USU1UOUNoMExYUmdOR0MwTGpSaE5DNDBMclFzTkdDSU5HQjBMN1F2dEdDMExMUXRkR0MwWUhSZ3RDeTBMalJqeURpaEpZZzBLSFFwQzh4TWpndE1qRTNOU0RRdnRHQ0lESXdMakEyTGpJd01UTXdEZ1lEVlIwUEFRSC9CQVFEQWdQb01DTUdBMVVkSlFRY01Cb0dDQ3NHQVFVRkJ3TUNCZzRxaFFNRFBaN1hOZ0VHQXdRQ0FqQXJCZ05WSFJBRUpEQWlnQTh5TURFMU1URXhNREUwTXpZMU9GcUJEekl3TVRjd01qQTVNVFF6TnpBd1dqQ0NBYUFHQTFVZEl3U0NBWmN3Z2dHVGdCUzhtTEtIUlVDVXV3OG1zRUhyUHh4SVI3dzJiS0dDQVhha2dnRnlNSUlCYmpFWU1CWUdDU3FHU0liM0RRRUpBaE1KVTJWeWRtVnlJRU5CTVNBd0hnWUpLb1pJaHZjTkFRa0JGaEYxWTE5bWEwQnliM05yWVhwdVlTNXlkVEVjTUJvR0ExVUVDQXdUTnpjZzBMTXVJTkNjMEw3UmdkQzYwTExRc0RFYU1CZ0dDQ3FGQXdPQkF3RUJFZ3d3TURjM01UQTFOamczTmpBeEdEQVdCZ1VxaFFOa0FSSU5NVEEwTnpjNU56QXhPVGd6TURFc01Db0dBMVVFQ1F3ajBZUFF1OUM0MFliUXNDRFFtTkM3MFl6UXVOQzkwTHJRc0N3ZzBMVFF2dEM4SURjeEZUQVRCZ05WQkFjTUROQ2MwTDdSZ2RDNjBMTFFzREVMTUFrR0ExVUVCaE1DVWxVeE9EQTJCZ05WQkFvTUw5Q2swTFhRdE5DMTBZRFFzTkM3MFl6UXZkQyswTFVnMExyUXNOQzMwTDNRc05HSDBMWFF1ZEdCMFlMUXN0QytNVkF3VGdZRFZRUURERWZRb3RDMTBZSFJndEMrMExMUmk5QzVJTkNqMEtZZzBLVFF0ZEMwMExYUmdOQ3cwTHZSak5DOTBMN1FzOUMrSU5DNjBMRFF0OUM5MExEUmg5QzEwTG5SZ2RHQzBMTFFzSUlCQVRCa0JnTlZIUjhFWFRCYk1DdWdLYUFuaGlWb2RIUndPaTh2WTNKc0xtWnpabXN1Ykc5allXd3ZZM0pzTDNSbGMzUXdNREV1WTNKc01DeWdLcUFvaGlab2RIUndPaTh2WTNKc0xuSnZjMnRoZW01aExuSjFMMk55YkM5MFpYTjBNREF4TG1OeWJEQWRCZ05WSFE0RUZnUVV1MWpzNFE3Sms2eDdpWUE1c1dwRjdQc0RTVVF3Q0FZR0tvVURBZ0lEQTBFQUt4eFBodDdESVFRdFdPaGNRNTJaTDB3OHlqWTVOL0tKMDNhckEwZlVOQlRCQXpPSE5ndkU3a2l4TTJsQmFheUp6NTRiY1FBVWVkdmppMGNkUGQxbExER0NCQmN3Z2dRVEFnRUJNSUlCZGpDQ0FXNHhHREFXQmdrcWhraUc5dzBCQ1FJVENWTmxjblpsY2lCRFFURWdNQjRHQ1NxR1NJYjNEUUVKQVJZUmRXTmZabXRBY205emEyRjZibUV1Y25VeEhEQWFCZ05WQkFnTUV6YzNJTkN6TGlEUW5OQyswWUhRdXRDeTBMQXhHakFZQmdncWhRTURnUU1CQVJJTU1EQTNOekV3TlRZNE56WXdNUmd3RmdZRktvVURaQUVTRFRFd05EYzNPVGN3TVRrNE16QXhMREFxQmdOVkJBa01JOUdEMEx2UXVOR0cwTEFnMEpqUXU5R00wTGpRdmRDNjBMQXNJTkMwMEw3UXZDQTNNUlV3RXdZRFZRUUhEQXpRbk5DKzBZSFF1dEN5MExBeEN6QUpCZ05WQkFZVEFsSlZNVGd3TmdZRFZRUUtEQy9RcE5DMTBMVFF0ZEdBMExEUXU5R00wTDNRdnRDMUlOQzYwTERRdDlDOTBMRFJoOUMxMExuUmdkR0MwTExRdmpGUU1FNEdBMVVFQXd4SDBLTFF0ZEdCMFlMUXZ0Q3kwWXZRdVNEUW85Q21JTkNrMExYUXROQzEwWURRc05DNzBZelF2ZEMrMExQUXZpRFF1dEN3MExmUXZkQ3cwWWZRdGRDNTBZSFJndEN5MExBQ0FnaFVNQW9HQmlxRkF3SUNDUVVBb0lJQ09EQVlCZ2txaGtpRzl3MEJDUU14Q3dZSktvWklodmNOQVFjQk1Cd0dDU3FHU0liM0RRRUpCVEVQRncweE56QXhNakl5TVRFME1EWmFNQzhHQ1NxR1NJYjNEUUVKQkRFaUJDQXdBb1A0dmJ5RWxlUE1BaVRpZUp2cVNSMlY0LzlGSGpWVkplLzdIRHBnc0RDQ0Fjc0dDeXFHU0liM0RRRUpFQUl2TVlJQnVqQ0NBYll3Z2dHeU1JSUJyakFJQmdZcWhRTUNBZ2tFSUNWbVRya2ZvaWFuUm9MNlZtYmg3L2UxSXE3Nmw2N3Uzb1VGMkQ1cTJ1RGZNSUlCZmpDQ0FYYWtnZ0Z5TUlJQmJqRVlNQllHQ1NxR1NJYjNEUUVKQWhNSlUyVnlkbVZ5SUVOQk1TQXdIZ1lKS29aSWh2Y05BUWtCRmhGMVkxOW1hMEJ5YjNOcllYcHVZUzV5ZFRFY01Cb0dBMVVFQ0F3VE56Y2cwTE11SU5DYzBMN1JnZEM2MExMUXNERWFNQmdHQ0NxRkF3T0JBd0VCRWd3d01EYzNNVEExTmpnM05qQXhHREFXQmdVcWhRTmtBUklOTVRBME56YzVOekF4T1Rnek1ERXNNQ29HQTFVRUNRd2owWVBRdTlDNDBZYlFzQ0RRbU5DNzBZelF1TkM5MExyUXNDd2cwTFRRdnRDOElEY3hGVEFUQmdOVkJBY01ETkNjMEw3UmdkQzYwTExRc0RFTE1Ba0dBMVVFQmhNQ1VsVXhPREEyQmdOVkJBb01MOUNrMExYUXROQzEwWURRc05DNzBZelF2ZEMrMExVZzBMclFzTkMzMEwzUXNOR0gwTFhRdWRHQjBZTFFzdEMrTVZBd1RnWURWUVFEREVmUW90QzEwWUhSZ3RDKzBMTFJpOUM1SU5DajBLWWcwS1RRdGRDMDBMWFJnTkN3MEx2UmpOQzkwTDdRczlDK0lOQzYwTERRdDlDOTBMRFJoOUMxMExuUmdkR0MwTExRc0FJQ0NGUXdDZ1lHS29VREFnSVRCUUFFUUhjRGIxRTArWHIwUy94cUdIY0x3bmE2M2RBdGVGbzRPKzg0aUtTa24rek5ZSlRqRUVDVWtZNTlicjhqR21XZ1ZMaC9PNFBpcVlnSGFodkNqcVo5M3QwPQ==</signature>
        </printForm>
        <purchaseObjectInfo>тест</purchaseObjectInfo>
        <purchaseResponsible>
            <responsibleOrg>
                <regNum>99111111124</regNum>
                <consRegistryNum>99TTS924</consRegistryNum>
                <fullName>Тестовая организация 24</fullName>
                <postAddress>Российская Федерация, 142114, Московская обл, Подольск г, пр Ватутинский 2-й проезд, 2</postAddress>
                <factAddress>Российская Федерация, 142114, Московская обл, Подольск г, пр Ватутинский 2-й проезд, 2</factAddress>
                <INN>4444444424</INN>
                <KPP>444444424</KPP>
            </responsibleOrg>
            <responsibleRole>RA</responsibleRole>
            <responsibleInfo>
                <orgPostAddress>Российская Федерация, 142114, Московская обл, Подольск г, пр Ватутинский 2-й проезд, 2</orgPostAddress>
                <orgFactAddress>Российская Федерация, 142114, Московская обл, Подольск г, пр Ватутинский 2-й проезд, 2</orgFactAddress>
                <contactPerson>
                    <lastName>тест</lastName>
                    <firstName>тест</firstName>
                    <middleName>тест</middleName>
                </contactPerson>
                <contactEMail>test@test.ru</contactEMail>
                <contactPhone>7-123-1234567</contactPhone>
            </responsibleInfo>
        </purchaseResponsible>
        <placingWay>
            <code>OKDP44</code>
            <name>Двухэтапный конкурс</name>
        </placingWay>
        <purchaseDocumentation>
            <grantStartDate>2017-01-23T00:09:00+03:00</grantStartDate>
            <grantPlace>Российская Федерация, 142114, Московская обл, Подольск г, пр Ватутинский 2-й проезд, 2</grantPlace>
            <grantOrder>тест</grantOrder>
            <languages>тест</languages>
            <grantMeans>тест</grantMeans>
            <grantEndDate>2017-01-24T23:23:00+03:00</grantEndDate>
        </purchaseDocumentation>
        <procedureInfo>
            <stageOne>
                <collecting>
                    <startDate>2017-01-23T11:11:00+03:00</startDate>
                    <place>Российская Федерация, 142114, Московская обл, Подольск г, пр Ватутинский 2-й проезд, 2</place>
                    <order>тест</order>
                    <endDate>2017-01-23T22:22:00+03:00</endDate>
                </collecting>
                <opening>
                    <date>2017-01-23T23:23:00+03:00</date>
                    <place>Российская Федерация, 142114, Московская обл, Подольск г, пр Ватутинский 2-й проезд, 2</place>
                    <addInfo>тест</addInfo>
                </opening>
                <scoring>
                    <date>2017-01-23T23:44:00+03:00</date>
                    <place>Российская Федерация, 142114, Московская обл, Подольск г, пр Ватутинский 2-й проезд, 2</place>
                </scoring>
            </stageOne>
            <stageTwo>
                <collecting>
                    <startDate>2017-01-24T11:11:00+03:00</startDate>
                    <place>Российская Федерация, 142114, Московская обл, Подольск г, пр Ватутинский 2-й проезд, 2</place>
                    <order>тест</order>
                    <endDate>2017-01-24T22:22:00+03:00</endDate>
                </collecting>
                <opening>
                    <date>2017-01-24T22:22:00+03:00</date>
                    <place>Российская Федерация, 142114, Московская обл, Подольск г, пр Ватутинский 2-й проезд, 2</place>
                </opening>
                <scoring>
                    <date>2017-01-24T09:00:00+03:00</date>
                    <place>Российская Федерация, 142114, Московская обл, Подольск г, пр Ватутинский 2-й проезд, 2</place>
                </scoring>
            </stageTwo>
        </procedureInfo>
        <lots>
            <lot>
                <lotNumber>1</lotNumber>
                <lotObjectInfo>тест</lotObjectInfo>
                <maxPrice>11</maxPrice>
                <currency>
                    <code>RUB</code>
                    <name>Российский рубль</name>
                </currency>
                <financeSource>тест</financeSource>
                <quantityUndefined>false</quantityUndefined>
                <customerRequirements>
                    <customerRequirement>
                        <customer>
                            <regNum>99111111120</regNum>
                            <consRegistryNum>99TTS920</consRegistryNum>
                            <fullName>Тестовая организация 20</fullName>
                        </customer>
                        <maxPrice>11</maxPrice>
                        <kladrPlaces>
                            <kladrPlace>
                                <kladr>
                                    <kladrType>R</kladrType>
                                    <kladrCode>01000000000</kladrCode>
                                    <fullName>Российская Федерация, Адыгея Респ</fullName>
                                </kladr>
                                <deliveryPlace>тест</deliveryPlace>
                            </kladrPlace>
                        </kladrPlaces>
                        <deliveryTerm>тест</deliveryTerm>
                        <applicationGuarantee>
                            <amount>0.33</amount>
                            <part>3.0</part>
                            <procedureInfo>тест</procedureInfo>
                            <settlementAccount>55555555555555555555</settlementAccount>
                            <personalAccount>544555555555555555555555555555</personalAccount>
                            <bik>555555555</bik>
                        </applicationGuarantee>
                        <contractGuarantee>
                            <amount>1.21</amount>
                            <part>11.0</part>
                            <procedureInfo>тест</procedureInfo>
                            <settlementAccount>45555555555555555555</settlementAccount>
                            <personalAccount>444555555555555555555555555555</personalAccount>
                            <bik>555555555</bik>
                        </contractGuarantee>
                        <purchaseCode>171444444442044444442011131130111333</purchaseCode>
                    </customerRequirement>
                </customerRequirements>
                <purchaseObjects>
                    <purchaseObject>
                        <OKPD2>
                            <code>01.11.11.111</code>
                            <name>Зерно озимой твердой пшеницы</name>
                        </OKPD2>
                        <name>тест</name>
                        <OKEI>
                            <code>732</code>
                            <nationalCode>ДЕС ПАР</nationalCode>
                        </OKEI>
                        <customerQuantities>
                            <customerQuantity>
                                <customer>
                                    <regNum>99111111120</regNum>
                                    <consRegistryNum>99TTS920</consRegistryNum>
                                    <fullName>Тестовая организация 20</fullName>
                                </customer>
                                <quantity>1</quantity>
                            </customerQuantity>
                        </customerQuantities>
                        <price>11</price>
                        <quantity>
                            <value>1.00</value>
                        </quantity>
                        <sum>11</sum>
                    </purchaseObject>
                    <totalSum>11</totalSum>
                </purchaseObjects>
                <restrictInfo>тест</restrictInfo>
                <mustPublicDiscussion>false</mustPublicDiscussion>
            </lot>
            <lot>
                <lotNumber>2</lotNumber>
                <lotObjectInfo>тест</lotObjectInfo>
                <maxPrice>12</maxPrice>
                <currency>
                    <code>RUB</code>
                    <name>Российский рубль</name>
                </currency>
                <financeSource>тест</financeSource>
                <quantityUndefined>false</quantityUndefined>
                <customerRequirements>
                    <customerRequirement>
                        <customer>
                            <regNum>99111111120</regNum>
                            <consRegistryNum>99TTS920</consRegistryNum>
                            <fullName>Тестовая организация 20</fullName>
                        </customer>
                        <maxPrice>12</maxPrice>
                        <kladrPlaces>
                            <kladrPlace>
                                <kladr>
                                    <kladrType>R</kladrType>
                                    <kladrCode>01000000000</kladrCode>
                                    <fullName>Российская Федерация, Адыгея Респ</fullName>
                                </kladr>
                                <deliveryPlace>тест</deliveryPlace>
                            </kladrPlace>
                        </kladrPlaces>
                        <deliveryTerm>тет</deliveryTerm>
                        <applicationGuarantee>
                            <amount>0.36</amount>
                            <part>3.0</part>
                            <procedureInfo>тест</procedureInfo>
                            <settlementAccount>45555555555555555555</settlementAccount>
                            <personalAccount>444555555555555555555555555555</personalAccount>
                            <bik>555555555</bik>
                        </applicationGuarantee>
                        <contractGuarantee>
                            <amount>1.32</amount>
                            <part>11.0</part>
                            <procedureInfo>тест</procedureInfo>
                            <settlementAccount>45555555555555555555</settlementAccount>
                            <personalAccount>444555555555555555555555555555</personalAccount>
                            <bik>555555555</bik>
                        </contractGuarantee>
                        <purchaseCode>171444444442044444442011141140111333</purchaseCode>
                    </customerRequirement>
                </customerRequirements>
                <purchaseObjects>
                    <purchaseObject>
                        <OKPD2>
                            <code>01.11.11.112</code>
                            <name>Семена озимой твердой пшеницы</name>
                        </OKPD2>
                        <name>тест</name>
                        <OKEI>
                            <code>732</code>
                            <nationalCode>ДЕС ПАР</nationalCode>
                        </OKEI>
                        <customerQuantities>
                            <customerQuantity>
                                <customer>
                                    <regNum>99111111120</regNum>
                                    <consRegistryNum>99TTS920</consRegistryNum>
                                    <fullName>Тестовая организация 20</fullName>
                                </customer>
                                <quantity>1</quantity>
                            </customerQuantity>
                        </customerQuantities>
                        <price>12</price>
                        <quantity>
                            <value>1.00</value>
                        </quantity>
                        <sum>12</sum>
                    </purchaseObject>
                    <totalSum>12</totalSum>
                </purchaseObjects>
                <restrictInfo>тест</restrictInfo>
                <mustPublicDiscussion>false</mustPublicDiscussion>
            </lot>
        </lots>
        <attachments>
            <attachment>
                <publishedContentId>46B50DF246FB011EE0530A86121F44C8</publishedContentId>
                <fileName>Нагаев Алексей.docx</fileName>
                <fileSize>10.12 Кб</fileSize>
                <docDescription>Нагаев Алексей</docDescription>
                <url>http://zakupki.gov.ru/44fz/filestore/public/1.0/download/priz/file.html?uid=46B50DF246FB011EE0530A86121F44C8</url>
                <cryptoSigns>
                    <signature type=""CAdES-BES"">TUlJTk5nWUpLb1pJaHZjTkFRY0NvSUlOSnpDQ0RTTUNBUUV4RERBS0JnWXFoUU1DQWdrRkFEQUxCZ2txaGtpRzl3MEJCd0dnZ2dqbU1JSUk0akNDQ0pHZ0F3SUJBZ0lDQ0ZRd0NBWUdLb1VEQWdJRE1JSUJiakVZTUJZR0NTcUdTSWIzRFFFSkFoTUpVMlZ5ZG1WeUlFTkJNU0F3SGdZSktvWklodmNOQVFrQkZoRjFZMTltYTBCeWIzTnJZWHB1WVM1eWRURWNNQm9HQTFVRUNBd1ROemNnMExNdUlOQ2MwTDdSZ2RDNjBMTFFzREVhTUJnR0NDcUZBd09CQXdFQkVnd3dNRGMzTVRBMU5qZzNOakF4R0RBV0JnVXFoUU5rQVJJTk1UQTBOemM1TnpBeE9UZ3pNREVzTUNvR0ExVUVDUXdqMFlQUXU5QzQwWWJRc0NEUW1OQzcwWXpRdU5DOTBMclFzQ3dnMExUUXZ0QzhJRGN4RlRBVEJnTlZCQWNNRE5DYzBMN1JnZEM2MExMUXNERUxNQWtHQTFVRUJoTUNVbFV4T0RBMkJnTlZCQW9NTDlDazBMWFF0TkMxMFlEUXNOQzcwWXpRdmRDKzBMVWcwTHJRc05DMzBMM1FzTkdIMExYUXVkR0IwWUxRc3RDK01WQXdUZ1lEVlFRRERFZlFvdEMxMFlIUmd0QyswTExSaTlDNUlOQ2owS1lnMEtUUXRkQzAwTFhSZ05DdzBMdlJqTkM5MEw3UXM5QytJTkM2MExEUXQ5QzkwTERSaDlDMTBMblJnZEdDMExMUXNEQWVGdzB4TlRFeE1UQXhORE0zTURCYUZ3MHhOekF5TVRBeE5ETTNNREJhTUlJQnl6RWFNQmdHQ0NxRkF3T0JBd0VCRWd3d01EUTBORFEwTkRRME1qUXhGakFVQmdVcWhRTmtBeElMTVRFeE1URXhNVEV4TWpJeEdEQVdCZ1VxaFFOa0FSSU5PVGs1T1RrNU9UazVPVGt5TkRFbE1DTUdDU3FHU0liM0RRRUpBUllXY21GemMydGhlbTkyYTNsQWIyNXNZVzUwWVM1eWRURUxNQWtHQTFVRUJoTUNVbFV4SERBYUJnTlZCQWdNRXpjM0lOQ3pMaURRbk5DKzBZSFF1dEN5MExBeEZUQVRCZ05WQkFjTUROQ2MwTDdSZ2RDNjBMTFFzREV6TURFR0ExVUVDZ3dxMFlMUXRkR0IwWUxRdnRDeTBMRFJqeURRdnRHQTBMUFFzTkM5MExqUXQ5Q3cwWWJRdU5HUElESTBNUlF3RWdZRFZRUUxEQXZRcHRDYUlOQ2UwSjdRb1RFc01Db0dBMVVFS2d3ajBKclF2dEM5MFlIUmd0Q3cwTDNSZ3RDNDBMMGcwSzdSZ05HTTBMWFFzdEM0MFljeEpEQWlCZ05WQkFRTUc5Q2cwTERSZ2RHQjBMclFzTkMzMEw3UXNpM1FvdEMxMFlIUmdqRXBNQ2NHQTFVRURBd2cwWUhRdjlDMTBZYlF1TkN3MEx2UXVOR0IwWUlnMEtiUW1pRFFudENlMEtFeFNEQkdCZ05WQkFNTVA5Q2cwTERSZ2RHQjBMclFzTkMzMEw3UXNpM1FvdEMxMFlIUmdpRFFtdEMrMEwzUmdkR0MwTERRdmRHQzBMalF2U0RRcnRHQTBZelF0ZEN5MExqUmh6QmpNQndHQmlxRkF3SUNFekFTQmdjcWhRTUNBaVFBQmdjcWhRTUNBaDRCQTBNQUJFQW5HSlN2cmdhVzJ0UGlpSlQyMlA2RTlReXVUb0FwMVdndUNIOWhIbXdlNWxFY2NHUkFjUGRrV1VKVmxTUTYxbFc5VEdGWFd1T1orLy9VZmRINkt6NTNvNElFdERDQ0JMQXdEQVlEVlIwVEFRSC9CQUl3QURBZEJnTlZIU0FFRmpBVU1BZ0dCaXFGQTJSeEFUQUlCZ1lxaFFOa2NRSXdXUVlEVlIwUkJGSXdVS0FUQmdOVkJBeWdEQk1LTVRFeE1ERTJOVGd4TTZBWkJnb3FoUU1EUFo3WE5nRUhvQXNUQ1RRME5EUTBORFF5TktBYkJnb3FoUU1EUFo3WE5nRUZvQTBUQ3prNU1URXhNVEV4TVRJMGhnRXdNRFlHQlNxRkEyUnZCQzBNS3lMUW10R0EwTGpRdjlHQzBMN1FuOUdBMEw0Z1ExTlFJaUFvMExMUXRkR0EwWUhRdU5HUElETXVOaWt3Z2dGaEJnVXFoUU5rY0FTQ0FWWXdnZ0ZTREVRaTBKclJnTkM0MEwvUmd0QyswSi9SZ05DK0lFTlRVQ0lnS05DeTBMWFJnTkdCMExqUmp5QXpMallwSUNqUXVOR0IwTC9RdnRDNzBMM1F0ZEM5MExqUXRTQXlLUXhvSXRDZjBZRFF2dEN6MFlEUXNOQzgwTHpRdmRDK0xkQ3cwTC9RdjlDdzBZRFFzTkdDMEwzUmk5QzVJTkM2MEw3UXZOQy8wTHZRdGRDNjBZRWdJdEN1MEwzUXVOR0IwTFhSZ05HQ0xkQ1QwSjdRb2RDaUlpNGcwSkxRdGRHQTBZSFF1TkdQSURJdU1TSU1UOUNoMExYUmdOR0MwTGpSaE5DNDBMclFzTkdDSU5HQjBMN1F2dEdDMExMUXRkR0MwWUhSZ3RDeTBMalJqeURpaEpZZzBLSFFwQzh4TWpFdE1UZzFPU0RRdnRHQ0lERTNMakEyTGpJd01USU1UOUNoMExYUmdOR0MwTGpSaE5DNDBMclFzTkdDSU5HQjBMN1F2dEdDMExMUXRkR0MwWUhSZ3RDeTBMalJqeURpaEpZZzBLSFFwQzh4TWpndE1qRTNOU0RRdnRHQ0lESXdMakEyTGpJd01UTXdEZ1lEVlIwUEFRSC9CQVFEQWdQb01DTUdBMVVkSlFRY01Cb0dDQ3NHQVFVRkJ3TUNCZzRxaFFNRFBaN1hOZ0VHQXdRQ0FqQXJCZ05WSFJBRUpEQWlnQTh5TURFMU1URXhNREUwTXpZMU9GcUJEekl3TVRjd01qQTVNVFF6TnpBd1dqQ0NBYUFHQTFVZEl3U0NBWmN3Z2dHVGdCUzhtTEtIUlVDVXV3OG1zRUhyUHh4SVI3dzJiS0dDQVhha2dnRnlNSUlCYmpFWU1CWUdDU3FHU0liM0RRRUpBaE1KVTJWeWRtVnlJRU5CTVNBd0hnWUpLb1pJaHZjTkFRa0JGaEYxWTE5bWEwQnliM05yWVhwdVlTNXlkVEVjTUJvR0ExVUVDQXdUTnpjZzBMTXVJTkNjMEw3UmdkQzYwTExRc0RFYU1CZ0dDQ3FGQXdPQkF3RUJFZ3d3TURjM01UQTFOamczTmpBeEdEQVdCZ1VxaFFOa0FSSU5NVEEwTnpjNU56QXhPVGd6TURFc01Db0dBMVVFQ1F3ajBZUFF1OUM0MFliUXNDRFFtTkM3MFl6UXVOQzkwTHJRc0N3ZzBMVFF2dEM4SURjeEZUQVRCZ05WQkFjTUROQ2MwTDdSZ2RDNjBMTFFzREVMTUFrR0ExVUVCaE1DVWxVeE9EQTJCZ05WQkFvTUw5Q2swTFhRdE5DMTBZRFFzTkM3MFl6UXZkQyswTFVnMExyUXNOQzMwTDNRc05HSDBMWFF1ZEdCMFlMUXN0QytNVkF3VGdZRFZRUURERWZRb3RDMTBZSFJndEMrMExMUmk5QzVJTkNqMEtZZzBLVFF0ZEMwMExYUmdOQ3cwTHZSak5DOTBMN1FzOUMrSU5DNjBMRFF0OUM5MExEUmg5QzEwTG5SZ2RHQzBMTFFzSUlCQVRCa0JnTlZIUjhFWFRCYk1DdWdLYUFuaGlWb2RIUndPaTh2WTNKc0xtWnpabXN1Ykc5allXd3ZZM0pzTDNSbGMzUXdNREV1WTNKc01DeWdLcUFvaGlab2RIUndPaTh2WTNKc0xuSnZjMnRoZW01aExuSjFMMk55YkM5MFpYTjBNREF4TG1OeWJEQWRCZ05WSFE0RUZnUVV1MWpzNFE3Sms2eDdpWUE1c1dwRjdQc0RTVVF3Q0FZR0tvVURBZ0lEQTBFQUt4eFBodDdESVFRdFdPaGNRNTJaTDB3OHlqWTVOL0tKMDNhckEwZlVOQlRCQXpPSE5ndkU3a2l4TTJsQmFheUp6NTRiY1FBVWVkdmppMGNkUGQxbExER0NCQmN3Z2dRVEFnRUJNSUlCZGpDQ0FXNHhHREFXQmdrcWhraUc5dzBCQ1FJVENWTmxjblpsY2lCRFFURWdNQjRHQ1NxR1NJYjNEUUVKQVJZUmRXTmZabXRBY205emEyRjZibUV1Y25VeEhEQWFCZ05WQkFnTUV6YzNJTkN6TGlEUW5OQyswWUhRdXRDeTBMQXhHakFZQmdncWhRTURnUU1CQVJJTU1EQTNOekV3TlRZNE56WXdNUmd3RmdZRktvVURaQUVTRFRFd05EYzNPVGN3TVRrNE16QXhMREFxQmdOVkJBa01JOUdEMEx2UXVOR0cwTEFnMEpqUXU5R00wTGpRdmRDNjBMQXNJTkMwMEw3UXZDQTNNUlV3RXdZRFZRUUhEQXpRbk5DKzBZSFF1dEN5MExBeEN6QUpCZ05WQkFZVEFsSlZNVGd3TmdZRFZRUUtEQy9RcE5DMTBMVFF0ZEdBMExEUXU5R00wTDNRdnRDMUlOQzYwTERRdDlDOTBMRFJoOUMxMExuUmdkR0MwTExRdmpGUU1FNEdBMVVFQXd4SDBLTFF0ZEdCMFlMUXZ0Q3kwWXZRdVNEUW85Q21JTkNrMExYUXROQzEwWURRc05DNzBZelF2ZEMrMExQUXZpRFF1dEN3MExmUXZkQ3cwWWZRdGRDNTBZSFJndEN5MExBQ0FnaFVNQW9HQmlxRkF3SUNDUVVBb0lJQ09EQVlCZ2txaGtpRzl3MEJDUU14Q3dZSktvWklodmNOQVFjQk1Cd0dDU3FHU0liM0RRRUpCVEVQRncweE56QXhNakl5TVRFME1EWmFNQzhHQ1NxR1NJYjNEUUVKQkRFaUJDQThMdUZLaTVTNyt4WkxKRmZ6QXhGRnZXUVVBSmtEa3A0b2xKWWN3U1hub3pDQ0Fjc0dDeXFHU0liM0RRRUpFQUl2TVlJQnVqQ0NBYll3Z2dHeU1JSUJyakFJQmdZcWhRTUNBZ2tFSUNWbVRya2ZvaWFuUm9MNlZtYmg3L2UxSXE3Nmw2N3Uzb1VGMkQ1cTJ1RGZNSUlCZmpDQ0FYYWtnZ0Z5TUlJQmJqRVlNQllHQ1NxR1NJYjNEUUVKQWhNSlUyVnlkbVZ5SUVOQk1TQXdIZ1lKS29aSWh2Y05BUWtCRmhGMVkxOW1hMEJ5YjNOcllYcHVZUzV5ZFRFY01Cb0dBMVVFQ0F3VE56Y2cwTE11SU5DYzBMN1JnZEM2MExMUXNERWFNQmdHQ0NxRkF3T0JBd0VCRWd3d01EYzNNVEExTmpnM05qQXhHREFXQmdVcWhRTmtBUklOTVRBME56YzVOekF4T1Rnek1ERXNNQ29HQTFVRUNRd2owWVBRdTlDNDBZYlFzQ0RRbU5DNzBZelF1TkM5MExyUXNDd2cwTFRRdnRDOElEY3hGVEFUQmdOVkJBY01ETkNjMEw3UmdkQzYwTExRc0RFTE1Ba0dBMVVFQmhNQ1VsVXhPREEyQmdOVkJBb01MOUNrMExYUXROQzEwWURRc05DNzBZelF2ZEMrMExVZzBMclFzTkMzMEwzUXNOR0gwTFhRdWRHQjBZTFFzdEMrTVZBd1RnWURWUVFEREVmUW90QzEwWUhSZ3RDKzBMTFJpOUM1SU5DajBLWWcwS1RRdGRDMDBMWFJnTkN3MEx2UmpOQzkwTDdRczlDK0lOQzYwTERRdDlDOTBMRFJoOUMxMExuUmdkR0MwTExRc0FJQ0NGUXdDZ1lHS29VREFnSVRCUUFFUUtCZWE4THA4MSs5Z1JYc3JZS05MODRiM0oyVStjdGhGUFducVhKY1RhNC94cEg2L0ZrbTJ5WjhMMlJUZDJpQ3BUcEZpQjA1UU95WEo1eWV0anltMkxFPQ==</signature>
                </cryptoSigns>
            </attachment>
        </attachments>
    </ns2:fcsNotificationOKD>
</ns2:export>
").Single().Price.ShouldBe(23);
        }
    }
}