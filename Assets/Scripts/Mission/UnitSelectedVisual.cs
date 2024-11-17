using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mission
{
    public class UnitSelectedVisual : MonoBehaviour
    {
        [FormerlySerializedAs("transform")] [SerializeField] private Unit unit;
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
            UpdateVisual();
        }

        private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
            {
                meshRenderer.enabled = true;
            }
            else
            {
                meshRenderer.enabled = false;
            }
        }

        private void OnDestroy()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
        }
    }
}
