namespace Bookkeeper
{
    public class TransactionProcessor
    {
        private readonly TransactionParser parser;
        private CategorizedResult result;

        public TransactionProcessor(TransactionParser parser)
        {
            this.parser = parser;
        }

        public decimal GetTotalAmount()
        {
            parser.Parse();
            return CalculateSumOfAllTransactions();
        }

        private decimal CalculateSumOfAllTransactions()
        {
            decimal sum = 0.0M;

            foreach (var transaction in parser.Transactions)
            {
                sum += transaction.Amount;
            }

            return sum;
        }

        public CategorizedResult SummizeCategories()
        {
            parser.Parse();

            result = new CategorizedResult();
            foreach(var transaction in parser.Transactions)
            {
                Categorize(transaction);
            }

            return result;
        }

        private void Categorize(Transaction transaction)
        {
            if (transaction.Description.ToLower().Contains("wallgreens") ||
                transaction.Description.ToLower().Contains("wallmart"))
            {
                ProcessGroceries(transaction);
            }
            else
            {
                ProcessUnknowns(transaction);
            }
        }

        private void ProcessUnknowns(Transaction transaction)
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

        private void ProcessGroceries(Transaction transaction)
        {
            if (result.Categories.ContainsKey("groceries"))
            {
                result.Categories["groceries"] += transaction.Amount;
            }
            else
            {
                result.Categories.Add("groceries", transaction.Amount);
            }
        }
    }
}