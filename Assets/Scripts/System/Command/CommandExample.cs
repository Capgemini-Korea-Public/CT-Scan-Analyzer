using System;
using UnityEngine;
public class CommandExample : MonoBehaviour
{
    private RotateCommand _rotateCommand;
    private DiagnoseCommand diagnoseCommand;
    private void Start()
    {
        _rotateCommand = new RotateCommand(this.transform);
        diagnoseCommand = new DiagnoseCommand();
    }

    [ContextMenu("ROTATE")]
    public async void Test()
    {
        string str = "I want to turn the camera 60 degrees to the right" + CommandSystemManager.instance.GetInputFormat("Rotate");
        string output = await LLMModule.Instance.Chat(str);

        RotationInformation rotationInformation = JsonUtility.FromJson<RotationInformation>(output);
        Debug.Log($" {rotationInformation.angle} / {rotationInformation.direction}");
    }
}

