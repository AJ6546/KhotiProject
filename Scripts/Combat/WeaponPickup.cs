using UnityEngine;

namespace KhotiProject.Scripts.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<Picker>())
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                gameObject.SetActive(false);
            }
        }
    }
}

