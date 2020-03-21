using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {

	[SerializeField]private float moveStrength = 7, jumpStrength = 3, ballStrength = 3;
	[SerializeField]GameObject ball;
	[SerializeField]private AudioClip splat, squish;
	private bool active = false;
	private enum States {Idle, Defending, Attacking};
	private States state = States.Attacking;
	private bool moving = false, jumping = false;
	private Rigidbody rb;
	private AudioSource audio;
	private Vector3 startPos;
	private Quaternion startRot;

	// Use this for initialization
	void Start () 
	{
		EventController.GameStartFunctions += onGameStart;
		EventController.GoalScoredFunctions += onGoalScored;
		rb = GetComponent<Rigidbody>();
		audio = GetComponent<AudioSource>();
		startPos = transform.position;
		startRot = transform.rotation;
		// startForward = transform.forward;
		// startRight = transform.right;
	}

	// Update is called once per frame
	void Update () 
	{
		
		if(!active)
			return;

		
		Vector3 distance = ball.transform.position - transform.position;
		distance.y = 0;
		if(Mathf.Sign(distance.z) > 0)
		{

			state = States.Defending;
		}
		else
		{
			state = States.Attacking;
		}

		switch(state)
		{
			case States.Idle:
				break;
			case States.Defending:
				if(! jumping)
				{
					if(transform.position.z < 4.5 && !moving)
					{
						Vector3 direction = new Vector3(0,0,4.5f) - transform.position;
						IEnumerator moveCoroutine = move(direction);
						StartCoroutine(moveCoroutine);
					}
					distance.y = 0;
					if(ball.transform.position.y > 0.6f && distance.magnitude < 1)// jump
					{
						StartCoroutine("jump");
					}
				}
				break;
			case States.Attacking:
				if(! jumping)
				{
					distance.y = 0;
					if(ball.transform.position.y > 0 && distance.magnitude < 1)// jump
					{
						StartCoroutine("jump");
					}
					if(!moving)// move
					{
						distance.x += Random.value - 0.5f;
						IEnumerator moveCoroutine = move(distance);
						StartCoroutine(moveCoroutine);
					}
				}
				break;
		}
		
	}public IEnumerator move(Vector3 direction)
	{
		/* set velocity to direction * speed then multiply it's normal by a quadratic or exponential function */

		moving = true;
		audio.clip = squish;
		audio.Play();
		float startTime = Time.time;
		while(Time.time - startTime < 0.5f)
		{
			Vector3 temp = direction.normalized * moveStrength * (0.25f + Time.time - startTime);
			temp.y = rb.velocity.y;
			rb.velocity = temp;
			if(rb.velocity.sqrMagnitude > 0.15f)
			{
				transform.eulerAngles = Vector3.up * Mathf.Atan2(rb.velocity.x, rb.velocity.z) * 180 / Mathf.PI;
			}
			yield return null;
		}
		while(Time.time - startTime < 1.0f)
		{
			Vector3 temp = direction.normalized * moveStrength * (1.25f - Time.time + startTime);
			temp.y = rb.velocity.y;
			rb.velocity = temp;
			if(rb.velocity.sqrMagnitude > 0.15f)
			{
				transform.eulerAngles = Vector3.up * Mathf.Atan2(rb.velocity.x, rb.velocity.z) * 180 / Mathf.PI;
			}
			yield return null;
		}
		moving = false;
	}

	public IEnumerator jump()
	{
		jumping = true;
		float startTime = Time.time;
		while(Time.time - startTime < 0.5f)
		{
			rb.velocity = new Vector3(rb.velocity.x, jumpStrength, rb.velocity.z);
			yield return null;
		}
		while(transform.position.y > 0)
		{
			yield return null;
		}
		transform.position = Vector3.Scale(transform.position, new Vector3(1, 0, 1));
		audio.clip = splat;
		audio.Play();
		jumping = false;
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
		moving = false;
		jumping = false;
		rb.velocity = Vector3.zero;
		transform.position = startPos;
		transform.rotation = startRot;
	}

	void OnDestroy()
	{
		EventController.GameStartFunctions -= onGameStart;
		EventController.GoalScoredFunctions -= onGoalScored;
	}
}
