using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _timeToDestroy = 5f;

    private Vector3 _direction;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        gameObject.SetActive(true);
        _rigidbody.velocity = Vector3.zero;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
    }

    internal void SetDirection(Vector3 direction)
    {
        _direction = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider is BoxCollider)
        {
            Destroy(gameObject, _timeToDestroy);
        }
    }
}
