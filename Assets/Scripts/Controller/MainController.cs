using SpeechToTextUnity;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [Header("Speech To Text Model Type")]
    [SerializeField] private ESTTModelType sttModelType;
    [Header("LLM Model Type")]
    [SerializeField] private EAPIType llmModelType;
    [Header("Sentence Similarity Model Type")]
    [SerializeField] private ActivateType sentenceSimilarityModelType;

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

}
