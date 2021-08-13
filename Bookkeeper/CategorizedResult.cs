using System;
using System.Collections;
using System.Collections.Generic;

namespace Bookkeeper
{
    public class CategorizedResult
    {
        public Dictionary<string, Transaction> Categories;
        public Hashtable CategorizedTransactions;

        public CategorizedResult()
        {
            Categories = new Dictionary<string, Transaction>();
            CategorizedTransactions = new Hashtable();
        }
    }
}