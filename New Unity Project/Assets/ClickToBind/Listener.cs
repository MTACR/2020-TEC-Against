﻿using UnityEngine;
using System.Collections;

/// <summary>
/// This script is included for the sake of the demo scene 
/// and is not required to use Click to Bind - feel free to delete
/// </summary>
public class Listener : MonoBehaviour {

	//Script used as a example of how to use keyBindingManager

	public GameObject box1;
	public GameObject box2;
	public GameObject box3;
	public GameObject box4;
	
	// Update is called once per frame
	void Update () {

		if(KeyBindingManager.GetKeyDown(KeyAction.jump))
		{
			box1.SetActive(!box1.activeSelf);
		}

	}
}
