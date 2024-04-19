using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bookkeeping
{
    public class RuntimeCategoryData
    {
        private Category category;
        private RuntimeCategoryData parent;
        private List<RuntimeCategoryData> children;
        private List<Entry> entries;

        public Category Category => category;
        public RuntimeCategoryData Parent => parent;
        public List<RuntimeCategoryData> Children => children;
        public List<Entry> Entries => entries;

        public RuntimeCategoryData(Category category)
        {
            this.category = category;
            children = new List<RuntimeCategoryData>();
            entries = new List<Entry>();
        }

        public int TotalEntriesCount
        {
            get
            {
                int total = Entries.Count;
                foreach (RuntimeCategoryData child in Children)
                {
                    total += child.TotalEntriesCount;
                }
                return total;
            }
        }

        public static void Link(RuntimeCategoryData child, RuntimeCategoryData parent)
        {
            child.parent = parent;
            parent.Children.Add(child);
        }
    }
}