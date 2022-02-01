using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ColliderDetector : MonoBehaviour
{
    public GameObject contactParticlePrefab;
    public CinemachineVirtualCamera cam;
    private CinemachineBasicMultiChannelPerlin noise;


    private NoiseSettings currentNoiseProfile;
    public NoiseSettings shakingNoiseProfile;
    private bool isShaking;
    public float timer;
    public float shakeTimer;


    private void Start()
    {
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        currentNoiseProfile = noise.m_NoiseProfile;
    }

    private void Update()
    {
        if (isShaking)
        {
            timer--;
            if (timer < 0)
            {
                isShaking = false;
            }
        }
        else
        {
            noise.m_NoiseProfile = currentNoiseProfile;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Instantiate(contactParticlePrefab, other.GetContact(0).point, Quaternion.identity);
        Debug.Log(other.transform.name);

        if (isShaking)
        {
            return;
        }
        
        noise.m_NoiseProfile = shakingNoiseProfile;
        timer = shakeTimer;
        isShaking = true;

    }
}
