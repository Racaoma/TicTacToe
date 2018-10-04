using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour 
{
	//Audios
	public AudioClip[] audios;
    public AudioSource music;

	//Audio Source
	public AudioSource audioSource;

    //Control Variable
    public float fadeTime = 1.75f;

    //Music
    public void playMusic()
    {
        music.Play();
    }

	//Method for Playing Sound
	public void playGrunt()
	{
		audioSource.clip = audios[Random.Range(0, audios.Length)];
		audioSource.Play();
	}

    public IEnumerator FadeMusic()
    {
        while (music.volume > .1F)
        {
            music.volume = Mathf.Lerp(music.volume, 0F, fadeTime * Time.deltaTime);
            yield return 0;
        }
        music.volume = 0;
        FindObjectOfType<GameFlowManager>().loadTitleScreen(false);
    }
}
