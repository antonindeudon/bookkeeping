using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

namespace Bookkeeping
{
    public class BookkeepingUI : MonoBehaviour
    {
        public FridayAfternoons.ObjectPool<EntryUI> entryUIPool;
        public FridayAfternoons.ObjectPool<CategoryUI> categoryUIPool;
        public TextMeshProUGUI totalAmountText;
        public TextMeshProUGUI debtsTotalText;
        public bool openCategoriesByDefault;
        public Toggle repaidAllToggle;

        public event Action<Entry> OnRepaidChangedEvent;

        private Person selectedPerson;
        private float debtsTotal;

        private bool empty;
        private bool allEntriesShared;
        private bool allEntriesRepaid;
        private bool settingEntries;

        private void Awake()
        {
            entryUIPool.OnInstantiate += OnEntryUIInstanceCreated;
            entryUIPool.Init(true);
            categoryUIPool.Init(true);
            repaidAllToggle.onValueChanged.AddListener(OnAllToggleValueChanged);
        }

        private void OnEntryUIInstanceCreated(EntryUI entryUI)
        {
            entryUI.OnRepaidChanged += OnRepaidChanged;
        }

        private void OnRepaidChanged(Entry entry)
        {
            OnRepaidChangedEvent?.Invoke(entry);

            if(!settingEntries)
            {
                empty = entryUIPool.ActiveObjects.Count == 0;
                allEntriesShared = true;
                allEntriesRepaid = true;
                foreach (EntryUI entryUI in entryUIPool.ActiveObjects)
                {
                    if (!entryUI.Entry.Shared) allEntriesShared = false;
                    if (!entryUI.Entry.repaid) allEntriesRepaid = false;
                }
                UpdateRepaidAllToggle();
            }
        }

        private void UpdateRepaidAllToggle()
        {
            repaidAllToggle.gameObject.SetActive(!empty && allEntriesShared);
            SetRepaidAllToggleOnWithoutTriggeringListener(allEntriesRepaid);
        }

        private void SetRepaidAllToggleOnWithoutTriggeringListener(bool on)
        {
            repaidAllToggle.onValueChanged.RemoveListener(OnAllToggleValueChanged);
            repaidAllToggle.isOn = on;
            repaidAllToggle.onValueChanged.AddListener(OnAllToggleValueChanged);
        }

        private EntryUI GetNewEntryUI(Entry entry)
        {
            EntryUI entryUI = entryUIPool.GetAvailableInstance();
            entryUI.SetData(entry);
            entryUI.gameObject.SetActive(true);
            return entryUI;
        }

        public void SetEntries(List<RuntimeCategoryData> rootCategories, FilteringController filteringController)
        {
            settingEntries = true;

            float globalTotal = 0;
            debtsTotal = 0;
            selectedPerson = filteringController.SelectedPerson;

            entryUIPool.ReleaseAll();
            categoryUIPool.ReleaseAll();

            empty = true;
            allEntriesShared = true;
            allEntriesRepaid = true;

            foreach (RuntimeCategoryData category in rootCategories)
            {
                float rootCategoryTotal = SetCategoryEntries(category, categoryUIPool.container, filteringController);
                globalTotal += rootCategoryTotal;
            }

            totalAmountText.text = BookkeepingUtils.GetCurrencyString(globalTotal);
            UpdateDebtsTotalText();
            UpdateRepaidAllToggle();
            
            settingEntries = false;
        }

        private void UpdateDebtsTotalText()
        {
            debtsTotalText.gameObject.SetActive(selectedPerson != null);
            if(selectedPerson != null)
            {
                string personName = selectedPerson.name;
                string totalAmount = BookkeepingUtils.GetCurrencyString(Mathf.Abs(debtsTotal));
                if (debtsTotal > 0) debtsTotalText.text = "Je dois " + totalAmount + " à " + personName;
                else if (debtsTotal < 0) debtsTotalText.text = personName + " me doit " + totalAmount;
                else debtsTotalText.text = "Tout est entièrement remboursé";
            }
        }

        private float SetCategoryEntries(RuntimeCategoryData category, Transform parent, FilteringController filteringController)
        {
            float totalAmount = 0;

            //Init
            CategoryUI categoryUI = categoryUIPool.GetAvailableInstance();
            categoryUI.SetBasicData(category, openCategoriesByDefault);
            categoryUI.transform.SetParent(parent);
            categoryUI.gameObject.SetActive(true);

            //Add entries
            foreach (Entry entry in category.Entries)
            {
                if (filteringController.Filter(entry))
                {
                    empty = false;
                    if (!entry.Shared) allEntriesShared = false;
                    if (!entry.repaid) allEntriesRepaid = false;

                    EntryUI entryUI = GetNewEntryUI(entry);
                    entryUI.transform.SetParent(categoryUI.childrenContainer);
                    totalAmount += entry.FinalAmount;
                    debtsTotal += entry.DebtAmount;
                }
            }

            //Add children categories
            foreach (RuntimeCategoryData child in category.Children)
            {
                totalAmount += SetCategoryEntries(child, categoryUI.childrenContainer, filteringController);
            }

            //Don't show if empty
            if (empty) categoryUI.gameObject.SetActive(false);

            //Set total amount
            categoryUI.SetTotalAmount(totalAmount);
            return totalAmount;
        }

        private void OnAllToggleValueChanged(bool value)
        {
            foreach(EntryUI entryUI in entryUIPool.ActiveObjects)
            {
                entryUI.repaidToggle.isOn = value;
            }
        }
    }
}