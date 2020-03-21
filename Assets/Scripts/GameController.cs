using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[SerializeField]private Text countDown, blueScore, redScore, timer, endGameCountdown;
	[SerializeField]private GameObject postGame;
	[SerializeField]private float maxTime = 180;
	private float time = 0;
	private bool counting = false;

	// Use this for initialization
	void Start () 
	{
		EventController.GoalScoredFunctions += onGoalScored;
		UpdateTimer();
		StartCoroutine("StartGame");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(counting)
		{
			time += Time.deltaTime;
			UpdateTimer();
		}
	}

	private IEnumerator StartGame()
	{
		countDown.gameObject.SetActive(true);
		float countDownStart = Time.time;
		while(Time.time - countDownStart < 4)
		{
			float currentTime = Time.time - countDownStart;
			float count = Mathf.Ceil(3 - currentTime);
			countDown.rectTransform.localScale = Vector3.one * (1 + (currentTime % 1));
			if(count > 0)
			{
				countDown.text = "" + count;
			}
			else
			{
				countDown.text = "GO!";
			}
			yield return null;
		}
		countDown.gameObject.SetActive(false);
		counting = true;
		EventController.onGameStart();
	}

	void onGoalScored(string team)
	{
		Debug.Log(gameObject.name + "OnGoalScored");
		counting = false;
		if(team == "Red")
		{
			blueScore.text = "" + (Int32.Parse(blueScore.text) + 1);
		}
		else if(team == "Blue")
		{
			redScore.text = "" + (Int32.Parse(redScore.text) + 1);
		}
		StartCoroutine("StartGame");
	}

	private IEnumerator OnGameEnd()
	{
		Debug.Log("Times Up!");
		timer.text = "" + 0;
		EventController.onGameEnd();
		counting = false;
		postGame.SetActive(true);
			Text text = postGame.GetComponentInChildren<Text>();
		if(Int32.Parse(blueScore.text) < Int32.Parse(redScore.text))
		{
			text.text = "Red Player Wins!";
			text.color = Color.red;
		}
		else
		{
			text.text = "Blue Player Wins!";
			text.color = Color.blue;
		}
		float startTime = Time.time;

		while(Time.time - startTime < 5)
		{
			yield return null;
		}
		SceneController.instance.LoadScene("Title");
	}

	void OnDestroy()
	{
		// EventController.GameStartFunctions -= onGameStart;
		EventController.GoalScoredFunctions -= onGoalScored;
	}

	private IEnumerator Overtime()
	{
		maxTime += 30;
		float startTime = Time.time;
		countDown.gameObject.SetActive(true);
		countDown.text = "OVERTIME!";
		while(Time.time - startTime < 2)
		{
			countDown.rectTransform.localScale = Vector3.one * (1 + ((Time.time - startTime) % 2));
			yield return null;
		}
		countDown.gameObject.SetActive(false);
	}

	private void UpdateTimer()
	{
		float currentTime = maxTime - time;
		if(currentTime > 60)
		{
			float minutes = Mathf.Ceil(maxTime / 60) - Mathf.Ceil(time / 60);
			float seconds = 60 - Mathf.Ceil(time % 60);
			timer.text = minutes + ":";
			if(seconds < 10)
			{
				timer.text += "0" + seconds;
			}
			else
			{
				timer.text += seconds;
			}
		}
		else if(currentTime > 0)
		{
			timer.text = "" + (Mathf.Ceil((maxTime - time) * 100) / 100);
			if(currentTime < 5)
			{
				endGameCountdown.gameObject.SetActive(true);
				endGameCountdown.rectTransform.localScale = Vector3.one * (1 + (currentTime % 1));
				endGameCountdown.text = "" + Mathf.Ceil(currentTime);
			}
		}
		else
		{
			endGameCountdown.gameObject.SetActive(false);
			if(Int32.Parse(blueScore.text) == Int32.Parse(redScore.text))
			{
				StartCoroutine("Overtime");
			}
			else
			{
				StartCoroutine("OnGameEnd");
			}
		}
	}
}
