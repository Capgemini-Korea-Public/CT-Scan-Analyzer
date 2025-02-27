using UnityEngine;

public class CameraFovController : MonoBehaviour
{
    // 카메라 컴포넌트 참조 (없다면 Start에서 자동으로 가져옴)
    public Camera cam;

    [Tooltip("FOV의 최소값 (예: 10도)")]
    public float minFov = 10f;
    [Tooltip("FOV의 최대값 (예: 120도)")]
    public float maxFov = 120f;

    void Start()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }
    }

    /// <summary>
    /// FOV를 조정하여 확대/축소 효과를 적용합니다.
    /// </summary>
    /// <param name="command">"zoom in" 또는 "zoom out" 등 명령 문자열</param>
    /// <param name="percent">현재 FOV에 대해 몇 퍼센트 변화시킬지 (예: 10은 10% 변화)</param>
    public void AdjustFov(string command, float percent = 10f)
    {
        if (cam == null)
        {
            Debug.LogWarning("CameraFovController: 카메라 컴포넌트가 없습니다.");
            return;
        }

        float currentFov = cam.fieldOfView;
        float newFov = currentFov;

        if (command.ToLower().Contains("in"))
        {
            // 줌 인: FOV 값을 줄여서 확대 효과
            newFov = currentFov * (1f - percent / 100f);
        }
        else if (command.ToLower().Contains("out"))
        {
            // 줌 아웃: FOV 값을 늘려서 축소 효과
            newFov = currentFov * (1f + percent / 100f);
        }
        else
        {
            Debug.LogWarning("AdjustFov: 알 수 없는 명령어 - " + command);
            return;
        }

        // FOV가 너무 작거나 크지 않도록 클램핑
        newFov = Mathf.Clamp(newFov, minFov, maxFov);
        cam.fieldOfView = newFov;

        Debug.Log($"FOV 조정: {currentFov} -> {newFov} ({command}, {percent}%)");
    }
}
