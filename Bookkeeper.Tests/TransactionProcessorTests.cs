using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Bookkeeper.Tests
{
    [TestFixture]
    public class TransactionProcessorTests
    {
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
            var tp = new TransactionProcessor(new TransactionParser(TransactionTestData.InvalidTransactionLineWithInvalidAmount));

            _ = Assert.Throws<ApplicationException>(
                delegate { _ = tp.GetTotalAmount(); });
        }
    }
}
