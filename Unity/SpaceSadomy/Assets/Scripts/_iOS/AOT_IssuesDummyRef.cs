using UnityEngine;
using System.Collections;
using Game.Space.UI;
using System.Collections.Generic;

public class AOT_IssuesDummyRef : MonoBehaviour {


	private bool advanceToNextLevel;

	void Start() {
		advanceToNextLevel = false;
	}

	void Update() {
		if(!advanceToNextLevel) {
			advanceToNextLevel = true;
			Application.LoadLevel("MAP"); 
		}
	}
}
