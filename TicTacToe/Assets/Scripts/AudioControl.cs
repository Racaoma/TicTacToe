using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour 
{
    //Singleton
    private static AudioControl instance;
    public static AudioControl Instance
    {
        get
        {
            return instance;
        }
    }

    //On Object Awake
    private void Awake()
    {
        //Check Singleton
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    //On Object Destroy (Safeguard)
    public void OnDestroy()
    {
        instance = null;
    }

    //Audios
    public AudioClip[] audiosClips;
    public AudioSource musicSource;

	//Audio Source
	public AudioSource audioSource;

    //Control Variable
    public float fadeTime = 1.75f;

    //Music
    public void playMusic()
    {
        musicSource.Play();
    }

	//Method for Playing Sound
	public void playGrunt()
	{
		audioSource.clip = audiosClips[Random.Range(0, audiosClips.Length)];
		audioSource.Play();
	}

    public IEnumerator FadeMusic()
    {
        while (musicSource.volume > .1F)
        {
            musicSource.volume = Mathf.Lerp(musicSource.volume, 0F, fadeTime * Time.deltaTime);
            yield return 0;
        }
        musicSource.volume = 0;
        FindObjectOfType<GameFlowManager>().loadTitleScreen(false);
    }
}
