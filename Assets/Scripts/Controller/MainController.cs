using SpeechToTextUnity;
using System;
using System.Collections.Generic;
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
    public List<Texture2D> CurCapturedImages = new List<Texture2D>();

    [Header(" Loading UI Element")]
    public LoadingUI LoadingUI;
    
    private static MainController instance;
    public static MainController Instance => instance;

    [SerializeField] private bool isImageInserted;

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
    public async Task ExecuteSpeechToText(string filePath)
    {
        STTConvertedString = await AudioConvertor.ConvertAudioToText(filePath, STTModelType, MaxAudioDuration);
    }

    /// <summary>
    /// A method that asks an LLM for an image or text.
    /// </summary>
    public void ExecuteSentenceSimilarity(string inputText)
    {
        LoadingUI.StartLoadingUI();
        SentenceSimilarityController.Instance.MeasureSentenceAccuracy(inputText);
    }

    /// <summary>
    /// A method to reset the selected image.
    /// </summary>
    public void ResetSelectedImage()
    {
        CurCapturedImages.Clear();
    }

    /// <summary>
    /// Add Selected Image
    /// </summary>
    public void CaptureImage(Texture2D image)
    {
        ResetSelectedImage();
        CurCapturedImages.Add(image);
    }
    
    /// <summary>
    /// A method Execute When Sentence Measure Success
    /// </summary>
    public async void SuccessMeasureSentence(SimilarityResult similarityResult)
    {
        string command = similarityResult.sentence;
        string format = CommandSystemManager.instance.GetInputFormat(command);
        if (format == null)
        {
            Debug.LogWarning($"{command} not found");
            return;
        }
        string input = similarityResult.enterdSentence + CommandSystemManager.instance.GetInputFormat(command);

        if (CurCapturedImages == null || !isImageInserted || command != "diagnose")
            LLMOutputString = await LLMModule.Instance.Chat(input);
        else
        {
            Texture2D[] curImages = CurCapturedImages.ToArray();
            LLMOutputString = await LLMModule.Instance.Chat(input, curImages);
            ResetSelectedImage();
        }
        LoadingUI.EndLoadingUI();

        CommandSystemManager.instance.ExecuteCommand(command, LLMOutputString);
    }

    /// <summary>
    /// A method Image Inserted bool variable 
    /// </summary>
    public void SetImageInserted(bool b)
    {
        isImageInserted = b;
    }
}
