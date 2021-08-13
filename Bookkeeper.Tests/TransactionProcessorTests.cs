using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Bookkeeper.Tests
{
    [TestFixture]
    public class TransactionProcessorTests
    {
        private string categoryMapJsonString =
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
                       "\"Keyword\": \"mortgage\"," +
                       "\"CategoryName\": \"finances\" " +
                   "}" +
               "]" +
           "}";

        [Test]
        public void SummizesTransactionsInGroceryCategory()
        {
            var expected = -123.01M;
            var processor = new TransactionProcessor(
                new TransactionParser(TransactionTestData.ValidTransactionsContainingGroceries), 
                categoryMapJsonString);

            var table = processor.SummizeCategories();
        }

        [Test]
        public void SummizesTransactionsBasedOnParameterizedCategories()
        {
            var expectedFinancesAmount = -10.00M;
            var transactionParserMock = new Mock<ITransactionParser>();
            transactionParserMock.Setup(x => x.Parse()).Returns(new List<Transaction>
            {
                new Transaction(-10.00M, "xyz", new DateTime(2020, 1, 1)),
                new Transaction(-10.00M, "Mortgage Inc.", new DateTime(2020, 1, 1)),
            });

            var processor = new TransactionProcessor(transactionParserMock.Object, categoryMapJsonString);
            CategorizedResult res = processor.SummizeCategories();

            Assert.AreEqual(expectedFinancesAmount, res.Categories["finances"]);
        }

        [Test]
        public void SummizesUnknownTransactionsInUnknownCategory()
        {
            var expected = -130.05M;
            
            var processor = new TransactionProcessor(
                new TransactionParser(TransactionTestData.ValidTransactionsContainingGroceries),
                string.Empty);

            CategorizedResult res = processor.SummizeCategories();

            Assert.AreEqual(expected, res.Categories["unknown"]);
        }

        [Test]
        public void CalculatesTotalAmountOfAllTransactions()
        {
            decimal expectedAmount = -7.04M;

            var tp = new TransactionProcessor(
                new TransactionParser(TransactionTestData.TwoValidTransactions),
                string.Empty);
            var actual = tp.GetTotalAmount();

            Assert.AreEqual(expectedAmount, actual);
        }

        [Test]
        public void ThrowsApplicationExceptionDuringTotalAmountCalculationWhenTransactionContainsInvalidAmount()
        {
            var tp = new TransactionProcessor(
                new TransactionParser(TransactionTestData.InvalidTransactionLineWithInvalidAmount),
                string.Empty);

            _ = Assert.Throws<ApplicationException>(
                delegate { _ = tp.GetTotalAmount(); });
        }
    }
}
