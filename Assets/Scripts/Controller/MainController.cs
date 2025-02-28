using SpeechToTextUnity;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [Header("Speech To Text Model")]
    public ESTTModelType STTModelType;
    [Range(0, 30)] public int MaxAudioDuration = 25;
    public string STTConvertedString;

    [Header("LLM Model")]
    public EAPIType LLMModelType;
    public string LLMOutputString;

    [Header("Sentence Similarity Model")]
    public ActivateType SentenceSimilarityModelType;

    [Header("Elements")]
    public Texture2D CurSelectedImageFile;

    private static MainController instance;
    public static MainController Instance => instance;

    public event Action<string> OnLLMResponseUpdated;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        SpeechToTextUnityModule.OnDestroy();
    }

    /// <summary>
    /// A method that converts a given audio file to text using an STT module.
    /// </summary>
    public async Task SpeechToText(string filePath)
    {
        STTConvertedString = await AudioConvertor.ConvertAudioToText(filePath, STTModelType, MaxAudioDuration);
    }

    /// <summary>
    /// A method that asks an LLM for an image or text.
    /// </summary>
    public async Task LLM(string inputText)
    {
        if (CurSelectedImageFile == null)
            LLMOutputString = await LLMModule.Instance.Chat(inputText);
        else
            LLMOutputString = await LLMModule.Instance.Chat(inputText, CurSelectedImageFile);

        if (!string.IsNullOrEmpty(LLMOutputString))
            OnLLMResponseUpdated?.Invoke(LLMOutputString);
    }

    /// <summary>
    /// A method to reset the selected image.
    /// </summary>
    public void ResetSelectedImage()
    {
        CurSelectedImageFile = null;
    }
}
