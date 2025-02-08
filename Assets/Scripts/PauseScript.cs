using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PauseScreenScript : MonoBehaviour
{
    [SerializeField]
    bool isGamePaused;

    private void OnEnable()
    {
        isGamePaused = true;
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        isGamePaused = false;
        Time.timeScale = 1;
    }
}
