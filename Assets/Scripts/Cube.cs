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

    public event Action<Cube> CubeParametersReset;

    private void Awake()
    {
        _renderer = this.GetComponent<Renderer>();
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    public void Init(ColorChanger colorChanger)
    {
        _colorChanger = colorChanger;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Platform platform))
        {
            if (_haveTouched == false)
            {
                _colorChanger.SetMaterial(_renderer);
                _haveTouched = true;
            }

            StartLifetimeCount();
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
        CubeParametersReset?.Invoke(this);
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
