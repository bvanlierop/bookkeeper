using Newtonsoft.Json;
using System.IO;

namespace Bookkeeper
{
    public class TransactionProcessor
    {
        private readonly ITransactionParser parser;
        private readonly string categoryMapJsonString;
        private CategorizedResult result;
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
            result = new CategorizedResult();
            ReadCategoryEntries();

            var transactions = parser.Parse();
            foreach (var transaction in transactions)
            {
                var category = DetermineCategory(transaction.Description);
                if (!result.Categories.ContainsKey(category.Name))
                {
                    result.Categories.Add(category.Name, transaction.Amount);
                }
                else
                {
                    result.Categories[category.Name] += transaction.Amount;
                }
            }

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
