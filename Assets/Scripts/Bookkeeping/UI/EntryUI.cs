using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bookkeeping
{
    public class EntryUI : MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI amountText;
        public TextMeshProUGUI sharedText;
        public TextMeshProUGUI debtsText;
        public Toggle repaidToggle;

        public event Action<Entry> OnRepaidChanged;

        private Entry entry;

        public Entry Entry => entry;

        public void SetData(Entry entry)
        {
            this.entry = entry;

            nameText.text = entry.name;
            BookkeepingUtils.SetAmountText(amountText, entry.FinalAmount);
            UpdateSharedText();
            UpdateDebtsText();
            InitRepaidToggle();
        }

        private void UpdateSharedText()
        {
            string sharedString = "";
            if (entry.Shared)
            {
                string payedBy = entry.owedByMe ? entry.sharedWith.name : "moi";
                string ower = entry.owedByMe ? "je lui" : entry.sharedWith.name + " me";
                sharedString = $"Dépense de {BookkeepingUtils.GetCurrencyString(entry.amount)} payée par {payedBy}, {ower} rembourse {entry.owedPercentage * 100}%";
            }
            sharedText.text = sharedString;
        }

        private void UpdateDebtsText()
        {
            string debtsString = "";
            if (entry.Shared)
            {
                string owedAmount = BookkeepingUtils.GetCurrencyString(Mathf.Abs(entry.DebtAmount));
                debtsString = entry.owedByMe ? "Je dois %%amount%% à %%person%%" : "%%person%% me doit %%amount%%";
                debtsString = debtsString.Replace("%%amount%%", owedAmount);
                debtsString = debtsString.Replace("%%person%%", entry.sharedWith.name);
                if (entry.repaid) debtsString = "<s>" + debtsString + "</s>";
            }
            debtsText.text = debtsString;
        }

        private void InitRepaidToggle()
        {
            repaidToggle.gameObject.SetActive(entry.Shared);
            repaidToggle.isOn = entry.repaid;
            repaidToggle.onValueChanged.AddListener(OnRepaidToggleValueChanged);
        }

        private void OnRepaidToggleValueChanged(bool value)
        {
            entry.repaid = value;
            UpdateDebtsText();
            OnRepaidChanged?.Invoke(entry);
        }
    }
}