using UnityEngine;
using UnityEditor;

namespace SpeechToTextUnity
{
    public static class FileSelector
    {
        #if UNITY_EDITOR
        public static string FileSelect()
        {
            string filePath = EditorUtility.OpenFilePanel("Select File", "", "");
            if (!string.IsNullOrEmpty(filePath))
            {
                return filePath;
            }
            else
            {
                Debug.LogWarning("Invalid File Path");
                return null;
            }
        }
        #endif
    }
}

