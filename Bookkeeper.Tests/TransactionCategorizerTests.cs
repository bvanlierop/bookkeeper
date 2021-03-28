using NUnit.Framework;
using System.Collections.Generic;

namespace Bookkeeper.Tests
{
    [TestFixture]
    public class TransactionCategorizerTests
    {
        [Test]
        public void CategorizerCategorizesHousingRelatedTransactionBasedOnDescription()
        {
            var fakeTransactions = new List<Transaction>
            {
                new Transaction(-844.34M, "Mortgage Inc."), // housing
                new Transaction(-320.01M, "Wallmart Co.")   // groceries
            };

            var categorizer = new TransactionCategorizer(fakeTransactions);
            var result = categorizer.Categorize();

            Assert.IsTrue(result.Categories.ContainsKey("housing"));
        }

        [Test]
        public void CategorizerCategorizesUnknownTransactionsWhenDescriptionIsUnknown()
        {
            var fakeTransactions = new List<Transaction>
            {
                new Transaction(-844.34M, "Mortgage Inc."), // housing
                new Transaction(-320.01M, "Wallmart Co.")   // groceries
            };

            var categorizer = new TransactionCategorizer(fakeTransactions);
            var result = categorizer.Categorize();

            Assert.IsTrue(result.Categories.ContainsKey("unknown"));
        }
    }
}
