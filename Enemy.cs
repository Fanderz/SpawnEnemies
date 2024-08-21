using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;

    private Vector3 _direction;
    private Rigidbody _rigidbody;

    public event Action<Enemy> OnRelease;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = Vector3.zero;
    }

    private void Update()
    {
        Rotate();
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider is BoxCollider)
        {
            OnRelease?.Invoke(this);
        }
    }

    internal void SetDirection(Vector3 direction) =>
        _direction = direction;

    internal void ChangeActivity(bool status) =>
        gameObject.SetActive(status);

    private void Move()
    { 
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector3 lookDirection = transform.position + _direction - transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        transform.rotation = rotation;
    }
}
