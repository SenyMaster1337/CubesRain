using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private CubesSpawner _cubesSpawner;
    private ColorChanger _colorChanger;
    private Coroutine _lifetimeCoroutine;

    private int _minLifetime = 2;
    private int _maxLifetime = 6;

    public void Init(CubesSpawner cubesSpawner, ColorChanger colorChanger)
    {
        _cubesSpawner = cubesSpawner;
        _colorChanger = colorChanger;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Platform>() != null)
        {
            if (this.GetComponent<Renderer>().material.color == Color.white)
            {
                _colorChanger.SetMaterial(this.GetComponent<Renderer>());
            }

            StartLifetimeCount();
        }
    }

    public void StartLifetimeCount()
    {
        if (_lifetimeCoroutine != null)
            StopCoroutine(_lifetimeCoroutine);

        _lifetimeCoroutine = StartCoroutine(CountLifetime());
    }

    private IEnumerator CountLifetime()
    {
        float delay = UnityEngine.Random.Range(_minLifetime, _maxLifetime);

        yield return new WaitForSeconds(delay);

        _colorChanger.SetDefaultMaterial(this.GetComponent<Renderer>());
        _cubesSpawner.GetCubeToPool(this);
    }
}
