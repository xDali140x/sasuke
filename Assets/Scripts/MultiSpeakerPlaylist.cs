using UnityEngine;

public class MultiSpeakerPlaylist : MonoBehaviour
{
    [Header("Bafles donde sonará la música")]
    public AudioSource[] speakers;    

    [Header("Canciones de la playlist")]
    public AudioClip[] playlist;     

    [Header("Opciones")]
    public bool loopPlaylist = true;  

    private int currentIndex = 0;

    void Start()
    {
        if (speakers == null || speakers.Length == 0)
        {
            Debug.LogError("MultiSpeakerPlaylist: No hay speakers asignados.");
            return;
        }

        if (playlist == null || playlist.Length == 0)
        {
            Debug.LogError("MultiSpeakerPlaylist: La playlist está vacía.");
            return;
        }

        PlayCurrentTrack();
    }

    void Update()
    {
        
        if (!speakers[0].isPlaying && speakers[0].clip != null)
        {
            NextTrack();
        }
    }

    void PlayCurrentTrack()
    {
        if (currentIndex < 0 || currentIndex >= playlist.Length)
            return;

        
        double startTime = AudioSettings.dspTime + 0.3;

        foreach (AudioSource s in speakers)
        {
            s.clip = playlist[currentIndex];
            s.PlayScheduled(startTime);
        }
    }

    void NextTrack()
    {
        currentIndex++;

        if (currentIndex >= playlist.Length)
        {
            if (loopPlaylist)
            {
                currentIndex = 0; 
            }
            else
            {
               
                foreach (AudioSource s in speakers)
                {
                    s.Stop();
                    s.clip = null;
                }
                return;
            }
        }

        PlayCurrentTrack();
    }
}
