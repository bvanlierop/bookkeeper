using NUnit.Framework;
using System;

namespace Bookkeeper.Tests
{
    [TestFixture]
    public class TransactionReaderTests
    {
        private const string ValidTransactionLineWithCreditAmount = "123551334	EUR	20201218	12958,40	12954,88	20201218	10,00	ACME Company";
        private const string ValidTransactionLineWithDebitAmount = "123551334	EUR	20201218	12958,40	12954,88	20201218	-1,00	ACME Company";
        private const string InvalidTransactionLineWithInvalidAmount = "123551334	EUR	20201218	12958,40	12954,88	20201218	-x,0y	ACME Company";
        private const string TwoValidTransactions = ValidTransactionLineWithCreditAmount + "\r\n" + ValidTransactionLineWithDebitAmount;

        [Test]
        public void CanParseAmountInTransactionLineWithCreditAmount()
        {
            decimal expectedAmount = 10.00M;

            var tr = new TransactionReader(new TransactionParser(ValidTransactionLineWithCreditAmount));
            var actual = tr.GetTotalAmount();

            Assert.AreEqual(expectedAmount, actual);
        }
        
        [Test]
        public void CanParseAmountInTransactionLinesWithDebitAmounts()
        {
            decimal expectedAmount = 9.00M;

            var tr = new TransactionReader(new TransactionParser(TwoValidTransactions));
            var actual = tr.GetTotalAmount();

            Assert.AreEqual(expectedAmount, actual);
        }

        [Test]
        public void ThrowsApplicationExceptionDuringParsingWhenTransactionContainsInvalidAmount()
        {
            var tr = new TransactionReader(new TransactionParser(InvalidTransactionLineWithInvalidAmount));

            _ = Assert.Throws<ApplicationException>(
                delegate { _ = tr.GetTotalAmount(); });
        }
    }
}
