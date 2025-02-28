using UnityEngine;

public class DiagnoseCommand : ICommand
{
    private const string CommandName = "Diagnose";
    private const string MessageFormat = "\nI need you to diagnose my condition \n";

    public DiagnoseCommand()
    {
        CommandSystemManager.instance.RegisterCommand(CommandName, this);
    }
    public void Execute(string output)
    {
        
    }
    public string GetInputFormat()
    {
        return MessageFormat;
    }
}
