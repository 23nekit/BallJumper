using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public float DestroyedPlatformsForNextLevel;
	public float MinCameraX;
	public float MaxCameraX;
	public Text LevelText;
	public GameObject LosePanel;
	public GameObject SettingsPanel;
	public Ball MainBall;
	public GameObject LoseZone;
	public PlatformManager MainPlatformManager;
	public GameObject[] StartGameObjects;
	public Vector2 MinBallAndGameSpeed;
	public Vector2 MaxBallAndGameSpeed;
	public AudioMixer MainAudioMixer;

	public static float DestroyedPlatforms = 0;
	public float ViewedDestroyedPlatforms;
	public static float CurrentLevel = 1;
	public static bool IsGameOver = false;
	public static bool IsGameStarted = false;

	private void Start()
	{
		Time.timeScale = 0;
	}

	private void Update()
	{
		IfGameStartedAndNotOver();
		IfGameOver();
		ViewedDestroyedPlatforms = DestroyedPlatforms;
		Camera.main.transform.position = new Vector3(Mathf.Clamp(MainBall.transform.position.x, MinCameraX, MaxCameraX), Camera.main.transform.position.y, MainBall.transform.position.z - 10);
	}
	private void IfGameStartedAndNotOver() 
	{
		if (!IsGameOver && IsGameStarted)
		{
			IfDestroyedPlatformsEnoughForNextLevel();
			LevelText.text = CurrentLevel.ToString();
			LoseZone.transform.position = new Vector3(MainBall.transform.position.x, LoseZone.transform.position.y, MainBall.transform.position.z);
			Time.timeScale = Mathf.Lerp(MinBallAndGameSpeed.y, MaxBallAndGameSpeed.y, CurrentLevel * 20 / 100);
			MainBall.BounceSpeed = Mathf.Lerp(MinBallAndGameSpeed.x, MaxBallAndGameSpeed.x, CurrentLevel * 20 / 100);

		}
	}
	private void IfDestroyedPlatformsEnoughForNextLevel() 
	{
		if (DestroyedPlatforms == DestroyedPlatformsForNextLevel)
		{
			DestroyedPlatforms = 0;
			DestroyedPlatformsForNextLevel += 5;
			if (CurrentLevel != 5)
			{
				CurrentLevel += 1;
			}
		}
	}
	private void IfGameOver() 
	{
		if (IsGameOver && IsGameStarted)
		{
			LosePanel.SetActive(true);
			LosePanel.GetComponent<RectTransform>().SetAsLastSibling();
			MainBall.gameObject.SetActive(false);
			for (int i = 0; i < PlatformManager.AllPlatforms.Count; i++)
			{
				Destroy(PlatformManager.AllPlatforms[i]);
			}
			Time.timeScale = 0;
		}
	}

	public void StartGame() 
	{
		IsGameStarted = true;
		for (int i = 0; i < StartGameObjects.Length; i++)
		{
			StartGameObjects[i].SetActive(false);
		}
		LevelText.gameObject.SetActive(true);
		RestartGame();
	}

	public void SettingsOnOrOff() 
	{
		SettingsPanel.SetActive(!SettingsPanel.activeSelf);
	}

	public void RestartGame()
	{
		Time.timeScale = 1;
		LosePanel.SetActive(false);
		DestroyedPlatforms = 0;
		DestroyedPlatformsForNextLevel = 10;
		CurrentLevel = 1;
		MainPlatformManager.GenerateStartPlatforms();
		MainBall.gameObject.SetActive(true);
		MainBall.Angle = MainBall.StartAngle;
		MainBall.Radius = MainPlatformManager.DistanceBetweenCenters / 2;
		MainBall.BounceCenter = new Vector3(0, MainPlatformManager.StartCoordinate.y, MainBall.Radius);
		MainBall.IsReachNewCenter = false;
		MainBall.IsFirstBounce = true;
		MainBall.transform.position = Vector3.zero;
		IsGameOver = false;
	}

	public void GoToMainMenu() 
	{
		LosePanel.SetActive(false);
		IsGameStarted = false;
		for (int i = 0; i < StartGameObjects.Length; i++)
		{
			StartGameObjects[i].SetActive(true);
		}
		LevelText.gameObject.SetActive(false);
	}

	public void EndGame() 
	{
		Application.Quit();
	}

	public void SetVolume(float Volume) 
	{
		MainAudioMixer.SetFloat("Volume", Volume);
	}
}
