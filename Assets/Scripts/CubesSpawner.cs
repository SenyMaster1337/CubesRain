using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubesSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private ColorChanger _colorChanger;
    [SerializeField] private float _delay;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private float _setRainTimeInSecond;

    private ObjectPool<Cube> _cubePool;
    private Coroutine _lifetimeCoroutine;
    private int _minRandomPositionX = -10;
    private int _maxRandomPositionX = 10;
    private int _minRandomPositionZ = -10;
    private int _maxRandomPositionZ = 10;
    private int _positionY = 10;
    private float _realTimeRainTime = 0;

    private void Awake()
    {
        _cubePool = new ObjectPool<Cube>
            (
            createFunc: () => CreateFunc(),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => ActionOnDestroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
            );
    }

    private void Start()
    {
        StartCubesSpawnCount();
    }

    private void Update()
    {
        if (_realTimeRainTime < _setRainTimeInSecond)
        _realTimeRainTime += Time.deltaTime;
    }

    private void StartCubesSpawnCount()
    {
        if (_lifetimeCoroutine != null)
            StopCoroutine(_lifetimeCoroutine);

        _lifetimeCoroutine = StartCoroutine(CountCubesSpawn());
    }

    private IEnumerator CountCubesSpawn()
    {
        while (_realTimeRainTime <= _setRainTimeInSecond)
        {
            yield return new WaitForSeconds(_delay);
            SpawnCube();
        }
    }

    private Cube CreateFunc()
    {
        Cube cube = Instantiate(_cubePrefab);
        cube.Init(_colorChanger);
        cube.GetCube += ReleaseCubeToPool;

        return cube;
    }

    private void ActionOnGet(Cube cube)
    {
        int randomPositionX = UnityEngine.Random.Range(_minRandomPositionX, _maxRandomPositionX);
        int randomPositionZ = UnityEngine.Random.Range(_minRandomPositionZ, _maxRandomPositionZ);

        cube.transform.position = new Vector3(randomPositionX, _positionY, randomPositionZ);
        cube.gameObject.SetActive(true);
    }

    private void ActionOnDestroy(Cube cube)
    {
        cube.GetCube -= ReleaseCubeToPool;
        Destroy(cube.gameObject);
    }

    private void SpawnCube()
    {
        _cubePool.Get();
    }

    public void ReleaseCubeToPool(Cube cube)
    {
        _cubePool.Release(cube);
    }
}
