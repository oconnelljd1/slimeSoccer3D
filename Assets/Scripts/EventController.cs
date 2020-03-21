using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour {

    public delegate void GameStart();
    public static event GameStart GameStartFunctions;
    public static void onGameStart()
    {
        if (GameStartFunctions != null)
            GameStartFunctions();
    }

    public delegate void GoalScored(string team);
    public static event GoalScored GoalScoredFunctions;
    public static void onGoalScored(string team)
    {
        if (GoalScoredFunctions != null)
            GoalScoredFunctions(team);
    }

    public delegate void GameEnd();
    public static event GameEnd GameEndFunctions;
    public static void onGameEnd()
    {
        if (GameEndFunctions != null)
            GameEndFunctions();
    }
}

