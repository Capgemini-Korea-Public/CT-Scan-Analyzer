using UnityEngine;
using System.IO;
using System;
using System.Threading.Tasks;

public class CameraCaptureSystem : MonoBehaviour
{
    private static CameraCaptureSystem instance;
    public static CameraCaptureSystem Instance => instance;

    public Camera CaptureCamera;
    public string CapturedImageSavePath;
    public event Action<string> OnLLMResponseUpdated;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }    
    }

    private void Start()
    {
        CaptureCamera = GameObject.FindGameObjectWithTag("SliceCamera").GetComponent<Camera>();
        CapturedImageSavePath = Path.Combine(Application.persistentDataPath, "CapturedImage.png");
    }


    [ContextMenu("Capture")]
    public void Capture()
    {
        int width = Screen.width; 
        int height = Screen.height;

        float originalAspect = CaptureCamera.aspect;
        CaptureCamera.aspect = (float)width / height;

        RenderTexture rt = new RenderTexture(width, height, 24);
        CaptureCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);

        CaptureCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        CaptureCamera.aspect = originalAspect;
        CaptureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToJPG(80); 
        File.WriteAllBytes(CapturedImageSavePath, bytes);

        Debug.Log("Screenshot saved: " + CapturedImageSavePath);
    }
}
