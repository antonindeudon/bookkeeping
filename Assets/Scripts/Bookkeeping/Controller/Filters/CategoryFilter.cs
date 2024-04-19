using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bookkeeping
{
    public class CategoryFilter : Filter
    {
        public RuntimeCategoryData category;
        public bool includeSubcategories;

        public override bool AcceptEntry(Entry entry)
        {
            if(category == null) return false;

            if(includeSubcategories)
            {
                return IsInCategoryRecursive(category, entry);
            }
            else
            {
                return entry.category == category.Category;
            }
        }

        private bool IsInCategoryRecursive(RuntimeCategoryData category, Entry entry)
        {
            if (category.Category == entry.category) return true;
            else
            {
                foreach(RuntimeCategoryData child in category.Children)
                {
                    if (IsInCategoryRecursive(child, entry)) return true;
                }
                return false;
            }
        }
    }
}