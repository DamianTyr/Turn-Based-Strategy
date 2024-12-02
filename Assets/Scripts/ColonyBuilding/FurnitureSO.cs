using UnityEngine;

[CreateAssetMenu(menuName = "Colony Building/Furniture")]
public class FurnitureSO : ScriptableObject
{
    [SerializeField] public Transform furnitureGhost;
    [SerializeField] public PlacedFurnitureGhost placedFurnitureGhost;
    [SerializeField] public Furniture furniture;
    [SerializeField] public Vector2 dimensions;
}
