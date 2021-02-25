using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSound : MonoBehaviour
{
    public AudioClip audioClip1;
    public AudioClip audioClip2;

    int i = 0;
    void OnTriggerEnter(Collider other)
    {
        i++;

        if ((i & 1) == 0)
            GameManager.Sound.Play(audioClip1);
        else
            GameManager.Sound.Play(audioClip2);
    }
}
