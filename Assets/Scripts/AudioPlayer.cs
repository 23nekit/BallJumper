using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
	public AudioSource GivenSource;
    public static AudioSource MainSource;
	public void Start()
	{
		MainSource = GivenSource;
	}
	public static void PlayAudio(AudioClip GivenClip) 
    {
        MainSource.clip = GivenClip;
        MainSource.Play();
    }
}
