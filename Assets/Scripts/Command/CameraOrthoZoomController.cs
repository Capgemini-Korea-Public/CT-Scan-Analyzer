using System.Collections;
using UnityEngine;

public class CameraOrthoZoomController : MonoBehaviour
{
    // ī�޶� ������Ʈ�� Inspector���� ���� �Ҵ��ϰų�, Start()���� �ڵ����� �����ɴϴ�.
    public Camera cam;

    [Tooltip("Orthographic Size�� �ּҰ� (��: 1)")]
    public float minOrthoSize = 1f;
    [Tooltip("Orthographic Size�� �ִ밪 (��: 100)")]
    public float maxOrthoSize = 1000f;

    [Tooltip("���� �� �� �ʴ� ��ȭ�� (�ۼ�Ʈ)")]
    public float continuousZoomSpeed = 10f; // ���� ���, �ʴ� 10% ��ȭ

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
            Debug.LogWarning("CameraOrthoZoomController: ī�޶� Orthographic ��尡 �ƴմϴ�.");
        }

        initialOrthoSize = cam.orthographicSize;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    /// <summary>
    /// AdjustOrthoZoom �Լ��� "zoom in" �Ǵ� "zoom out" ��ɰ� percent �Ű������� �޾�
    /// ���� orthographicSize���� ��ǥ�� (���簪�� percent% ��ŭ ��ȭ�� ��)���� �ε巴�� ��ȭ��ŵ�ϴ�.
    /// percent�� ���޵��� ������ �⺻�� 10%�� ����˴ϴ�.
    /// </summary>
    /// <param name="command">"zoom in" �Ǵ� "zoom out"</param>
    /// <param name="percent">��ȭ�� �ۼ�Ʈ (��: 30�̸� 30% ��ȭ)</param>
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
            Debug.LogWarning("AdjustOrthoZoom: �� �� ���� ��ɾ� - " + command);
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
        Debug.Log("ī�޶� �ʱ� ���·� ������.");
    }
}
