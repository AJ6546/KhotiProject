using KhotiProject.Scripts.Combat;
using KhotiProject.Scripts.Combat.Enum;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float lifeTime = 10;
    [SerializeField] bool followTarget = false;
    [SerializeField] bool locateTarget = false;
    [SerializeField] Spawnable hitEffect = Spawnable.None;
    [SerializeField] int damage = 2;

    PoolManager poolManager;

    Health target = null;
    Transform instigator = null;
    Vector3 targetPosition = Vector3.zero;

    void Awake()
    {
        poolManager = PoolManager.instance;
    }

    void OnEnable()
    {
        StartCoroutine(Disable(lifeTime));
    }

    void Update()
    {
        if (!followTarget && !locateTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }

        if (target == null) { return; }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget()
    {
        targetPosition = instigator.position + instigator.forward * 100;

        if (target == null) { return; }
    }

    public void SetInstantiator(Transform instigator)
    {
        this.instigator = instigator;
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<Health>();
        if (!target) { return; }
        target.TakeDamage(damage);

        gameObject.SetActive(false);
    }

    IEnumerator Disable(float time)
    {
        yield return new WaitForSeconds(time);

        SpawnHitEffect(transform.position);

        gameObject.SetActive(false);
    }

    private void SpawnHitEffect(Vector3 position)
    {
        if (hitEffect != Spawnable.None)
        {
            poolManager.SpawnEffect(hitEffect, position, transform.rotation);
        }
    }
}
