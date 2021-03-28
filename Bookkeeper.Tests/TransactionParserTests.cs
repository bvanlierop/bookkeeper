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

        private const string ValidTransactionLineWithCreditAmount = "123551334	EUR	20201218	12958,40	12954,88	20201218	17,75	ACME Company";
        private const string ValidTransactionLineWithDebitAmount  = "123551334	EUR	20201218	12958,40	12954,88	20201218	-3,52	ACME Company";
        private const string TwoValidTransactions = ValidTransactionLineWithCreditAmount + "\r\n" + ValidTransactionLineWithDebitAmount;

        [Test]
        public void ParserParsesAllTransactions()
        {
            var testTransactionLine = "123551334	EUR	20201218	12958,40	12954,88	20201218	-3,52	ABN AMRO Bank N.V.               Rekening                    1,94Betaalpas                   1,58                                 ";

            var parser = new TransactionParser(testTransactionLine);
            parser.Parse();

            Assert.AreEqual(-3.52M, parser.Transactions[0].Amount);
        }

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

            Assert.AreEqual(1, parser.Transactions.Count);
        }
    }
}
