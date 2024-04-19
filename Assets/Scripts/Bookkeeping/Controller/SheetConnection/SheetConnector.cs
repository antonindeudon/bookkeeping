using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using UnityEngine;

namespace Bookkeeping
{
    public class SheetConnector
    {
        private SheetsService service;
        private ConnectionData connectionData;

        public SheetConnector(ConnectionData connectionData)
        {
            this.connectionData = connectionData;
            InitSheetsService();
        }

        private void InitSheetsService()
        {
            try
            {
                string fullJsonPath = Path.Combine(Application.dataPath, connectionData.CredentialsFilePath);
                Stream jsonCredentials = File.Open(fullJsonPath, FileMode.Open);
                ServiceAccountCredential credential = ServiceAccountCredential.FromServiceAccountData(jsonCredentials);
                service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential
                });
            }
            catch(Exception exception)
            {
                throw new ConnectionException("Cannot init sheets service", exception);
            }
        }

        public IList<IList<object>> GetSheetRange(string sheetNameAndRange)
        {
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(connectionData.SpreadsheetId, sheetNameAndRange);
            try
            {
                ValueRange response = request.Execute();

                IList<IList<object>> values = response.Values;
                if (values != null && values.Count > 0)
                {
                    return values;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception exception)
            {
                throw new ConnectionException("Failed executing request to retrieve data", exception);
            }
        }

        public void WriteData(string range, IList<IList<object>> values)
        {
            ValueRange valueRange = new ValueRange();
            valueRange.Range = range;
            valueRange.Values = values;

            List<ValueRange> dataToWrite = new List<ValueRange>();
            dataToWrite.Add(valueRange);

            BatchUpdateValuesRequest requestBody = new BatchUpdateValuesRequest();
            requestBody.ValueInputOption = "USER_ENTERED";
            requestBody.Data = dataToWrite;

            SpreadsheetsResource.ValuesResource.BatchUpdateRequest request = service.Spreadsheets.Values.BatchUpdate(requestBody, connectionData.SpreadsheetId);
            try
            {
                BatchUpdateValuesResponse response = request.Execute();
            }
            catch(HttpRequestException exception)
            {
                throw new ConnectionException("Failed executing request to save data", exception);
            }
        }
    }
}