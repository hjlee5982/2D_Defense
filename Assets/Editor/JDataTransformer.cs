using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using ExcelDataReader;
using System.Data;
using Unity.Plastic.Newtonsoft.Json;

public class JDataTransformer : EditorWindow
{
#if UNITY_EDITOR
    [MenuItem("Data/ExcelToJson")]
    public static void TestFunc()
    {
        ParseExcelDataToJson<AllyUnitDataLoader,    AllyUnitData   >("AllyUnitData");
        ParseExcelDataToJson<MonsterUnitDataLoader, MonsterUnitData>("MonsterUnitData");
        ParseExcelDataToJson<StageDataLoader,       StageData      >("StageData");
        ParseExcelDataToJson<GameRuleDataLoader,    GameRuleData   >("GameRuleData");
        ParseExcelDataToJson<RouteDataLoader,       RouteData      >("RouteData");
        ParseExcelDataToJson<EnhancementDataLoader, EnhancementData>("EnhancementData");
        ParseExcelDataToJson<LocalizeDataLoader,    LocalizeData   >("Localizer");
        ParseExcelDataToJson<SettingDataLoader,     SettingData    >("Setting");

        Debug.Log("Change Success");
    }

    private static void ParseExcelDataToJson<Loader, LoaderData>(string fileName) where Loader : new() where LoaderData : new()
    {
        string fullPath = JPathManager.ExcelFilePath(fileName);

        if (File.Exists(fullPath) == false)
        {
            Debug.LogError("FilePath Error");
            return;
        }

        FileStream stream = File.Open(fullPath, FileMode.Open, FileAccess.Read);

        IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);

        DataSet result = reader.AsDataSet();
        DataTable table = result.Tables[0];

        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

        List<string> headers = new List<string>();

        for (int col = 0; col < table.Columns.Count; col++)
        {
            headers.Add(table.Rows[0][col].ToString());
        }
        for (int row = 1; row < table.Rows.Count; row++)
        {
            Dictionary<string, object> rowData = new Dictionary<string, object>();

            for (int col = 0; col < table.Columns.Count; col++)
            {
                rowData[headers[col]] = table.Rows[row][col].ToString();
            }
            data.Add(rowData);
        }

        reader.Close();

        string json = JsonConvert.SerializeObject(new { Items = data }, Formatting.Indented);

        File.WriteAllText(JPathManager.JsonFilePath(fileName), json);
    }
#endif
}
