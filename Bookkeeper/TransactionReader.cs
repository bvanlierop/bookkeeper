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
                var transactionPieces = transaction.Split(';');
                decimal amount = 0.0M;
                try
                {
                    amount = decimal.Parse(transactionPieces[0].Replace(",", "."));
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Unable to process amount.", ex);
                }
                sum += amount;
            }

            return sum;
        }
    }
}