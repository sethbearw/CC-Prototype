using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ActorAction
{
	Idle,
	Running,
	Attack,
	Jump,
	Fall
}

public class Actors {

	public float tmpForce = 0 ;
	public float Speed = 10;
	public int Level = 0;
	public bool Jumpable = false;
}
