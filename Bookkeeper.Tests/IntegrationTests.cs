using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace Bookkeeper.Tests
{
    [TestFixture]
    [Category("Integration")]
    class IntegrationTests
    {
        [Test]
        public void ShouldProcessOfficialAbnAmroBankExportFile()
        {
            // TODO: no real integration test if just reading from disk (should use file abstraction here)
            var inputFileData = File.ReadAllText($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\TestFiles\THREE_MONTH_ABN_AMRO_EXPORT_FILE.TAB");

            var categoryMapJsonString = 
                "{" +
                    "\"Categories\": " +
                    "[ " +
                        "{" +
                            "\"Keyword\": \"Cars are Us\"," +
                            "\"CategoryName\": \"car\" " +
                        "}," +
                        "{" +
                            "\"Keyword\": \"Zorgverzekeringen\"," +
                            "\"CategoryName\": \"healthcare\" " +
                        "}," +
                        "{" +
                            "\"Keyword\": \"Contactalook\"," +
                            "\"CategoryName\": \"healthcare\" " +
                        "}" +
                    "]" +
                "}";

            var parser = new TransactionParser(inputFileData);
            var processor = new TransactionProcessor(parser, categoryMapJsonString);
            var cr = new CategoryReporter(processor);
            var report = cr.CreateReport();

            // TODO: split up expenses with income
            StringAssert.Contains("unknown: 2453.43 EUR", report);
        }
    }
}
