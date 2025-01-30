using UnityEngine;

namespace KhotiProject.Scripts.Stats
{
    public class KhotiStats : MonoBehaviour
    {
        public Rigidbody RB;
        public float MinLaunchForce = 1f;
        public float MaxLaunchForce = 10f;
        public float RotationSpeed = 5f;
        public float MaxLaunchTime = 10f;
        public float ForceChangeRate = 3f;
        public float StopTime = 5f;
        public int MaxHealth = 100;
        public int Health = 100;
        public int Defence = 10;
        public int Resistance = 2;

        private void Start()
        {
            Health = MaxHealth;
        }
    }
}