using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodePlacer : MonoBehaviour {

	public static NodePlacer instance;

	public GameObject Nodes;
	public List<GameObject> Platform;

	private GameObject[] mObject;
	private Vector3 mStartPos;
	private Vector3 mSmPos;
	private Vector3 mMidPos;
	private Vector3 mMePos;
	private Vector3 mEndPos;

	void Awake()
	{
		instance = this;
		mObject = GameObject.FindGameObjectsWithTag("Platforms");

		foreach (GameObject c in mObject)
		{
			GameObject temp = c.transform.GetChild(1).gameObject;

			Platform.Add(temp);
		}

		foreach (GameObject c in Platform)
		{
			float currX = c.transform.position.x;
			float currY = c.transform.position.y;
			float currZ = c.transform.position.z;
			float offsetX = 0.5f;
			float offsetY = 1f;
			float newY = currY + offsetY;
			float newStartX = currX + offsetX - c.renderer.bounds.size.x/2;
			float newEndX = currX - offsetX + c.renderer.bounds.size.x/2;
			float betSMPosX = (Mathf.Abs(currX) - Mathf.Abs(newStartX))/2;

			mStartPos = new Vector3(newStartX, newY, currZ);
			mSmPos = new Vector3(currX - betSMPosX, newY, currZ);
			mMidPos = new Vector3(currX, newY, currZ);
			mMePos = new Vector3(currX + betSMPosX, newY, currZ);
			mEndPos = new Vector3(newEndX, newY, currZ);
			
			Instantiate(Nodes, mStartPos, Quaternion.identity);
			Instantiate(Nodes, mSmPos, Quaternion.identity);
			Instantiate(Nodes, mMidPos, Quaternion.identity);
			Instantiate(Nodes, mMePos, Quaternion.identity);
			Instantiate(Nodes, mEndPos, Quaternion.identity);
		}
	}

	public void Initialize(Vector3 pos)
	{
		Instantiate(Nodes, pos, Quaternion.identity);
	}
}
