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
using TMPro;

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

    [SerializeField]
    private Camera _mainCam;
    [SerializeField]

    private Vector2 _playerPos;
    private Vector3 _camPos;

    public UnityEvent onPlayerGameOver;
    PlayerController player;

    private void Start()
    {
        Instance = this;
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _playerPos = player.transform.position;
        _camPos = _mainCam.transform.position;
    }

    private void FixedUpdate()
    {
        if (player.playerMovingForward)
        {
            _playerPos = player.transform.position;
            _camPos.x = _playerPos.x;
            _mainCam.transform.position = _camPos;
        }
    }

    public void RestartGame() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame() 
    {
        Application.Quit();
    }

    public void PlayerDamage() 
    {
        _lives--;
        player.playerAnimator.SetBool("Damage", true);
        StartCoroutine(PlayerHitDelay());
    }

    IEnumerator PlayerHitDelay() 
    {
        //TODO: Place code here to trigger the player fall over animation

        yield return new WaitForSeconds(_playerHitWaitDelay);

        if (_lives <= 0)
        {
            player.playerAnimator.SetBool("Damage", false);
            player.playerAnimator.SetBool("Die", true);
            onPlayerGameOver.Invoke();
        }
        else 
        {
            player.playerAnimator.SetBool("Damage", false);
            player.transform.position = _gameBeginningTransform.position;
            StartCoroutine(player.EnableControls());
        }
    }
}
