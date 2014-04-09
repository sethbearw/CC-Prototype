using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class LogTest : MonoBehaviour {
	
	int PowerupCount = 23;
	
	void Awake () 
	{		
		for(int i = 0; i<=5; i++)
		{	
			// Registration Log.
			Debug.Log(10%2.1153f);
			Debug.Log("PowerupCount " + PowerupCount);
			
			// Registration Group Log.
			Debug.Log("Test 1324567980123456789", "LogTest1");
			Debug.Log("test", "LogTest2");
			Debug.Log("PowerupCount " + PowerupCount, "LogTest2");
			
			// Registration LogWarning. 
			Debug.LogWarning("test", "LogTest3");
			
			// Registration LogError.
			Debug.LogError("Hello ", "LogTest4");
		}
		
		// Watcher in the window register value of the variable.
		Debug.Watcher("PowerupCount", PowerupCount);
		
		// ShowFunc Command Register.
		DebugConsole.RegisterCommand("ShowFunc", ShowFuncCommand);				
	}
	
	void Update()
	{
		PowerupCount ++;		
		for(int i = 0; i<5; i++)
		{
			// Watcher in the window register value of the variable
			Debug.Watcher("PowerupCount " + i , PowerupCount);
		}
	}
	
	// Function to be called by the command
	public object ShowFuncCommand(params string[] args)
	{
		return ("PowerupCount " + PowerupCount);
	}
	
	// Function to be called by the command
	public void ShowDate()
	{
		Debug.LogCommand(DateTime.Now);
	}

}
