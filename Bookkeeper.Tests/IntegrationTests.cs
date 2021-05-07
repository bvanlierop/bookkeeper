using NUnit.Framework;
using System.Collections.Generic;
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

            // TODO: use input file as category map instead of this hardcoded list
            var categoryMap = new Dictionary<string, string>()
            {
                { "Cars are Us", "car" },
                { "Zorgverzekeringen", "healthcare" },
                { "Contactalook", "healthcare" },
                { "PHONE COMPANY", "phone" },
                { "Broker Corp.", "banking"}
            };

            var parser = new TransactionParser(inputFileData);
            var processor = new TransactionProcessor(parser, categoryMap);
            var cr = new CategoryReporter(processor);
            var report = cr.CreateReport();

            // TODO: split up expenses with income
            StringAssert.Contains("unknown: 1188.22 EUR", report);
        }
    }
}
