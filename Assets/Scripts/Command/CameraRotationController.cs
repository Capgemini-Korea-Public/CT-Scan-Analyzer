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

    // 외부에서 설정되는 연속 회전 명령 ("left", "right", "up", "down")
    private string currentContinuousDirection = "";

    public VTKCamera vtkCamera;

    // 초기 상태 저장용 변수
    private float initialYaw;
    private float initialPitch;
    private float initialDistance;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    // 누적 yaw와 pitch (단위: 도)
    private float yaw = 0f;
    private float pitch = 0f;

    private void Start()
    {
        vtkCamera = GetComponent<VTKCamera>();
        IntPtr renderWindowHandle = vtkCamera.GetRenderWindowHandle();
        SetTargetToVolumeCenter(renderWindowHandle);

        // 초기 오프셋 계산하여 yaw, pitch, distance 초기화
        Vector3 offset = transform.position - target.position;
        initialDistance = offset.magnitude;
        yaw = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg;
        pitch = Mathf.Asin(offset.y / initialDistance) * Mathf.Rad2Deg;
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        // 초기 상태 저장
        initialYaw = yaw;
        initialPitch = pitch;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void SetTargetToVolumeCenter(IntPtr renderWindowHandle)
    {
        Vector3 origin = Vector3.zero;
        Vector3 spacing = new Vector3(1, 1, 2);
        int xmin = 0, xmax = 255, ymin = 0, ymax = 255, zmin = 0, zmax = 93;

        // 인덱스 기준으로 중간값 계산 후, Spacing 반영
        float centerX = origin.x + ((xmax - xmin) * 0.5f * spacing.x); // 127.5
        float centerY = origin.y + ((ymax - ymin) * 0.5f * spacing.y); // 127.5
        float centerZ = origin.z + ((zmax - zmin) * 0.5f * spacing.z); // 93

        Vector3 volumeCenter = new Vector3(centerX, centerY, centerZ);

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
    /// - angle 매개변수가 있으면 단일 회전 모드로, 없으면 연속 회전 모드로 동작합니다.
    /// </summary>
    /// <param name="direction">"left", "right", "up", "down"</param>
    /// <param name="angle">회전할 각도(도). null이면 연속 회전</param>
    public void RotateSmooth(string direction, float? angle = null)
    {
        if (!angle.HasValue)
        {
            // 연속 회전: 새 방향 명령을 currentContinuousDirection에 저장
            SetRotationCommand(direction);
        }
        else
        {
            // 단일 회전 모드: 진행 중인 연속 회전이 있다면 중지
            if (continuousRotationCoroutine != null)
            {
                StopCoroutine(continuousRotationCoroutine);
                continuousRotationCoroutine = null;
                currentContinuousDirection = "";
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
                case "none":
                    axis = transform.right;
                    rotationAngle = -Mathf.Abs(rotationAngle);
                    Debug.LogWarning("사방 외 방향 명령으로 기본 값 적용");
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
                transform.LookAt(target.position, Vector3.up);
            }
            yield return null;
        }
        transform.rotation = endRot;
        if (target != null)
        {
            transform.LookAt(target.position, Vector3.up);
        }
        isRotating = false;
    }

    IEnumerator ContinuousRotation()
    {
        // 초기 오프셋 계산
        Vector3 offset = transform.position - target.position;
        float r = offset.magnitude;
        // φ: 폴라 각도, 0~π (0: target 바로 위, π: target 바로 아래)
        float phi = Mathf.Acos(offset.y / r);
        // θ: 아지무스 각도, XZ 평면에서의 각도 (기준: Z축)
        float theta = Mathf.Atan2(offset.x, offset.z);

        while (true)
        {
            // 만약 새 방향 명령이 들어오면 currentContinuousDirection가 업데이트되어 반영됩니다.
            switch (currentContinuousDirection)
            {
                case "left":
                    theta += continuousRotationSpeed * Time.deltaTime * Mathf.Deg2Rad;
                    break;
                case "right":
                    theta -= continuousRotationSpeed * Time.deltaTime * Mathf.Deg2Rad;
                    break;
                case "up":
                    phi -= continuousRotationSpeed * Time.deltaTime * Mathf.Deg2Rad;
                    break;
                case "down":
                    phi += continuousRotationSpeed * Time.deltaTime * Mathf.Deg2Rad;
                    break;
                default:
                    break;
            }

            // 클램핑: φ를 10° ~ 170° (라디안: 10°≈0.175, 170°≈2.967)
            float minPhi = 10f * Mathf.Deg2Rad;
            float maxPhi = 170f * Mathf.Deg2Rad;
            phi = Mathf.Clamp(phi, minPhi, maxPhi);

            // θ는 0~2π로 반복
            theta = Mathf.Repeat(theta, 2 * Mathf.PI);

            // 구면 좌표 -> 데카르트 좌표 변환
            float sinPhi = Mathf.Sin(phi);
            float cosPhi = Mathf.Cos(phi);
            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);

            Vector3 newOffset = new Vector3(
                r * sinPhi * sinTheta, // x
                r * cosPhi,            // y
                r * sinPhi * cosTheta  // z
            );

            transform.position = target.position + newOffset;
            transform.LookAt(target.position, Vector3.up);

            yield return null;
        }
    }

    /// <summary>
    /// 외부에서 연속 회전 명령을 설정합니다.
    /// 만약 이미 연속 회전 중이면, 새 명령으로 바로 전환됩니다.
    /// </summary>
    /// <param name="command">"left", "right", "up", "down"</param>
    public void SetRotationCommand(string command)
    {
        currentContinuousDirection = command.ToLower();
        if (continuousRotationCoroutine == null)
        {
            continuousRotationCoroutine = StartCoroutine(ContinuousRotation());
        }
    }

    public void StopContinuousRotation()
    {
        currentContinuousDirection = "";
        if (continuousRotationCoroutine != null)
        {
            StopCoroutine(continuousRotationCoroutine);
            continuousRotationCoroutine = null;
        }
    }

    /// <summary>
    /// 회전 상태를 초기화(런타임 시작 시 저장한 상태로 복원)합니다.
    /// </summary>
    public void ResetRotationState()
    {
        StopContinuousRotation();
        yaw = initialYaw;
        pitch = initialPitch;
        Vector3 offset = new Vector3(0, 0, -initialDistance);
        // yaw 먼저, 그 다음 pitch
        Quaternion qYaw = Quaternion.AngleAxis(yaw, Vector3.up);
        Quaternion qPitch = Quaternion.AngleAxis(pitch, Vector3.right);
        Quaternion rotation = qYaw * qPitch;
        transform.position = target.position + rotation * offset;
        transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
    }
}



