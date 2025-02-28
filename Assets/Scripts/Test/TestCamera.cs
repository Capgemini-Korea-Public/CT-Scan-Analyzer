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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            // �� ��: ���� FOV�� 30% ���Ҹ� �ε巴�� ����
            fovController.SmoothZoom("zoom in", 30f);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            // �� �ƿ�: ���� FOV�� 30% ������ �ε巴�� ����
            fovController.SmoothZoom("zoom out", 30f);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            // �� ��: 30% ��ȭ (�ε巴�� ����)
            orthoZoomController.AdjustOrthoZoom("zoom in", 30f);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            // �� �ƿ�: 30% ��ȭ (�ε巴�� ����)
            orthoZoomController.AdjustOrthoZoom("zoom out", 30f);
        }

        if (Input.GetKeyDown(KeyCode.R))
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

