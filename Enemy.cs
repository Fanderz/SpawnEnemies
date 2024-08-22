using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;

    private Vector3 _direction;
    private Rigidbody _rigidbody;

    public event Action<Enemy> DeactivateEnemy;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider is BoxCollider)
        {
            DeactivateEnemy?.Invoke(this);
        }
    }

    public void Activate() =>
        SetActivity(true);

    public void Deactivate() =>
        SetActivity(false);

    internal void SetDirection(Vector3 direction)
    {
        _direction = direction;
        Rotate();
    }

    private void SetActivity(bool status) =>
        gameObject.SetActive(status);

    private void Move() =>
        _rigidbody.MovePosition(transform.position + _direction * _speed * Time.deltaTime);

    private void Rotate() =>
        transform.rotation = Quaternion.LookRotation(transform.position + _direction - transform.position, Vector3.up);
}
