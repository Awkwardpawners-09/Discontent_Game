using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickering : MonoBehaviour
{
    public Light _Light;
    public float MinTime;
    public float MaxTime;
    public float Timer;

    public AudioSource AS;
    public AudioClip LightAudio;

    void Start()
    {
        Timer = Random.Range(MinTime, MaxTime);
    }

    void Update()
    {
        FlickerLight();
    }

    void FlickerLight()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }

        if (Timer <= 0)
        {
            _Light.enabled = !_Light.enabled;
            Timer = Random.Range(MinTime, MaxTime);
            AS.PlayOneShot(LightAudio);
        }
    }
}
