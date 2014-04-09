#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
#define DEBUG
#endif

using UnityEngine;
using System;
using System.Collections;

[AddComponentMenu("Log Watcher Tool/DebugPreferences")]
public class DebugPreferences : MonoBehaviour {
	
	public bool m_ShowDebugView = false;	
	public int m_MaxLinesForDisplay;
	public KeyCode m_ToggleKey = KeyCode.BackQuote;
	public WindowSize m_WindowSize;
	public Vector2 m_WindowPos;
	public DebugCommand[] m_Commands;	
	
	static DebugPreferences m_Instance;
	
	void Awake ()
	{
		if (m_Instance == null) {
			#if DEBUG
			Debug.OnEnable();
			m_Instance = this;			
			DebugConsole.m_Console = this.gameObject;
			DebugConsole.IsOn = true; //Debug Console Active.
			DebugConsole.IsOpen = m_ShowDebugView; //Debug Console Show.
			DebugConsole.m_toggleKey = m_ToggleKey;
			DebugConsole.m_MaxLineForDisplay = m_MaxLinesForDisplay;				
			DebugConsole.m_WindowRect = m_WindowSize;
			DebugConsole.m_WindowLoc = m_WindowPos;
			DebugConsole.SetWindowSize();
			
			foreach(DebugCommand t_Command in m_Commands)
			{
				t_Command.hInit();
			}			
			
			DontDestroyOnLoad (gameObject);
			#endif
			
		} else if (m_Instance != this) {
			Destroy (gameObject);
		}		
	}
}
	
[System.Serializable]
public class WindowSize {
	public float m_WindowWidth = 300;
	public float m_WindowHeight = 400;
}

[System.Serializable]
public class DetailObj {
	public GameObject m_TargetObject;
	public string m_ComponetName;
}

[System.Serializable]
public class DebugCommand {
	public string m_Command;	
	public string m_FunctionName;	
	public DetailObj m_TargetDetail;
	
	public void hInit()
	{
		if ((m_Command != null) && (m_FunctionName != null))
		DebugConsole.RegisterCommand(m_Command, hRun);
	}
	
	public object hRun(params string[] args)
	{		
		if (m_TargetDetail.m_TargetObject == null)
		{
			GameObject[] gos = (GameObject[]) GameObject.FindObjectsOfType(typeof(GameObject));
			foreach(GameObject go in gos)
			{
				if (go && go.transform.parent == null)
				{
					go.gameObject.BroadcastMessage(m_FunctionName, SendMessageOptions.DontRequireReceiver);	
				}
			}
		}else if (m_TargetDetail.m_ComponetName == "")
		{
			m_TargetDetail.m_TargetObject.BroadcastMessage(m_FunctionName, SendMessageOptions.DontRequireReceiver);
		}else
		{
			m_TargetDetail.m_TargetObject.GetComponent(m_TargetDetail.m_ComponetName).SendMessage(m_FunctionName, SendMessageOptions.DontRequireReceiver);
		}
		return ("End: "+m_Command);
	}	
}