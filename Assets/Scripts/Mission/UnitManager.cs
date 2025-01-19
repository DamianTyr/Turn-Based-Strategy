using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mission
{
    public class UnitManager : MonoBehaviour
    {
        public static UnitManager Instance { get; private set; }

        private List<Unit> unitList;
        private List<Unit> friendlyUnitList;
        private List<Unit> enemyUnitList;
    
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There is more then one Unit Manager!" + transform + " - " + Instance);
                Destroy(gameObject);
            }
            Instance = this;
        
            unitList = new List<Unit>();
            friendlyUnitList = new List<Unit>();
            enemyUnitList = new List<Unit>();
        }

        private void Start()
        {
            Unit.OnAnyUnitSpawned += Unit_OnOnAnyUnitSpawned;
            Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
        }

        private void Unit_OnAnyUnitDead(object sender, EventArgs e)
        {
            Unit unit = sender as Unit;
            unitList.Remove(unit);
        
            if (unit.IsEnemy())
            {
                enemyUnitList.Remove(unit);
            }
            else
            {
                friendlyUnitList.Remove(unit);
            }
        }

        private void Unit_OnOnAnyUnitSpawned(object sender, EventArgs e)
        {
            Unit unit = sender as Unit;
            unitList.Add(unit);
        
            if (unit.IsEnemy())
            {
                enemyUnitList.Add(unit);
            }
            else
            {
                friendlyUnitList.Add(unit);
            }
        }

        public List<Unit> GetUnitList()
        {
            return unitList;
        }

        public List<Unit> GetFriendlyUnitList()
        {
            return friendlyUnitList;
        }

        public List<Unit> GetEnemyUnitList()
        {
            return enemyUnitList;
        }
    }
}
