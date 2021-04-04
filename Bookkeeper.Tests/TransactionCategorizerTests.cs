using NUnit.Framework;
using System.Collections.Generic;

namespace Bookkeeper.Tests
{
    [TestFixture]
    public class TransactionCategorizerTests
    {
        [Test]
        public void CategorizerCategorizesTransactionDescriptionsBasedOnBucketLookup()
        {
            var expectedGroceriesAmount = -354.21M;

            var fakeTransactions = new List<Transaction>
            {
                new Transaction(-844.34M, "Mortgage Inc."),
                new Transaction(-320.01M, "Wallmart Co."),
                new Transaction(-34.20M, "wallgreens")
            };

            var buckets = new Dictionary<string, string>
            {
                { "wallgreens", "groceries" },
                { "wallmart", "groceries" },
                { "mortgage", "financial" }
            };

            var categorizer = new TransactionCategorizer(buckets, fakeTransactions);
            var actual = categorizer.GetGroceriesAmount();

            Assert.AreEqual(expectedGroceriesAmount, actual);
        }

        [Test]
        public void CategorizerCategorizesUnknownTransactionDescriptions()
        {
            var expected = -4.00M;

            var fakeTransactions = new List<Transaction>
            {
                new Transaction(-34.20M, "wallgreens"),
                new Transaction(-4.00M, "some_store")
            };

            var buckets = new Dictionary<string, string>
            {
                { "wallgreens", "groceries" }
            };

            var categorizer = new TransactionCategorizer(buckets, fakeTransactions);
            var actual = categorizer.GetUnknownAmount();

            Assert.AreEqual(expected, actual);
        }
    }
}
