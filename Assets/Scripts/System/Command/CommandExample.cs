using System;
using UnityEngine;
public class CommandExample : MonoBehaviour
{
    public CameraRotationController rotationController;
    public CameraFovController fovController;             // Perspective 카메라용 줌
    public CameraOrthoZoomController orthoZoomController;

    private RotateCommand _rotateCommand;
    private ZoomCommand _zoomCommand;
    private CameraResetCommand _cameraResetCommand;
   
    private void Start()
    {
        _rotateCommand = new RotateCommand(rotationController.transform);
        _zoomCommand = new ZoomCommand(fovController);
        _cameraResetCommand = new CameraResetCommand(rotationController, fovController, orthoZoomController);
    }

    [ContextMenu("ROTATE")]
    public async void Test()
    {
        string str = "I want to turn the camera 60 degrees to the right" + CommandSystemManager.instance.GetInputFormat("Rotate");
        string output = await LLMModule.Instance.Chat(str);

        RotationInformation rotationInformation = JsonUtility.FromJson<RotationInformation>(output);
        Debug.Log($" {rotationInformation.angle} / {rotationInformation.direction}");
    }

    [ContextMenu("Test Zoom Command")]
    public async void TestZoom()
    {
        string input = "Please zoom in by 30 percent" + CommandSystemManager.instance.GetInputFormat("Zoom");
        string output = await LLMModule.Instance.Chat(input);

        ZoomInformation zoomInfo = JsonUtility.FromJson<ZoomInformation>(output);
        Debug.Log($"Zoom Command Test - cameraType: {zoomInfo.cameraType}, command: {zoomInfo.command}, percent: {zoomInfo.percent}");
    }

    [ContextMenu("Test Reset Command")]
    public async void TestReset()
    {
        string input = "Reset camera state" + CommandSystemManager.instance.GetInputFormat("CameraReset");
        string output = await LLMModule.Instance.Chat(input);
        Debug.Log("Camera Reset Command Test Output: " + output);
    }
}

