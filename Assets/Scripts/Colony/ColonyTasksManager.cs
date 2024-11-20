using System.Collections.Generic;
using UnityEngine;

namespace Colony
{
    public class ColonyTasksManager: MonoBehaviour
    {
        [SerializeField] private List<ColonyTask> colonyTaskList = new();
        
        private void Awake()
        {
            Mineable.OnAnyMineableSpawned += OnAnyMineableSpawned;
        }
        
        private void OnAnyMineableSpawned(GridPosition gridPosition)
        {
            colonyTaskList.Add(new ColonyTask(gridPosition, ColonyActionType.Mining));
        }

        public List<ColonyTask> GetColonyTaskList()
        {
            return colonyTaskList;
        }
    }
}