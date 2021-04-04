namespace Bookkeeper
{
    public class TransactionProcessor
    {
        private readonly TransactionParser parser;

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
    }
}