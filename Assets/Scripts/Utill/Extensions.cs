using UnityEngine;
using UnityEditor;

public static class TextureConverter
{
    /// <summary>
    /// 지정한 Texture2D의 임포트 설정을 변경합니다.
    /// - isReadable를 true로 설정
    /// - textureCompression을 Uncompressed(압축 없음)로 설정
    /// </summary>
    /// <param name="texture">변경할 Texture2D 에셋</param>
    public static void ConvertTexture(Texture2D texture)
    {
        // 에셋 경로 얻기
        string path = AssetDatabase.GetAssetPath(texture);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("해당 Texture2D의 에셋 경로를 찾을 수 없습니다.");
            return;
        }

        // TextureImporter 가져오기
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        if (textureImporter == null)
        {
            Debug.LogError("TextureImporter를 가져오지 못했습니다: " + path);
            return;
        }

        // 설정 변경
        textureImporter.isReadable = true;
        textureImporter.textureCompression = TextureImporterCompression.Uncompressed;

        // 변경 사항 적용 (에셋 다시 임포트)
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        Debug.Log("Texture 변환 완료: " + path);
    }
}