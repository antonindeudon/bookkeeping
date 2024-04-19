using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bookkeeping
{
    public class Data 
    {
        [Header("Imported data")]
        public List<Category> allCategories;
        public List<Entry> entries;
        public List<Person> persons;

        public Data()
        {
            allCategories = new List<Category>();
            entries = new List<Entry>();
            persons = new List<Person>();
        }

        public void Clear()
        {
            allCategories.Clear();
            entries.Clear();
            persons.Clear();
        }

        public Category FindCategory(string categoryName)
        {
            foreach (Category category in allCategories)
            {
                if (category.name == categoryName)
                {
                    return category;
                }
            }
            return null;
        }
    }
}