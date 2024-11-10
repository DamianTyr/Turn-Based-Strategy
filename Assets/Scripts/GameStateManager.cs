using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameState _currentGameState = GameState.TurnBased;

    public Action<GameState> OnGameStateChanged;
    
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
    Bunker
}
