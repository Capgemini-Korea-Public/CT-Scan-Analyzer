using System.Collections;
using UnityEngine;

public class CameraFovController : MonoBehaviour
{
    // ī�޶� ������Ʈ ���� (���ٸ� Start���� �ڵ����� ������)
    public Camera cam;

    [Tooltip("FOV�� �ּҰ� (��: 10��)")]
    public float minFov = 10f;
    [Tooltip("FOV�� �ִ밪 (��: 120��)")]
    public float maxFov = 120f;

    [Tooltip("���� �� �� �ʴ� FOV ��ȭ�� (�ۼ�Ʈ ����)")]
    public float continuousZoomSpeed = 10f; // ���� ��� 10% ��ȭ

    // �ʱ� ���� ����� ����
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
    /// SmoothZoom �Լ��� �־��� command("zoom in" �Ǵ� "zoom out")�� percent�� ����
    /// ���� FOV���� ��ǥ FOV�� �ε巴�� ��ȭ��ŵ�ϴ�.
    /// percent �Ű������� �����Ǹ� �⺻�� 10%�� ����˴ϴ�.
    /// </summary>
    /// <param name="command">"zoom in" �Ǵ� "zoom out"</param>
    /// <param name="percent">��ȭ�� �ۼ�Ʈ (��: 30�̸� ���� FOV�� 30% ��ȭ)</param>
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
            Debug.LogWarning("SmoothZoom: �� �� ���� ���ɾ� - " + command);
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
    /// ī�޶��� FOV�� ��ġ, ȸ���� �ʱ� ���·� �����մϴ�.
    /// </summary>
    public void ResetFovState()
    {
        cam.fieldOfView = initialFov;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        Debug.Log("ī�޶� FOV �� ���°� �ʱ�ȭ�Ǿ����ϴ�.");
    }
}
