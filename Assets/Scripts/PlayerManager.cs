using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    int _lives;
    [SerializeField]
    string _enemyBulletTagName;
    [SerializeField]
    Transform _gameBeginningTransform;
    [SerializeField]
    [Tooltip("Period of time to wait after the player gets hit to play an animation, before sending them to the start of the level or triggering the game over screen")]
    float _playerHitWaitDelay;

    [SerializeField]
    GameObject _newspaperBulletPrefab;

    public UnityEvent onPlayerGameOver;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(_enemyBulletTagName))
        {
            Destroy(collision.gameObject);
            _lives--;
            StartCoroutine(OnPlayerHit());
        }
    }

    IEnumerator OnPlayerHit()
    {
        //TODO: Place code here to trigger the player fall over animation


        yield return new WaitForSeconds(_playerHitWaitDelay);
        if (_lives == 0)
        {
            onPlayerGameOver.Invoke();
        }
        else
        {
            transform.position = _gameBeginningTransform.position;
        }
    }

}
