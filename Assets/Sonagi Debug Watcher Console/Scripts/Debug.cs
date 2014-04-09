#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
#define DEBUG
#endif

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityDebug = UnityEngine.Debug;
public static class Debug {
	
	static List<WatchVar<object>> m_WatchVars;	
	static WatchVar<object> m_WatchVar;
	static WatchVarBase m_WatchVarBase;
	
	[Conditional("DEBUG")]
	public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
	{
		UnityDebug.DrawLine(start, end, color, duration, depthTest);
	}
	
	[Conditional("DEBUG")]
	public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
	{
		UnityDebug.DrawLine(start, end, color, duration);
	}
	
	[Conditional("DEBUG")]
	public static void DrawLine(Vector3 start, Vector3 end, Color color)
	{
		UnityDebug.DrawLine(start, end, color);
	}
	
	[Conditional("DEBUG")]
	public static void DrawLine(Vector3 start, Vector3 end)
	{
		UnityDebug.DrawLine(start, end);
	}
	
	[Conditional("DEBUG")]
	public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest)
	{
		UnityDebug.DrawRay(start, dir, color, duration, depthTest);
	}
	
	[Conditional("DEBUG")]
	public static void DrawRay(Vector3 start, Vector3 dir, Color color)
	{
		UnityDebug.DrawRay(start, dir, color);
	}
	
	[Conditional("DEBUG")]
	public static void DrawRay(Vector3 start, Vector3 dir)
	{
		UnityDebug.DrawRay(start, dir);
	}
	
	[Conditional("DEBUG")]
	public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
	{
		UnityDebug.DrawRay(start, dir, color);
	}
	
	[Conditional("DEBUG")]
	public static void Break()
	{
		UnityDebug.Break();
	}
	
	[Conditional("DEBUG")]
	public static void DebugBreak()
	{
		UnityDebug.DebugBreak();
	}
	
	[Conditional("DEBUG")]
	public static void Log(object message)
	{	
		DebugExecution.AddLog(DebugExecution.LogType.Log, message.ToString(), "ALL");
		DebugConsole.Log(message);
	}
		
	[Conditional("DEBUG")]
	public static void Log(object message, string messagegroup)
	{
		DebugExecution.AddLog(DebugExecution.LogType.Log, message.ToString(), messagegroup);
		DebugConsole.Log(messagegroup, message);
	}
	
	[Conditional("DEBUG")]
	public static void Log(object message, UnityEngine.Object context)
	{
		DebugExecution.AddLog(DebugExecution.LogType.Log, message.ToString(), "ALL");
		DebugConsole.Log(message);
	}
	
	[Conditional("DEBUG")]
	public static void LogError(object message)
	{
		DebugExecution.AddLog(DebugExecution.LogType.Error, message.ToString(), "ALL");
		DebugConsole.LogError(message);
	}
	
	[Conditional("DEBUG")]
	public static void LogError(object message, string messagegroup)
	{
		DebugExecution.AddLog(DebugExecution.LogType.Error, message.ToString(), messagegroup);
		DebugConsole.LogError(messagegroup, message);
	}
	
	[Conditional("DEBUG")]
	public static void LogError(object message, UnityEngine.Object context)
	{
		DebugExecution.AddLog(DebugExecution.LogType.Error, message.ToString(), "ALL");
		DebugConsole.LogError(message);
	}
	
	[Conditional("DEBUG")]
	public static void LogWarning(object message)
	{
		DebugExecution.AddLog(DebugExecution.LogType.Warning, message.ToString(), "ALL");
		DebugConsole.LogWarning(message);
	}
	
	[Conditional("DEBUG")]
	public static void LogWarning(object message, string messagegroup)
	{
		DebugExecution.AddLog(DebugExecution.LogType.Warning, message.ToString(), messagegroup);
		DebugConsole.LogWarning(messagegroup, message);
	}
	
	[Conditional("DEBUG")]
	public static void LogWarning(object message, UnityEngine.Object context)
	{
		DebugExecution.AddLog(DebugExecution.LogType.Warning, message.ToString(), "ALL");
		DebugConsole.LogWarning(message);
	}
	
	[Conditional("DEBUG")]
	public static void LogCommand(object message)
	{						
		DebugExecution.AddLog(DebugExecution.LogType.Log, message.ToString(), "_Command");
		DebugConsole.LogCommand(message);
	}
	
	[Conditional("DEBUG")]
	public static void Check(bool condition)
	{
	    Check(condition, "Wrong condition!");
	}
	
	[Conditional("DEBUG")]
	public static void Check(bool condition, object message)
	{
	    if (!condition)
	    {
	        LogError(string.Format("Assert! {0}", message));
	    }
	}
	
	[Conditional("DEBUG")]
	public static void SystemLog(string message, string stackTrace)
	{	
		DebugExecution.AddSystemLog(DebugExecution.LogType.Log, message, stackTrace, "ALL");
		DebugConsole.Log(message);
	}
	
	[Conditional("DEBUG")]
	public static void SystemLogWarning(string message, string stackTrace)
	{
		DebugExecution.AddSystemLog(DebugExecution.LogType.Warning, message, stackTrace, "ALL");
		DebugConsole.LogWarning(message);
	}
	
	[Conditional("DEBUG")]
	public static void SystemLogError(string message, string stackTrace)
	{
		DebugExecution.AddSystemLog(DebugExecution.LogType.Error, message, stackTrace, "ALL");
		DebugConsole.LogError(message);
	}
	
	[Conditional("DEBUG")]
	public static void SystemLogException(string message, string stackTrace)
	{
		DebugExecution.AddSystemLog(DebugExecution.LogType.Exception, message, stackTrace, "ALL");
		DebugConsole.LogError(message);
	}	

	[Conditional("DEBUG")]
	public static void Watcher(string watcherName, object watcherValue)
	{
		if (DebugConsole.m_watchVarTable.TryGetValue(watcherName, out m_WatchVarBase))
		{
		    m_WatchVarBase.m_value = watcherValue;
		}else
		{
			m_WatchVar = new WatchVar<object>(watcherName);
			m_WatchVar.Value = watcherValue;	
		}
	}
	
	[Conditional("DEBUG")]
	public static void WatcherClear()
	{
		DebugConsole.m_watchVarTable.Clear();
	}
	
	public static bool isDebugBuild
	{
	#if DEBUG
	  get { return true; }
	#else
	  get { return false; }
	#endif
	}
	
	public static void OnEnable() {
        Application.RegisterLogCallback(HandleLog);
    }
	
    public static void OnDisable() {
        Application.RegisterLogCallback(null);
    }

	public static void HandleLog (string logString, string stackTrace, LogType type) 
	{
		switch(type)
		{
		case LogType.Log:
			SystemLog(logString, stackTrace);
			break;
			
		case LogType.Warning:
			SystemLogWarning(logString, stackTrace);
			break;
			
		case LogType.Error:
			SystemLogError(logString, stackTrace);
			break;
			
		case LogType.Exception:
			SystemLogException(logString, stackTrace);
			break;
		
		default:
			SystemLogException(logString, stackTrace);
			break;			
		}		
	}
}