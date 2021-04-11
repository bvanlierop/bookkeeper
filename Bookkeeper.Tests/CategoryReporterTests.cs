using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Bookkeeper.Tests
{
    [TestFixture]
    public class CategoryReporterTests
    {
        [Test]
        public void ReportAmountPerCategory()
        {
            var categoryMap = new Dictionary<string, string>()
            {
                { "wallgreens", "groceries" },
                { "wallmart", "groceries" },
                { "mortgage", "finances" },
                { "visa", "finances"}
            };

            var transactionParserMock = new Mock<ITransactionParser>();
            transactionParserMock.Setup(x => x.Parse()).Returns(new List<Transaction>
            {
                new Transaction(-10.00M, "xyz", new DateTime(2020, 1, 1)),
                new Transaction(-10.00M, "mortgage inc.", new DateTime(2020, 1, 1)),
                new Transaction(-30.00M, "VISA", new DateTime(2020, 1, 1)),
            });

            var processor = new TransactionProcessor(transactionParserMock.Object, categoryMap);
            var cr = new CategoryReporter(processor);

            var report = cr.CreateReport();

            StringAssert.Contains("finances: -40.00 EUR", report);
        }
    }
}
