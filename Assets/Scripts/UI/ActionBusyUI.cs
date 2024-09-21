using System;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    [SerializeField] private GameObject busyVisual;

    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChange += UnitActionSystem_OnBusyChange;
        Hide();
    }

    private void UnitActionSystem_OnBusyChange(object sender, bool isBusy)
    {
        if (isBusy) Show();
        else Hide();
    }

    private void Hide()
    {
        busyVisual.SetActive(false);
    }

    private void Show()
    {
        busyVisual.SetActive(true);
    }
}
