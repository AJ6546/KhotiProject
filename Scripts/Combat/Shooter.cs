using KhotiProject.Scripts.Combat;
using KhotiProject.Scripts.Combat.Enum;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] Spawnable projectileTag = Spawnable.None;

    PoolManager poolManager;

    void Start()
    {
        poolManager = PoolManager.instance;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            var projectileInstance = poolManager.SpawnProjectile(projectileTag, transform.position, transform.rotation);
            projectileInstance.SetInstantiator(transform);
            projectileInstance.SetTarget();
        }
    }
}
