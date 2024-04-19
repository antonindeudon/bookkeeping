using System;
using System.Collections.Generic;

namespace Bookkeeping
{
    [Serializable]
    public class Category
    {
        public string name;
        public string parentName;

        public Category(string name, string parentName)
        {
            this.name = name;
            this.parentName = parentName;
        }
    }
}