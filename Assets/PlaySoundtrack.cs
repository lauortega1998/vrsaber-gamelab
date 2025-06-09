using UnityEngine;

public class PlaySoundtrack : MonoBehaviour
{
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.Play("soundtrack");
        }
        else
        {
            Debug.LogError("AudioManager not found!");
        }
    }

    void Update()
    {
    }
}
