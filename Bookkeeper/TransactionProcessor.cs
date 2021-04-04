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
            foreach (var transaction in parser.Parse())
            {
                Categorize(transaction);
            }

            return result;
        }

        private void Categorize(Transaction transaction)
        {
            result = new CategorizedResult();
            bool added = false;

            foreach (var descriptionKeyword in categoryMap.Keys)
            {
                var categoryName = categoryMap[descriptionKeyword];
                if (TransactionMatchesKnownCategory(transaction, descriptionKeyword))
                {
                    AddToCategory(transaction, categoryName);
                    added = true;
                }
            }

            if (!added)
            {
                AddToUnknownCategory(transaction);
            }
        }

        private static bool TransactionMatchesKnownCategory(Transaction transaction, string descriptionKeyword)
        {
            return transaction.Description.ToLower().Contains(descriptionKeyword);
        }

        private void AddToCategory(Transaction transaction, string categoryName)
        {
            if (result.Categories.ContainsKey(categoryName))
            {
                result.Categories[categoryName] += transaction.Amount;
            }
            else
            {
                result.Categories.Add(categoryName, transaction.Amount);
            }
        }

        private void AddToUnknownCategory(Transaction transaction)
        {
            if (result.Categories.ContainsKey("unknown"))
            {
                result.Categories["unknown"] += transaction.Amount;
            }
            else
            {
                result.Categories.Add("unknown", transaction.Amount);
            }
        }
    }
}
