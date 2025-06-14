using System;
using System.Collections;
using UnityEngine;

public class RobotAudio : MonoBehaviour
{

    private AudioSource audioSource;
    private int playCount = 0;
    private float delayBetweenPlays = 2f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    public void startAudio(AudioClip audio, Action finish)
    {
        playCount = 0;
        StartCoroutine(PlaySoundThreeTimes(audio, finish));

    }


    public IEnumerator PlaySoundThreeTimes(AudioClip audio, Action finish)
    {
        while (playCount < 3)
        {
            PlaySound(audio);
            playCount++;
            yield return new WaitForSeconds(delayBetweenPlays); // Wait before playing again

        }
        finish();

    }



    public void PlaySound(AudioClip audio)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audio);
        }
    }






}

