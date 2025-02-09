using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject CarObjRef;

    [SerializeField] private GameObject LightObjRef;

    [SerializeField] private float CarTimer;

    [SerializeField] private float CarDuration;

    void Start()
    {
        StartCoroutine(CarCycle(CarTimer));
    }

    IEnumerator CarCycle(float Timer)
    {
        //car is gone
        LightObjRef.GetComponent<TrafficLightBehaviour>().SetTrafficLight(1); //green
        CarObjRef.SetActive(false);
        yield return new WaitForSeconds(Timer * 2 / 3);

        //car is soon
        LightObjRef.GetComponent<TrafficLightBehaviour>().SetTrafficLight(2); //yellow
        yield return new WaitForSeconds(Timer / 3);

        //car is here
        LightObjRef.GetComponent<TrafficLightBehaviour>().SetTrafficLight(3); //red
        CarObjRef.SetActive(true);
        yield return new WaitForSeconds(CarDuration);

        StartCoroutine(CarCycle(CarTimer));
    }
}
