using UnityEngine;

public class PlaySoundtrack : MonoBehaviour
{
    AudioManager audioManager = FindAnyObjectByType<AudioManager>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager.Play("soundtrack");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
