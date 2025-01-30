using KhotiProject.Scripts.Constants;
using KhotiProject.Scripts.Combat;
using KhotiProject.Scripts.Stats;
using KhotiProject.Scripts.UI;
using System.Collections;
using UnityEngine;

namespace KhotiProject.Scripts.Controllers.Khoti
{
    public class BaseController : MonoBehaviour
    {
        public KhotiStats stats;
        public GameObject arrow;

        [SerializeField] Health health;
        [SerializeField] CameraFollow cameraFollow;

        protected AttributesDisplay attributesDisplay;
        protected TurnManager turnManager;

        protected bool isLaunched = true;
        protected bool isHit = false;
        protected int playerIndex;

        protected virtual void Start()
        {
            cameraFollow = Camera.main.GetComponent<CameraFollow>();
            attributesDisplay = FindAnyObjectByType<AttributesDisplay>();
            turnManager = FindAnyObjectByType<TurnManager>();
        }

        public void SetPlayerIndex(int index)
        {
            playerIndex = index;
        }

        public virtual void ResetLaunchStatus()
        {
            isLaunched = !isLaunched;
        }

        public void IsHit()
        {
            if(health.IsDead) return;
            isHit = true;
            StartCoroutine(StopKhotiAfterDelay(false));
        }

        public void Launch()
        {
            isLaunched = !isLaunched;

            cameraFollow?.SetCameraFollow();

            attributesDisplay.UpdateTurnStart(false);

            arrow.SetActive(false);

            if (stats.RB != null)
            {
                stats.RB.AddForce(transform.forward * attributesDisplay.CalculateForce(), ForceMode.Impulse);
                StartCoroutine(StopKhotiAfterDelay());
            }
        }

        protected IEnumerator StopKhotiAfterDelay(bool endTurn = true)
        {
            yield return new WaitForSeconds(stats.StopTime);

            ResetKhotiMotion();

            if (endTurn)
            {
                turnManager.NextTurn();
            }
            if (isHit)
            {
                isHit = !isHit;
            }
        }

        protected void ResetKhotiMotion()
        {
            if (stats.RB != null)
            {
                stats.RB.velocity = Vector3.zero;
                stats.RB.angularVelocity = Vector3.zero;

                Vector3 forwardDirection = transform.forward;
                forwardDirection.y = 0;

                if (forwardDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(forwardDirection.normalized, Vector3.up);
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(TagConstants.KhotiTag))
            {
                IsHit();

                var otherGO = other.gameObject;
                var otherGOController = otherGO.GetComponent<BaseController>();
                otherGOController.IsHit();
                if (otherGOController.playerIndex.Equals(turnManager.CurrentPlayerIndex)) return;
                var relativeVelocity = other.relativeVelocity.magnitude;
                float adjustedForce = relativeVelocity * GetComponent<Rigidbody>().mass;
                otherGO.GetComponent<Health>().TakeDamage(adjustedForce);
            }
        }
    }
}

