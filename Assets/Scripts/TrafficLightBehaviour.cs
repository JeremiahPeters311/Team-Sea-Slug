using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightBehaviour : MonoBehaviour
{
    public Sprite LightGreen;

    public Sprite LightYellow;

    public Sprite LightRed;

    public void SetTrafficLight(int state)
    {
        switch (state)
        {
            case 3:
                //red
                GetComponent<SpriteRenderer>().sprite = LightRed;
                break;
            case 2:
                //yellow
                GetComponent<SpriteRenderer>().sprite = LightYellow;
                break;
            case 1:
                //green
                GetComponent<SpriteRenderer>().sprite = LightGreen;
                break;
            default:
                Debug.Log("uh oh");
                break;
        }
    }
}
