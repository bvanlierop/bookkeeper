﻿using Newtonsoft.Json;
using System.Collections;
using System.IO;

namespace Bookkeeper
{
    public class TransactionProcessor
    {
        private readonly ITransactionParser parser;
        private readonly string categoryMapJsonString;
        private CategoryEntries categoryEntries;

        public TransactionProcessor(ITransactionParser parser, string categoryMapJsonString)
        {
            this.parser = parser;
            this.categoryMapJsonString = categoryMapJsonString;
        }

        public decimal GetTotalAmount()
        {
            parser.Parse();
            return CalculateSumOfAllTransactions();
        }

        private decimal CalculateSumOfAllTransactions()
        {
            decimal sum = 0.0M;

            foreach (var transaction in parser.Parse())
            {
                sum += transaction.Amount;
            }

            return sum;
        }

        public CategorizedResult SummizeCategories()
        {
            var result = new CategorizedResult();
            ReadCategoryEntries();
            var transactionAndCategoryTable = new Hashtable();
            var transactions = parser.Parse();
            foreach (var transaction in transactions)
            {
                var category = DetermineCategory(transaction.Description);
                transactionAndCategoryTable.Add(transaction, category);
            }

            result.CategorizedTransactions = transactionAndCategoryTable;
            return result;
        }

        private void ReadCategoryEntries()
        {
            categoryEntries = new CategoryEntries();

            using (var stringReader = new StringReader(categoryMapJsonString))
            {
                using (var jsonReader = new JsonTextReader(stringReader))
                {
                    var jsonSerializer = new JsonSerializer();
                    categoryEntries = jsonSerializer.Deserialize<CategoryEntries>(jsonReader);
                    if(categoryEntries == null)
                    {
                        // when no json data is provided
                        categoryEntries = new CategoryEntries();
                    }
                }
            }
        }

        private Category DetermineCategory(string description)
        {
            var category = new Category("unknown");

            foreach (var categoryEntry in categoryEntries.Categories)
            {
                if (TransactionMatchesKnownCategory(description, categoryEntry.Keyword))
                {
                    category = new Category(categoryEntry.CategoryName);
                    break;
                }
            }

            return category;
        }

        private static bool TransactionMatchesKnownCategory(string description, string descriptionKeyword)
        {
            return description.ToLower().Contains(descriptionKeyword.ToLower());
        }
    }
}
