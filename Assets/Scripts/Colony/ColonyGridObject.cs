using System.Collections.Generic;
using UnityEngine;

public class ColonyGridObject
{
    protected GridPosition gridPosition;
    protected GridSystem<ColonyGridObject> gridSystem;
    protected readonly List<Transform> _occupantList;
    protected Mineable _mineable;
    protected bool isReserved = false;
    
    public ColonyGridObject(GridSystem<ColonyGridObject> gridSystem, GridPosition gridPosition) 
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        _occupantList = new List<Transform>();
    }
    
    public override string ToString()
    {
        string occupantString = "";
        foreach (Transform occupant in _occupantList)
        {
            occupantString += occupant + "\n";
        }
        return gridPosition.ToString() + "\n" + occupantString;
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
        this.isReserved = isReserved;
    }

    public bool GetIsReseved()
    {
        return isReserved;
    }
}
