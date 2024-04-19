using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Bookkeeping
{
    public static class BookkeepingUtils
    {
        private static readonly Color LOSS_COLOR = Color.red;
        private static readonly Color GAIN_COLOR = Color.green;
        private static readonly Color NEUTRAL_COLOR = Color.gray;

        public static void SetAmountText(TextMeshProUGUI text, float amount)
        {
            text.text = GetCurrencyString(amount);
            text.color = amount > 0 ? LOSS_COLOR : (amount < 0 ? GAIN_COLOR : NEUTRAL_COLOR);
        }

        public static string GetCurrencyString(float amount)
        {
            return amount.ToString("n2") + "€";
        }
    }
}