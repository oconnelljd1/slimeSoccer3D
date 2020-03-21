using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public static SceneController instance;

	void Awake()
	{
		if(instance)
			Destroy(gameObject);
		else
			instance = this;
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void LoadScene(string scene)
	{
		SceneManager.LoadScene(scene);
	}
}
