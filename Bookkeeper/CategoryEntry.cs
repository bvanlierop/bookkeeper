namespace Bookkeeper
{
    public class CategoryEntry
    {
        private MainExpenseCategory expenseCategory;

        public string Keyword;

        public MainExpenseCategory ExpenseCategory { get => expenseCategory; set => expenseCategory = value; }

        public CategoryEntry(MainExpenseCategory category, string keyword)
        {
            ExpenseCategory = category;
            Keyword = keyword;
        }
    }
}
