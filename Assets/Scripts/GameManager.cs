using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameState currentState;
    public event Action<GameState> OnStateChanged;
    public event Action OnGameReset;
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

    public void ResetLevel()
    {
        CodeManager.Instance.StopAllCoroutines();
        SetState(GameState.Programming);
        OnGameReset?.Invoke();
    }
}
public enum GameState
{
    Programming,
    Executing
}