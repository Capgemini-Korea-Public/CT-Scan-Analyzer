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
    public void RegisterCommand(string input, ICommand command)
    {
        commandDict[input] = command;
    }

    public void ExecuteCommand(string input, string sentence)
    {
        if (commandDict.ContainsKey(input))
        {
            commandDict[input].Execute(sentence);
        }
    }

    public string GetInputFormat(string input)
    {
        return commandDict.TryGetValue(input, out ICommand command) ? command.GetInputFormat() : "";
    }
    
    
}

public interface ICommand
{
    public void Execute(string output);
    public string GetInputFormat();
}

public class RotateCommand : ICommand
{
    private const string CommandName = "Rotate";
    private const string MessageFormat = "\nplease convert this by only this json format.";
    private Transform targetTransform;

    protected RotationInformation rotationInformation;
    public RotateCommand(Transform target)
    {
        targetTransform = target;
        CommandSystemManager.instance.RegisterCommand(CommandName, this);
        rotationInformation = new RotationInformation();
    }

    public void Execute(string output)
    {
        try
        {
            rotationInformation = JsonUtility.FromJson<RotationInformation>(output);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public string GetInputFormat()
    {
        string message = MessageFormat + JsonUtility.ToJson(rotationInformation);
        return message;
    }
}

public class RotationInformation
{
    public float angle;
    public string direction;
}
