using SpeechToTextUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Collections;

public class HUDUI : MonoBehaviour
{
    [Header("Image Input")]
    [SerializeField] private Button imageSelectButton;
    [SerializeField] private string curSelectedImagePath;
    [SerializeField] private Texture2D imageFile;

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
    [SerializeField] private TextMeshProUGUI warningText;

    private readonly HashSet<string> imageExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private Coroutine fadeCor;
    private WaitForSeconds defaultSeconds = new WaitForSeconds(2f);

    private void Start()
    {
        if (imageSelectButton != null) 
             imageSelectButton.onClick.AddListener(OnClickImageSelectBtn);
        if (textInputButton != null)
            textInputButton.onClick.AddListener(() => ToggleInputMode(true));
        if (audioInputButton != null)
            audioInputButton.onClick.AddListener (() => ToggleInputMode(false));
        if (textInputEnterButton != null)
            textInputEnterButton.onClick.AddListener(OnClickTextInputEnterBtn);

        if (textInputField != null)
            textInputField.onValueChanged.AddListener(OnTextInputValChanged);

        // Init With Audio Input Mode
        ToggleInputMode(false);
    }

    private void OnClickImageSelectBtn()
    {
        _ = SelectImage();
    }

    private void OnClickTextInputEnterBtn()
    {
        _ = RunLLM();
    }

    private void OnTextInputValChanged(string value)
    {
        curInputText = value;
    }

    private async Task RunLLM()
    {
        await MainController.Instance.LLM(curInputText);
    }

    private async Task SelectImage()
    {
        string filePath = FileSelector.FileSelect();
        if (IsValidImageFormat(filePath))
        {
            curSelectedImagePath = filePath;
            imageFile = await AudioConvertor.LoadTexture(curSelectedImagePath);
        }
        else
            Warning("Unsupported Image format");
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
}

