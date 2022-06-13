using UnityEngine;

public class BoneBulletController : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;

    public void Prepare(Vector2 direction, float speed)
    {
        _direction = direction;
        _speed = speed;
    }

    public void ChangeDirection()
    {
        _direction *= -1;

        gameObject.GetComponent<Rigidbody2D>().AddForce(_direction * 2 * _speed);
    }
}
