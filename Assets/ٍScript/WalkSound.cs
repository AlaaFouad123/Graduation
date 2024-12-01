using UnityEngine;

public class WalkSound : MonoBehaviour
{
    public AudioSource Walk;
    public AudioSource Pop;
    void Start()
    {   
        Pop.Stop();
        Invoke("StopSound", 6f);
        Invoke("StopSoundPop", 8.9f);
    }

    void StopSound()
    {
        Walk.Stop();
    }
    
        void StopSoundPop()
    {
        Pop.Play();
    }

    void Update()
    {

    }
}
