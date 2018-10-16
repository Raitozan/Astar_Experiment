using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour {

	public Text scr;

	void Start() {
		scr.text = "Your Score: " + ((int)GameManager.survivedTime).ToString ();
	}
}
