using System.Collections;
using UnityEngine;

public class CameraFovController : MonoBehaviour
{
    // 카메라 컴포넌트 참조 (없다면 Start에서 자동으로 가져옴)
    public Camera cam;

    [Tooltip("FOV의 최소값 (예: 10도)")]
    public float minFov = 10f;
    [Tooltip("FOV의 최대값 (예: 120도)")]
    public float maxFov = 120f;

    [Tooltip("연속 줌 시 초당 FOV 변화율 (퍼센트 단위)")]
    public float continuousZoomSpeed = 10f; // 예를 들어 10% 변화

    // 초기 상태 저장용 변수
    private float initialFov;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Coroutine smoothZoomCoroutine;

    void Start()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }

        initialFov = cam.fieldOfView;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    /// <summary>
    /// SmoothZoom 함수는 주어진 command("zoom in" 또는 "zoom out")와 percent에 따라
    /// 현재 FOV에서 목표 FOV로 부드럽게 변화시킵니다.
    /// percent 매개변수가 생략되면 기본값 10%가 적용됩니다.
    /// </summary>
    /// <param name="command">"zoom in" 또는 "zoom out"</param>
    /// <param name="percent">변화할 퍼센트 (예: 30이면 현재 FOV의 30% 변화)</param>
    public void SmoothZoom(string command, float percent = 10f)
    {
        if (smoothZoomCoroutine != null)
        {
            StopCoroutine(smoothZoomCoroutine);
        }
        smoothZoomCoroutine = StartCoroutine(SmoothZoomCoroutine(command, percent));
    }

    IEnumerator SmoothZoomCoroutine(string command, float percent)
    {
        float startFov = cam.fieldOfView;
        float targetFov = startFov;

        string lowerCmd = command.ToLower();
        if (lowerCmd.Contains("in"))
        {
            targetFov = startFov * (1f - percent / 100f);
        }
        else if (lowerCmd.Contains("out"))
        {
            targetFov = startFov * (1f + percent / 100f);
        }
        else
        {
            Debug.LogWarning("SmoothZoom: 알 수 없는 명령어 - " + command);
            yield break;
        }

        targetFov = Mathf.Clamp(targetFov, minFov, maxFov);

        while (Mathf.Abs(cam.fieldOfView - targetFov) > 0.001f)
        {
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, targetFov, continuousZoomSpeed * Time.deltaTime);
            yield return null;
        }
        cam.fieldOfView = targetFov;
        smoothZoomCoroutine = null;
    }


    /// <summary>
    /// 카메라의 FOV와 위치, 회전을 초기 상태로 복원합니다.
    /// </summary>
    public void ResetFovState()
    {
        cam.fieldOfView = initialFov;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        Debug.Log("카메라 FOV 및 상태가 초기화되었습니다.");
    }
}
