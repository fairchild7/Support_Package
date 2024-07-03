using UnityEngine;

public enum GameState
{
    MainMenu, Gameplay, Pause
}

public class GameManager : Singleton<GameManager>
{
    private static GameState gameState;

    private static float normalDeltaTime;
    private static float pauseDeltaTime = 0f;
    private static float normalFixedDeltaTime;

    private void Awake()
    {
        Input.multiTouchEnabled = true;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Start()
    {
        normalDeltaTime = Time.deltaTime;
        normalFixedDeltaTime = Time.fixedDeltaTime;

        ChangeState(GameState.MainMenu);
    }


    public static void ChangeState(GameState state)
    {
        gameState = state;
    }

    public static bool IsState(GameState state) => gameState == state;

    public static float DeltaTime
    {
        get
        {
            if (IsState(GameState.Pause))
            {
                return pauseDeltaTime;
            }
            else
            {
                return normalDeltaTime;
            }
        }
    }

    public static float FixedDeltaTime
    {
        get
        {
            if (IsState(GameState.Pause))
            {
                return pauseDeltaTime;
            }
            else
            {
                return normalFixedDeltaTime;
            }
        }
    }
}
