using System;
using UnityEngine;

[RequireComponent(typeof(VTKCamera))]
public class CameraRotationController : MonoBehaviour
{
    public Transform target;

    public float rotationAngle = 15f;

    private VTKCamera vtkCam;

    private void Start()
    {
        vtkCam = GetComponent<VTKCamera>();
        if(vtkCam == null)
        {
            Debug.LogWarning("CameraRotationController: VTKCamera�� �����ϴ�.");
            return;
        }

        if (target == null)
        {
            GameObject pivot = new GameObject("VolumeCenterPivot");
            target = pivot.transform;
        }

        // ���� VTK ������Ʈ(���� ��)�� ��� �ʱ�ȭ�� �ڿ� ȣ��Ǿ��
        // �ùٸ� SceneBounds�� ������ �� �ֽ��ϴ�.
        SetTargetToVolumeCenter();
    }

    private void SetTargetToVolumeCenter()
    {
        IntPtr renderWindowHandle = vtkCam.GetRenderWindowHandle();
        if(renderWindowHandle == IntPtr.Zero)
        {
            Debug.LogWarning("���� ������ �ڵ��� �������� ���߽��ϴ�.");
            return;
        }

        double[] bounds = new double[6];
        VTKUnityNativePluginLiteScene.GetSceneBounds(renderWindowHandle, ref bounds);

        // bounds[0,1,2,3,4,5] = xmin, xmax, ymin, ymax, zmin, zmax
        float xCenter = 0.5f * (float)(bounds[0] + bounds[1]);
        float yCenter = 0.5f * (float)(bounds[2] + bounds[3]);
        float zCenter = 0.5f * (float)(bounds[4] + bounds[5]);

        Vector3 volumeCenter = new Vector3(xCenter, yCenter, zCenter);

        target.position = volumeCenter;
    }

    public void RotateByCommand(string direction)
    {
        if (target == null)
        {
            Debug.LogWarning("CameraRotationController: Ÿ���� �������� �ʾҽ��ϴ�.");
            return;
        }

        switch (direction.ToLower())
        {
            case "left":
                transform.RotateAround(target.position, Vector3.up, rotationAngle);
                break;
            case "right":
                transform.RotateAround(target.position, Vector3.up, -rotationAngle);
                break;
            case "up":
                transform.RotateAround(target.position, transform.right, rotationAngle);
                break;
            case "down":
                transform.RotateAround(target.position, transform.right, -rotationAngle);
                break;
            default:
                Debug.LogWarning($"�� �� ���� ��ɾ�: {direction}");
                return;
        }

        // ȸ�� �� Ÿ���� �ٶ󺸵���
        transform.LookAt(target.position);
    }
}
