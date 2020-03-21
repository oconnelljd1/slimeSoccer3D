using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetController : MonoBehaviour {

	[SerializeField] private string team;

	// Use this for initialization
	void Start () 
	{
		if(team == "")
		{
			Debug.LogWarning("Team has not been assigned to " + gameObject);
		}
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) 
	{
		if(other.tag == "Ball")
		{
			EventController.onGoalScored(team);
			GetComponent<AudioSource>().Play();
		}
	}
}
