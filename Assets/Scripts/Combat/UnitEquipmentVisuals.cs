using UnityEngine;

namespace Combat
{
    public class UnitEquipmentVisuals : MonoBehaviour
    {
        [SerializeField] private Transform weaponSpawnPointTransform;
        [SerializeField] private GameObject weaponVisual;
    
        public GameObject SpawnWeaponVisual(GameObject weaponVisualPrefab)
        {
            if (weaponVisual != null) Destroy(weaponVisual.gameObject);
            weaponVisual = Instantiate(weaponVisualPrefab, weaponSpawnPointTransform);
            return weaponVisual;
        }

        public void DestroyWeaponVisual()
        {
            if (weaponVisual != null) Destroy(weaponVisual.gameObject);
        }
    }
}
