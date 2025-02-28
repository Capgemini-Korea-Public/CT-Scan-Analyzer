using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using OpenAI;
using SpeechToTextUnity;

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

    [Header("Output Panel")]
    [SerializeField] private GameObject outputPanel;
    [SerializeField] private TextMeshProUGUI outputText;
    [SerializeField] private TextMeshProUGUI warningText;

    private readonly HashSet<string> imageExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private Coroutine fadeCor;
    private WaitForSeconds defaultSeconds = new WaitForSeconds(2f);

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
        MainController.Instance.ResetSelectedImage();
    }

    private void OnTextInputValChanged(string value)
    {
        curInputText = value;
    }

    private async Task CaptureImage()
    {
        // Method - Select Image File Directly
        //string filePath = FileSelector.FileSelect();
        //if (IsValidImageFormat(filePath))
        //{
        //    MainController.Instance.CaptureImage(await AudioConvertor.LoadTexture(filePath));
        //}
        //else
        //    Warning("Unsupported Image format");

        CameraCaptureSystem.Instance.Capture();
        string filePath = CameraCaptureSystem.Instance.CapturedImageSavePath;
        MainController.Instance.CaptureImage(await AudioConvertor.LoadTexture(filePath));
    }

    private bool IsValidImageFormat(string filePath)
    {
        if (filePath == null) 
            return false;

        string extension = Path.GetExtension(filePath);
        if (!imageExtensions.Contains(extension))
        {
            Debug.LogWarning("Invalid File Extension: " + Path.GetExtension(filePath));
            return false;
        }
         return true;
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
        outputPanel.SetActive(true);
        outputText.text = outputString;
    }
}

