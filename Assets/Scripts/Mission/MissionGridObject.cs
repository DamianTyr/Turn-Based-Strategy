using System.Collections.Generic;
using UnityEngine;

public class MissionGridObject : IGridObject
{
    protected GridPosition _gridPosition;
    protected GridSystem<MissionGridObject> _gridSystem;
    protected List<Transform> _occupantList;

    private IInteractable _interactable;
    
    public MissionGridObject(GridSystem<MissionGridObject> gridSystem, GridPosition gridPosition)
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
        _occupantList = new List<Transform>();
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
    
    public IInteractable GetInteractable()
    {
        return _interactable;
    }

    public void SetInteractable(IInteractable interactable)
    {
        _interactable = interactable;
    }
}
