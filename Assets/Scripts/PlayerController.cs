/******************************************************************
 *    Author: Dalsten Yan
 *    Contributors: Mitchell Young
 *    Date Created: 2/7/25
 *    Description: Player movement and teleport controls.
 *******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int _lives;
    [SerializeField]
    private float _jumpHeight;
    [SerializeField]
    private float groundHorizontalMoveSpeed;
    [SerializeField]
    private float airHorizontalMoveSpeed;

    [SerializeField]
    LayerMask groundLayer, platformLayer;
    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    PassthroughPlatform platform;

    Rigidbody2D rb;
    Vector2 _smoothMovementVelocity, _currentVelocityVector;

    float verticalDirection, horizontalVelocity;

    [SerializeField]
    GameObject _newspaperBulletPrefab;

    [SerializeField] private GameObject _playerReticle;
    [SerializeField] private GameObject _managerObject;
    private PlayerControls _playerControls;
    private GameManager _gameManager;
    [SerializeField] private float _reticleSpeed = 0.3f;
    private Vector3 _reticlePosition;
    [SerializeField] private bool _placingReticle = false;
    private TeleportCollision _teleportCollision;
    private bool _moveUp = false;
    private bool _moveDown = false;
    private bool _moveLeft = false;
    private bool _moveRight = false;

    private void Awake()
    {
        _playerReticle.SetActive(false);
        _playerControls = new PlayerControls();
        _reticlePosition = _playerReticle.transform.position;
        _teleportCollision = _playerReticle.GetComponent<TeleportCollision>();
        _gameManager = _managerObject.GetComponent<GameManager>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        _playerControls.TeleportMap.Teleport.performed += TeleportReticleInput;
    }

    private void OnDisable()
    {
        _playerControls.TeleportMap.Teleport.performed -= TeleportReticleInput;
        _playerControls.TeleportMap.Up.performed -= ReticleUp;
        _playerControls.TeleportMap.Down.performed -= ReticleDown;
        _playerControls.TeleportMap.Left.performed -= ReticleLeft;
        _playerControls.TeleportMap.Right.performed -= ReticleRight;

        _playerControls.TeleportMap.Up.canceled -= ReticleUpCancel;
        _playerControls.TeleportMap.Down.canceled -= ReticleDownCancel;
        _playerControls.TeleportMap.Left.canceled -= ReticleLeftCancel;
        _playerControls.TeleportMap.Right.canceled -= ReticleRightCancel;
        _playerReticle.SetActive(false);
        _playerControls.Disable();
    }

    private void TeleportReticleInput(InputAction.CallbackContext obj)
    {
        if (!_placingReticle && _gameManager.teleportMeter >= 2f)
        {
            _playerReticle.SetActive(true);
            _placingReticle = true;
            _playerControls.TeleportMap.Up.performed += ReticleUp;
            _playerControls.TeleportMap.Down.performed += ReticleDown;
            _playerControls.TeleportMap.Left.performed += ReticleLeft;
            _playerControls.TeleportMap.Right.performed += ReticleRight;

            _playerControls.TeleportMap.Up.canceled += ReticleUpCancel;
            _playerControls.TeleportMap.Down.canceled += ReticleDownCancel;
            _playerControls.TeleportMap.Left.canceled += ReticleLeftCancel;
            _playerControls.TeleportMap.Right.canceled += ReticleRightCancel;
        }
        else
        {
            if (_teleportCollision.canTeleport && _gameManager.teleportMeter >= 2f)
            {
                transform.position = _reticlePosition;
                _placingReticle = false;
                _gameManager.teleportMeter -= 2f;
                _playerReticle.SetActive(false);
            }
        }
    }
    private void ReticleUp(InputAction.CallbackContext obj)
    {
        _moveUp = true;
    }
    private void ReticleDown(InputAction.CallbackContext obj)
    {
        _moveDown = true;
    }
    private void ReticleLeft(InputAction.CallbackContext obj)
    {
        _moveLeft = true;
    }
    private void ReticleRight(InputAction.CallbackContext obj)
    {
        _moveRight = true;
    }

    private void ReticleUpCancel(InputAction.CallbackContext obj)
    {
        _moveUp = false;
    }
    private void ReticleDownCancel(InputAction.CallbackContext obj)
    {
        _moveDown = false;
    }
    private void ReticleLeftCancel(InputAction.CallbackContext obj)
    {
        _moveLeft = false;
    }
    private void ReticleRightCancel(InputAction.CallbackContext obj)
    {
        _moveRight = false;
    }

    private void FixedUpdate()
    {
        if (!_placingReticle)
        {
            MovePlayer();
        }
        else
        {
            if (_moveUp)
            {
                _reticlePosition.y += _reticleSpeed;
            }
            if (_moveDown)
            {
                _reticlePosition.y -= _reticleSpeed;
            }
            if (_moveLeft)
            {
                _reticlePosition.x -= _reticleSpeed;
            }
            if (_moveRight)
            {
                _reticlePosition.x += _reticleSpeed;
            }
            _playerReticle.transform.position = _reticlePosition;
        }
    }

    /// <summary>
    /// Read Player Input from Input Action
    /// </summary>
    public void OnPlayerMoveInput(InputAction.CallbackContext context)
    {
        verticalDirection = context.ReadValue<Vector2>().y;
        horizontalVelocity = context.ReadValue<Vector2>().x;
    }

    public void OnPlayerJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpAction();
        }
        else if (context.canceled)
        {
            _smoothMovementVelocity.y = 0;
        }

    }

    private void JumpAction()
    {
        if (IsTouchingLayer(platformLayer) && verticalDirection <= -1 && platform != null)
        {
            platform.SetPlayerFallThrough(true);
        }
        else if (IsTouchingLayer(groundLayer) || IsTouchingLayer(platformLayer))
        {
            _smoothMovementVelocity.y = _jumpHeight;
        }

    }

    /// <summary>
    /// Move player smoothly based on current Input velocity, target speed, and also move speed
    /// </summary>
    private void MovePlayer()
    {
        _smoothMovementVelocity = Vector2.SmoothDamp(_smoothMovementVelocity, new Vector2(horizontalVelocity, 0), ref _currentVelocityVector, 0.1f);
        rb.velocity = (_smoothMovementVelocity * groundHorizontalMoveSpeed);
    }

    /// <summary>
    /// Returns true if the layer is touching the player feet bottom
    /// </summary>
    /// <param name="layerMask">the layer to check</param>
    /// <returns></returns>
    private bool IsTouchingLayer(LayerMask layerMask)
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(2.86f, 0.1f), CapsuleDirection2D.Horizontal, 0, layerMask);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Entered: " + collision.gameObject.name);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            platform = collision.gameObject.GetComponent<PassthroughPlatform>();
            platform.EnterPlatform();
        }
        if (collision.gameObject.CompareTag("Projectile")) 
        {
            _playerControls.Disable();
            GameManager.Instance.PlayerDamage();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Exited: " + collision.gameObject.name);
        if (platform != null)
        //    Debug.Log("Boolean 2: " + platform.GetIfPlayerPassedThrough());
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Debug.Log("Resetting Platform");
            platform.ExitPlatform();
            platform = null;
        }
    }

    public void EnableControls() 
    {
        _playerControls.Disable();
    }
}

