using UnityEngine;
using UnityEditor;

public static class TextureConverter
{
    /// <summary>
    /// ������ Texture2D�� ����Ʈ ������ �����մϴ�.
    /// - isReadable�� true�� ����
    /// - textureCompression�� Uncompressed(���� ����)�� ����
    /// </summary>
    /// <param name="texture">������ Texture2D ����</param>
    public static void ConvertTexture(Texture2D texture)
    {
        // ���� ��� ���
        string path = AssetDatabase.GetAssetPath(texture);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("�ش� Texture2D�� ���� ��θ� ã�� �� �����ϴ�.");
            return;
        }

        // TextureImporter ��������
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        if (textureImporter == null)
        {
            Debug.LogError("TextureImporter�� �������� ���߽��ϴ�: " + path);
            return;
        }

        // ���� ����
        textureImporter.isReadable = true;
        textureImporter.textureCompression = TextureImporterCompression.Uncompressed;

        // ���� ���� ���� (���� �ٽ� ����Ʈ)
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        Debug.Log("Texture ��ȯ �Ϸ�: " + path);
    }
}