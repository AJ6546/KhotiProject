using KhotiProject.Scripts.Combat;
using KhotiProject.Scripts.Combat.Enum;
using UnityEngine;

namespace KhotiProject.Scripts.UI.DamageTxt
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;
        PoolManager poolManager;

        void Start()
        {
            poolManager = PoolManager.instance;
        }

        public void Spawn(float damageAmount)
        {
            var instance = poolManager.SpawnEffect(Spawnable.DamageText, transform.position, Quaternion.identity);
            instance.GetComponent<DamageText>().SetValue(damageAmount);
        }
    }
}
