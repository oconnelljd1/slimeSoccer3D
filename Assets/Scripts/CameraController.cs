using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	[SerializeField]private GameObject target;
	[SerializeField] private float followDistance = 3, followHeight = 2.5f, followSpeed = 3;
	private Vector3 startRot;

	// Use this for initialization
	void Start () 
	{
		startRot = -target.transform.forward;
	}
	
	// Update is called once per frame
	void Update () 
	{ 
		Vector3 temp = target.transform.position;
		temp += startRot * followDistance;
		temp.y = followHeight;
		transform.position = Vector3.MoveTowards(transform.position, temp, followSpeed * Time.deltaTime);

	}
}
