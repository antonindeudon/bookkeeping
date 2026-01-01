using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace Bookkeeping
{
    public class BookkeepingController : MonoBehaviour
    {
        public string downloadDataMessage;
        public string savingDataMessage;
        public string applySettingsString;
        public string connectString;

        public ConnectionController connectionController;
        public FilteringController filteringController;
        public BookkeepingUI ui;
        public SettingsPanel settingsPanel;
        
        private ConnectionData connectionData;
        private Data data;
        private RuntimeData runtimeData;
        private SheetConnector sheetConnector;
        private bool dirty;


        void Start()
        {
            ui.gameObject.SetActive(true);
            connectionController.connectionPanel.gameObject.SetActive(true);

            connectionData = new ConnectionData();
            settingsPanel.Init(connectionData);

            if (connectionData.Valid)
            {
                TryDownloadingData();
            }
            else
            {
                settingsPanel.Open(connectString, TryDownloadingData);
            }
        }

        private void TryDownloadingData()
        {
            connectionController.TryConnecting(downloadDataMessage, DownloadData);
        }

        private void DownloadData()
        { 
            data = DataLoader.LoadData(connectionData);
            runtimeData = new RuntimeData();
            runtimeData.Init(data);
            filteringController.Init(data);
            filteringController.onFiltersUpdated += OnFiltersUpdated;
            UpdateEntries();
            ui.OnRepaidChangedEvent += OnRepaidChanged;
            sheetConnector = new SheetConnector(connectionData);
        }

        private void OnFiltersUpdated()
        {
            UpdateEntries();
        }

        private void UpdateEntries()
        {
            ui.SetEntries(runtimeData.rootCategories, filteringController);
        }

        private void OnRepaidChanged(Entry entry)
        {
            dirty = true;
        }

        private void Save()
        {
            IList<IList<object>> values = new List<IList<object>>();
            foreach (Entry entry in data.entries)
            {
                IList<object> rowValues = new List<object>();
                rowValues.Add(entry.repaid ? "TRUE" : "FALSE");
                values.Add(rowValues);
            }

            string[] sheetNames = connectionData.EntriesSheets.Split(',');
            foreach(string sheetName in sheetNames)
            {
                string range = "'"+sheetName+"'!I2:I" + (values.Count + 1);
                sheetConnector.WriteData(range, values);
            }

            Debug.Log("Save complete!");

            ExitWithoutSaving();
        }

        public void Exit()
        {
            if (dirty)
            {
                connectionController.TryConnecting(savingDataMessage, Save);
            }
            else
            {
                ExitWithoutSaving();
            }
        }

        public void ExitWithoutSaving()
        {
            Utility.QuitApplication();
        }

        public void OpenSettings()
        {
            settingsPanel.Open(applySettingsString);
        }
    }
}