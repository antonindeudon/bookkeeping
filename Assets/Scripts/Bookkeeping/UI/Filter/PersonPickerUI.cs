using System.Collections.Generic;
using TMPro;

namespace Bookkeeping
{
    public class PersonPickerUI : FilterUI
    {
        public TMP_Dropdown personDropdown;

        private PersonFilter filter;
        private List<Person> persons;

        protected override Filter Filter => filter;


        protected override void Awake()
        {
            base.Awake();
            personDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        public void Init(PersonFilter filter, List<Person> persons)
        {
            this.filter = filter;
            this.persons = persons;

            List<string> names = new List<string>();
            foreach(Person person in persons)
            {
                names.Add(person.name);
            }
            personDropdown.AddOptions(names);

            UpdateFilter();
        }

        private void OnDropdownValueChanged(int value)
        {
            UpdateFilter();
        }

        private void UpdateFilter()
        {
            filter.UpdateFilter(persons.Count > 0 ? persons[personDropdown.value] : null);
        }
    }
}