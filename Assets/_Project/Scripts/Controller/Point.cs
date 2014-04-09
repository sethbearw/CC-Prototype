using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Point {

	Vector2 m_pos;
	char m_state = 'u';
	int m_Level = 0;
	float m_score = 0;
	float m_distanceToStart = 0;
	Point m_prevPoint;
	
	List<Point> m_connectedPoints = new List<Point>();
	List<Point> m_potentialPrevPoints = new List<Point>();
	
	public Point(Vector2 pos, int lvl = 0)
	{
		m_pos = pos;
		m_Level = lvl;
	}

	public int GetLevel()
	{
		return m_Level;
	}

	public char GetState()
	{
		return m_state;
	}
	
	public Vector2 GetPos()
	{
		return m_pos;
	}
	
	public List<Point> GetConnectedPoints()
	{
		return m_connectedPoints;
	}
	
	public Point GetPrevPoint()
	{
		return m_prevPoint;
	}
	
	public float GetScore()
	{
		return m_score;
	}
	
	public float GetDistanceToStart()
	{
		return m_distanceToStart;
	}
	
	public List<Point> GetPotentialPrevPoints()
	{
		return m_potentialPrevPoints;
	}
	
	public void AddConnectedPoint(Point point)
	{
		m_connectedPoints.Add(point);
	}
	
	public void AddPotentialPrevPoint(Point point)
	{
		m_potentialPrevPoints.Add(point);
	}
	
	public void SetPrevPoint(Point point)
	{
		m_prevPoint = point;
	}

	public void SetLevel(int lvl)
	{
		m_Level = lvl;
	}

	public void SetState(char newState)
	{
		m_state = newState;
	}
	
	public void SetScore(float newScore)
	{
		m_score = newScore;
	}
	
	public void SetDistanceToStart(float newScore)
	{
		m_distanceToStart = newScore;
	}
}
