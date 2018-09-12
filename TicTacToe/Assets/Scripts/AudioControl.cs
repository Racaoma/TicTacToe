using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour 
{
	//Audios
	public AudioClip[] audios;

	//Audio Source
	public AudioSource audioSource;

	//Method for Playing Sound
	public void playSound()
	{
		audioSource.clip = audios[Random.Range(0, audios.Length)];
		audioSource.Play();
	}
}
