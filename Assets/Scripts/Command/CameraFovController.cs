using UnityEngine;

public class CameraFovController : MonoBehaviour
{
    // ī�޶� ������Ʈ ���� (���ٸ� Start���� �ڵ����� ������)
    public Camera cam;

    [Tooltip("FOV�� �ּҰ� (��: 10��)")]
    public float minFov = 10f;
    [Tooltip("FOV�� �ִ밪 (��: 120��)")]
    public float maxFov = 120f;

    void Start()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }
    }

    /// <summary>
    /// FOV�� �����Ͽ� Ȯ��/��� ȿ���� �����մϴ�.
    /// </summary>
    /// <param name="command">"zoom in" �Ǵ� "zoom out" �� ��� ���ڿ�</param>
    /// <param name="percent">���� FOV�� ���� �� �ۼ�Ʈ ��ȭ��ų�� (��: 10�� 10% ��ȭ)</param>
    public void AdjustFov(string command, float percent = 10f)
    {
        if (cam == null)
        {
            Debug.LogWarning("CameraFovController: ī�޶� ������Ʈ�� �����ϴ�.");
            return;
        }

        float currentFov = cam.fieldOfView;
        float newFov = currentFov;

        if (command.ToLower().Contains("in"))
        {
            // �� ��: FOV ���� �ٿ��� Ȯ�� ȿ��
            newFov = currentFov * (1f - percent / 100f);
        }
        else if (command.ToLower().Contains("out"))
        {
            // �� �ƿ�: FOV ���� �÷��� ��� ȿ��
            newFov = currentFov * (1f + percent / 100f);
        }
        else
        {
            Debug.LogWarning("AdjustFov: �� �� ���� ��ɾ� - " + command);
            return;
        }

        // FOV�� �ʹ� �۰ų� ũ�� �ʵ��� Ŭ����
        newFov = Mathf.Clamp(newFov, minFov, maxFov);
        cam.fieldOfView = newFov;

        Debug.Log($"FOV ����: {currentFov} -> {newFov} ({command}, {percent}%)");
    }
}
