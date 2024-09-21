using System;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
   public static Pathfinding Instance { get; private set; } 
   
   private const int MoveStraightCost = 10;
   private const int MoveDiagonalCost = 14;
   
   [SerializeField] private Transform gridDebugObjectPrefab;
   [SerializeField] private LayerMask obstaclesLayerMask;
   
   private int _width;
   private int _height;
   private float _cellSize;
   private GridSystem<PathNode> _gridSystem;

   private void Awake()
   {
      if (Instance != null)
      {
         Debug.LogError("There is more then one Pathfinding!" + transform + " - " + Instance);
         Destroy(gameObject);
      }
      else Instance = this;
   }
   
   public void Setup(int width, int height, float cellSize)
   {
      _width = width;
      _height = height;
      _cellSize = cellSize;
      
      _gridSystem = new GridSystem<PathNode>(_width, _height, _cellSize, (GridSystem<PathNode> gridSystem, GridPosition gridPosition) => new PathNode(gridPosition));
      //_gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

      for (int x = 0; x < width; x++)
      {
         for (int z = 0; z < height; z++)
         {
            GridPosition gridPosition = new GridPosition(x, z);
            Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);

            float raycastOffsetDistance = 5f;
            if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up,
                   raycastOffsetDistance * 2, obstaclesLayerMask))
            {
               GetNode(x, z).SetIsWalkable(false);  
            }
         }
      }
   }

   public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLenght)
   {
      List<PathNode> openList = new List<PathNode>();
      List<PathNode> closedList = new List<PathNode>();

      PathNode startNode = _gridSystem.GetGridObject(startGridPosition);
      PathNode endNode = _gridSystem.GetGridObject(endGridPosition);
      openList.Add(startNode);

      for (int x = 0; x < _gridSystem.GetWidth(); x++)
      {
         for (int z = 0; z < _gridSystem.GetHeight(); z++)
         {
            GridPosition gridPosition = new GridPosition(x, z);
            PathNode pathNode = _gridSystem.GetGridObject(gridPosition);
            pathNode.SetGCost(int.MaxValue);
            pathNode.SetHCost(0);
            pathNode.CalculateFCost();
            pathNode.ResetCameFromPathNode();
         }
      }
      
      startNode.SetGCost(0);
      startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
      startNode.CalculateFCost();

      while (openList.Count > 0)
      {
         PathNode currentNode = GetLowestFCostPathNode(openList);
         if (currentNode == endNode)
         {
            //Reached final Node
            pathLenght = endNode.GetFCost();
            return CalculatePath(endNode);
         }

         openList.Remove(currentNode);
         closedList.Add(currentNode);

         foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
         {
            if (closedList.Contains(neighbourNode)) continue;
            if (!neighbourNode.IsWalkable())
            {
               closedList.Add(neighbourNode);
               continue;
            }

            int tentativeGCost = currentNode.GetGCost() +
                                 CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

            if (tentativeGCost < neighbourNode.GetGCost())
            {
               neighbourNode.SetCameFromPathNode(currentNode);
               neighbourNode.SetGCost(tentativeGCost);
               neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
               neighbourNode.CalculateFCost();

               if (!openList.Contains(neighbourNode))
               {
                  openList.Add(neighbourNode);
               }
            }
         }
      }
      
      //NoPathFound
      pathLenght = 0;
      return null;
   }

   public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
   {
      GridPosition gridPostionDistance = gridPositionA - gridPositionB;
      int xDistance = Mathf.Abs(gridPostionDistance.X);
      int zDistance = Mathf.Abs(gridPostionDistance.Z);
      int remaining = Mathf.Abs(xDistance - zDistance);

      return MoveDiagonalCost * Mathf.Min(xDistance, zDistance) + MoveStraightCost * remaining;
   }

   private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
   {
      PathNode lowestFCostPathNode = pathNodeList[0];
      for (int i = 0; i < pathNodeList.Count; i++)
      {
         if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
         {
            lowestFCostPathNode = pathNodeList[i];
         }
      }
      return lowestFCostPathNode;
   }

   private PathNode GetNode(int x, int z)
   {
      return _gridSystem.GetGridObject(new GridPosition(x, z));
   }
   
   private List<PathNode> GetNeighbourList(PathNode currentNode)
   {
      List<PathNode> neighbourList = new List<PathNode>();
      GridPosition gridPosition = currentNode.GetGridPosition();

      if (gridPosition.X - 1 >= 0)
      {
         //Left
         neighbourList.Add(GetNode(gridPosition.X - 1, gridPosition.Z + 0));

         if (gridPosition.Z - 1 >= 0)
         {
            //Left Down
            neighbourList.Add(GetNode(gridPosition.X - 1, gridPosition.Z - 1));
         }

         if (gridPosition.Z + 1 < _gridSystem.GetHeight())
         {
            //Left Up
            neighbourList.Add(GetNode(gridPosition.X - 1, gridPosition.Z + 1));
         }
      }

      if (gridPosition.X + 1 < _gridSystem.GetWidth())
      {
         //Right
         neighbourList.Add(GetNode(gridPosition.X + 1, gridPosition.Z + 0));
         
         if (gridPosition.Z - 1 >= 0)
         {
            //Right Down
            neighbourList.Add(GetNode(gridPosition.X + 1, gridPosition.Z - 1));
         }
         if (gridPosition.Z + 1 < _gridSystem.GetHeight())
         {
            //Right Up
            neighbourList.Add(GetNode(gridPosition.X + 1, gridPosition.Z + 1));
         }
      }
      
      if (gridPosition.Z - 1 >= 0)
      {
         //Down
         neighbourList.Add(GetNode(gridPosition.X + 0, gridPosition.Z - 1));
      }
      
      if (gridPosition.Z + 1 < _gridSystem.GetHeight())
      {
         //Up
         neighbourList.Add(GetNode(gridPosition.X + 0, gridPosition.Z + 1));
      }
      return neighbourList;
   }
   
   private List<GridPosition> CalculatePath(PathNode endNode)
   {
      List<PathNode> pathNodeList = new List<PathNode>();
      pathNodeList.Add(endNode);
      PathNode currentNode = endNode;
      while (currentNode.GetCameFromPathNode() != null)
      {
         pathNodeList.Add(currentNode.GetCameFromPathNode());
         currentNode = currentNode.GetCameFromPathNode();
      }
      
      pathNodeList.Reverse();

      List<GridPosition> gridPositionList = new List<GridPosition>();

      foreach (PathNode pathNode in pathNodeList)
      {
         gridPositionList.Add(pathNode.GetGridPosition());
      }

      return gridPositionList;
   }

   public bool IsWalkableGridPosition(GridPosition gridPosition)
   {
      return _gridSystem.GetGridObject(gridPosition).IsWalkable();
   }

   public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
   {
      return FindPath(startGridPosition, endGridPosition, out int pathLenght) != null;
   }

   public int GetPathLenght(GridPosition startGridPosition, GridPosition endGridPosition)
   {
      FindPath(startGridPosition, endGridPosition, out int pathLenght);
      return pathLenght;
   }

   public void SetIsWalkableGridPosition(GridPosition gridPosition, bool isWalkable)
   {
      _gridSystem.GetGridObject(gridPosition).SetIsWalkable(isWalkable);
   }
}
