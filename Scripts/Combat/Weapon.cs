using KhotiProject.Scripts.Combat.Enum;
using UnityEngine;

namespace KhotiProject.Scripts.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] Spawnable equipedWeapon = Spawnable.None;
        [SerializeField] Spawnable projectile = Spawnable.None;
        [SerializeField] float weaponDamage = 2f;
        [SerializeField] bool usedInPrimaryWeaponHolder = true;
        public void Spawn(Transform primaryWeaponHolder, Transform secondaryWeaponHolder,
            Animator animator, PoolManager poolManager)
        {
            if(animator != null && animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }

            if(!projectile.Equals(Spawnable.None))
            {
                Transform weaponHolder = GetWeaponHolder(primaryWeaponHolder, secondaryWeaponHolder);

                poolManager.SpawnWeapon(projectile, weaponHolder);
            }
        }

        public void SpawnProjectile(Transform primaryWeaponHolder, Transform secondaryWeaponHolder, PoolManager poolManager)
        {
            if(!projectile.Equals(Spawnable.None))
            {
                Transform weaponHolder = GetWeaponHolder(primaryWeaponHolder, secondaryWeaponHolder);
                poolManager.SpawnProjectile(projectile, weaponHolder.transform.position,
                    weaponHolder.transform.rotation);
            }
        }

        public float GetDamage()
        {
            return weaponDamage;
        }

        private Transform GetWeaponHolder(Transform primaryWeaponHolder, Transform secondaryWeaponHolder)
        {
            return usedInPrimaryWeaponHolder ? primaryWeaponHolder : secondaryWeaponHolder;
        }
    }
}