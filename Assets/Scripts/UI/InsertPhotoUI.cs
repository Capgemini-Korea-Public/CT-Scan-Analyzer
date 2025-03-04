using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InsertPhotoUI : MonoBehaviour
{
    [SerializeField] private Button insertBtn;
    [SerializeField] private Image checkMark;
    [SerializeField] private Image capturedImage;

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
        capturedImage.gameObject.SetActive(!b); 

        //if (b)
        //    ViewCapturedImage();
    }

    private void ViewCapturedImage()
    {
        Texture2D texture = MainController.Instance.CurCapturedImages[0];
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        capturedImage.sprite = sprite;
    }
}
