using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NodeController : MonoBehaviour {

	public static NodeController instance;
	public string nodeTag;
	public bool AltCheck = false;
	public int detectRange;
	Point startPoint;
	Point endPoint;

	void Awake()
	{
		instance = this;
	}

	private static int CompareListByDistanceToStart(Point p1, Point p2)
	{
		return p1.GetDistanceToStart().CompareTo(p2.GetDistanceToStart());
	}

	private static int CompareListByScore(Point p1, Point p2)
	{
		return p1.GetScore().CompareTo(p2.GetScore());
	}

	public List<Point> Path(Vector2 startPos, Vector2 targetPos, int Level)
	{	
		GameObject[] nodes = GameObject.FindGameObjectsWithTag(nodeTag);
		GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platforms");
		List<Point> points = new List<Point>();
		List<Point> path = new List<Point>();
		List<Point> foundConnection = new List<Point>();
		List<Point> alternatePath = new List<Point>();
		startPoint = new Point(startPos, Level);
		endPoint = new Point(targetPos, 'e');
		float dist;
		int mMaxLevel = platforms.Length;
		int thisLevel;
		int nextLevel;

		foreach (GameObject node in nodes)
		{
			Point currNode = new Point(node.transform.position);
			float distS = Vector3.Distance(startPos, currNode.GetPos());
			float distT = Vector3.Distance(targetPos, currNode.GetPos());
			RaycastHit2D hit;

			hit = Physics2D.Raycast(currNode.GetPos(), Vector2.right);

			for (int k = 0; k <= mMaxLevel; k++)	//Determines the Height Level of the Hero
			{
				string bTag = "Level_" + k;
				
				if(hit.transform.tag == bTag)
				{
					currNode.SetLevel(k);

					if(currNode.GetPos() == targetPos)
					{
						endPoint.SetLevel(k);
					}
				}
			}

			currNode.SetScore(distT);
			currNode.SetDistanceToStart(distS);
			points.Add(currNode);

			if(currNode.GetLevel() == Level)
			{
				alternatePath.Add(currNode);
			}
		}

		alternatePath.Sort(CompareListByDistanceToStart);
		points.Sort(CompareListByDistanceToStart);

		path.Add(startPoint);

		foreach(Point startingNode in points)
		{
			if(startPoint.GetScore() > startingNode.GetScore())
			{
				startPoint = startingNode;
			}
			else
			{
				foreach(Point comparingNode in points)
				{
					dist = Vector2.Distance(startingNode.GetPos(), comparingNode.GetPos());
					int startingLevel = startingNode.GetLevel();
					int comparingLevel = comparingNode.GetLevel();
					int LCheck = Mathf.Abs(startingLevel - comparingLevel);
					
					if(dist != 0)
					{
						if(dist <= detectRange)
						{
							if(LCheck < 2)
							{
								foundConnection.Add(comparingNode);
								foundConnection.Sort(CompareListByScore);
							}
						}
					}
				}
				path.Add(foundConnection[0]);
			}
		}

		for (int u = 0; u < path.Count - 1; u++)
		{
			thisLevel = path[u].GetLevel();
			nextLevel = path[u + 1].GetLevel();
			int LCheck = Mathf.Abs(thisLevel - nextLevel);
			List<Point> detour = new List<Point>();

			Vector2 thisPos = path[u].GetPos();
			Vector2 nextPos = path[u + 1].GetPos();
			float distCheck = Vector2.Distance(path[u].GetPos(), path[u + 1].GetPos());

			if(LCheck > 1)
			{
				path.Remove(path[u + 1]);
				
				detour = FindDetour(alternatePath, points);
				
				if(detour.Count > 0)
				{
					path.Insert(1, detour[0]);
				}
			}
		}

		return path;
	}

	List<Point> FindDetour(List<Point> A, List<Point> P)
	{
		List<Point> D = new List<Point>();

		foreach(Point startNode in A)
		{
			foreach(Point compareNode in P)
			{
				int thisAltNode = startNode.GetLevel();
				int nextAltNode = compareNode.GetLevel();
				int AltLCheck = Mathf.Abs(thisAltNode - nextAltNode);
				float distCheck = Vector2.Distance(startNode.GetPos(), compareNode.GetPos());

				if(AltLCheck == 1)
				{
					if(startPoint.GetLevel() < endPoint.GetLevel())
					{
						if(compareNode.GetLevel() > startNode.GetLevel())
						{
							D.Add(compareNode);
							D.Sort(CompareListByScore);
						}
					}
					else
					{
						if(compareNode.GetLevel() < startPoint.GetLevel())
						{
							D.Add(compareNode);
							D.Sort(CompareListByScore);
						}
					}
				}
			}
		}

		return D;
	}
}
