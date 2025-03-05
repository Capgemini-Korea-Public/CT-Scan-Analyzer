using System;
using System.Collections;
using UnityEngine;

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
    public void RotateSmooth(string direction, float? angle = 60f)
    {
        // direction이 null이거나 빈 문자열이면 기본값 "right"로 설정합니다.
        if (string.IsNullOrEmpty(direction))
            direction = "right";

        //if (!angle.HasValue || Mathf.Approximately(angle.Value, 0f))
        //{
        //    // 연속 회전: 새 방향 명령을 currentContinuousDirection에 저장
        //    //SetRotationCommand(direction);
        //    //return;
        //}


        // 단일 회전 모드: 진행 중인 연속 회전이 있다면 중지
        if (continuousRotationCoroutine != null)
        {
            StopCoroutine(continuousRotationCoroutine);
            continuousRotationCoroutine = null;
            currentContinuousDirection = "";
        }
        if (isRotating) return;

        float deltaAngle = angle.Value;
        // 여기서는 실제 현재 상태를 반영하기 위해, GetCurrentYawPitch()를 사용합니다.
        float currentYaw, currentPitch;
        GetCurrentYawPitch(out currentYaw, out currentPitch);

        float targetYaw = currentYaw;
        float targetPitch = currentPitch;

        switch (direction.ToLower())
        {
            case "left":
                targetYaw += Mathf.Abs(deltaAngle);
                break;
            case "right":
                targetYaw -= Mathf.Abs(deltaAngle);
                break;
            case "up":
                targetPitch = Mathf.Clamp(currentPitch + Mathf.Abs(deltaAngle), -80f, 80f);
                break;
            case "down":
                targetPitch = Mathf.Clamp(currentPitch - Mathf.Abs(deltaAngle), -80f, 80f);
                break;
            default:
                targetYaw -= Mathf.Abs(deltaAngle);
                Debug.LogWarning("알 수 없는 회전 명령: " + direction);
                return;
        }
        StartCoroutine(RotateCoroutine(targetYaw, targetPitch));

    }

    // 현재 카메라의 실제 회전에서 yaw와 pitch를 추출하는 함수 (간단한 예시)
    private void GetCurrentYawPitch(out float currentYaw, out float currentPitch)
    {
        // transform.rotation을 EulerAngles로 변환하면, 회전값을 얻을 수 있습니다.
        Vector3 euler = transform.rotation.eulerAngles;
        // Unity의 EulerAngles는 0~360 범위이므로, -180~180 범위로 변환해주면 보간이 자연스럽습니다.
        currentYaw = euler.y > 180 ? euler.y - 360 : euler.y;
        currentPitch = euler.x > 180 ? euler.x - 360 : euler.x;
    }

    IEnumerator RotateCoroutine(float targetYaw, float targetPitch)
    {
        isRotating = true;
        // 시작 시점의 실제 yaw, pitch 추출
        float startYaw, startPitch;
        GetCurrentYawPitch(out startYaw, out startPitch);

        float t = 0f;
        while (t < rotationDuration)
        {
            t += Time.deltaTime;
            float progress = t / rotationDuration;
            float currentYaw = Mathf.Lerp(startYaw, targetYaw, progress);
            float currentPitch = Mathf.Lerp(startPitch, targetPitch, progress);

            // 보간된 yaw, pitch를 기반으로 회전 쿼터니언 생성 (여기서는 Vector3.right 대신 고정 축 사용)
            Quaternion currentRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

            Vector3 newOffset = currentRotation * new Vector3(0, 0, -initialDistance);
            transform.position = target.position + newOffset;
            transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
            yield return null;
        }
        // 최종 상태 적용
        Quaternion finalRotation = Quaternion.Euler(targetPitch, targetYaw, 0f);
        transform.position = target.position + finalRotation * new Vector3(0, 0, -initialDistance);
        transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        isRotating = false;
    }

    IEnumerator ContinuousRotation()
    {
        // 초기 오프셋 기반: r, φ, θ 계산
        Vector3 offset = transform.position - target.position;
        float r = offset.magnitude;
        float phi = Mathf.Acos(offset.y / r); // 폴라 각도 (0: target 바로 위, π: target 바로 아래)
        float theta = Mathf.Atan2(offset.x, offset.z); // 아지무스 각도 (XZ 평면, Z축 기준)

        while (true)
        {
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
            // 클램핑: φ를 10° ~ 170°로 제한 (즉, 피치가 -80° ~ 80°로 유지)
            float minPhi = 10f * Mathf.Deg2Rad;
            float maxPhi = 170f * Mathf.Deg2Rad;
            phi = Mathf.Clamp(phi, minPhi, maxPhi);

            theta = Mathf.Repeat(theta, 2 * Mathf.PI);

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
        Quaternion qYaw = Quaternion.AngleAxis(yaw, Vector3.up);
        Quaternion qPitch = Quaternion.AngleAxis(pitch, transform.right);
        Quaternion rotation = qYaw * qPitch;
        transform.position = target.position + rotation * offset;
        transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
    }
}



