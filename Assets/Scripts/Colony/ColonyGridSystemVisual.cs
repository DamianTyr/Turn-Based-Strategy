using System.Collections.Generic;
using ColonyBuilding;
using Grid;
using UnityEngine;

namespace Colony
{
    public class ColonyGridSystemVisual : BaseGridSystemVisual
    {
        private static ColonyGridSystemVisual Instance { get; set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There is more then one Grid System Visual!" + transform + " - " + Instance);
                Destroy(gameObject);
            }

            Instance = this;
        }

        protected override void Start()
        {
            base.Start();
            FurniturePlacer.OnAnyGhostManipulated += FurniturePlacerOnOnAnyGhostManipulated;
            FurniturePlacer.OnPlacingFurnitureDisabled += FurniturePlacerOnOnPlacingFurnitureDisabled;
        }

        private void FurniturePlacerOnOnPlacingFurnitureDisabled()
        {
            HideAllGridPosition();
        }

        private void FurniturePlacerOnOnAnyGhostManipulated(List<GridPosition> gridPositions)
        {
            UpdateGridVisual();
            ShowGridPositionList(gridPositions, GridVisualType.Blue);
        }

        private void OnDestroy()
        {
            FurniturePlacer.OnAnyGhostManipulated -= FurniturePlacerOnOnAnyGhostManipulated;
            FurniturePlacer.OnPlacingFurnitureDisabled -= FurniturePlacerOnOnPlacingFurnitureDisabled;
        }
    }
}
