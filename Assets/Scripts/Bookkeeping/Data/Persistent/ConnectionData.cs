using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bookkeeping
{
    public class ConnectionData
    {
        private string spreadsheetId;
        private string credentialsFilePath;
        private string categoriesRange;
        private string entriesRange;

        public ConnectionData()
        {
            LoadFromPlayerPrefs();
        }

        private void LoadFromPlayerPrefs()
        {
            spreadsheetId = PlayerPrefs.GetString("spreadsheetId", "");
            credentialsFilePath = PlayerPrefs.GetString("credentialsFilePath", "");
            categoriesRange = PlayerPrefs.GetString("categoriesRange", "'Cat�gories'!$A$2:$B$1000");
            entriesRange = PlayerPrefs.GetString("entriesRange", "'Entr�es'!$A$2:$J$1000");
        }

        public string SpreadsheetId
        {
            get => spreadsheetId;
            set
            {
                spreadsheetId = value;
                PlayerPrefs.SetString("spreadsheetId", spreadsheetId);
                PlayerPrefs.Save();
            }
        }

        public string CredentialsFilePath
        {
            get => credentialsFilePath;
            set
            {
                credentialsFilePath = value;
                PlayerPrefs.SetString("credentialsFilePath", credentialsFilePath);
                PlayerPrefs.Save();
            }
        }

        public string CategoriesRange
        {
            get => categoriesRange;
            set
            {
                categoriesRange = value;
                PlayerPrefs.SetString("categoriesRange", categoriesRange);
                PlayerPrefs.Save();
            }
        }

        public string EntriesRange
        {
            get => entriesRange;
            set
            {
                entriesRange = value;
                PlayerPrefs.SetString("entriesRange", entriesRange);
                PlayerPrefs.Save();
            }
        }

        public bool Valid => !string.IsNullOrEmpty(spreadsheetId) && !string.IsNullOrEmpty(credentialsFilePath) && !string.IsNullOrEmpty(categoriesRange) && !string.IsNullOrEmpty(entriesRange);
    }
}