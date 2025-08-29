using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private ColorChanger _colorChanger;
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private Coroutine _lifetimeCoroutine;

    private int _minLifetime = 2;
    private int _maxLifetime = 6;
    private bool _haveTouched = false;

    public event Action<Cube> CubeParametersReseted;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Init(ColorChanger colorChanger)
    {
        _colorChanger = colorChanger;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_haveTouched == false)
        {
            if (collision.collider.TryGetComponent(out Platform platform))
            {
                _haveTouched = true;
                _colorChanger.SetMaterial(_renderer);
                StartLifetimeCount();
            }
        }
    }

    private void StartLifetimeCount()
    {
        if (_lifetimeCoroutine != null)
            StopCoroutine(_lifetimeCoroutine);

        _lifetimeCoroutine = StartCoroutine(CountLifetime());
    }

    private IEnumerator CountLifetime()
    {
        float delay = UnityEngine.Random.Range(_minLifetime, _maxLifetime);

        yield return new WaitForSeconds(delay);

        ResetParameters();
        CubeParametersReseted?.Invoke(this);
    }

    private void ResetParameters()
    {
        _colorChanger.SetDefaultMaterial(_renderer);
        this.transform.rotation = Quaternion.identity;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _haveTouched = false;
    }
}
