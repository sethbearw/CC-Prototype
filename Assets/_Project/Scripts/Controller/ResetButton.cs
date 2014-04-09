using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour {
	
	// Use this for initialization
	void OnEnable () {
		EasyTouch.On_SimpleTap += HandleOn_SimpleTap;
	}

	void OnDisable () {
		EasyTouch.On_SimpleTap -= HandleOn_SimpleTap;
	}

	
	void HandleOn_SimpleTap (Gesture gesture)
	{
		
		if ( gesture.pickObject == gameObject )
		{
			Application.LoadLevel(0);
		}
	}
}
