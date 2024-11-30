using System.Collections.Generic;
using UnityEngine;

namespace Colony
{
    public class ColonyTasksManager: MonoBehaviour
    {
        public static ColonyTasksManager Instance;
        
        [SerializeField] private List<ColonyTask> colonyTaskList = new();
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There is more then one Colony Grid!" + transform + " - " + Instance);
                Destroy(gameObject);
            }
            Instance = this;
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

        public void RegisterTask(GridPosition gridPosition, ColonyActionType colonyActionType)
        {
            colonyTaskList.Add(new ColonyTask(gridPosition, colonyActionType));
        }

        public List<ColonyTask> GetColonyTaskListOfType(ColonyActionType colonyActionType)
        {
            List<ColonyTask> colonyTasksOfTypeList = new List<ColonyTask>();

            foreach (ColonyTask colonyTask in colonyTaskList)
            {
                if (colonyTask.ActionType == colonyActionType)
                {
                    colonyTasksOfTypeList.Add(colonyTask);
                }
            }

            return colonyTasksOfTypeList;
        }
    }
}