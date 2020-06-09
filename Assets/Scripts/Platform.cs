using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Platform : MonoBehaviour
{
	public float SecondsForPlatformDie = 1;
	public Animation DeathAnimation;
	public AnimationClip DeathClip;
	public AudioClip DeathSound;
	public float ForAnimationY;

	private void Update()
	{
		transform.position = new Vector3(transform.position.x, ForAnimationY, transform.position.z);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Ball")
		{
			DestroyPlatformAddScorePlayDeathAnimAndAudio();
		}
	}
	private void DestroyPlatformAddScorePlayDeathAnimAndAudio() 
	{
		DeathAnimation.Play(DeathClip.name);
		PlatformManager.AllPlatforms.Remove(gameObject);
		Destroy(gameObject, SecondsForPlatformDie * Time.timeScale);
		GameManager.DestroyedPlatforms += 0.5f;
		AudioPlayer.PlayAudio(DeathSound);
	}
}
