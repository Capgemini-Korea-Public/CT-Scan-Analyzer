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
        // RenderTexture를 생성하여 카메라의 출력을 저장할 준비
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraToCapture.targetTexture = renderTexture;

        // 텍스처를 캡처한 후, 다시 카메라의 출력 대상을 원래대로 복원
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        cameraToCapture.Render();

        // 캡처된 텍스처를 2D 텍스처로 변환
        Texture2D capturedImage = new Texture2D(Screen.width, Screen.height);
        capturedImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        capturedImage.Apply();

        // 텍스처를 PNG 파일로 저장
        byte[] bytes = capturedImage.EncodeToPNG();
        System.IO.File.WriteAllBytes(savePath, bytes);

        // 원래 상태로 복원
        RenderTexture.active = currentRT;
        cameraToCapture.targetTexture = null;

        Debug.Log("Captured image saved to " + savePath);
    }
}
