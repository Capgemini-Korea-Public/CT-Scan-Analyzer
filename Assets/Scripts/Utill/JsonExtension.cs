using System;
using UnityEngine;
using System.Text.RegularExpressions;

public static class JsonExtension 
{
    public static T DeserializeObject<T>(string json)
    {
        try
        {
            return JsonUtility.FromJson<T>(json);
        }
        catch (Exception)
        {
            return ParseJson<T>(json);
        }
    }

    private static T ParseJson<T>(string json)
    {
        try
        {
            Match match = Regex.Match(json, @"\{.*\}");
            if (match.Success)
            {
                json = match.Value;
            }  

            return JsonUtility.FromJson<T>(json);
        }
        catch (Exception e)
        {
            Debug.LogError($"JSON parsing Error : {e.Message}");
            return default;
        }
    }
}
