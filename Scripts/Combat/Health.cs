using KhotiProject.Scripts.Stats;
using KhotiProject.Scripts.UI;
using KhotiProject.Scripts.UI.OverheadDisplays;
using UnityEngine;

namespace KhotiProject.Scripts.Combat
{
    public class Health : MonoBehaviour
    {
        public bool IsDead = false;

        private AttributesDisplay attributesDisplay;
        private TurnManager turnManager;

        [SerializeField] OverheadHealthBar overheadHealthBar;
        [SerializeField] KhotiStats stats;

        [SerializeField] TakeDamageEvent takeDamage;

        void Start()
        {
            attributesDisplay = FindAnyObjectByType<AttributesDisplay>();
            turnManager = FindAnyObjectByType<TurnManager>();
        }

        public void TakeDamage(float damage)
        {
            if (stats.Health.Equals(0)) { return; }

            stats.Defence = (int) Mathf.Max(stats.Defence - (damage / stats.Resistance), 0);

            if(stats.Defence.Equals(0))
            {
                stats.Resistance = 1;
                stats.Health = (int) Mathf.Max(0, stats.Health - damage);
                takeDamage.Invoke(damage);
            }
            
            attributesDisplay.UpdateForceHit(damage / stats.Resistance);
            overheadHealthBar.UpdateRootCanvas((float) stats.Health / stats.MaxHealth);

            if (stats.Health.Equals(0))
            {
                IsDead = true;
                turnManager.RemoveKhoti(transform);
                transform.parent.gameObject.SetActive(false);
            }
        }
    }
}

