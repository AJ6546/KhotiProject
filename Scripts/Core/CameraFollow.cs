using UnityEngine;

namespace KhotiProject.Scripts.Combat
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] Vector3 offset = new Vector3(0, 5, -10);
        [SerializeField] float smoothSpeed = 0.125f;

        private bool isFollowing = false;

        void LateUpdate()
        {
            if (!isFollowing || target == null)
                return;

            Vector3 rotatedOffset = target.rotation * offset;

            Vector3 desiredPosition = target.position + rotatedOffset;

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;

            transform.LookAt(target);

        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        public void SetCameraFollow()
        {
            isFollowing = !isFollowing;
        }
    }
}