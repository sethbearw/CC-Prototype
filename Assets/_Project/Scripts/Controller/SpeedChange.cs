using UnityEngine;
using System.Collections;

public class SpeedChange : MonoBehaviour {

	void OnEnable () {
		EasyTouch.On_SimpleTap += HandleOn_SimpleTap;
	}
	
	void OnDisable () {
		EasyTouch.On_SimpleTap -= HandleOn_SimpleTap;
	}
	
	
	void HandleOn_SimpleTap (Gesture gesture)
	{

		if(gesture.pickObject == null)
		{

		}
		else if ( gesture.pickObject.tag == "UP" )
		{
			PlayerControl.instance.Speed++;
		}
		else if (gesture.pickObject.tag == "DOWN")
		{
			PlayerControl.instance.Speed--;
		}
	}
}
