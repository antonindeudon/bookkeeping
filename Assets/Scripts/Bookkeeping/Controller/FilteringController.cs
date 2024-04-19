using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bookkeeping
{
    public class FilteringController : MonoBehaviour
    {
        public FiltersUI filtersUI;

        public event Action onFiltersUpdated;

        private List<Filter> filters;
        private PersonFilter personFilter;

        public void Init(Data data)
        {
            filters = new List<Filter>();

            DateFilter dateFilter = new DateFilter();
            filtersUI.monthYearPickerUI.Init(dateFilter);
            filters.Add(dateFilter);

            personFilter = new PersonFilter();
            filtersUI.personPickerUI.Init(personFilter, data.persons);
            filters.Add(personFilter);

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            foreach(Filter filter in filters)
            {
                filter.onFilterUpdated += OnFilterUpdated;
            }
        }

        private void OnFilterUpdated()
        {
            onFiltersUpdated?.Invoke();
        }

        public List<Entry> Filter(List<Entry> entries)
        {
            List<Entry> filteredEntries = new List<Entry>();
            foreach(Entry entry in entries)
            {
                if(Filter(entry)) filteredEntries.Add(entry);
            }
            return filteredEntries;
        }

        public bool Filter(Entry entry)
        {
            foreach(Filter filter in filters)
            {
                if(filter.Active && !filter.AcceptEntry(entry)) return false;
            }
            return true;
        }

        public Person SelectedPerson
        {
            get
            {
                if (!personFilter.Active) return null;
                else return personFilter.person;
            }
        }
    }
}