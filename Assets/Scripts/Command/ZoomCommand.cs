using System;
using UnityEngine;

public class ZoomCommand : ICommand
{
    private const string CommandName = "Zoom";
    private const string MessageFormat = "\nplease convert this by only this json format. cameraType: fov, ortho; command: zoom in, zoom out; percent: <number>\n";
    private CameraFovController fovController;
    private CameraOrthoZoomController orthoZoomController;
    private ZoomInformation zoomInfo;

    public ZoomCommand(CameraFovController cameraFovController, CameraOrthoZoomController cameraOrthoZoomController)
    {
        fovController = cameraFovController;
        orthoZoomController = cameraOrthoZoomController;
        zoomInfo = new ZoomInformation();
        CommandSystemManager.instance.RegisterCommand(CommandName, this);
    }

    public void Execute(string output)
    {
        try
        {
            zoomInfo = JsonExtension.DeserializeObject<ZoomInformation>(output);
            if(string.IsNullOrEmpty(zoomInfo.cameraType))
            {
                zoomInfo.cameraType = "ortho";
            }

            if(zoomInfo.cameraType.ToLower() == "fov")
            {
                fovController.SmoothZoom(zoomInfo.command, zoomInfo.percent);
            }
            else if(zoomInfo.cameraType.ToLower() == "ortho")
            {
                orthoZoomController.AdjustOrthoZoom(zoomInfo.command, zoomInfo.percent);
            }
            
            string printText = $"I {zoomInfo.command} on the {zoomInfo.cameraType} camera by {zoomInfo.percent}% !";
            CommandSystemManager.instance.OnUIUpdate(printText);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
    }

    public string GetInputFormat()
    {
        return MessageFormat + JsonUtility.ToJson(zoomInfo);
    }
}

public class ZoomInformation
{
    public float percent;
    public string command;
    public string cameraType;
}
