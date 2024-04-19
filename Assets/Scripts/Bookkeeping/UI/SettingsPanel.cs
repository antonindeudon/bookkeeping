using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bookkeeping
{
    public class SettingsPanel : MonoBehaviour
    {
        public TMP_InputField spreadsheetIdField;
        public TMP_InputField credentialsFilePathField;
        public TMP_InputField categoriesRangeField;
        public TMP_InputField entriesRangeField;

        public TextMeshProUGUI buttonText;

        private Action onClose;

        public void Init(ConnectionData connectionData)
        {
            spreadsheetIdField.text = connectionData.SpreadsheetId;
            credentialsFilePathField.text = connectionData.CredentialsFilePath;
            categoriesRangeField.text = connectionData.CategoriesRange;
            entriesRangeField.text = connectionData.EntriesRange;

            spreadsheetIdField.onValueChanged.AddListener(value => connectionData.SpreadsheetId = value);
            credentialsFilePathField.onValueChanged.AddListener(value => connectionData.CredentialsFilePath = value);
            categoriesRangeField.onValueChanged.AddListener(value => connectionData.CategoriesRange = value);
            entriesRangeField.onValueChanged.AddListener(value => connectionData.EntriesRange = value);
        }

        public void Open(string buttonString, Action onClose = null)
        {
            buttonText.text = buttonString;
            this.onClose = onClose;
            gameObject.SetActive(true);
        }

        public void Close()
        {
            onClose?.Invoke();
            gameObject.SetActive(false);
        }
    }
}