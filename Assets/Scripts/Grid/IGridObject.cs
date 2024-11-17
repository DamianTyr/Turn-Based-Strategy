using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGridObject
{
    public void AddOccupant(Transform occupant);

    public void RemoveOccupant(Transform occupant);

    public List<Transform> GetOccupantList();
    
    public bool HasAnyOccupants();
    
    public Transform GetOccupant();
}
