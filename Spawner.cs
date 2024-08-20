using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] private float _repeatRate;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private List<GameObject> _spawnPoints;

    private ObjectPool<GameObject> _pool;
    private List<Vector3> _directions = new List<Vector3>();

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_prefab.gameObject),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);

        _directions.AddRange(new Vector3[] { Vector3.forward, Vector3.back, Vector3.right, Vector3.left });
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetEnemy), 0.0f, _repeatRate);
    }

    private void GetEnemy()
    {
        _pool.Get();
    }

    private void ActionOnGet(GameObject obj)
    {
        Vector3 startPosition = _spawnPoints[Random.Range(0, _spawnPoints.Count)].transform.position;
        obj.transform.position = startPosition;
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);
        obj.GetComponent<Enemy>().SetDirection(_directions[Random.Range(0, _directions.Count)]);
    }

    private void OnTriggerEnter(Collider other)
    {
        _pool.Release(other.gameObject);
    }
}
