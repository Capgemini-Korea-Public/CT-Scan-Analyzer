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

        // Z 키를 누르면 줌 인 (기본 10% 확대)
        if (Input.GetKeyDown(KeyCode.Z))
        {
            fovController.AdjustFov("zoom in");  // percent 매개변수를 생략하면 기본 10% 적용
        }

        // X 키를 누르면 줌 아웃 (기본 10% 축소)
        if (Input.GetKeyDown(KeyCode.X))
        {
            fovController.AdjustFov("zoom out"); // percent 매개변수를 생략하면 기본 10% 적용
        }

        // C 키를 누르면 줌 인 (기본 10% 확대)
        if (Input.GetKeyDown(KeyCode.C))
        {
            orthoZoomController.AdjustOrthoZoom("zoom in");
        }

        // V 키를 누르면 줌 아웃 (기본 10% 축소)
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

