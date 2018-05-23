using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TestSound : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    float repeatTime;

    [SerializeField, Range(0f, 1f)]
    float volume;
    
    [SerializeField, Range(0.5f, 2f)]
    float pitch;

    AudioSource source;

    float timer;

	// Use this for initialization
	void Start ()
    {
        source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer -= Time.deltaTime;
        if (timer > 0) return;

        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        timer = repeatTime;
    }
}
