using System;
using Mission;
using UnityEngine;

namespace Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        private enum State
        {
            WaitingForEnemyTurn, 
            TakingTurn, 
            Busy,
        }
    
        private float _timer;
        private State _state;

        private void Awake()
        {
            _state = State.WaitingForEnemyTurn;
        }

        private void Start()
        {
            TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        }

        private void TurnSystem_OnTurnChange(object sender, EventArgs e)
        {
            if (!TurnSystem.Instance.IsPlayerTurn())
            {
                _state = State.TakingTurn;
                _timer = 2f;
            }
        }

        private void Update()
        {
            if (TurnSystem.Instance.IsPlayerTurn()) return;

            switch (_state)
            {
                case State.WaitingForEnemyTurn:
                    break;
                case State.TakingTurn:
                    _timer -= Time.deltaTime;
                    if (_timer <= 0f)
                    {
                        if (TryTakeEnemyAIAction(SetStateTakingTurn))
                        {
                            _state = State.Busy;
                        }
                        else
                        {
                            TurnSystem.Instance.NextTurn();
                        }
                    }
                    break;
                case State.Busy:
                    break;
            }
        }

        private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
        {
            foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())   
            {
                if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete)) return true;
            }

            return false;
        }
    
        private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
        {
            AIAction bestAIAction = null;
            BaseAction bestBaseAction = null;
        
            foreach (BaseAction baseAction in enemyUnit.GetActonHolder().GetBaseActionList())
            {
                if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
                {
                    //Enemy cannot afford this action;
                    continue;
                }

                if (bestAIAction == null)
                {
                    bestAIAction = baseAction.GetBestAIAction();
                    bestBaseAction = baseAction;
                }
                else
                {
                    AIAction testAIAction = baseAction.GetBestAIAction();
                    if (testAIAction != null && testAIAction.ActionValue > bestAIAction.ActionValue)
                    {
                        bestAIAction = baseAction.GetBestAIAction();
                        bestBaseAction = baseAction;
                    }
                }
            }

            if (bestAIAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
            {
                bestBaseAction.TakeAction(enemyUnit.GetGridPosition(), bestAIAction.GridPosition, onEnemyAIActionComplete);
                return true;
            }
            return false;
        }

        private void SetStateTakingTurn()
        {
            _timer = .5f;
            _state = State.TakingTurn;
        }
    }
}
