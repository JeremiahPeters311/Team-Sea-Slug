using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenScrolling : MonoBehaviour
{
    [SerializeField] private Transform _movingDestination;

    [SerializeField] private float _screenScrollSpeed;

    [SerializeField] private Canvas _sceneCanvas;

    [SerializeField] private Animator Anim;

    [SerializeField] private Animator FiredAnim;

    [SerializeField] private Animator JerryAnim;

    [SerializeField] private float AnimationWaitTime;

    [SerializeField] private GameObject IntroPlayer;

    [SerializeField] private GameObject RealPlayer;

    [SerializeField] private GameObject MainCamera;

    [SerializeField] private AudioClip windowBreak;

    private bool _hasScrollingStarted = false;

    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerControls.PlayerActionMap.StartGame.performed += ctx => StartCoroutine(IntroTiming());
    }

    private IEnumerator IntroTiming()
    {
        FiredAnim.SetBool("bro is fired", true);
        SFXManager.instance.PlaySoundEffct(windowBreak, transform, 1f);
        yield return new WaitForSeconds(1.5f);
        Anim.SetBool("Game Started", true);
        yield return new WaitForSeconds(0.5f);
        JerryAnim.SetBool("Jerry Jumpscare", true);
        yield return new WaitForSeconds(1);
        StartCoroutine(ScrollingScreen(_movingDestination.position, _screenScrollSpeed));
        Destroy(IntroPlayer);
        yield return new WaitForSeconds(3f);

        StartCoroutine(RealPlayer.GetComponent<PlayerController>().ControlsDuringTitleScreen());

        Destroy(gameObject);
    }

    private IEnumerator ScrollingScreen(Vector3 destination, float scrollSpeed)
    {
        if (_screenScrollSpeed == 0)
        {
            Debug.LogWarning("scroll speed is set to zero, now it won't scroll, please fix that, thanks");
        }

        if (!_hasScrollingStarted)
        {
            _hasScrollingStarted = true;
            while (transform.position != destination)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(destination.x, destination.y, transform.position.z),
                    scrollSpeed * (_sceneCanvas.renderingDisplaySize.x / 100) * Time.deltaTime);

                //for whatever reason (probably float bs) this coroutine doesn't actually stop itself properly when done
                //this fixes that
                if (Mathf.Approximately(transform.position.y, destination.y))
                {
                    yield break;
                }

                yield return null;
            }
        }
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
}
