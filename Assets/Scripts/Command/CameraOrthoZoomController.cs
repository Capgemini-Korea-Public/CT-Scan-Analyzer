using UnityEngine;

public class CameraOrthoZoomController : MonoBehaviour
{
    // ī�޶� ������Ʈ�� Inspector���� ���� �Ҵ��ϰų�, Start()���� �ڵ����� �����ɴϴ�.
    public Camera cam;

    [Tooltip("Orthographic Size�� �ּҰ� (��: 1)")]
    public float minOrthoSize = 1f;
    [Tooltip("Orthographic Size�� �ִ밪 (��: 100)")]
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
            Debug.LogWarning("CameraOrthoZoomController: ī�޶� Orthographic ��尡 �ƴմϴ�.");
        }

        initialOrthoSize = cam.orthographicSize;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    /// <summary>
    /// Orthographic ī�޶��� orthographicSize�� �����Ͽ� Ȯ��/��� ȿ���� �����մϴ�.
    /// </summary>
    /// <param name="command">"zoom in" �Ǵ� "zoom out" �� ��� ���ڿ�</param>
    /// <param name="percent">�󸶳� �ۼ�Ʈ ��ȭ��ų�� (�⺻��: 10)</param>
    public void AdjustOrthoZoom(string command, float percent = 10f)
    {
        if (cam == null)
        {
            Debug.LogWarning("CameraOrthoZoomController: ī�޶� ������Ʈ�� �����ϴ�.");
            return;
        }

        float currentSize = cam.orthographicSize;
        float newSize = currentSize;

        if (command.ToLower().Contains("in"))
        {
            // �� ��: orthographicSize�� �ٿ��� Ȯ�� ȿ��
            newSize = currentSize * (1f - percent / 100f);
        }
        else if (command.ToLower().Contains("out"))
        {
            // �� �ƿ�: orthographicSize�� �÷��� ��� ȿ��
            newSize = currentSize * (1f + percent / 100f);
        }
        else
        {
            Debug.LogWarning("AdjustOrthoZoom: �� �� ���� ��ɾ� - " + command);
            return;
        }

        newSize = Mathf.Clamp(newSize, minOrthoSize, maxOrthoSize);
        cam.orthographicSize = newSize;
        Debug.Log($"OrthographicSize ����: {currentSize} -> {newSize} ({command}, {percent}%)");
    }

    public void ResetCameraState()
    {
        cam.orthographicSize = initialOrthoSize;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        Debug.Log("ī�޶� �ʱ� ���·� ������.");
    }
}
