using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

	[SerializeField]private float moveStrength = 7, jumpStrength = 3, ballStrength = 3;
	[HideInInspector]public bool moving = false, jumping = false;

	[SerializeField]private AudioClip splat, squish;
	private Rigidbody rb;
	private Vector3 startForward, startRight;
	private Vector3 startPos;
	private Quaternion startRot;
	private AudioSource audio;

	// Use this for initialization
	void Start () 
	{
		// EventController.GameStartFunctions += onGameStart;
		EventController.GoalScoredFunctions += onGoalScored;
		rb = GetComponent<Rigidbody>();
		audio = GetComponent<AudioSource>();
		startPos = transform.position;
		startRot = transform.rotation;
		startForward = transform.forward;
		startRight = transform.right;
	}
	
	// Update is called once per frame

	public IEnumerator move(string axisPrefix)
	{
		/* set velocity to direction * speed then multiply it's normal by a quadratic or exponential function */

		moving = true;
		audio.clip = squish;
		audio.Play();
		float startTime = Time.time;
		while(Time.time - startTime < 0.5f)
		{
			Vector3 temp = startRight * Input.GetAxis(axisPrefix + "Horizontal");
			temp += startForward * Input.GetAxis(axisPrefix + "Vertical");
			temp = temp.normalized * moveStrength * (0.25f + Time.time - startTime);
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
			Vector3 temp = startRight * Input.GetAxis(axisPrefix + "Horizontal");
			temp += startForward * Input.GetAxis(axisPrefix + "Vertical");
			temp = temp.normalized * moveStrength * (1.25f - Time.time + startTime);
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

	void onGoalScored(string team)
	{
		Debug.Log(gameObject.name + "OnGoalScored");
		StopAllCoroutines();
		moving = false;
		jumping = false;
		rb.velocity = Vector3.zero;
		transform.position = startPos;
		transform.rotation = startRot;
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Ball")
		{
			Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
			Vector3 forceToAdd = (other.gameObject.transform.position - transform.position).normalized * rb.velocity.magnitude * ballStrength;
			forceToAdd.y = Mathf.Min(forceToAdd.y, 0);
			otherRb.AddForce(forceToAdd, ForceMode.VelocityChange);
			other.gameObject.GetComponent<AudioSource>().Play();
		}
	}

	void OnDestroy()
	{
		// EventController.GameStartFunctions -= onGameStart;
		EventController.GoalScoredFunctions -= onGoalScored;
	}
}
