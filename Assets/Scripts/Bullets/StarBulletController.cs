using System.Collections;
using UnityEngine;

public class StarBulletController : MonoBehaviour
{
    [SerializeField] private GameObject _trailObj;
    [SerializeField] private TrailRenderer _trail;

    private Vector2 _direction;
    private float _speed;

    private bool _ready = false;

    private float _borderX;
    private float _borderY;

    private bool _onScreen = true;

    void Start()
    {
        _borderX = Camera.main.pixelWidth;
        _borderY = Camera.main.pixelHeight;
    }

    void Update()
    {
        var objPosInCamera = Camera.main.WorldToScreenPoint(transform.TransformPoint(Vector3.zero));

        if (_onScreen)
        {
            if (objPosInCamera.x < 0 || objPosInCamera.x > _borderX)
            {
                _onScreen = false;
                ChangeDirection();
            }

            else if (objPosInCamera.y < 0 || objPosInCamera.y > _borderY)
            {
                _onScreen = false;
                ChangeDirection(false);
            }
        }

        else 
            if (objPosInCamera.x > 0 && objPosInCamera.x < _borderX && objPosInCamera.y > 0 && objPosInCamera.y < _borderY)
                _onScreen = true;
    }

    private void ChangeDirection(bool changeXAxis = true)
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(_direction * -_speed);
        _direction = changeXAxis ? new Vector2(-_direction.x, _direction.y) : new Vector2(_direction.x, -_direction.y);
        gameObject.GetComponent<Rigidbody2D>().AddForce(_direction * _speed);
    }

    public void Shoot(float lifeTime, Vector2 direction, float speed)
    {
        _direction = direction;
        _speed = speed;
        _ready = true;

        StartCoroutine(ShowTrail(lifeTime));
    }











    private IEnumerator ShowTrail(float delay)
    {
        _trail.Clear();

        yield return new WaitForSeconds(0.1f);
        _trail.emitting = true;

        yield return new WaitForSeconds(delay);
        _trail.emitting = false;
    }
}
