using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(VTKCamera))]
public class CameraRotationController : MonoBehaviour
{
    public Transform target;

    [Header("�� �� ȸ���� �� ����")]
    [Tooltip("�� �� ��� �� ȸ���� �⺻ ���� (��)")]
    public float defaultAngle = 90f;
    [Tooltip("���� ȸ���� �ɸ��� �ð� (��)")]
    public float rotationDuration = 1f;

    [Header("���� ȸ�� ����")]
    [Tooltip("���� ȸ�� �� �ʴ� ȸ�� �ӵ� (��)")]
    public float continuousRotationSpeed = 30f;

    // ���� ���� �÷���
    private bool isRotating = false;
    private Coroutine continuousRotationCoroutine;

    public VTKCamera vtkCamera;

    private void Start()
    {
        vtkCamera = GetComponent<VTKCamera>();
        IntPtr renderWindowHandle = vtkCamera.GetRenderWindowHandle();

        SetTargetToVolumeCenter(renderWindowHandle);
    }

    public void SetTargetToVolumeCenter(IntPtr renderWindowHandle)
    {
        Vector3 origin = new Vector3(0, 0, 0);
        Vector3 spacing = new Vector3(1, 1, 2);
        int xmin = 0, xmax = 255, ymin = 0, ymax = 255, zmin = 0, zmax = 93;

        // �ε��� �������� �߰��� ��� ��, Spacing �ݿ�
        float centerX = origin.x + ((xmax - xmin) * 0.5f * spacing.x); // 0 + (255 * 0.5) = 127.5
        float centerY = origin.y + ((ymax - ymin) * 0.5f * spacing.y); // 127.5
        float centerZ = origin.z + ((zmax - zmin) * 0.5f * spacing.z); // 0 + (93 * 0.5 * 2) = 93

        Vector3 volumeCenter = new Vector3(centerX, centerY, centerZ);

        // �ʿ� �� �������� �߰��� �Ӹ��� ��Ȯ�� �߽ɿ� ���� �� ����.
        // ��: volumeCenter.y -= offset;

        if (target != null)
        {
            target.position = volumeCenter;
        }
        else
        {
            Debug.LogWarning("Ÿ���� �����Ǿ� ���� �ʽ��ϴ�. �� ������Ʈ�� �����Ͽ� target���� �Ҵ��ϼ���.");
        }
    }

    /// <summary>
    /// ȸ�� ����� �����մϴ�.
    /// - angle �Ű������� ������ �� ������ŭ �ε巴�� ȸ���մϴ�.
    /// - angle �Ű������� ������ ���� ȸ�� ���� �����մϴ�.
    /// </summary>
    /// <param name="direction">"left", "right", "up", "down"</param>
    /// <param name="angle">ȸ���� ����(��). null�̸� ���� ȸ��</param>
    public void RotateSmooth(string direction, float? angle = null)
    {
        if (!angle.HasValue)
        {
            if (continuousRotationCoroutine == null)
            {
                continuousRotationCoroutine = StartCoroutine(ContinuousRotation(direction));
            }
        }
        else
        {
            if (continuousRotationCoroutine != null)
            {
                StopCoroutine(continuousRotationCoroutine);
                continuousRotationCoroutine = null;
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
                default:
                    Debug.LogWarning("�� �� ���� ȸ�� ���: " + direction);
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
                transform.LookAt(target.position);
            }
            yield return null;
        }
        transform.rotation = endRot;
        if (target != null)
        {
            transform.LookAt(target.position);
        }
        isRotating = false;
    }

    IEnumerator ContinuousRotation(string direction)
    {
        Vector3 axis = Vector3.up;
        float sign = 1f;
        switch (direction.ToLower())
        {
            case "left":
                axis = Vector3.up;
                sign = 1f;
                break;
            case "right":
                axis = Vector3.up;
                sign = -1f;
                break;
            case "up":
                axis = transform.right;
                sign = 1f;
                break;
            case "down":
                axis = transform.right;
                sign = -1f;
                break;
            default:
                Debug.LogWarning("�� �� ���� ���� ȸ�� ���: " + direction);
                yield break;
        }

        while (true)
        {
            float angleThisFrame = continuousRotationSpeed * Time.deltaTime * sign;
            Quaternion deltaRotation = Quaternion.AngleAxis(angleThisFrame, axis);

            // Ÿ�ٰ��� ��� ��ġ(������)�� ���� �� ȸ��
            Vector3 offset = transform.position - target.position;
            offset = deltaRotation * offset;
            transform.position = target.position + offset;

            // ī�޶��� up ���Ͱ� �ް��� �ٲ��� �ʵ���, LookAt ȣ�� �� ���� up (Vector3.up) ����
            transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);

            yield return null;
        }
    }

    public void StopContinuousRotation()
    {
        if (continuousRotationCoroutine != null)
        {
            StopCoroutine(continuousRotationCoroutine);
            continuousRotationCoroutine = null;
        }
    }
}

