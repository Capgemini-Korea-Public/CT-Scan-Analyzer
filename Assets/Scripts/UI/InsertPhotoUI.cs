using UnityEngine;
using UnityEngine.UI;

public class InsertPhotoUI : MonoBehaviour
{
    [SerializeField] private Button insertBtn;
    [SerializeField] private Image checkMark;
    [SerializeField] private Image capturedImageBG;
    [SerializeField] private Image capturedImage;

    private Texture curTexture;
    private Sprite curSprite;

    private void Start()
    {
        insertBtn.onClick.AddListener(OnClickInsertBtn);
    }

    private void OnClickInsertBtn()
    {
        ToggleCheckBox();
    }

    public void HideCapturedImage()
    {
        checkMark.gameObject.SetActive(false);
        capturedImageBG.gameObject.SetActive(false);
        capturedImage.gameObject.SetActive(false);
    }

    private void ToggleCheckBox()
    {
        bool b = checkMark.gameObject.activeInHierarchy;
        checkMark.gameObject.SetActive(!b);
        MainController.Instance.SetImageInserted(!b);

        capturedImageBG.gameObject.SetActive(!b);
        capturedImage.gameObject.SetActive(!b);

        if (b)
            UpdateCapturedImage();
    }

    public void UpdateCapturedImage()
    {
        Texture2D texture = MainController.Instance.CurCapturedImages[0];
        if (curTexture != texture)
            curSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        capturedImage.sprite = curSprite;
    }
}
