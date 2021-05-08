using NUnit.Framework;
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
        public void ShouldProcessOfficialAbnAmroBankExportFile()
        {
            var inputFileData = File.ReadAllText(Path.Combine(LocationOfTestFiles, "THREE_MONTH_ABN_AMRO_EXPORT_FILE.TAB"));
            var inputCategoryMapJson = File.ReadAllText(Path.Combine(LocationOfTestFiles, "TestCategories.json"));

            var parser = new TransactionParser(inputFileData);
            var processor = new TransactionProcessor(parser, inputCategoryMapJson);
            var cr = new CategoryReporter(processor);

            var report = cr.CreateReport();

            StringAssert.Contains("unknown: 2453.43 EUR", report);
        }
    }
}
