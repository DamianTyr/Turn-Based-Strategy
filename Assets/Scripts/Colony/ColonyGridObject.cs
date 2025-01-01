using System.Collections.Generic;
using UnityEngine;

public class ColonyGridObject
{
    private GridPosition _gridPosition;
    private GridSystem<ColonyGridObject> _gridSystem;
    private readonly List<Transform> _occupantList;
    private Mineable _mineable;
    private PlacedFurnitureGhost _furnitureGhost;
    private bool _isReserved;
    
    public ColonyGridObject(GridSystem<ColonyGridObject> gridSystem, GridPosition gridPosition) 
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
        _occupantList = new List<Transform>();
    }
    
    public ColonyGridObject()
    {
        
    }
    
    public override string ToString()
    {
        string occupantString = "";
        foreach (Transform occupant in _occupantList)
        {
            occupantString += occupant + "\n";
        }
        return _gridPosition.ToString() + "\n" + occupantString;
    }

    public void AddOccupant(Transform occupant)
    {
        _occupantList.Add(occupant);
    }

    public void RemoveOccupant(Transform occupant)
    {
        _occupantList.Remove(occupant);
    }

    public List<Transform> GetOccupantList()
    {
        return _occupantList;
    }

    public bool HasAnyOccupants()
    {
        return _occupantList.Count > 0;
    }

    public Transform GetOccupant()
    {
        if (HasAnyOccupants())
        {
            return _occupantList[0];
        }
        return null;
    }

    public void SetMineable(Mineable mineable)
    {
        _mineable = mineable;
    }

    public Mineable GetMineable()
    {
        return _mineable;
    }

    public void SetReserved(bool isReserved)
    {
        _isReserved = isReserved;
    }

    public bool GetIsReseved()
    {
        return _isReserved;
    }

    public void SetFurnitureGhost(PlacedFurnitureGhost placedFurnitureGhost)
    {
        _furnitureGhost = placedFurnitureGhost;
    }

    public PlacedFurnitureGhost GetFurnitureGhost()
    {
        return _furnitureGhost;
    }
}
