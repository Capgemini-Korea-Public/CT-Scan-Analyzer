

public class ZoomCommand : ICommand
{
    private const string CommandName = "Zoom";
    private const string MessageFormat = "\nPlease provide zoom command in JSON format.";
    private CameraFovController fovController;
    private ZoomInformation zoomInfo;

    public ZoomCommand(CameraFovController cameraFovController)
    {
        fovController = cameraFovController;
        zoomInfo = new ZoomInformation();
        CommandSystemManager.instance.RegisterCommand(CommandName, this);
    }

    public void Execute(string output)
    {
        throw new System.NotImplementedException();
    }

    public string GetInputFormat()
    {
        throw new System.NotImplementedException();
    }
}

public class ZoomInformation
{

}
