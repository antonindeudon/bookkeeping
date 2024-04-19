using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

namespace Bookkeeping
{
    public class CategoryUI : MonoBehaviour
    {
        public Toggle toggle;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI totalAmountText;
        public Transform childrenContainer;

        private bool open;

        private void Awake()
        {
            Open = childrenContainer.gameObject.activeSelf;
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool value)
        {
            Open = value;
        }

        public void SetBasicData(RuntimeCategoryData category, bool open)
        {
            nameText.text = category.Category.name;
            toggle.isOn = open;
        }

        public void SetTotalAmount(float totalAmount)
        {
            BookkeepingUtils.SetAmountText(totalAmountText, totalAmount);
        }

        private bool Open
        {
            get => open;
            set
            {
                if (value != open)
                {
                    open = value;
                    childrenContainer.gameObject.SetActive(open);
                }
            }
        }
    }
}