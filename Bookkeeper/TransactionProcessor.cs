using System.Collections.Generic;

namespace Bookkeeper
{
    public class TransactionProcessor
    {
        private readonly ITransactionParser parser;
        private readonly Dictionary<string, string> categoryMap;
        private CategorizedResult result;

        public TransactionProcessor(ITransactionParser parser, Dictionary<string, string> categoryMap)
        {
            this.parser = parser;
            this.categoryMap = categoryMap;
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

            var transactions = parser.Parse();
            foreach (var transaction in transactions)
            {
                var category = DetermineCategory(transaction.Description);
                if(!result.Categories.ContainsKey(category.Name))
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

        private Category DetermineCategory(string description)
        {
            var category = new Category("unknown");

            foreach (var descriptionKeyword in categoryMap.Keys)
            {
                var categoryName = categoryMap[descriptionKeyword];
                if (TransactionMatchesKnownCategory(description, descriptionKeyword))
                {
                    category = new Category(categoryName);
                    break;
                }
            }

            return category;
        }

        private static bool TransactionMatchesKnownCategory(string description, string descriptionKeyword)
        {
            return description.ToLower().Contains(descriptionKeyword);
        }
    }
}
