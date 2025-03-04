using System;
using UnityEngine;

[Serializable]
public class CameraResetInformation
{
}

public class CameraResetCommand : ICommand
{
    private const string MessageFormat = "\nplease convert this by only this json format. cameraReset: reset all state.";

    // 초기화할 각 컨트롤러에 대한 참조
    private CameraRotationController rotationController;
    private CameraFovController fovController;
    private CameraOrthoZoomController orthoZoomController; // 만약 Orthographic용이 있다면

    private CameraResetInformation resetInfo;

    public CameraResetCommand(CameraRotationController rotCtrl, CameraFovController fovCtrl, CameraOrthoZoomController orthoCtrl = null)
    {
        rotationController = rotCtrl;
        fovController = fovCtrl;
        orthoZoomController = orthoCtrl;
        resetInfo = new CameraResetInformation();
        CommandSystemManager.instance.RegisterCommand("CameraReset", this);
    }

    public void Execute(string output)
    {
        // output JSON은 필요 없을 수도 있지만, 만약 확장이 필요하면 여기서 파싱할 수 있습니다.
        if (rotationController != null)
        {
            rotationController.ResetRotationState();
        }
        if (fovController != null)
        {
            fovController.ResetFovState();
        }
        if (orthoZoomController != null)
        {
            orthoZoomController.ResetCameraState();
        }
        Debug.Log("Camera reset command executed.");
    }

    public string GetInputFormat()
    {
        return MessageFormat + JsonUtility.ToJson(resetInfo);
    }
}
