using UnityEngine;
using UnityEngine.UI;
using MyAudioPackage.Core;
using TMPro;
using System.Threading.Tasks;

public class AudioRecorderUI : MonoBehaviour
{
    [Header("Button Panel")]
    public Button recordButton;
    public Button stopButton;
    public Button playButton;

    [Header("Recording Duration Input")]
    public Button durationEnterButton;
    public TMP_InputField durationInputField;

    [Header("Audio Source")]
    public AudioSource analysisSource;
    public AudioSource playbackSource;

    [Header("Info Text")]
    public TextMeshProUGUI infoText;

    [Header("Etc Elements")]
    public Button sttEnterButton;

    private AudioRecorder recorderCore;
    private AudioFileManager fileManagerCore;

    private int duration;

    private void Awake()
    {
        recorderCore = new AudioRecorder(analysisSource, playbackSource);
        fileManagerCore = new AudioFileManager();

        if (recordButton != null)
            recordButton.onClick.AddListener(OnStartButtonClicked);
        if (stopButton != null)
            stopButton.onClick.AddListener(OnStopButtonClicked);
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayButtonClicked);
        if (durationEnterButton != null)
            durationEnterButton.onClick.AddListener(OnDurationEnterButtonClicked);
        if (sttEnterButton != null)
            sttEnterButton.onClick.AddListener(OnSTTEnterButtonClicked);

        if (durationInputField != null)
            durationInputField.onValueChanged.AddListener(OnDurationInputChanged);

        recorderCore.OnRecordingStarted += HandleRecordingStarted;
        recorderCore.OnRecordingStopped += HandleRecordingStopped;
        recorderCore.OnLog += AppendLog;
    }

    private async void OnStartButtonClicked()
    {
        if (int.TryParse(durationInputField.text, out int duration))
        {
            recorderCore.SetRecordingDuration(duration);
        }
        await recorderCore.StartRecordingAsync();
    }

    private void OnStopButtonClicked()
    {
        recorderCore.StopRecording();
    }

    private void OnPlayButtonClicked()
    {
        recorderCore.PlayRecording();
    }

    private void OnDurationEnterButtonClicked()
    {
        recorderCore.SetRecordingDuration(duration);
    }

    private void OnSTTEnterButtonClicked()
    {
        _ = SttToSentenceSimilarity();
    }

    private async Task SttToSentenceSimilarity()
    {
        await MainController.Instance.ExecuteSpeechToText(recorderCore.FileManager.CurRecordedFilePath);
        MainController.Instance.ExecuteSentenceSimilarity(MainController.Instance.STTConvertedString);
    }

    private void OnDurationInputChanged(string value)
    {
        if (int.TryParse(value, out int duration))
        {
           this.duration = duration;
        }
    }

    private void HandleRecordingStarted()
    {
        AppendLog("Recording started.");
    }

    private void HandleRecordingStopped()
    {
        AppendLog("Recording stopped.");
    }

    private void AppendLog(string message)
    {
        infoText.text = "";
        if (infoText != null)
            infoText.text += message + "\n";
        Debug.Log(message);
    }
}
