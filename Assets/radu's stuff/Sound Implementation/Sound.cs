using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{

    public string name;
    public AudioClip clip;

    [Range(0f, 6f)]
    public float volume;
    [Range(.1f, 2f)]
    public float pitch;
    public bool loop = false; 

    [HideInInspector]
    public AudioSource source;
}

