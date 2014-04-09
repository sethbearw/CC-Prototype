using UnityEngine;
using System.Collections;

public class Speedometer : MonoBehaviour {

	private GUIText mSpeed;

	// Use this for initialization
	void Awake () {
		mSpeed = GetComponent<GUIText>();
	}
	
	// Update is called once per frame
	void Update () {
		mSpeed.text = ""+PlayerControl.instance.Speed;
	}
}
