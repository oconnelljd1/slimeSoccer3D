using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour {

	[SerializeField]private float rotSpeed = 1, distance = 3.45f;
	[SerializeField] private GameObject myCamera;
	private Vector3 cameraStart;

	// Use this for initialization
	void Start () 
	{
		cameraStart = myCamera.transform.position;
		cameraStart += myCamera.transform.forward * distance / 2;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed);

		Vector3 temp = cameraStart;
		temp.z += Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad * 2 - 90) * distance / 2;
		myCamera.transform.position = temp;
	}
}
