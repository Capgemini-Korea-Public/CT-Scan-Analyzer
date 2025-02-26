using UnityEngine;

public class TestCameraRotation : MonoBehaviour
{
    public CameraRotationController rotationController;

    void Update()
    {
        if (rotationController == null) return;

        // ����Ű �Է� �� �׽�Ʈ
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

