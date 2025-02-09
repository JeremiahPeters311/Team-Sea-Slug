using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject CarObjRef;

    [SerializeField] private GameObject LightObjRef;

    [SerializeField] private GameObject _carPreview;

    [SerializeField] private float CarTimer;

    [SerializeField] private float CarDuration;

    [SerializeField]
    private int _currentCar;
    private Animator _carAnimator;
    private Animator _lightAnimator;

    void Start()
    {
        _carAnimator = CarObjRef.GetComponent<Animator>();
        _lightAnimator = LightObjRef.GetComponent<Animator>();
        StartCoroutine(CarCycle(CarTimer));
    }

    IEnumerator CarCycle(float Timer)
    {
        _carAnimator.SetBool("YellowCar", false);
        _carAnimator.SetBool("RedCar", false);
        _carAnimator.SetBool("GreenCar", false);
        _currentCar = Random.Range(1, 4);
        //car is gone
        //LightObjRef.GetComponent<TrafficLightBehaviour>().SetTrafficLight(1); //green
        _lightAnimator.SetBool("Stop", false);
        CarObjRef.SetActive(false);
        _carPreview.SetActive(false);
        yield return new WaitForSeconds(Timer * 2 / 3);
        

        //car is soon
        //LightObjRef.GetComponent<TrafficLightBehaviour>().SetTrafficLight(2); //yellow
        _carPreview.SetActive(true);
        _lightAnimator.SetBool("Stop", true);
        yield return new WaitForSeconds(Timer / 3);

        //car is here
        //LightObjRef.GetComponent<TrafficLightBehaviour>().SetTrafficLight(3); //red
        _carPreview.SetActive(false);
        CarObjRef.SetActive(true);
        if (_currentCar == 1)
        {
            _carAnimator.SetBool("YellowCar", true);
        }

        if (_currentCar == 2)
        {
            _carAnimator.SetBool("RedCar", true);
        }

        if (_currentCar == 3)
        {
            _carAnimator.SetBool("GreenCar", true);
        }
        yield return new WaitForSeconds(CarDuration);

        StartCoroutine(CarCycle(CarTimer));
    }
}
