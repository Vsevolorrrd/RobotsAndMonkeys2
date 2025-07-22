using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameState currentState;
    public event Action<GameState> OnStateChanged;
    public GameState CurrentState => currentState;

    private void Start()
    {
        SetState(GameState.Programming);
    }

    public void SetState(GameState newState)
    {
        if (currentState == newState)
        return;

        currentState = newState;
        Debug.Log("Game state changed to: " + currentState);
        OnStateChanged?.Invoke(currentState);
    }
}
public enum GameState
{
    Programming,
    Executing
}