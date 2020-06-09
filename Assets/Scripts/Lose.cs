using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : MonoBehaviour
{
	public AudioClip LoseSound;

	private void OnCollisionEnter(Collision collision)
	{
		AudioPlayer.PlayAudio(LoseSound);
	}
}
