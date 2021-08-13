using NUnit.Framework;
using System.Collections;
using System.IO;
using System.Reflection;

namespace Bookkeeper.Tests
{
    [TestFixture]
    [Category("Integration")]
    class IntegrationTests
    {
        private readonly string LocationOfTestFiles = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestFiles");

        [Test]
        public void ShouldProcessOfficialSmallAbnAmroBankExportFile()
        {
           var categoryMapJsonString =
               "{" +
                   "\"Categories\": " +
                   "[ " +
                       "{" +
                           "\"Keyword\": \"zorgverzekeringen\"," +
                           "\"CategoryName\": \"healthcare\" " +
                       "}," +
                       "{" +
                           "\"Keyword\": \"contactalook\"," +
                           "\"CategoryName\": \"healthcare\" " +
                       "}," +
                       "{" +
                           "\"Keyword\": \"cars are us\"," +
                           "\"CategoryName\": \"car\" " +
                       "}" +
                   "]" +
               "}";

            var inputFileData = File.ReadAllText(Path.Combine(LocationOfTestFiles, "SMALL_ABN_AMRO_EXPORT_FILE.TAB"));
            var inputCategoryMapJson = File.ReadAllText(Path.Combine(LocationOfTestFiles, "TestCategories.json"));
            var parser = new TransactionParser(inputFileData);
            var categorizer = new TransactionCategorizer(parser, categoryMapJsonString);
            var categorizedResult = categorizer.CategorizeTransactions();
            var cr = new CategoryReporter(categorizedResult);

            var report = cr.CreateReport();

            StringAssert.Contains("car: -350.12 EUR", report);
            StringAssert.Contains("healthcare: -1259.56 EUR", report);
            StringAssert.Contains("unknown: -1.23 EUR", report);
            StringAssert.Contains("[UNKNOWN] -1.23 EUR @ 1/4/2021 12:00:00 AM - BEA   NR:CT653123 04.01.21/16.33 CCV POSTMASTERS,PAS123   AMSTERDAM", report);
        }
    }
}
