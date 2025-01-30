using KhotiProject.Scripts.Combat;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KhotiProject.Scripts.Controllers.Khoti
{
    public class AIController : BaseController
    {
        private bool actionInitiated = false;
        private bool isSimulating = false;

        private float dragStartTime;

        [SerializeField] List<Health> otherKhotis;

        protected override void Start()
        {
            base.Start();
            otherKhotis = FindObjectsOfType<Health>()
            .Where(khoti => khoti != GetComponent<Health>())
            .ToList();
        }

        void Update()
        {
            if (isLaunched || turnManager == null || turnManager.CurrentPlayerIndex != playerIndex || isHit || actionInitiated || isSimulating) return;

            SimulateAIInput();
        }

        public void SetupTurn()
        {
            arrow.SetActive(true);
            dragStartTime = Time.time;
        }

        private void SimulateAIInput()
        {
            isSimulating = true;
            Health nearestKhoti = FindNearestKhoti();
            float targetXRotation; 
            float targetYRotation;


            if (nearestKhoti == null)
            {
                targetYRotation = Random.Range(0f, 360f);
                targetXRotation = Random.Range(-90f, 0f);
            }
            else
            {
                Vector3 directionToTarget = (nearestKhoti.transform.position - transform.position).normalized;
                targetXRotation = Mathf.Clamp(-directionToTarget.y * 90f, -90f, 0f);
                targetYRotation = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
            }

            StartCoroutine(RotateAndLaunch(targetXRotation, targetYRotation));
        }

        public override void ResetLaunchStatus()
        {
            base.ResetLaunchStatus();
            actionInitiated = false;
        }

        private IEnumerator RotateAndLaunch(float targetXRotation, float targetYRotation)
        {
            float rotationDuration = Random.Range(0.5f, stats.MaxLaunchTime);
            float elapsedTime = 0f;

            Quaternion initialRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(targetXRotation, targetYRotation, 0);

            while (elapsedTime < rotationDuration)
            {
                attributesDisplay.UpdateLaunchTimeLimit((int)(stats.MaxLaunchTime - Time.time + dragStartTime));

                transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / rotationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;

            attributesDisplay.UpdateRotations(targetYRotation, targetXRotation);

            Launch();
            isSimulating = false;
        }

        private Health FindNearestKhoti()
        {
            return otherKhotis
                .Where(khoti => !khoti.IsDead)
                .OrderBy(khoti => Vector3.Distance(transform.position, khoti.transform.position))
                .FirstOrDefault();
        }
    }
}

