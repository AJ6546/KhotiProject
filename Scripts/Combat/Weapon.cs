using KhotiProject.Scripts.Combat.Enum;
using UnityEngine;

namespace KhotiProject.Scripts.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] Spawnable spawnable = Spawnable.None;
        [SerializeField] float weaponDamage = 2f;

        public void Spawn(Transform weaponHolder, PoolManager poolManager)
        {
            poolManager.SpawnProjectile(spawnable, weaponHolder.transform.position,
                weaponHolder.transform.rotation);
        }
        
        public float GetDamage()
        {
            return weaponDamage;
        }
    }
}