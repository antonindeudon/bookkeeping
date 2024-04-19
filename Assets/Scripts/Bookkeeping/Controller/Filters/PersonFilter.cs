using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bookkeeping
{
    public class PersonFilter : Filter
    {
        public Person person;

        public override bool AcceptEntry(Entry entry)
        {
            return entry.Shared == true && entry.sharedWith.name == person.name;
        }

        public void UpdateFilter(Person person)
        {
            this.person = person;
            OnFilterUpdated();
        }
    }
}