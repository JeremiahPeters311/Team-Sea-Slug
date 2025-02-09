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

    [SerializeField] private float AnimationWaitTime;

    private bool _hasScrollingStarted = false;

    private void Update()
    {
        if (Input.GetKey("space"))
        {
            if (_screenScrollSpeed == 0)
            {
                Debug.LogWarning("scroll speed is set to zero, now it won't scroll, please fix that, thanks");
            }
            
            StartCoroutine(IntroTiming());
            //StartCoroutine(ScrollingScreen(_movingDestination.position, _screenScrollSpeed));
        }
    }

    private IEnumerator IntroTiming()
    {
        FiredAnim.SetBool("bro is fired", true);
        yield return new WaitForSeconds(1.5f);
        Anim.SetBool("Game Started", true);
        yield return new WaitForSeconds(1);
        StartCoroutine(ScrollingScreen(_movingDestination.position, _screenScrollSpeed));
    }

    private IEnumerator ScrollingScreen(Vector3 destination, float scrollSpeed)
    {
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
}
