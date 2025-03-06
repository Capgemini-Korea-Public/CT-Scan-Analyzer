using System;
using System.Collections.Generic;
using UnityEngine;


public class CommandSystemManager : MonoBehaviour
{
    public static CommandSystemManager instance;
    private Dictionary<string, ICommand> commandDict = new Dictionary<string, ICommand>();
    public Action<string> onCommandUIEvent;

    private void Awake()
    {
        instance = this;
    }
    public void RegisterCommand(string input, ICommand command)
    {
        string str = StringUtility.NormalizeText(input);
        commandDict[str] = command;
        SentenceSimilarityController.Instance.RegisterSentence(str);
    }

    public void ExecuteCommand(string input, string sentence)
    {
        string str = StringUtility.NormalizeText(input);
        if (commandDict.ContainsKey(str))
        {
            commandDict[str].Execute(sentence);
        }
    }

    public string GetInputFormat(string input)
    {
        return commandDict.TryGetValue(input, out ICommand command) ? command.GetInputFormat() : "";
    }
    
    private const string UpdateFormat = "\nLet me know if you need anything else.";
    public void OnUIUpdate(string output)
    {
        if (!string.IsNullOrEmpty(output))
            onCommandUIEvent?.Invoke(output + UpdateFormat);
    }
}

public interface ICommand
{
    public void Execute(string output);
    public string GetInputFormat();
}