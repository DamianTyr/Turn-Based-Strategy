using System;
using UnityEngine;


public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance  { get; private set; }

    public event EventHandler OnTurnChange;
    
    private int _turnNumber = 1;

    private bool isPlayerTurn = true;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than one Turn System");
            Destroy(gameObject);
        }
        else Instance = this;
    }
    
    public void NextTurn()
    {
        _turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        OnTurnChange(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return _turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
