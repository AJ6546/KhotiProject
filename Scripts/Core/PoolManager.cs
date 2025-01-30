using KhotiProject.Scripts.Combat.Enum;
using System.Collections.Generic;
using UnityEngine;

namespace KhotiProject.Scripts.Combat
{
    public class PoolManager : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public int size;
            public GameObject prefab;
            public Spawnable tag;
            public Vector3 minPos, maxPos;
            public bool activateOnStart;
        }
        public static PoolManager instance;
        void Awake()
        {
            instance = this;
        }
        public Dictionary<Spawnable, Queue<GameObject>> pooldictionary;
        [SerializeField] List<Pool> pools = new List<Pool>();
        void Start()
        {
            pooldictionary = new Dictionary<Spawnable, Queue<GameObject>>();
            foreach (Pool pool in pools)
            {
                Vector3 minPosition = pool.minPos;
                Vector3 maxPosition = pool.maxPos;

                Queue<GameObject> objPool = new Queue<GameObject>();
                for (int i = 0; i < pool.size; i++)
                {
                    Vector3 pos = new Vector3(Random.Range(minPosition.x, maxPosition.x),
                        Random.Range(minPosition.y, maxPosition.y), Random.Range(minPosition.z, maxPosition.z));
                    GameObject prefab = Instantiate(pool.prefab, pos, pool.prefab.transform.rotation);

                    bool setActive = pool.activateOnStart ? true : false;
                    prefab.SetActive(setActive);

                    objPool.Enqueue(prefab);
                }
                pooldictionary.Add(pool.tag, objPool);
            }
        }

        public Projectile SpawnProjectile(Spawnable tag, Vector3 position, Quaternion rotation)
        {
            GameObject obj = SetInstantiatedObject(tag, position, rotation);
            return obj.GetComponent<Projectile>();
        }

        public GameObject SpawnEffect(Spawnable tag, Vector3 position, Quaternion rotation)
        {
            return SetInstantiatedObject(tag, position, rotation);
        }

        public void PlaySoundEffect(Spawnable tag, Vector3 position)
        {
            Debug.Log(tag);
            GameObject obj = pooldictionary[tag].Dequeue();
            Debug.Log(obj);
            obj.transform.position = position;
            obj.GetComponent<AudioSource>().Play();
        }

        private GameObject SetInstantiatedObject(Spawnable tag, Vector3 position, Quaternion rotation)
        {
            GameObject obj = pooldictionary[tag].Dequeue();
            obj.SetActive(true);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            var anim = obj.GetComponent<Animation>();
            if (anim != null)
            {
                anim.Play();
            }

            pooldictionary[tag].Enqueue(obj);
            return obj;
        }
    }
}