using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public CameraRotationController rotationController;
    public CameraFovController fovController;
    public CameraOrthoZoomController orthoZoomController;

    void Update()
    {
        if (rotationController == null) return;

        // ����Ű �Է� �� �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rotationController.RotateSmooth("left");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rotationController.RotateSmooth("right");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rotationController.RotateSmooth("up");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            rotationController.RotateSmooth("down");
        }

        // Z Ű�� ������ �� �� (�⺻ 10% Ȯ��)
        if (Input.GetKeyDown(KeyCode.Z))
        {
            fovController.AdjustFov("zoom in");  // percent �Ű������� �����ϸ� �⺻ 10% ����
        }

        // X Ű�� ������ �� �ƿ� (�⺻ 10% ���)
        if (Input.GetKeyDown(KeyCode.X))
        {
            fovController.AdjustFov("zoom out"); // percent �Ű������� �����ϸ� �⺻ 10% ����
        }

        // C Ű�� ������ �� �� (�⺻ 10% Ȯ��)
        if (Input.GetKeyDown(KeyCode.C))
        {
            orthoZoomController.AdjustOrthoZoom("zoom in");
        }

        // V Ű�� ������ �� �ƿ� (�⺻ 10% ���)
        if (Input.GetKeyDown(KeyCode.V))
        {
            orthoZoomController.AdjustOrthoZoom("zoom out");
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            AllReset();
        }
    }

    public void AllReset()
    {
        rotationController.ResetRotationState();
        fovController.ResetFovState();
        orthoZoomController.ResetCameraState();
    }
}

