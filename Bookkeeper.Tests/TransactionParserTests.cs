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

            [0] = AccountNumber of exported accounts
            [1] = Currency (EUR)
            [2] = Date of transaction
            [3] = Account Balance before transaction
            [4] = Account Balance after transaction
            [5] = Date transaction processed?
            [7] = Amount
            [8] = Descripition

            Assumptions:
                - the TAB file is syntactically correct
                - amounts are correct and valid (don't account for invalid transaction lines)
        */

        [Test]
        public void ParserParsesCreditTransaction()
        {
            var parser = new TransactionParser(TransactionTestData.ValidTransactionLineWithCreditAmount);
            parser.Parse();

            Assert.AreEqual(17.75M, parser.Transactions[0].Amount);
        }

        [Test]
        public void ParserParsesDebitTransaction()
        {
            var parser = new TransactionParser(TransactionTestData.ValidTransactionLineWithDebitAmount);
            parser.Parse();

            Assert.AreEqual(-3.52M, parser.Transactions[0].Amount);
        }

        [Test]
        public void ParserCanParseMultipleTransactions()
        {
            var parser = new TransactionParser(TransactionTestData.TwoValidTransactions);

            parser.Parse();

            Assert.AreEqual(2, parser.Transactions.Count);
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
            var parser = new TransactionParser(TransactionTestData.ValidTransactionLineWithDebitAmount);

            parser.Parse();

            Assert.AreEqual(1, parser.Transactions.Count);
        }
    }
}
