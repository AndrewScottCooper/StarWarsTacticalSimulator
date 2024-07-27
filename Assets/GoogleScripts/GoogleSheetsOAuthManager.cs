using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using UnityEngine;

public class GoogleSheetsOAuthManager : MonoBehaviour
{
    static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
    static readonly string ApplicationName = "Unity Google Sheets OAuth";
    SheetsService service;
    public GameObject planetUIPrefab;
    public Transform contentParent; // The parent object to hold UI elements

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
        string sheetName = GetSheetNameByIndex(spreadsheetId, 8);
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
            List<Planet> planets = new List<Planet>();
            for (int i = 1; i < values.Count; i++) // Start at 1 to skip the header row
            {
                IList<object> row = values[i];
                if (row.Count < 14)
                {
                    Debug.LogWarning($"Row {i} does not have enough columns.");
                    continue;
                }

                Planet planet = new Planet
                {
                    PlanetName = row[0].ToString(),
                    Character = row[1].ToString(),
                    Specialisation = row[2].ToString(),
                    Population = ParseFloat(row[3].ToString()),
                    GDP = ParseFloat(row[4].ToString()),
                    SectorTax = ParseFloat(row[5].ToString()),
                    BaseCredits = ParseFloat(row[6].ToString()),
                    MilitaryCapacity = ParseFloat(row[7].ToString()),
                    Stability = ParseFloat(row[8].ToString()),
                    NetCredits = ParseFloat(row[9].ToString()),
                    TradeRoutes = row[10].ToString(),
                    ShipbuildingSlots = ParseFloat(row[11].ToString()),
                    ArmySlots = ParseFloat(row[12].ToString()),
                    PlanetaryModifiers = ParseModifiers(row[13].ToString())
                };
                planets.Add(planet);
            }

            // Display the data in the UI
            foreach (Planet planet in planets)
            {
                CreateUIElement(planet);
            }
        }
        else
        {
            Debug.Log("No data found.");
        }
    }

    float ParseFloat(string input)
    {
        float result;
        if (float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
        {
            return result;
        }
        else
        {
            Debug.LogWarning($"Unable to parse '{input}' as float.");
            return 0f; // Default value if parsing fails
        }
    }

    List<Modifier> ParseModifiers(string modifiersString)
    {
        List<Modifier> modifiers = new List<Modifier>();
        string[] modifierArray = modifiersString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string modifierName in modifierArray)
        {
            Modifier modifier = new Modifier { Name = modifierName.Trim() };
            modifiers.Add(modifier);
        }
        return modifiers;
    }

    void CreateUIElement(Planet planet)
    {
        GameObject newUIElement = Instantiate(planetUIPrefab, contentParent);
        // Assuming you have a script to set up the UI element with the planet data
        PlanetUI uiScript = newUIElement.GetComponent<PlanetUI>();
        uiScript.Setup(planet);
    }
}