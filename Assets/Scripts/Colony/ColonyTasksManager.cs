using System.Collections.Generic;
using ColonyActions;
using Grid;
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
        }
        
        public List<ColonyTask> GetColonyTaskList()
        {
            return colonyTaskList;
        }

        public void RegisterTask(GridPosition gridPosition, ColonyActionType colonyActionType, IColonyActionTarget colonyActionTarget)
        {
            colonyTaskList.Add(new ColonyTask(gridPosition, colonyActionType, colonyActionTarget));
        }

        public void RemoveTask(ColonyTask colonyTask)
        {
            colonyTaskList.Remove(colonyTask);
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