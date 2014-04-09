using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {

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
			GameController.instance.StartTurn () ;
		}
	}
}
