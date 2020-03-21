using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

	private Rigidbody rb;
	private Vector3 startPos;
	private Quaternion startRot;
	private AudioSource audio;

	// Use this for initialization
	void Start () 
	{
		EventController.GameStartFunctions += onGameStart;
		EventController.GoalScoredFunctions += onGoalScored;
		rb = GetComponent<Rigidbody>();
		startPos = transform.position;
		startRot = transform.rotation;
		rb.isKinematic = true;
	}
	
	void onGameStart()
	{
		Debug.Log(gameObject.name + "OnGameStart");
		rb.isKinematic = false;
	}

	void onGoalScored(string team)
	{
		Debug.Log(gameObject.name + "OnGoalScored");
		transform.position = startPos;
		transform.rotation = startRot;
		rb.isKinematic = true;
	}

	void OnDestroy()
	{
		EventController.GameStartFunctions -= onGameStart;
		EventController.GoalScoredFunctions -= onGoalScored;
	}
}
