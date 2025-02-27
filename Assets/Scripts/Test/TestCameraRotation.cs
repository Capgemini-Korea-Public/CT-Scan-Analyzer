using UnityEngine;

public class TestCameraRotation : MonoBehaviour
{
    public CameraRotationController rotationController;

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
    }
}

