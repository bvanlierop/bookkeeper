using NUnit.Framework;
using System;

namespace Bookkeeper.Tests
{
    [TestFixture]
    public class TransactionReaderTests
    {
        private const string ValidTransactionLineWithCreditAmount = "17,75;Jumbo Bergeijk;PAS241";
        private const string ValidTransactionLineWithDebitAmount = "-1,22;Jumbo Bergeijk;PAS241";
        private const string TwoValidTransactions = ValidTransactionLineWithCreditAmount + "\r\n" + ValidTransactionLineWithDebitAmount;

        [Test]
        public void CanParseAmountInTransactionLineWithCreditAmount()
        {
            decimal expectedAmount = 17.75M;

            var tr = new TransactionReader(new TransactionParser(ValidTransactionLineWithCreditAmount));
            var actual = tr.GetTotalAmount();

            Assert.AreEqual(expectedAmount, actual);
        }
        
        [Test]
        public void CanParseAmountInTransactionLinesWithDebitAmounts()
        {
            decimal expectedAmount = 16.53M;

            var tr = new TransactionReader(new TransactionParser(TwoValidTransactions));
            var actual = tr.GetTotalAmount();

            Assert.AreEqual(expectedAmount, actual);
        }

        [Test]
        public void ThrowsApplicationExceptionDuringParsingWhenTransactionContainsInvalidAmount()
        {
            const string invalidAmount = "1x,9y";
            var tr = new TransactionReader(new TransactionParser($"{invalidAmount};McDonalds;PAS123"));

            _ = Assert.Throws<ApplicationException>(
                delegate { _ = tr.GetTotalAmount(); });
        }
    }
}
