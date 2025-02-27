using SpeechToTextUnity;
using System.Threading.Tasks;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [Header("Speech To Text Model Type")]
    public ESTTModelType STTModelType;
    [Header("LLM Model Type")]
    public EAPIType LLMModelType;
    [Header("Sentence Similarity Model Type")]
    public ActivateType SentenceSimilarityModelType;

    [Header("Speech To Text Elements")]
    [Range(0,30)] public int MaxAudioDuration = 25;
    public string STTConvertedString;

    [Header("LLM Elements")]
    public string LLMOutputString;

    private static MainController instance;
    public static MainController Instance => instance;

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

    public async Task SpeechToText(string filePath)
    {
        STTConvertedString = await AudioConvertor.ConvertAudioToText(filePath, STTModelType, MaxAudioDuration);
    }

    public async Task LLM(string inputText)
    {
        LLMOutputString = await LLMModule.Instance.Chat(inputText);
    }
}
