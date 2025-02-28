using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public CameraRotationController rotationController;
    public CameraFovController fovController;
    public CameraOrthoZoomController orthoZoomController;

    void Update()
    {
        if (rotationController == null) return;

        // 방향키 입력 시 테스트
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
            // 줌 인: 현재 FOV의 30% 감소를 부드럽게 적용
            fovController.SmoothZoom("zoom in", 30f);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            // 줌 아웃: 현재 FOV의 30% 증가를 부드럽게 적용
            fovController.SmoothZoom("zoom out", 30f);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            // 줌 인: 30% 변화 (부드럽게 적용)
            orthoZoomController.AdjustOrthoZoom("zoom in", 30f);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            // 줌 아웃: 30% 변화 (부드럽게 적용)
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

