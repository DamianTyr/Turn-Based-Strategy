using Colony;
using Grid;
using UnityEngine;

namespace ColonyBuilding
{
   public class CraftingSpot : MonoBehaviour
   {
      public GridPosition craftingSpotGridPosition;
   
      private void Start()
      {
         craftingSpotGridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
      }
   }
}
