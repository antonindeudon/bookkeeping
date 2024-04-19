using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Google.Apis.Sheets.v4.Data;

namespace Bookkeeping
{
    public class MonthYearPickerUI : FilterUI
    {
        public TMP_Dropdown monthDropdown;
        public TMP_Dropdown yearDropdown;

        private DateFilter filter;

        protected override Filter Filter => filter;

        protected override void Awake()
        {
            base.Awake();
            monthDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            yearDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        public void Init(DateFilter filter)
        {
            this.filter = filter;
            UpdateFilter();
        }

        private void OnDropdownValueChanged(int value)
        {
            UpdateFilter();
        }

        private void UpdateFilter()
        { 
            int month = monthDropdown.value + 1;
            int year = yearDropdown.value + 2024;
            filter.UpdateFilter(
                new DateTime(year, month, 1),
                new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59)
            );
        }
    }
}