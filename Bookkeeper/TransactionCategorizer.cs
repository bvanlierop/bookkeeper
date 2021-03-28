using System.Collections.Generic;

namespace Bookkeeper
{
    public class TransactionCategorizer
    {
        public List<Transaction> Transactions { get; }

        public TransactionCategorizer(List<Transaction> transactions)
        {
            Transactions = transactions;
        }

        public CategorizedResult Categorize()
        {
            var result = new CategorizedResult();

            foreach(var transaction in Transactions)
            {
                if(transaction.Description.ToLower().Contains("mortgage"))
                {
                    result.Categories.Add("housing", transaction.Amount);
                }
                else
                {
                    result.Categories.Add("unknown", transaction.Amount);
                }
            }

            return result;
        }
    }
}
