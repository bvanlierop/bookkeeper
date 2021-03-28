using System;

namespace Bookkeeper
{
    public class TransactionReader
    {
        private readonly TransactionParser parser;

        public TransactionReader(TransactionParser parser)
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
    }
}