using UnityEngine;
using UnityEngine.UI;
using MyAudioPackage.Core;
using TMPro;
using System.Threading.Tasks;

public class AudioRecorderUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Button recordButton;
    public Button stopButton;
    public Button playButton;
    public Button durationEnterButton;
    public Button sttEnterButton;
    public TMP_InputField durationInputField;

    public AudioSource analysisSource;
    public AudioSource playbackSource;

    // �ھ� ���� �ν��Ͻ�: MonoBehaviour�� �������� �ʴ� ���� C# Ŭ����
    private AudioRecorder recorderCore;
    private AudioFileManager fileManagerCore;

    private int duration;

    private void Awake()
    {
        recorderCore = new AudioRecorder(analysisSource, playbackSource);
        fileManagerCore = new AudioFileManager();

        // UI ��ư �̺�Ʈ ����
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

        // �Է� �ʵ� ���� �� ���� �ð� ������Ʈ
        if (durationInputField != null)
            durationInputField.onValueChanged.AddListener(OnDurationInputChanged);

        // �ھ� �̺�Ʈ ����: ���� ����/���� �� �α� �޽��� ����
        recorderCore.OnRecordingStarted += HandleRecordingStarted;
        recorderCore.OnRecordingStopped += HandleRecordingStopped;
        recorderCore.OnLog += AppendLog;
    }

    private async void OnStartButtonClicked()
    {
        // �Էµ� ���� �ð��� ������ �ھ �ݿ�
        if (int.TryParse(durationInputField.text, out int duration))
        {
            recorderCore.SetRecordingDuration(duration);
        }
        // ���� ���� (�񵿱� ȣ��)
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
        MainController.Instance.OnLLMUpdated();
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
        // UI ������Ʈ: ���� ���, ��ư ���� ���� �� �߰� ����
    }

    private void HandleRecordingStopped()
    {
        AppendLog("Recording stopped.");
        // UI ������Ʈ: ���� ���, ��ư ���� ���� �� �߰� ����
    }

    private void AppendLog(string message)
    {
        //logText.text = "";
        //if (logText != null)
        //    logText.text += message + "\n";
        //Debug.Log(message);
    }
}
