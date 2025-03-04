using UnityEngine;
using UnityEngine.UI;

public class InsertPhotoUI : MonoBehaviour
{
    [SerializeField] private Button insertBtn;
    [SerializeField] private Image checkMark;

    private void Start()
    {
        insertBtn.onClick.AddListener(OnClickInsertBtn);
    }

    private void OnClickInsertBtn()
    {
        ToggleCheckBox();
    }

    private void ToggleCheckBox()
    {
        bool b = checkMark.gameObject.activeInHierarchy;
        checkMark.gameObject.SetActive(!b);
        MainController.Instance.SetImageInserted(!b);
    }
}
