using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	public float MovingSpeed;
	public float BounceSpeed;
	public float MinX;
	public float MaxX;
	public PlatformManager MainPlatformManager;
	public float StartAngle = 0.299f;
	public Rigidbody BallRigidbody;

	[HideInInspector] public float Angle;
	[HideInInspector] public float Radius;
	[HideInInspector] public Vector3 BounceCenter;
	[HideInInspector] public bool IsReachNewCenter = false;
	[HideInInspector] public bool IsFirstBounce = true;

	private void Update()
	{
		if (!GameManager.IsGameOver && GameManager.IsGameStarted)
		{
			MoveBallOnXOrNot();
			PlayBounceAndRestartItIfNeedIt();
			
			BallRigidbody.velocity = Vector3.zero;
		}
	}
	private void MoveBallOnXOrNot() 
	{
		if (Input.touchCount > 0)
		{
			Touch NewTouch = Input.GetTouch(0);
			if (NewTouch.phase == TouchPhase.Moved)
			{
				transform.position = new Vector3(Mathf.Clamp(transform.position.x + (NewTouch.deltaPosition.x * MovingSpeed * Time.deltaTime), MinX, MaxX), transform.position.y, transform.position.z);
			}
		}
	}
	private void PlayBounceAndRestartItIfNeedIt() 
	{
		if (!IsReachNewCenter)
		{
			Angle += Time.deltaTime * BounceSpeed;
			float z = Mathf.Cos(Angle) * Radius;
			float y = Mathf.Sin(Angle) * Radius;
			transform.position = new Vector3(transform.position.x, y, -z) + BounceCenter;
		}
		else
		{
			Angle = StartAngle;
			BounceCenter += new Vector3(0, 0, Radius * 2);
			IsReachNewCenter = false;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Platform")
		{
			IfIsntFirstBouncePlayNewBounce();
		}
		if (collision.transform.tag == "LoseZone") 
		{
			GameManager.IsGameOver = true;
		}
	}
	private void IfIsntFirstBouncePlayNewBounce() 
	{
		if (IsFirstBounce)
		{
			IsFirstBounce = false;
		}
		else
		{
			IsReachNewCenter = true;
		}
	}
}
