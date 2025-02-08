/******************************************************************
 *    Author: Dalsten Yan
 *    Contributors: Mitchell Young
 *    Date Created: 2/8/25
 *    Description: Player movement and teleport controls.
 *******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    int _lives;
    [SerializeField]
    string _enemyBulletTagName;
    [SerializeField]
    Transform _gameBeginningTransform;
    [SerializeField]
    [Tooltip("Period of time to wait after the player gets hit to play an animation, before sending them to the start of the level or triggering the game over screen")]
    float _playerHitWaitDelay;

    public float teleportMeter = 10f;

    [SerializeField]
    private float _refillRate = 0.01f;

    public UnityEvent onPlayerGameOver;

    PlayerController player;

    private void Start()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (teleportMeter < 10)
        {
            teleportMeter += _refillRate;
        }

        if (teleportMeter <= 10)
        {
            return;
        }

        teleportMeter = 10;
    }

    public void RestartGame() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayerDamage() 
    {
        _lives--;
        StartCoroutine(PlayerHitDelay());
    }

    IEnumerator PlayerHitDelay() 
    {
        //TODO: Place code here to trigger the player fall over animation

        yield return new WaitForSeconds(_playerHitWaitDelay);

        if (_lives <= 0)
        {
            onPlayerGameOver.Invoke();
        }
        else 
        {
            player.transform.position = _gameBeginningTransform.position;
            player.EnableControls();
        }
    }
}
