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

    public UnityEvent onPlayerGameOver;

    PlayerController player;

    private void Start()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
