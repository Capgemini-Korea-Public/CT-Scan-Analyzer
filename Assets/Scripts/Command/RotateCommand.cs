using System;
using UnityEngine;

public class RotateCommand : ICommand
{
    private const string CommandName = "Rotate";
    private const string MessageFormat = "\nplease convert this by only this json format. direction type : left, right, up, down \n";
    private Transform targetTransform;
    private CameraRotationController rotationController;

    protected RotationInformation rotationInformation;
    public RotateCommand(Transform target)
    {
        targetTransform = target;
        rotationController = targetTransform.GetComponent<CameraRotationController>();
        if (rotationController != null)
        {
            Debug.Log("rotation Controller is null!!");
        }
        CommandSystemManager.instance.RegisterCommand(CommandName, this);
        rotationInformation = new RotationInformation();
    }

    public void Execute(string output)
    {
        try
        {
            rotationInformation = JsonExtension.DeserializeObject<RotationInformation>(output);
            rotationController.RotateSmooth(rotationInformation.direction, rotationInformation.angle);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
    }

    public string GetInputFormat()
    {
        string message = MessageFormat + JsonUtility.ToJson(rotationInformation);
        Debug.Log(message);
        return message;
    }
}

[Serializable]
public class RotationInformation
{
    public float angle;
    public string direction;
}
