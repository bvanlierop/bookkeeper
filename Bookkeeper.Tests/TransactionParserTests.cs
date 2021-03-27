using NUnit.Framework;
using System;

namespace Bookkeeper.Tests
{
    [TestFixture]
    public class TransactionParserTests
    {
        /*
            ABN AMRO Spec:
            .TAB file contains UTF8 text with all transactions from the users selected export action.
            Each transaction is seperated with a CRLF (\n\r)
            Within a transaction the fields are TAB seperated (in case of .tab file) '\t'

            Assumptions:
                - the TAB file is syntactically correct
                - amounts are correct and valid (don't account for invalid transaction lines)
        */

        private const string ValidTransactionLineWithCreditAmount = "17,75;Jumbo Bergeijk;PAS241";
        private const string ValidTransactionLineWithDebitAmount = "-1,22;Jumbo Bergeijk;PAS241";
        private const string TwoValidTransactions = ValidTransactionLineWithCreditAmount + "\r\n" + ValidTransactionLineWithDebitAmount;

        [Test]
        [TestCase(1, ValidTransactionLineWithCreditAmount)]
        [TestCase(2, TwoValidTransactions)]
        public void ParserCanParseAllTransactions(int expectedCount, string inputTransactions)
        {
            var parser = new TransactionParser(inputTransactions);

            parser.Parse();

            Assert.AreEqual(expectedCount, parser.Transactions.Count);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ParserThrowsArgumentExceptionWhenParsingEmptyTransactions(string invalidInputTransaction)
        {
            var parser = new TransactionParser(invalidInputTransaction);

            _ = Assert.Throws<ArgumentException>(
                delegate { parser.Parse(); });
        }

        [Test]
        public void ParserReadsLine()
        {
            var parser = new TransactionParser(ValidTransactionLineWithCreditAmount);

            parser.Parse();

            Assert.AreEqual(ValidTransactionLineWithCreditAmount, parser.Transactions[0]);
        }
    }
}
