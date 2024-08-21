using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] private float _repeatRate;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private List<Transform> _spawnPoints;

    private ObjectPool<Enemy> _pool;
    private List<Vector3> _directions = new List<Vector3>();

    private Coroutine _coroutine;
    private bool _isRunning = false;

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => SpawnEnemy(obj),
            actionOnRelease: (obj) => obj.ChangeActivity(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);

        _directions.AddRange(new Vector3[] { Vector3.forward, Vector3.back, Vector3.right, Vector3.left });
    }

    private void OnEnable()
    {
        _isRunning = true;
        _coroutine = StartCoroutine(GetEnemy());
    }

    private void OnDisable()
    {
        _isRunning = false;

        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private void SpawnEnemy(Enemy obj)
    {
        Vector3 startPosition = _spawnPoints[Random.Range(0, _spawnPoints.Count)].position;
        obj.transform.position = startPosition;
        obj.ChangeActivity(true);
        obj.SetDirection(_directions[Random.Range(0, _directions.Count)]);
    }

    private void ReleaseEnemy(Enemy enemy)
    {
        _pool.Release(enemy);
        enemy.OnRelease -= ReleaseEnemy;
    }

    private IEnumerator GetEnemy()
    {
        var wait = new WaitForSeconds(_repeatRate);

        while (_isRunning)
        {
            var obj = _pool.Get();
            obj.OnRelease += ReleaseEnemy;

            yield return wait;
        }
    }
}
