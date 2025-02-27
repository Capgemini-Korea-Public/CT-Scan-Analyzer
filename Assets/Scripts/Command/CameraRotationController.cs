using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(VTKCamera))]
public class CameraRotationController : MonoBehaviour
{
    public Transform target;

    [Header("한 번 회전할 때 설정")]
    [Tooltip("한 번 명령 시 회전할 기본 각도 (도)")]
    public float defaultAngle = 90f;
    [Tooltip("단일 회전에 걸리는 시간 (초)")]
    public float rotationDuration = 1f;

    [Header("연속 회전 설정")]
    [Tooltip("연속 회전 시 초당 회전 속도 (도)")]
    public float continuousRotationSpeed = 30f;

    // 내부 상태 플래그
    private bool isRotating = false;
    private Coroutine continuousRotationCoroutine;

    public VTKCamera vtkCamera;

    private void Start()
    {
        vtkCamera = GetComponent<VTKCamera>();
        IntPtr renderWindowHandle = vtkCamera.GetRenderWindowHandle();

        SetTargetToVolumeCenter(renderWindowHandle);
    }

    public void SetTargetToVolumeCenter(IntPtr renderWindowHandle)
    {
        Vector3 origin = new Vector3(0, 0, 0);
        Vector3 spacing = new Vector3(1, 1, 2);
        int xmin = 0, xmax = 255, ymin = 0, ymax = 255, zmin = 0, zmax = 93;

        // 인덱스 기준으로 중간값 계산 후, Spacing 반영
        float centerX = origin.x + ((xmax - xmin) * 0.5f * spacing.x); // 0 + (255 * 0.5) = 127.5
        float centerY = origin.y + ((ymax - ymin) * 0.5f * spacing.y); // 127.5
        float centerZ = origin.z + ((zmax - zmin) * 0.5f * spacing.z); // 0 + (93 * 0.5 * 2) = 93

        Vector3 volumeCenter = new Vector3(centerX, centerY, centerZ);

        // 필요 시 오프셋을 추가해 머리의 정확한 중심에 맞출 수 있음.
        // 예: volumeCenter.y -= offset;

        if (target != null)
        {
            target.position = volumeCenter;
        }
        else
        {
            Debug.LogWarning("타겟이 설정되어 있지 않습니다. 빈 오브젝트를 생성하여 target으로 할당하세요.");
        }
    }

    /// <summary>
    /// 회전 명령을 실행합니다.
    /// - angle 매개변수가 있으면 그 각도만큼 부드럽게 회전합니다.
    /// - angle 매개변수가 없으면 연속 회전 모드로 동작합니다.
    /// </summary>
    /// <param name="direction">"left", "right", "up", "down"</param>
    /// <param name="angle">회전할 각도(도). null이면 연속 회전</param>
    public void RotateSmooth(string direction, float? angle = null)
    {
        if (!angle.HasValue)
        {
            if (continuousRotationCoroutine == null)
            {
                continuousRotationCoroutine = StartCoroutine(ContinuousRotation(direction));
            }
        }
        else
        {
            if (continuousRotationCoroutine != null)
            {
                StopCoroutine(continuousRotationCoroutine);
                continuousRotationCoroutine = null;
            }
            if (isRotating) return;

            float rotationAngle = angle.Value;
            Vector3 axis = Vector3.up;

            switch (direction.ToLower())
            {
                case "left":
                    axis = Vector3.up;
                    rotationAngle = Mathf.Abs(rotationAngle);
                    break;
                case "right":
                    axis = Vector3.up;
                    rotationAngle = -Mathf.Abs(rotationAngle);
                    break;
                case "up":
                    axis = transform.right;
                    rotationAngle = Mathf.Abs(rotationAngle);
                    break;
                case "down":
                    axis = transform.right;
                    rotationAngle = -Mathf.Abs(rotationAngle);
                    break;
                default:
                    Debug.LogWarning("알 수 없는 회전 명령: " + direction);
                    return;
            }
            StartCoroutine(RotateCoroutine(rotationAngle, axis));
        }
    }
    IEnumerator RotateCoroutine(float rotationAngle, Vector3 axis)
    {
        isRotating = true;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.AngleAxis(rotationAngle, axis) * startRot;

        float t = 0f;
        while (t < rotationDuration)
        {
            t += Time.deltaTime;
            float progress = t / rotationDuration;
            transform.rotation = Quaternion.Slerp(startRot, endRot, progress);
            if (target != null)
            {
                transform.LookAt(target.position);
            }
            yield return null;
        }
        transform.rotation = endRot;
        if (target != null)
        {
            transform.LookAt(target.position);
        }
        isRotating = false;
    }

    IEnumerator ContinuousRotation(string direction)
    {
        Vector3 axis = Vector3.up;
        float sign = 1f;
        switch (direction.ToLower())
        {
            case "left":
                axis = Vector3.up;
                sign = 1f;
                break;
            case "right":
                axis = Vector3.up;
                sign = -1f;
                break;
            case "up":
                axis = transform.right;
                sign = 1f;
                break;
            case "down":
                axis = transform.right;
                sign = -1f;
                break;
            default:
                Debug.LogWarning("알 수 없는 연속 회전 명령: " + direction);
                yield break;
        }

        while (true)
        {
            float angleThisFrame = continuousRotationSpeed * Time.deltaTime * sign;
            Quaternion deltaRotation = Quaternion.AngleAxis(angleThisFrame, axis);

            // 타겟과의 상대 위치(오프셋)를 구한 후 회전
            Vector3 offset = transform.position - target.position;
            offset = deltaRotation * offset;
            transform.position = target.position + offset;

            // 카메라의 up 벡터가 급격히 바뀌지 않도록, LookAt 호출 시 월드 up (Vector3.up) 고정
            transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);

            yield return null;
        }
    }

    public void StopContinuousRotation()
    {
        if (continuousRotationCoroutine != null)
        {
            StopCoroutine(continuousRotationCoroutine);
            continuousRotationCoroutine = null;
        }
    }
}

