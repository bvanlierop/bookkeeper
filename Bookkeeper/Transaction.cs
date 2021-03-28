namespace Bookkeeper
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }

        public Transaction(decimal amount, string description)
        {
            Amount = amount;
            Description = description;
        }
    }
}