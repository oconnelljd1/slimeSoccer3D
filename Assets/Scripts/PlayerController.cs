using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField]private string axisPrefix = "P1";
	private bool active = false;
	private MovementController mover;
	// Use this for initialization
	void Start () 
	{
		EventController.GameStartFunctions += onGameStart;
		EventController.GoalScoredFunctions += onGoalScored;
		mover = GetComponent<MovementController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if(!active)
			return;

		if((Input.GetButton(axisPrefix + "Vertical") || Input.GetButton(axisPrefix + "Horizontal")) && !mover.moving && !mover.moving)
		{
			IEnumerator moveCoroutine = mover.move(axisPrefix);
			StartCoroutine(moveCoroutine);
		}
		if(Input.GetButtonDown(axisPrefix + "Jump") && !mover.jumping)
		{
			IEnumerator jumpCoroutine = mover.jump();
			StartCoroutine(jumpCoroutine);
		}
	}

	void onGameStart()
	{
		Debug.Log(gameObject.name + "OnGameStart");
		active = true;
	}

	void onGoalScored(string team)
	{
		Debug.Log(gameObject.name + "OnGoalScored");
		StopAllCoroutines();
		active = false;
	}

	void OnDestroy()
	{
		EventController.GameStartFunctions -= onGameStart;
		EventController.GoalScoredFunctions -= onGoalScored;
	}
}
