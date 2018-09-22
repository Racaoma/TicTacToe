using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour 
{
	//Audios
	public AudioClip[] audios;
    public AudioSource music;

	//Audio Source
	public AudioSource audioSource;

    //Music
    public void playMusic()
    {
        music.Play();
    }

	//Method for Playing Sound
	public void playSound()
	{
		audioSource.clip = audios[Random.Range(0, audios.Length)];
		audioSource.Play();
	}
}
