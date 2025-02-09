using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Manhole : MonoBehaviour
{
    [SerializeField] private float spawnInterval;
    private float spawnTime;
    [SerializeField] private float waterInterval;
    private float waterTime;

    [SerializeField] private AudioClip waterFlow;

    private bool waterCount;

    [SerializeField] private float launchForce;

    [SerializeField] private GameObject leftDrop;
    [SerializeField] private GameObject rightDrop;

    [SerializeField] private AudioClip manholeDrop;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime > spawnInterval)
        {
            Launch();
            SFXManager.instance.PlaySoundEffct(waterFlow, transform, 1f);
            spawnTime = 0f;
        }

        if (waterCount)
        {
            waterTime += Time.deltaTime;
            if (waterTime > waterInterval)
            {
                WaterShoot();
                waterTime = 0f;
                SFXManager.instance.PlaySoundEffct(manholeDrop, transform, 0.5f);
            }
        }
    }

    private void Launch()
    {
        rb2d.AddForce(new Vector2(0, launchForce));

        waterCount = true;
    }

    private void WaterShoot()
    {
        var offset1 = new Vector3(-0.5f, -0.2f, 0f);
        var offset2 = new Vector3(-0.5f, -0.45f, 0f);
        var offset3 = new Vector3(-0.5f, -0.7f, 0f);

        var offset4 = new Vector3(0.5f, -0.2f, 0f);
        var offset5 = new Vector3(0.5f, -0.45f, 0f);
        var offset6 = new Vector3(0.5f, -0.7f, 0f);

        Instantiate(leftDrop, gameObject.transform.position + offset1, leftDrop.transform.rotation);
        Instantiate(leftDrop, gameObject.transform.position + offset2, leftDrop.transform.rotation);
        Instantiate(leftDrop, gameObject.transform.position + offset3, leftDrop.transform.rotation);

        Instantiate(rightDrop, gameObject.transform.position + offset4, rightDrop.transform.rotation);
        Instantiate(rightDrop, gameObject.transform.position + offset5, rightDrop.transform.rotation);
        Instantiate(rightDrop, gameObject.transform.position + offset6, rightDrop.transform.rotation);

        waterCount = false;
    }
}
