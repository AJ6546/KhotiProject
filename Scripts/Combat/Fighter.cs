using UnityEngine;

namespace KhotiProject.Scripts.Combat
{
    public class Fighter : MonoBehaviour
    {
        public PoolManager poolManager;

        [SerializeField] Transform primaryWeaponHolder = null;
        [SerializeField] Transform secondaryWeaponHolder = null;
        [SerializeField] Weapon defaultWeapon;
        [SerializeField] Animator animator = null;

        Transform target;
        Weapon currentWeapon;

        void Start ()
        {
            poolManager = PoolManager.instance;
            currentWeapon = defaultWeapon;
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
            transform.LookAt(target.position);

            Health healthComponent = target.GetComponent<Health>();
            healthComponent.TakeDamage(currentWeapon.GetDamage());
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            weapon.Spawn(primaryWeaponHolder, secondaryWeaponHolder, animator, poolManager);
        }

        public void Cancel()
        {
            target = null;
        }
    }
}