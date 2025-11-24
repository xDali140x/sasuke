using UnityEngine;

public class BathroomSoundController : MonoBehaviour
{
    public float outsideVolume = 1f;   // Volumen normal fuera del baño
    public float insideVolume = 0.5f;  // Volumen reducido dentro del baño

    private AudioSource[] musicSources;

    void Start()
    {
        // Busca todos los AudioSource con tag "Music"
        GameObject[] musicObjects = GameObject.FindGameObjectsWithTag("Music");
        musicSources = new AudioSource[musicObjects.Length];

        for (int i = 0; i < musicObjects.Length; i++)
        {
            musicSources[i] = musicObjects[i].GetComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que tu player tenga tag "Player"
        {
            SetMusicVolume(insideVolume);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetMusicVolume(outsideVolume);
        }
    }

    void SetMusicVolume(float vol)
    {
        if (musicSources == null) return;

        foreach (AudioSource src in musicSources)
        {
            if (src != null)
            {
                src.volume = vol;
            }
        }
    }
}
