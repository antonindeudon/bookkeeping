using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Bookkeeping
{
    public static class DataLoader
    {
        private static SheetConnector connector;
        private static CultureInfo culture;
        private static Data data;

        public static Data LoadData(ConnectionData connectionData)
        {
            culture = CultureInfo.CreateSpecificCulture("fr-FR");
            connector = new SheetConnector(connectionData);
            data = new Data();

            ProcessCategories(connector.GetSheetRange(connectionData.CategoriesRange));
            ProcessAllEntries(connectionData);

            return data;
        }

        private static void ProcessCategories(IList<IList<object>> values)
        {
            if(values == null)
            {
                Debug.LogWarning("No categories found");
                return;
            }

            //Create categories with name only and store parent category name
            for(int i=0; i<values.Count; i++)
            {
                //Check data exists in this line
                IList<object> line = values[i];
                if (line.Count < 1)
                {
                    Debug.LogError("No data in line " + i);
                    continue;
                }

                //Check category name exists
                string name = line[0] as string;
                if(string.IsNullOrEmpty(name))
                {
                    Debug.LogError("Category name is empty in line " + i);
                    continue;
                }

                //Check category name is unique
                if(data.FindCategory(name) != null)
                {
                    Debug.LogError("Duplicate category name \"" + name + "\" in line " + i);
                    continue;
                }

                //Create category
                string parentName = line.Count < 2 ? null : line[1] as string;
                data.allCategories.Add(new Category(line[0] as string, parentName));
            }
        }

        private static void ProcessAllEntries(ConnectionData connectionData)
        {
            string[] sheetNames = connectionData.EntriesSheets.Split(',');
            foreach(string sheetName in sheetNames)
            {
                string range = "'" + sheetName + "'!" + connectionData.EntriesRange;
                ProcessEntries(connector.GetSheetRange(range));
            }
        }

        private static void ProcessEntries(IList<IList<object>> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].Count == 0 || string.IsNullOrEmpty(values[i][0].ToString())) continue;

                Entry entry = new Entry()
                {
                    name = GetString(values, i, 2),
                    amount = GetFloat(values, i, 1),
                    category = data.FindCategory(GetString(values, i, 3)),
                    sharedWith = FindPerson(GetString(values, i, 5, false), true),
                };
                
                entry.SetDate(GetString(values, i, 0));

                float thisPersonOwesMe = GetFloat(values, i, 6, false) / 100;
                float iOweThisPerson = GetFloat(values, i, 7, false) / 100;
                if(thisPersonOwesMe > 0)
                {
                    entry.owedPercentage = thisPersonOwesMe;
                    entry.owedByMe = false;
                }
                else if(iOweThisPerson > 0)
                {
                    entry.owedPercentage = iOweThisPerson;
                    entry.owedByMe = true;
                }

                entry.repaid = GetBool(values, i, 8);

                data.entries.Add(entry);
            }
        }

        private static string GetString(IList<IList<object>> values, int lineIndex, int cellIndex, bool log = true)
        {
            if (cellIndex >= values[lineIndex].Count)
            {
                if(log) Debug.Log("No data in cell " + cellIndex + " in line " + lineIndex+" (count is " + values[lineIndex].Count+")");
                return null;
            }

            return values[lineIndex][cellIndex] as string;
        }

        private static float GetFloat(IList<IList<object>> values, int lineIndex, int cellIndex, bool log = true)
        {
            string stringValue = GetString(values, lineIndex, cellIndex, log);
            if (string.IsNullOrEmpty(stringValue)) return 0;

            try
            {
                return float.Parse(stringValue, culture);
            }
            catch(FormatException)
            {
                Debug.Log("Cannot parse data to float in cell " + cellIndex + " in line " + lineIndex + ": " + stringValue);
                return 0;
            }
        }

        private static bool GetBool(IList<IList<object>> values, int lineIndex, int cellIndex, bool log = true)
        {
            return GetString(values, lineIndex, cellIndex, log)  == "TRUE";
        }

        private static Person FindPerson(string name, bool createIfNotFound)
        {
            if(string.IsNullOrEmpty(name)) return null;

            foreach (Person person in data.persons)
            {
                if (person.name == name) return person;
            }

            if (createIfNotFound)
            {
                Person newPerson = new Person() { name = name };
                data.persons.Add(newPerson);
                return newPerson;
            }
            else
            {
                return null;
            }
        }
    }
}