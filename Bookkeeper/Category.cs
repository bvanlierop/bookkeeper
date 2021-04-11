using System.Collections.Generic;

namespace Bookkeeper
{
    public class Category
    {
        public string Name { get; }

        public List<Transaction> Transactions;

        public Category(string name)
        {
            Name = name;
            Transactions = new List<Transaction>();
        }

        public decimal GetAmountInPeriod(int year, int month)
        {
            decimal total = 0.0M;

            foreach(var transaction in Transactions)
            {
                if(transaction.Date.Year == year && transaction.Date.Month == month)
                {
                    total += transaction.Amount;
                }
            }

            return total;
        }

        public override string ToString()
        {
            return $"{Name} ({Transactions.Count})";
        }
    }
}