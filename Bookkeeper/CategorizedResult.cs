using System;
using System.Collections.Generic;

namespace Bookkeeper
{
    public class CategorizedResult
    {
        public Dictionary<string, decimal> Categories;

        public CategorizedResult()
        {
            Categories = new Dictionary<string, decimal>();
        }
    }
}