using UnityEngine;

public class DiagnoseCommand : ICommand
{
    private const string CommandName = "Diagnose";
    private const string MessageFormat = "\nPlease look at the attached picture and determine what the picture looks like \n";

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
