using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SoundEffect : MonoBehaviour
{
    private UnityAction<GameObject> _onDestroy;

    public void SetInfo(float timeToDestroy, UnityAction<GameObject> onDestroy)
    {
        _onDestroy = onDestroy;

        StartCoroutine(DestroyWithDelay(timeToDestroy));
    }

    private IEnumerator DestroyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        _onDestroy?.Invoke(gameObject);
    }
}
