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
    [SerializeField] private GameObject _teleportRange;
    private PlayerControls _playerControls;
    [SerializeField] private float _reticleSpeed = 0.3f;
    private Vector3 _reticlePosition;
    [SerializeField] private bool _placingReticle = false;
    private bool _teleported = false;
    private TeleportCollision _teleportCollision;
    private bool _moveUp = false;
    private bool _moveDown = false;
    private bool _moveLeft = false;
    private bool _moveRight = false;

    private bool _lookingUp = false;
    private bool _lookingDown = false;

    //Prevents the player from teleporting beneath the level.
    [SerializeField] private float _worldBaseY = -1.255f;

    public Animator playerAnimator;

    [SerializeField] private float _maxTeleportDistance = 10f;
    
    //How far back the player can move. Position will update as player moves forward.
    private Vector2 _playerMaxBackPos;
    private Vector2 _currentCenter;
    private Vector2 _updatePlayerPos;
    [SerializeField]
    private float _maxBackRange = 7f;
    public bool playerMovingForward = true;

    [SerializeField] private float _cooldownTime = 1f;
    SpriteRenderer _spriteRenderer;
    [SerializeField]
    private string[] hurtPlayerCollisionTags;
    [SerializeField]
    private bool isInvulnerable = false;
    [SerializeField]
    private float invulnerabilitySeconds = 2;
    [SerializeField]
    private float _defaultTime = 1f;
    [SerializeField]
    private float _slowedTime = 0.2f;

    public bool gameOver = false;

    private void Awake()
    {
        _playerReticle.SetActive(false);
        _teleportRange.SetActive(false);
        _teleportRange.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        _playerControls = new PlayerControls();
        _reticlePosition = _playerReticle.transform.position;
        _playerMaxBackPos = transform.position;
        _currentCenter = transform.position;
        _playerMaxBackPos.x -= _maxBackRange;
        _teleportCollision = _playerReticle.GetComponent<TeleportCollision>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        _playerControls.PlayerActionMap.Disable();
        Time.timeScale = _defaultTime;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public IEnumerator ControlsDuringTitleScreen()
    {
        airHorizontalMoveSpeed = 4.5f;
        groundHorizontalMoveSpeed = 4.5f;

        yield return null;
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
        _teleportRange.SetActive(false);
        _playerControls.Disable();
    }

    private void TeleportReticleInput(InputAction.CallbackContext obj)
    {
        if (!_placingReticle && !_teleported && !gameOver)
        {
            _playerReticle.SetActive(true);
            _teleportRange.SetActive(true);
            _placingReticle = true;
            playerAnimator.SetBool("TeleportPre", true);
            playerAnimator.SetBool("Walking", false);
            playerAnimator.SetBool("WalkingLookUp", false);
            playerAnimator.SetBool("WalkingLookDown", false);
            _playerControls.TeleportMap.Up.performed += ReticleUp;
            _playerControls.TeleportMap.Down.performed += ReticleDown;
            _playerControls.TeleportMap.Left.performed += ReticleLeft;
            _playerControls.TeleportMap.Right.performed += ReticleRight;

            _playerControls.TeleportMap.Up.canceled += ReticleUpCancel;
            _playerControls.TeleportMap.Down.canceled += ReticleDownCancel;
            _playerControls.TeleportMap.Left.canceled += ReticleLeftCancel;
            _playerControls.TeleportMap.Right.canceled += ReticleRightCancel;
            Time.timeScale = _slowedTime;
        }
        else
        {
            if (_teleportCollision.canTeleport && !_teleported && _reticlePosition.y >= _worldBaseY && !gameOver)
            {
                transform.position = _reticlePosition;
                _teleported = true;
                playerAnimator.SetBool("TeleportPre", false);
                playerAnimator.SetBool("TeleportPost", true);
                StartCoroutine(TeleportCooldown());
                StartCoroutine(TeleportAnimation());
                _playerReticle.SetActive(false);
                _teleportRange.SetActive(false);
                Time.timeScale = _defaultTime;
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

    private IEnumerator TeleportCooldown()
    {
        _placingReticle = false;
        yield return new WaitForSeconds(_cooldownTime);
        _teleported = false;
    }

    private IEnumerator TeleportAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        playerAnimator.SetBool("TeleportPost", false);
    }

    private void FixedUpdate()
    {
        if (!_placingReticle)
        {
            MovePlayer();

            if (transform.position.x >= _currentCenter.x)
            {
                _currentCenter = transform.position;
                _playerMaxBackPos.x = transform.position.x - _maxBackRange;
                playerMovingForward = true;
            }
            else
            {
                playerMovingForward = false;
            }

            if (transform.position.x <= _playerMaxBackPos.x)
            {
                _updatePlayerPos = transform.position;
                _updatePlayerPos.x = _playerMaxBackPos.x;
                transform.position = _updatePlayerPos;
            }
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

            if (_reticlePosition.y >= transform.position.y + _maxTeleportDistance)
            {
                _reticlePosition.y = transform.position.y + _maxTeleportDistance;
            }

            if (_reticlePosition.y <= transform.position.y - _maxTeleportDistance)
            {
                _reticlePosition.y = transform.position.y - _maxTeleportDistance;
            }

            if (_reticlePosition.x >= transform.position.x + _maxTeleportDistance)
            {
                _reticlePosition.x = transform.position.x + _maxTeleportDistance;
            }

            if (_reticlePosition.x <= transform.position.x - _maxTeleportDistance)
            {
                _reticlePosition.x = transform.position.x - _maxTeleportDistance;
            }

            _playerReticle.transform.position = _reticlePosition;

            if (!_teleportCollision.canTeleport || _reticlePosition.y < _worldBaseY)
            {
                _teleportRange.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            }
            else
            {
                _teleportRange.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            }
        }
    }

    /// <summary>
    /// Read Player Input from Input Action
    /// </summary>
    public void OnPlayerMoveInput(InputAction.CallbackContext context)
    {
        verticalDirection = context.ReadValue<Vector2>().y;
        horizontalVelocity = context.ReadValue<Vector2>().x;
        if (horizontalVelocity < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if(horizontalVelocity > 0)
        {
            _spriteRenderer.flipX = false;
        }

        if (!_placingReticle)
        {
            if (verticalDirection > 0)
            {
                _lookingUp = true;
                _lookingDown = false;
                playerAnimator.SetBool("LookDown", false);
                playerAnimator.SetBool("WalkingLookDown", false);
            }
            if (verticalDirection < 0)
            {
                _lookingUp = false;
                _lookingDown = true;
                playerAnimator.SetBool("LookUp", false);
                playerAnimator.SetBool("WalkingLookUp", false);
            }
            if (verticalDirection == 0)
            {
                _lookingUp = false;
                _lookingDown = false;
                playerAnimator.SetBool("LookUp", false);
                playerAnimator.SetBool("LookDown", false);
                playerAnimator.SetBool("WalkingLookUp", false);
                playerAnimator.SetBool("WalkingLookDown", false);
            }
            if (!_lookingDown && !_lookingUp || _lookingDown && _lookingUp)
            {
                if (horizontalVelocity != 0)
                {
                    playerAnimator.SetBool("Walking", true);
                }
                if (horizontalVelocity == 0)
                {
                    playerAnimator.SetBool("Walking", false);
                }
            }
            if (!_lookingDown && _lookingUp)
            {
                if (horizontalVelocity != 0)
                {
                    playerAnimator.SetBool("WalkingLookUp", true);
                    playerAnimator.SetBool("LookUp", false);
                    playerAnimator.SetBool("Walking", false);
                }
                if (horizontalVelocity == 0)
                {
                    playerAnimator.SetBool("LookUp", true);
                    playerAnimator.SetBool("WalkingLookUp", false);
                    playerAnimator.SetBool("Walking", false);
                }
            }
            if (_lookingDown && !_lookingUp)
            {
                if (horizontalVelocity != 0)
                {
                    playerAnimator.SetBool("WalkingLookDown", true);
                    playerAnimator.SetBool("LookDown", false);
                    playerAnimator.SetBool("Walking", false);
                }
                if (horizontalVelocity == 0)
                {
                    playerAnimator.SetBool("LookDown", true);
                    playerAnimator.SetBool("WalkingLookDown", false);
                    playerAnimator.SetBool("Walking", false);
                }
            }
        }
    }

    public void OnPlayerJump(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        if (!_placingReticle)
        {
            if (context.started)
            {
                JumpAction();
                playerAnimator.SetBool("Jump", true);
            }
            else if (context.canceled)
            {
                _smoothMovementVelocity.y = 0;
                playerAnimator.SetBool("Jump", false);
            }
        }
    }

    public void OnPlayerFire(InputAction.CallbackContext context) 
    {
        if (context.started) 
        {
            var bulletSent = Instantiate(_newspaperBulletPrefab, transform.position, Quaternion.identity).GetComponent<PlayerBullet>();
            bulletSent.SetDirection(new Vector2(horizontalVelocity, verticalDirection), _spriteRenderer.flipX);
        }
    }

    private void JumpAction()
    {
        Debug.Log(IsTouchingLayer(groundLayer));
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
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.61f, 0.04f), CapsuleDirection2D.Horizontal, 0, layerMask);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            platform = collision.gameObject.GetComponent<PassthroughPlatform>();
            platform.EnterPlatform();
        }
        if (CheckEnemyTagMatch(collision.gameObject)) 
        {
            if (isInvulnerable) 
            {
                return;
            }
            isInvulnerable = true;
            groundHorizontalMoveSpeed = 0;
            GameManager.Instance.PlayerDamage();
        }
    }

    private bool CheckEnemyTagMatch(GameObject collidedObject) 
    {
        foreach (string tag in hurtPlayerCollisionTags) 
        {
            if (collidedObject.CompareTag(tag)) 
            {
                return true;
            }
        }
        return false;
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

    public IEnumerator EnableControls() 
    {
        groundHorizontalMoveSpeed = 4.5f;
        float waitInterval = invulnerabilitySeconds / 60;
        
        for (int i = 0; i < 60; i++) 
        {
            _spriteRenderer.color = new Color(1, 1, 1, _spriteRenderer.color.a == 1 ? 0.15f : 1f);
            yield return new WaitForSeconds(waitInterval);
        }
        _spriteRenderer.color = new Color(1, 1, 1, 1f);
        isInvulnerable = false;
    }
}

