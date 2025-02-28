using UnityEngine;
using UnityEditor;

namespace SpeechToTextUnity
{
    public static class FileSelector
    {
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

        //public static string[] FileSelectMultiple()
        //{
        //    string[] filePaths = EditorUtility.OpenFilePanelWithFilters("Select Files", "", new string[] { "All Files", "*.*" });
        //    if (filePaths.Length > 0)
        //    {
        //        return filePaths;
        //    }
        //    else
        //    {
        //        Debug.LogWarning("Invalid File Paths");
        //        return null;
        //    }
        //}
    }
}

