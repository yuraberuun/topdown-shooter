using System.Collections;
using UnityEngine;

public class BoomerangBulletController : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;

    private float currentSpeed;

    public void Prepare(Vector2 direction, float speed, float changeDirectionTime)
    {
        _direction = direction;
        _speed = speed;

        StartCoroutine(ChangeDirection(changeDirectionTime));
    }

    private IEnumerator ChangeDirection(float delay)
    {
        yield return new WaitForSeconds(delay);

        GetComponent<ParticleSystem>().Play();

        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.002f);
            gameObject.GetComponent<Rigidbody2D>().AddForce(_direction * _speed / 100);
        }
    }   
}
