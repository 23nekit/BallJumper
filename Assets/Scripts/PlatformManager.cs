using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
	public GameObject PlatformPrefab;
	public Vector3 StartCoordinate;
	public Rigidbody BallRigidbody;
	public float MinX;
	public float MaxX;
	public Vector3 MinSize;
	public Vector3 MaxSize;
	public int MaxPlatformsCount;
	public float DistanceBetweenCenters;
	public float SecondsForPlatformDie = 1;

	public static List<GameObject> AllPlatforms;

	[HideInInspector] public int CurrentPlatform;
	[HideInInspector] public int Variant = 1;

	public void GenerateStartPlatforms() 
	{
		Variant = 1;
		AllPlatforms = new List<GameObject>();
		for (CurrentPlatform = 0; CurrentPlatform < MaxPlatformsCount; CurrentPlatform++)
		{
			SpawnNextVariantOfPlatform();
		}
	}
	private void SpawnNextVariantOfPlatform()
	{
		switch (Variant) 
		{
			case 1:
				{
					SpawnStandartPlatform();
					Variant += 1;
					break;
				}
			case 2: 
				{
					SpawnUniquePlatform();
					Variant += 1;
					break;
				}
			case 3: 
				{
					int NewRandom = Random.Range(1, 3);
					if (NewRandom == 1) 
					{
						SpawnStandartPlatform();
					}
					else 
					{
						SpawnUniquePlatform();
					}
					Variant = 1;
					break;
				}
		}
	}
	private void SpawnStandartPlatform()
	{
		StartCoordinate = new Vector3(Random.Range(MinX, MaxX), StartCoordinate.y, DistanceBetweenCenters * (CurrentPlatform + 1));
		if (CurrentPlatform == 0) 
		{
			StartCoordinate = new Vector3(0, StartCoordinate.y, DistanceBetweenCenters * (CurrentPlatform + 1));
		}
		GameObject NewPlatform = Instantiate(PlatformPrefab, StartCoordinate, Quaternion.identity);
		NewPlatform.AddComponent<MeshCollider>();
		NewPlatform.transform.name = "Platform" + CurrentPlatform.ToString();
		AllPlatforms.Add(NewPlatform);
	}
	private void SpawnUniquePlatform() 
	{
		SpawnStandartPlatform();
		if (GameManager.CurrentLevel > 1) 
		{
			AllPlatforms[AllPlatforms.Count-1].transform.localScale = Vector3.Lerp(MaxSize, MinSize, 5f * (float)GameManager.CurrentLevel / 100f);
		}
	}
	private void Update()
	{
		if (!GameManager.IsGameOver && GameManager.IsGameStarted)
		{
			if (AllPlatforms.Count < MaxPlatformsCount)
			{
				SpawnNextVariantOfPlatform();
				CurrentPlatform += 1;
			}
		}
	}
}
