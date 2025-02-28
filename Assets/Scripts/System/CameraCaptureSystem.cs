using UnityEngine;
using System.IO;

public class CameraCaptureSystem :MonoBehaviour
{
    public Camera cameraToCapture;
    public string savePath;

    private void Start()
    {
        savePath = Application.persistentDataPath;
    }

    [ContextMenu("Capture")]
    public void Capture()
    {
        // RenderTexture�� �����Ͽ� ī�޶��� ����� ������ �غ�
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraToCapture.targetTexture = renderTexture;

        // �ؽ�ó�� ĸó�� ��, �ٽ� ī�޶��� ��� ����� ������� ����
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        cameraToCapture.Render();

        // ĸó�� �ؽ�ó�� 2D �ؽ�ó�� ��ȯ
        Texture2D capturedImage = new Texture2D(Screen.width, Screen.height);
        capturedImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        capturedImage.Apply();

        // �ؽ�ó�� PNG ���Ϸ� ����
        byte[] bytes = capturedImage.EncodeToPNG();
        System.IO.File.WriteAllBytes(savePath, bytes);

        // ���� ���·� ����
        RenderTexture.active = currentRT;
        cameraToCapture.targetTexture = null;

        Debug.Log("Captured image saved to " + savePath);
    }
}
