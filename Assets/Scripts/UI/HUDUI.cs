using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections;
using SpeechToTextUnity;
using UnityEngine.Events;
using DG.Tweening;

public class HUDUI : MonoBehaviour
{
    [Header("Image Input")]
    [SerializeField] private Button imageCaptureButton;

    [Header("Text Input")]
    [SerializeField] private GameObject textInput;
    [SerializeField] private Button textInputButton;
    [SerializeField] private Button textInputEnterButton;
    [SerializeField] private TMP_InputField textInputField;
    [SerializeField] private string curInputText;

    [Header("Audio Input")]
    [SerializeField] private GameObject audioInput;
    [SerializeField] private Button audioInputButton;

    [Header("Audio Input")]
    [SerializeField] private GameObject insertPhoto;

    [Header("Input Panel")]
    [SerializeField] private RectTransform inputPanel;
    [SerializeField] private GameObject shortInputPanel;
    [SerializeField] private Button inputPanelBtn;
    [SerializeField] private Button shortInputPanelBtn;

    [Header("Output Panel")]
    [SerializeField] private RectTransform outputPanel;
    [SerializeField] private Button outputPanelBtn;
    [SerializeField] private TextMeshProUGUI outputText;
    [SerializeField] private TextMeshProUGUI warningText;

    private Coroutine fadeCor;
    private WaitForSeconds defaultSeconds = new WaitForSeconds(2f);

    public UnityEvent OnChangedCapturedImage;
    public UnityEvent OnHideImage;

    private bool isOutputPanelUp = false;

    private void Start()
    {
        if (imageCaptureButton != null) 
             imageCaptureButton.onClick.AddListener(OnClickImageSelectBtn);
        if (textInputButton != null)
            textInputButton.onClick.AddListener(() => ToggleInputMode(true));
        if (audioInputButton != null)
            audioInputButton.onClick.AddListener (() => ToggleInputMode(false));
        if (textInputEnterButton != null)
            textInputEnterButton.onClick.AddListener(OnClickTextInputEnterBtn);
        if (inputPanelBtn != null)
            inputPanelBtn.onClick.AddListener(SlideInputPanelLeft);
        if (shortInputPanelBtn != null)
            shortInputPanelBtn.onClick.AddListener(SlideInputPanelRight);
        if (outputPanelBtn != null)
            outputPanelBtn.onClick.AddListener(ToggleOutputPanel);

        if (textInputField != null)
            textInputField.onValueChanged.AddListener(OnTextInputValChanged);

        MainController.Instance.OnLLMResponseUpdated  += OnOutputChanged;

        // Init With Audio Input Mode
        ToggleInputMode(false);
    }

    private void OnClickImageSelectBtn()
    {
        _ = CaptureImage();
    }

    private void OnClickTextInputEnterBtn()
    {
        MainController.Instance.ExecuteSentenceSimilarity(curInputText);
    }

    private void OnTextInputValChanged(string value)
    {
        curInputText = value;
    }

    private async Task CaptureImage()
    {
        insertPhoto.SetActive(true);

        CameraCaptureSystem.Instance.Capture();
        string filePath = CameraCaptureSystem.Instance.CapturedImageSavePath;
        MainController.Instance.CaptureImage(await AudioConvertor.LoadTexture(filePath));

        OnChangedCapturedImage?.Invoke();
    }

    private void Warning(string warnText)
    {
        warningText.text = warnText;
        if (fadeCor != null)
            StopCoroutine(fadeCor);
        fadeCor = StartCoroutine(FadeCor(warningText, 1f));
    }

    private IEnumerator FadeCor(TextMeshProUGUI text ,float duration)
    {
        yield return defaultSeconds; 

        float elapsedTime = 0f;
        Color textColor = text.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            textColor.a = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            text.color = textColor;
            yield return null; 
        }
        text.text = ""; 
    }

    private void ToggleInputMode(bool isTextMode)
    {
        textInputButton.interactable = !isTextMode;
        audioInputButton.interactable = isTextMode;

        textInput.SetActive(isTextMode);
        audioInput.SetActive(!isTextMode);
    }
    
    private void OnOutputChanged(string outputString)
    {
        SlideOutputPanelUp();
        outputText.text = outputString;
    }

    #region Panel
    #region Input Panel
    private void SlideInputPanelLeft()
    {
        inputPanel.DOAnchorPos(new Vector3(-400, 0, 0), 1f).SetEase(Ease.InOutQuad).OnComplete(() => shortInputPanel.SetActive(true));
        OnHideImage?.Invoke();
    }

    private void SlideInputPanelRight()
    {
        shortInputPanel.SetActive(false);
        inputPanel.DOAnchorPos(new Vector3(0, 0, 0), 1f).SetEase(Ease.InOutQuad);
    }
    #endregion

    #region Output Panel
    private void ToggleOutputPanel()
    {
        if (isOutputPanelUp)
            SlideOutputPanelDown();
        else
            SlideOutputPanelUp();

        isOutputPanelUp = !isOutputPanelUp;
    }

    private void SlideOutputPanelDown()
    {
        if (!isOutputPanelUp) return;
        outputPanel.DOAnchorPos(new Vector3(0, -350, 0), 1f).SetEase(Ease.InOutQuad);
    }

    private void SlideOutputPanelUp()
    {
        if (isOutputPanelUp) return;
        outputPanel.DOAnchorPos(new Vector3(0,0,0), 1f).SetEase(Ease.InOutQuad);
    }
    #endregion
    #endregion
}

