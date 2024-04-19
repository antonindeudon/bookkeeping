using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bookkeeping
{
    public class RuntimeData
    {
        public List<RuntimeCategoryData> rootCategories;

        private List<RuntimeCategoryData> allCategories;
        private Data data;

        public void Init(Data data)
        {
            this.data = data;
            CreateAllRuntimeCategoryData();
            rootCategories = new List<RuntimeCategoryData>();
            LinkChildrenToParent();
            AddEntriesToCategories();
        }

        private void CreateAllRuntimeCategoryData()
        {
            allCategories = new List<RuntimeCategoryData>();
            foreach(Category category in data.allCategories)
            {
                allCategories.Add(new RuntimeCategoryData(category));
            }
        }

        private void LinkChildrenToParent()
        {
            //Link children to parents
            for (int i = 0; i < allCategories.Count; i++)
            {
                RuntimeCategoryData category = allCategories[i];
                RuntimeCategoryData parentCategory = string.IsNullOrEmpty(category.Category.parentName) ? null : FindCategory(category.Category.parentName);
                if (parentCategory != null)
                {
                    RuntimeCategoryData.Link(category, parentCategory);
                }
                else
                {
                    rootCategories.Add(category);
                }
            }
        }

        private void AddEntriesToCategories()
        {
            foreach (Entry entry in data.entries)
            {
                if (entry.category == null)
                {
                    Debug.LogWarning("No category for entry " + entry.dateString + " - " + entry.name);
                }
                else
                {
                    RuntimeCategoryData category = FindCategory(entry.category.name);
                    category.Entries.Add(entry);
                }
            }
        }

        public RuntimeCategoryData FindCategory(string categoryName)
        {
            foreach (RuntimeCategoryData category in allCategories)
            {
                if (category.Category.name == categoryName)
                {
                    return category;
                }
            }
            return null;
        }
    }
}