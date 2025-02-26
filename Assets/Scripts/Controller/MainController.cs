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
    public string ConvertedString;

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

    public async Task SpeechToText(string filePath)
    {
        ConvertedString = await AudioConvertor.ConvertAudioToText(filePath, STTModelType, MaxAudioDuration);
    }
}
