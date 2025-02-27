using SpeechToTextUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class HUDUI : MonoBehaviour
{
    [Header("Image Input")]
    [SerializeField] private Button imageSelectButton;

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
        FileSelector.FileSelect();
    }

    private void OnClickTextInputEnterBtn()
    {
        RunLLM();
    }

    private async Task RunLLM()
    {
        await MainController.Instance.LLM(curInputText);
    }

    private void OnTextInputValChanged(string value)
    {
        curInputText = value;
    }

    private void ToggleInputMode(bool isTextMode)
    {
        textInputButton.interactable = !isTextMode;
        audioInputButton.interactable = isTextMode;

        textInput.SetActive(isTextMode);
        audioInput.SetActive(!isTextMode);
    }
}

