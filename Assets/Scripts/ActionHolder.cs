using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionHolder : MonoBehaviour
{
    private List<BaseAction> _baseActionList = new();
    private EquipmentSetupManager _equipmentSetupManager;
    
    public static Action OnAnyActionListChanged;

    private void Start()
    {
        _equipmentSetupManager = GetComponent<EquipmentSetupManager>();
        _equipmentSetupManager.onEquipmentSetup += OnEquipmentSetup;
        UpdateActionList();
    }
    
    private void OnEquipmentSetup()
    {
        StartCoroutine(UpdateActionListNextFrame());
    }
    
    private void UpdateActionList()
    {
        BaseAction[] baseActionArray = GetComponents<BaseAction>();
        _baseActionList = baseActionArray.ToList();
        OnAnyActionListChanged?.Invoke();
    }
    
    private IEnumerator UpdateActionListNextFrame()
    {
        yield return null;
        UpdateActionList();
    }
    
    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in _baseActionList)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }
    
    public List<BaseAction>GetBaseActionList()
    {
        return _baseActionList;
    }
}
