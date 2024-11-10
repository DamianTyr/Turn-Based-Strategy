using System.Collections.Generic;

public class GridObject
{
    private GridSystem<GridObject> _gridSystem;
    private GridPosition _gridPosition;
    private List<Combat.Unit> _unitList;
    private IInteractable _interactable;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
        _unitList = new List<Combat.Unit>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Combat.Unit unit in _unitList)
        {
            unitString += unit + "\n";
        }
        return _gridPosition.ToString() + "\n" + unitString;
    }

    public void AddUnit(Combat.Unit unit)
    {
        _unitList.Add(unit);
    }

    public void RemoveUnit(Combat.Unit unit)
    {
        _unitList.Remove(unit);
    }

    public List<Combat.Unit> GetUnitList()
    {
        return _unitList;
    }

    public bool HasAnyUnit()
    {
        return _unitList.Count > 0;
    }

    public Combat.Unit GetUnit()
    {
        if (HasAnyUnit())
        {
            return _unitList[0];
        }
        else
        {
            return null;
        }
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
