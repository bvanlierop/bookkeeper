using System;

namespace Bookkeeper
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Category Category { get; set; }

        public Transaction(decimal amount, string description, DateTime date)
        {
            Amount = amount;
            Description = description;
            Date = date;
            Category = new Category("unknown");
        }

        public override string ToString()
        {
            return $"Transaction: {Amount} - {Description} - {Category} [{Date:yyyy-MM-dd}]";
        }
    }
}