using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance  { get; private set; }
    public GameState _currentGameState = GameState.TurnBased;

    public Action<GameState> OnGameStateChanged;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than one Turn System");
            Destroy(gameObject);
        }
        else Instance = this;
    }
    
    public GameState GetGameState()
    {
        return _currentGameState;
    }

    public void SetGameState(GameState gameState)
    {
        _currentGameState = gameState;
        OnGameStateChanged?.Invoke(_currentGameState);
    }
}

public enum GameState{
    TurnBased, 
    Armory, 
    RealTime
}
