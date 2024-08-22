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
            actionOnGet: (enemy) => SpawnEnemy(enemy),
            actionOnRelease: (enemy) => enemy.Deactivate(),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);

        _directions.AddRange(new Vector3[] { Vector3.forward, Vector3.back, Vector3.right, Vector3.left });
    }

    private void OnEnable()
    {
        _isRunning = true;
        _coroutine = StartCoroutine(Spawning());
    }

    private void OnDisable()
    {
        _isRunning = false;

        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private void SpawnEnemy(Enemy enemy)
    {
        Vector3 startPosition = _spawnPoints[Random.Range(0, _spawnPoints.Count)].position;
        enemy.transform.position = startPosition;
        enemy.Activate();
        enemy.SetDirection(_directions[Random.Range(0, _directions.Count)]);
    }

    private void OnReleaseEnemy(Enemy enemy)
    {
        _pool.Release(enemy);
        enemy.DeactivateEnemy -= OnReleaseEnemy;
    }

    private IEnumerator Spawning()
    {
        var wait = new WaitForSeconds(_repeatRate);

        while (_isRunning)
        {
            var enemy = _pool.Get();
            enemy.DeactivateEnemy += OnReleaseEnemy;

            yield return wait;
        }
    }
}
