using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bookkeeping
{
    public class DateFilter : Filter
    {
        public DateTime startDate;
        public DateTime endDate;

        public override bool AcceptEntry(Entry entry)
        {
            return entry.date >= startDate && entry.date <= endDate;
        }

        public void UpdateFilter(DateTime startDate, DateTime endDate)
        {
            this.startDate = startDate;
            this.endDate = endDate;
            OnFilterUpdated();
        }
    }
}