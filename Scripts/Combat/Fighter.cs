using UnityEngine;

namespace KhotiProject.Scripts.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] Transform weaponHolder;
        [SerializeField] Weapon defaultWeapon = null;

        Transform target;
        Weapon currentWeapon = null;

        void Start ()
        {
            currentWeapon = defaultWeapon;
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
            transform.LookAt(target.position);

            Health healthComponent = target.GetComponent<Health>();
            healthComponent.TakeDamage(currentWeapon.GetDamage());
        }

        private void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
        }

        public void Cancel()
        {
            target = null;
        }
    }
}