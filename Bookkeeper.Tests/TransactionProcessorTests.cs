using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Bookkeeper.Tests
{
    [TestFixture]
    public class TransactionProcessorTests
    {
        [Test]
        public void SummizesTransactionsInGroceryCategory()
        {
            var expected = -123.01M;

            var categoryMap = new Dictionary<string, string>()
            {
                { "wallgreens", "groceries" },
                { "wallmart", "groceries" },
                { "mortgage", "finances" }
            };
            var processor = new TransactionProcessor(
                new TransactionParser(TransactionTestData.ValidTransactionsContainingGroceries), 
                categoryMap);

            CategorizedResult res = processor.SummizeCategories();

            Assert.AreEqual(expected, res.Categories["groceries"]);
        }

        [Test]
        public void SummizesTransactionsBasedOnParameterizedCategories()
        {
            var expectedFinancesAmount = -10.00M;

            var categoryMap = new Dictionary<string, string>()
            {
                { "wallgreens", "groceries" },
                { "wallmart", "groceries" },
                { "mortgage", "finances" }
            };

            var transactionParserMock = new Mock<ITransactionParser>();
            transactionParserMock.Setup(x => x.Parse()).Returns(new List<Transaction>
            {
                new Transaction(-10.00M, "xyz", new DateTime(2020, 1, 1)),
                new Transaction(-10.00M, "mortgage inc.", new DateTime(2020, 1, 1)),
            });

            var processor = new TransactionProcessor(transactionParserMock.Object, categoryMap);
            CategorizedResult res = processor.SummizeCategories();

            Assert.AreEqual(expectedFinancesAmount, res.Categories["finances"]);
        }

        [Test]
        public void SummizesUnknownTransactionsInUnknownCategory()
        {
            var expected = -108.78M;
            
            var processor = new TransactionProcessor(
                new TransactionParser(TransactionTestData.ValidTransactionsContainingGroceries),
                new Dictionary<string, string>());

            CategorizedResult res = processor.SummizeCategories();

            Assert.AreEqual(expected, res.Categories["unknown"]);
        }

        [Test]
        public void CalculatesTotalAmountOfAllTransactions()
        {
            decimal expectedAmount = 14.23M;

            var tp = new TransactionProcessor(
                new TransactionParser(TransactionTestData.TwoValidTransactions),
                new Dictionary<string, string>());
            var actual = tp.GetTotalAmount();

            Assert.AreEqual(expectedAmount, actual);
        }

        [Test]
        public void ThrowsApplicationExceptionDuringTotalAmountCalculationWhenTransactionContainsInvalidAmount()
        {
            var tp = new TransactionProcessor(
                new TransactionParser(TransactionTestData.InvalidTransactionLineWithInvalidAmount),
                new Dictionary<string, string>());

            _ = Assert.Throws<ApplicationException>(
                delegate { _ = tp.GetTotalAmount(); });
        }
    }
}
