using NUnit.Framework;
using System;

namespace Bookkeeper.Tests
{
    [TestFixture]
    public class TransactionReaderTests
    {
        [Test]
        public void CalculatesTotalAmountOfAllTransactions()
        {
            decimal expectedAmount = 14.23M;

            var tr = new TransactionReader(new TransactionParser(TransactionTestData.TwoValidTransactions));
            var actual = tr.GetTotalAmount();

            Assert.AreEqual(expectedAmount, actual);
        }

        [Test]
        public void ThrowsApplicationExceptionDuringTotalAmountCalculationWhenTransactionContainsInvalidAmount()
        {
            var tr = new TransactionReader(new TransactionParser(TransactionTestData.InvalidTransactionLineWithInvalidAmount));

            _ = Assert.Throws<ApplicationException>(
                delegate { _ = tr.GetTotalAmount(); });
        }
    }
}
