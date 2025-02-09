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
    public float teleportMeter = 10f;
    [SerializeField]
    private float _refillRate = 0.01f;
    [SerializeField]
    private TMP_Text _meterText;

    [SerializeField]
    private Camera _mainCam;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private float _checkAdjust = 10f;
    [SerializeField]
    private GameObject _levelChecker;
    private RoomCheckerCollision _levelCheckerScript;

    [SerializeField]
    private float _defaultStartPos = -7f;
    [SerializeField]
    private float _startAreaCameraOffset = 5f;
    [SerializeField]
    private float _levelCheckPosOffset = 5f;

    private Vector2 _playerPos;
    private Vector3 _camPos;

    public UnityEvent onPlayerGameOver;
    PlayerController player;

    private void Start()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _levelCheckerScript = _levelChecker.GetComponent<RoomCheckerCollision>();
        _levelCheckerScript.startAreaPos.x = _defaultStartPos;
    }

    private void Awake()
    {
        _playerPos = _player.transform.position;
        _camPos = _mainCam.transform.position;
    }

    private void FixedUpdate()
    {
        if (!_levelCheckerScript.atEndOfArea)
        {
            _playerPos = _player.transform.position;

            if (_player.transform.position.x >= _levelCheckerScript.startAreaPos.x + _startAreaCameraOffset) 
            {
                _camPos.x = _playerPos.x;
                _mainCam.transform.position = _camPos; 
            }

            _playerPos.x += _levelCheckPosOffset;
            _levelChecker.transform.position = _playerPos;
        }

        if (!player.nextRoom)
        {
            return;
        }

        _camPos.x = _levelCheckerScript.startAreaPos.x + 10f;
        _mainCam.transform.position = _camPos;
        _levelCheckerScript.atEndOfArea = false;
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
