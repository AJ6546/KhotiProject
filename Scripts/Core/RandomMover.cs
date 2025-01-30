using System.Collections;
using UnityEngine;

namespace KhotiProject.Scripts.Combat
{
    public class RandomMover : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 2f;
        [SerializeField] float range = 3f;
        [SerializeField] float waitTime = 2f;
        [SerializeField] float xMin, xMax;
        [SerializeField] float yMin, yMax;
        [SerializeField] float zMin, zMax;

        private Vector3 targetPosition;
        private bool isWaiting = false;

        void Start()
        {
            CalculateNewTargetPosition();
        }

        void Update()
        {
            if (!isWaiting)
            {
                MoveTowardsTarget();
            }
        }

        private void CalculateNewTargetPosition()
        {
            Vector3 currentPosition = transform.position;

            float randomX;
            float randomY;
            float randomZ;

            do
            {
                randomX = Random.Range(currentPosition.x - range, currentPosition.x + range);
                randomY = Random.Range(currentPosition.y - range, currentPosition.y + range);
                randomZ = Random.Range(currentPosition.z - range, currentPosition.z + range);
            }
            while (
                randomX < xMin || randomX > xMax ||
                randomY < yMin || randomY > yMax ||
                randomZ < zMin || randomZ > zMax
            );

            targetPosition = new Vector3(randomX, randomY, randomZ);
        }

        private void MoveTowardsTarget()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Mathf.Approximately(Vector3.Distance(transform.position, targetPosition), 0f))
            {
                StartCoroutine(WaitBeforeNextMove());
            }
        }

        private IEnumerator WaitBeforeNextMove()
        {
            isWaiting = true;
            yield return new WaitForSeconds(waitTime);
            isWaiting = false;
            CalculateNewTargetPosition();
        }
    }
}