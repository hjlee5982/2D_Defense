using UnityEngine;

public static class JPathManager
{
    #region FILE_PATH
    private static readonly string _excelFilePath = $"{Application.dataPath}/Refactoring/06_Data/Excel/";
    private static readonly string _jsonFilePath  = $"{Application.dataPath}/Refactoring/06_Data/Json/";
    #endregion





    #region FUNCTIONS
    public static string ExcelFilePath(string fileName)
    {
        return $"{_excelFilePath}/{fileName}.xlsx";
    }

    public static string JsonFilePath(string fileName)
    {
        return $"{_jsonFilePath}/{fileName}.json";
    }
    #endregion
}
