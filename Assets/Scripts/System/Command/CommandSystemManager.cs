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
        SentenceSimilarityController.Instance.RegisterSentence(input);
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