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
            rotationController.RotateByCommand("left");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rotationController.RotateByCommand("right");
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rotationController.RotateByCommand("up");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            rotationController.RotateByCommand("down");
        }
    }
}

