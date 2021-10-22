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
            var categoryMapJsonString =
               "{" +
                   "\"Categories\": " +
                   "[ " +
                       "{" +
                           "\"Keyword\": \"wallgreens\"," +
                           "\"CategoryName\": \"groceries\" " +
                       "}," +
                       "{" +
                           "\"Keyword\": \"wallmart\"," +
                           "\"CategoryName\": \"groceries\" " +
                       "}," +
                       "{" +
                           "\"Keyword\": \"visa\"," +
                           "\"CategoryName\": \"finances\" " +
                       "}," +
                       "{" +
                           "\"Keyword\": \"mortgage\"," +
                           "\"CategoryName\": \"finances\" " +
                       "}" +
                   "]" +
               "}";
            
            var transactionParserMock = new Mock<ITransactionParser>();
            transactionParserMock.Setup(x => x.ParseExpenses()).Returns(new List<Transaction>
            {
                new Transaction(-10.00M, "xyz", new DateTime(2020, 1, 1)),
                new Transaction(-10.00M, "mortgage inc.", new DateTime(2020, 1, 1)),
                new Transaction(-30.00M, "VISA", new DateTime(2020, 1, 1)),
            });

            var processor = new TransactionProcessor(transactionParserMock.Object, categoryMapJsonString);
            //var cr = new CategoryReporter(processor);

            //var report = cr.CreateReport();

            //StringAssert.Contains("finances: -40.00 EUR", report);
        }
    }
}
