using NUnit.Framework;
using System;

namespace Bookkeeper.Tests
{
    [TestFixture]
    public class TransactionProcessorTests
    {
        [Test]
        public void SummizesTransactionsInGroceryCategory()
        {
            var expected = -123.01M;

            var processor = new TransactionProcessor(
                new TransactionParser(TransactionTestData.ValidTransactionsContainingGroceries));

            CategorizedResult res = processor.SummizeCategories();

            Assert.AreEqual(expected, res.Categories["groceries"]);
        }

        [Test]
        public void SummizesUnknownTransactionsInUnknownCategory()
        {
            var expected = 14.23M;

            var processor = new TransactionProcessor(
                new TransactionParser(TransactionTestData.ValidTransactionsContainingGroceries));

            CategorizedResult res = processor.SummizeCategories();

            Assert.AreEqual(expected, res.Categories["unknown"]);
        }

        [Test]
        public void CalculatesTotalAmountOfAllTransactions()
        {
            decimal expectedAmount = 14.23M;

            var tp = new TransactionProcessor(new TransactionParser(TransactionTestData.TwoValidTransactions));
            var actual = tp.GetTotalAmount();

            Assert.AreEqual(expectedAmount, actual);
        }

        [Test]
        public void ThrowsApplicationExceptionDuringTotalAmountCalculationWhenTransactionContainsInvalidAmount()
        {
            var tp = new TransactionProcessor(
                new TransactionParser(TransactionTestData.InvalidTransactionLineWithInvalidAmount));

            _ = Assert.Throws<ApplicationException>(
                delegate { _ = tp.GetTotalAmount(); });
        }
    }
}
