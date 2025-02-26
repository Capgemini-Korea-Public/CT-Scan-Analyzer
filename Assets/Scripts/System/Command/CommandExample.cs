using UnityEngine;
public class CommandExample : MonoBehaviour
{
    private CommandSystemManager commandManager;
    private Transform targetObject;

    void Start()
    {
        commandManager = GetComponent<CommandSystemManager>();
        targetObject = transform;

        // 커맨드 등록
        commandManager.RegisterCommand("rotate", new RotateCommand(targetObject));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 69도 회전
            commandManager.ExecuteCommand("rotate", 69f);
        }
    }
}

