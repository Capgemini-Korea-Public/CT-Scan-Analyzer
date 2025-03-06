using System;
using UnityEngine;

[Serializable]
public class CameraResetInformation
{
}

public class CameraResetCommand : ICommand
{
    private const string MessageFormat = "\nplease convert this by only this json format. cameraReset: reset all state.";

    // �ʱ�ȭ�� �� ��Ʈ�ѷ��� ���� ����
    private CameraRotationController rotationController;
    private CameraFovController fovController;
    private CameraOrthoZoomController orthoZoomController; // ���� Orthographic���� �ִٸ�

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
        // output JSON�� �ʿ� ���� ���� ������, ���� Ȯ���� �ʿ��ϸ� ���⼭ �Ľ��� �� �ֽ��ϴ�.
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
        
        string printText = $"The camera has been reset!";
        CommandSystemManager.instance.OnUIUpdate(printText);
        Debug.Log("Camera reset command executed.");
    }

    public string GetInputFormat()
    {
        return MessageFormat + JsonUtility.ToJson(resetInfo);
    }
}
