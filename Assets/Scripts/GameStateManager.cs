using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private GameState _currentGameState = GameState.TurnBased;

    public GameState GetGameState()
    {
        return _currentGameState;
    }
}



public enum GameState{
    TurnBased, 
    Armory, 
    Bunker
}
