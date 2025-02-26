using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandSystemManager : MonoBehaviour
{
    public static CommandSystemManager instance;
    
    private Dictionary<string, ICommand> commandDict = new Dictionary<string, ICommand>();

    private void Awake()
    {
        instance = this;
    }
    public void RegisterCommand(string key,ICommand command)
    {
        commandDict[key] = command;
    }

    public void ExecuteCommand(string key, params object[] parameters)
    {
        if (commandDict.ContainsKey(key))
        {
            commandDict[key].Execute(parameters);
        }
    }
}

public interface ICommand
{
    public void Execute(params object[] parameters);
}


// 회전 명령 예시
public class RotateCommand : ICommand
{
    private const string COMMAND_NAME = "Rotate";
    private Transform targetTransform;

    public RotateCommand(Transform target)
    {
        targetTransform = target;
        CommandSystemManager.instance.RegisterCommand(COMMAND_NAME , this);
    }

    public void Execute(params object[] parameters)
    {
        if (parameters.Length > 0 && parameters[0] is float angle)
        {
            targetTransform.Rotate(Vector3.up, angle);
        }
        else
        {
            targetTransform.Rotate(Vector3.up, 45f);
        }
    }
}
