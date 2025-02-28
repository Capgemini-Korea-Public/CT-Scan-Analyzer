using System.Collections;
using UnityEngine;

public class CameraOrthoZoomController : MonoBehaviour
{
    // 카메라 컴포넌트를 Inspector에서 직접 할당하거나, Start()에서 자동으로 가져옵니다.
    public Camera cam;

    [Tooltip("Orthographic Size의 최소값 (예: 1)")]
    public float minOrthoSize = 1f;
    [Tooltip("Orthographic Size의 최대값 (예: 100)")]
    public float maxOrthoSize = 1000f;

    [Tooltip("연속 줌 시 초당 변화율 (퍼센트)")]
    public float continuousZoomSpeed = 10f; // 예를 들어, 초당 10% 변화

    private float initialOrthoSize;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Coroutine continuousZoomCoroutine;

    void Start()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }

        if (!cam.orthographic)
        {
            Debug.LogWarning("CameraOrthoZoomController: 카메라가 Orthographic 모드가 아닙니다.");
        }

        initialOrthoSize = cam.orthographicSize;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    /// <summary>
    /// AdjustOrthoZoom 함수는 "zoom in" 또는 "zoom out" 명령과 percent 매개변수를 받아
    /// 현재 orthographicSize에서 목표값 (현재값의 percent% 만큼 변화된 값)으로 부드럽게 변화시킵니다.
    /// percent가 전달되지 않으면 기본값 10%가 적용됩니다.
    /// </summary>
    /// <param name="command">"zoom in" 또는 "zoom out"</param>
    /// <param name="percent">변화할 퍼센트 (예: 30이면 30% 변화)</param>
    public void AdjustOrthoZoom(string command, float percent = 10f)
    {
        if (continuousZoomCoroutine != null)
        {
            StopCoroutine(continuousZoomCoroutine);
        }
        continuousZoomCoroutine = StartCoroutine(AdjustOrthoZoomCoroutine(command, percent));
    }

    IEnumerator AdjustOrthoZoomCoroutine(string command, float percent)
    {
        float startSize = cam.orthographicSize;
        float targetSize = startSize;
        string lowerCmd = command.ToLower();

        if (lowerCmd.Contains("in"))
        {
            targetSize = startSize * (1f - percent / 100f);
        }
        else if (lowerCmd.Contains("out"))
        {
            targetSize = startSize * (1f + percent / 100f);
        }
        else
        {
            Debug.LogWarning("AdjustOrthoZoom: 알 수 없는 명령어 - " + command);
            yield break;
        }

        targetSize = Mathf.Clamp(targetSize, minOrthoSize, maxOrthoSize);

        while (Mathf.Abs(cam.orthographicSize - targetSize) > 0.001f)
        {
            cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetSize, continuousZoomSpeed * Time.deltaTime);
            yield return null;
        }
        cam.orthographicSize = targetSize;
        continuousZoomCoroutine = null;
    }


    public void ResetCameraState()
    {
        cam.orthographicSize = initialOrthoSize;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        Debug.Log("카메라 초기 상태로 복원됨.");
    }
}
