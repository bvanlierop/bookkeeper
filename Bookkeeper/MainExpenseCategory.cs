using System.Collections.Generic;

namespace Bookkeeper
{
    public class MainExpenseCategory
    {
        private string mainExpenseCategoryName;
        private decimal amount;
        private string[] descriptionKeywords;
        
        public decimal TotalAmount { get => amount; set => amount = value; }
        public string[] DescriptionKeywords { get => descriptionKeywords; set => descriptionKeywords = value; }

        public MainExpenseCategory(string mainExpenseCategoryName, string[] descriptionKeywords)
        {
            this.mainExpenseCategoryName = mainExpenseCategoryName;
            this.amount = 0.0M;
            this.descriptionKeywords = descriptionKeywords;
        }

        public override string ToString()
        {
            return mainExpenseCategoryName; 
        }
    }
}