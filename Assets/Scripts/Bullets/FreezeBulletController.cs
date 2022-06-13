using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FreezeBulletController : MonoBehaviour
{
    private float _freezeTime = 1f;
    private float _timer = 1f;

    [SerializeField] private Transform _rayWrap;
    [SerializeField] private Animator _animator;

    private UnityAction<GameObject> _destroyAction;

    private bool _ready = false;

    private void OnEnable()
    {
        _rayWrap.localScale = new Vector2(1f, 0f);
    }

    internal void Shoot(float freezeTime, float lifeTime, UnityAction<GameObject> destroyAction)
    {
        _freezeTime = freezeTime;
        _timer = lifeTime;
        _destroyAction = destroyAction;
        _ready = false;
    }

    private void Update()
    {
        if (_ready)
        {
            if (_timer < 0)
            {
                MakeHideAnim();
            }

            _timer -= Time.deltaTime;
        }
        else
            MakeAnimation();
    }

    private void MakeAnimation()
    {
        if (_rayWrap.localScale.y < 15f)
            _rayWrap.localScale = new Vector2(_rayWrap.localScale.x, _rayWrap.localScale.y + .55f);
        else
            _ready = true;
    }

    private void MakeHideAnim()
    {
        _animator.SetTrigger("Hide");
        var clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
        var animationTime = clipInfo.Length > 1 ? clipInfo[1].clip.length : 0.5f;
        StartCoroutine(WaitForEndAnim(animationTime));
    }

    private IEnumerator WaitForEndAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
        _ready = false;
        DestroyBullet();
    }

    private void DestroyBullet() => _destroyAction?.Invoke(gameObject);

    internal float GetFreezeTime() => _freezeTime;
}
