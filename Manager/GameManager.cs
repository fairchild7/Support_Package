using UnityEngine;

public enum GameState
{
    MainMenu, Gameplay, Pause
}

public class GameManager : Singleton<GameManager>
{
    private static GameState gameState;

    private void Awake()
    {
        Input.multiTouchEnabled = true;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Start()
    {
        ChangeState(GameState.MainMenu);
    }


    public static void ChangeState(GameState state)
    {
        gameState = state;
    }

    public static bool IsState(GameState state) => gameState == state;
}
