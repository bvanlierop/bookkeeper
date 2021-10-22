namespace Bookkeeper
{
    public class MainExpenseCategory
    {
        private string mainExpenseCategoryName;
        private decimal amount;
        
        public decimal TotalAmount { get => amount; set => amount = value; }

        public MainExpenseCategory(string mainExpenseCategoryName)
        {
            this.mainExpenseCategoryName = mainExpenseCategoryName;
            this.amount = 0.0M;
        }

        public override string ToString()
        {
            return mainExpenseCategoryName; 
        }
    }
}