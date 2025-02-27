using UnityEngine;

public class CameraOrthoZoomController : MonoBehaviour
{
    // 카메라 컴포넌트를 Inspector에서 직접 할당하거나, Start()에서 자동으로 가져옵니다.
    public Camera cam;

    [Tooltip("Orthographic Size의 최소값 (예: 1)")]
    public float minOrthoSize = 1f;
    [Tooltip("Orthographic Size의 최대값 (예: 100)")]
    public float maxOrthoSize = 1000f;

    private float initialOrthoSize;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

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
    /// Orthographic 카메라의 orthographicSize를 조정하여 확대/축소 효과를 적용합니다.
    /// </summary>
    /// <param name="command">"zoom in" 또는 "zoom out" 등 명령 문자열</param>
    /// <param name="percent">얼마나 퍼센트 변화시킬지 (기본값: 10)</param>
    public void AdjustOrthoZoom(string command, float percent = 10f)
    {
        if (cam == null)
        {
            Debug.LogWarning("CameraOrthoZoomController: 카메라 컴포넌트가 없습니다.");
            return;
        }

        float currentSize = cam.orthographicSize;
        float newSize = currentSize;

        if (command.ToLower().Contains("in"))
        {
            // 줌 인: orthographicSize를 줄여서 확대 효과
            newSize = currentSize * (1f - percent / 100f);
        }
        else if (command.ToLower().Contains("out"))
        {
            // 줌 아웃: orthographicSize를 늘려서 축소 효과
            newSize = currentSize * (1f + percent / 100f);
        }
        else
        {
            Debug.LogWarning("AdjustOrthoZoom: 알 수 없는 명령어 - " + command);
            return;
        }

        newSize = Mathf.Clamp(newSize, minOrthoSize, maxOrthoSize);
        cam.orthographicSize = newSize;
        Debug.Log($"OrthographicSize 조정: {currentSize} -> {newSize} ({command}, {percent}%)");
    }

    public void ResetCameraState()
    {
        cam.orthographicSize = initialOrthoSize;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        Debug.Log("카메라 초기 상태로 복원됨.");
    }
}
