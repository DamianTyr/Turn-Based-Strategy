using UnityEngine;

public class CraftingSpot : MonoBehaviour
{
   public GridPosition craftingSpotGridPosition;
   
   private void Start()
   {
      craftingSpotGridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
   }
}
