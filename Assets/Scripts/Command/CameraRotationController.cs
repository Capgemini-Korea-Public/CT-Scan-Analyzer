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
            Debug.LogWarning("CameraRotationController: VTKCamera가 없습니다.");
            return;
        }

        if (target == null)
        {
            GameObject pivot = new GameObject("VolumeCenterPivot");
            target = pivot.transform;
        }

        // 씬의 VTK 오브젝트(볼륨 등)가 모두 초기화된 뒤에 호출되어야
        // 올바른 SceneBounds를 가져올 수 있습니다.
        SetTargetToVolumeCenter();
    }

    private void SetTargetToVolumeCenter()
    {
        IntPtr renderWindowHandle = vtkCam.GetRenderWindowHandle();
        if(renderWindowHandle == IntPtr.Zero)
        {
            Debug.LogWarning("렌더 윈도우 핸들을 가져오지 못했습니다.");
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
            Debug.LogWarning("CameraRotationController: 타겟이 설정되지 않았습니다.");
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
                Debug.LogWarning($"알 수 없는 명령어: {direction}");
                return;
        }

        // 회전 후 타겟을 바라보도록
        transform.LookAt(target.position);
    }
}
