using System;
using System.Collections.Generic;

namespace Bookkeeper
{
    public class TransactionCategorizer
    {
        private Dictionary<string, string> buckets;

        public List<Transaction> Transactions { get; }
        
        public TransactionCategorizer(Dictionary<string, string> buckets, List<Transaction> transactions)
        {
            this.buckets = buckets;
            Transactions = transactions;
        }

        public decimal GetGroceriesAmount()
        {
            decimal groceriesAmount = 0.0M;

            foreach (var transaction in Transactions)
            {
                foreach (var key in buckets.Keys)
                {
                    if (transaction.Description.ToLower().Contains(key))
                    {
                        if (buckets[key] == "groceries")
                        {
                            groceriesAmount += transaction.Amount;
                        }
                    }
                }
            }

            return groceriesAmount;
        }

        public decimal GetUnknownAmount()
        {
            decimal totalAmount = 0.0M;

            foreach (var transaction in Transactions)
            {
                bool categoryFound = false;
                foreach (var key in buckets.Keys)
                {
                    if (transaction.Description.ToLower().Contains(key))
                    {
                        if (buckets[key] == "groceries")
                        {
                            categoryFound = true;
                        }
                    }
                }

                if(!categoryFound)
                {
                    totalAmount += transaction.Amount;
                }
            }

            return totalAmount;
        }
    }
}
