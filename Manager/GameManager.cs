using UnityEngine;

public enum GameState   
{
    MainMenu, SoundMode, Pause, Refuel,
}
public enum GameMode
{
    Sound,Stunts
}

public class GameManager : Singleton<GameManager>
{
    private static GameState gameState;

    private void Awake()
    {
        Input.multiTouchEnabled = true;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
    }

    private void Start()
    {
        ChangeState(GameState.MainMenu);

        //CS_UIManager.Ins.OpenUI<UISplash>();
        //CS_UIManager.Ins.GetUI<UISettings>().GetSetting();
    }

    
    public static void ChangeState(GameState state)
    {
        gameState = state;
    }

    public static bool IsState(GameState state) => gameState == state;
}
