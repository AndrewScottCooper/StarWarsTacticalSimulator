using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class GoogleSheetsOAuthManager : MonoBehaviour
{
    static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
    static readonly string ApplicationName = "Unity Google Sheets OAuth";
    SheetsService service;

    void Start()
    {
        StartCoroutine(AuthenticateAndAccessGoogleSheets());
    }

    IEnumerator AuthenticateAndAccessGoogleSheets()
    {
        string clientSecretsPath = Application.streamingAssetsPath + "/client_secret.json";
        UserCredential credential;

        using (var stream = new FileStream(clientSecretsPath, FileMode.Open, FileAccess.Read))
        {
            string credPath = "token.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
        }

        service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        //Reading the 5th sheet (Galatic planet stats) (index 4)
        string spreadsheetId = "11HFEG9JwHWMdfLer6Kj9g8Y2s-mQgDHvvEDrwDhJxJQ";
        string sheetName = GetSheetNameByIndex(spreadsheetId, 7);
        if (!string.IsNullOrEmpty(sheetName))
        {
            ReadSheetData(spreadsheetId, sheetName);
        }

        yield return null;
    }

    string GetSheetNameByIndex(string spreadsheetId, int sheetIndex)
    {
        var request = service.Spreadsheets.Get(spreadsheetId);
        Spreadsheet spreadsheet = request.Execute();
        if (spreadsheet.Sheets.Count > sheetIndex)
        {
            return spreadsheet.Sheets[sheetIndex].Properties.Title;
        }
        else
        {
            Debug.LogError($"Sheet index {sheetIndex} is out of range.");
            return null;
        }
    }

    void ReadSheetData(string spreadsheetId, string sheetName)
    {
        var request = service.Spreadsheets.Values.Get(spreadsheetId, sheetName);
        ValueRange response = request.Execute();
        IList<IList<object>> values = response.Values;

        if (values != null && values.Count > 0)
        {
            foreach (var row in values)
            {
                Debug.Log(string.Join(", ", row));
            }
        }
        else
        {
            Debug.Log("No data found.");
        }
    }
}