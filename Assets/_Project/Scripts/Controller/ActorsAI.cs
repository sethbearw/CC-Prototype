using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Actors Movement A.I. Controller
/// </summary>
public class ActorsAI : MonoBehaviour {
	
	public static ActorsAI instance;
	
	class Target
	{
		int mLevel;
		Transform mTrans;
		PlayerControl mPC;
		EnemyControl mEC;
		
		public int Level
		{
			get
			{
				return mLevel;
			}
			set
			{
				mLevel = value;
			}
		}
		
		public Transform Trans
		{
			get
			{
				return mTrans;
			}
			set
			{
				mTrans = value;
			}
		}
		
		public PlayerControl PC
		{
			get
			{
				return mPC;
			}
			set
			{
				mPC = value;
			}
		}
		
		public EnemyControl EC
		{
			get
			{
				return mEC;
			}
			set
			{
				mEC = value;
			}
		}
	}

	class Actor
	{
		int mLevel;
		Transform mTrans;
		PlayerControl mPC;
		EnemyControl mEC;
		
		public int Level
		{
			get
			{
				return mLevel;
			}
			set
			{
				mLevel = value;
			}
		}
		
		public Transform Trans
		{
			get
			{
				return mTrans;
			}
			set
			{
				mTrans = value;
			}
		}
		
		public PlayerControl PC
		{
			get
			{
				return mPC;
			}
			set
			{
				mPC = value;
			}
		}
		
		public EnemyControl EC
		{
			get
			{
				return mEC;
			}
			set
			{
				mEC = value;
			}
		}
	}

	public int mFacing = 0;								//	-1 for Left, 1 for Right
	private int mActor = 0;								//  1 for Player Movement, -1 for Enemy Movement
	public float jumpForce = 1000;						// 	Force being applied to RigidBody2D to jump
	public float Speed = 12;							//	The movespeed for all Actors
	public float jumpDist = 7;							// 	Jump Range Check
	public float fallDist = 7;							//	Fall Range Check
	private float mDistX;								// 	Distance to Target
	private float mTDist;								//	Final move variable for x position
	private Target target;
	private Actor actor;
	
	void Awake()
	{
		instance = this;
	}

	/// <summary>
	/// Starts the move.
	/// Overload for Points
	/// </summary>
	/// <param name="act">Act.</param>
	/// <param name="tar">Tar. Nodes placed on the world.</param>
	/// <param name="mode">Mode.</param>
	public void StartMove(Transform act,  Point tar, int mode)
	{
		target = new Target();
		actor = new Actor();
		
		ReadActor(act);		//Determine Actor
		ReadTarget(tar);	//Determine Target

//		Debug.Watcher("Target Level", target.Level);

		mTDist = Vector2.Distance(act.position, tar.GetPos());										//Compute Move Destination and Speed of movement
		mDistX = Mathf.Lerp(act.position.x, tar.GetPos().x, (Speed * Time.deltaTime)/mTDist);
		
		float tmpDist = Vector2.Distance(act.position, tar.GetPos());								//Determinant for Jump Distance to reach target

		if(act.position.x < tar.GetPos().x)
		{
			mFacing = -1;	//Left
		}
		else
		{
			mFacing = 1;	//Right
		}

		if ( mFacing == -1 )	//Facing left
		{
			act.localRotation = Quaternion.Euler(new Vector3(0,180,0));	//Rotation is done in respect to texture orientation
			
			if(mActor == 1)		//Actor is Hero
			{
				if(target.Level > actor.Level)		//Target is above Actor
				{
					if(actor.PC.Jumpable)			//Determine if Actor is in a state that is permitted to jump
					{ 
						if(tmpDist <= jumpDist)		//Jump distance detection
						{
							float applyForce = jumpForce * tmpDist / jumpDist;
							actor.PC.Jumpable = false;
							actor.PC.HeroAct = ActorAction.Jump;
							actor.PC.rigidbody2D.AddForce(Vector2.up * jumpForce);
//							actor.PC.rigidbody2D.AddForce(Vector2.up * applyForce);
						}
					}
				}
				
				if(target.Level < actor.Level)		//Target is below Actor
				{
					if(tmpDist <= fallDist)			//Fall distance Ddtection
					{
						actor.PC.HeroAct = ActorAction.Fall;
					}
				}
				
				if( actor.PC.HeroAct != ActorAction.Jump && actor.PC.HeroAct != ActorAction.Fall && actor.PC.HeroAct != ActorAction.Attack)	//Actor running animation condition check
				{
					actor.PC.HeroAct = ActorAction.Running;
				}
			}
			
			if(mActor == -1)	//Actor is Enemy
			{
				if(target.Level > actor.Level)		//Target.y is higher than Actor.y
				{
					if(actor.EC.Jumpable)			//Determine if Actor is in a state that is permitted to jump
					{
						if(tmpDist <= jumpDist)		//Jump distant detection
						{
							float applyForce = jumpForce * tmpDist / jumpDist;
							actor.EC.Jumpable = false;
							actor.EC.EnemAct = ActorAction.Jump;
							actor.EC.rigidbody2D.AddForce(Vector2.up * jumpForce);
//							actor.EC.rigidbody2D.AddForce(Vector2.up * applyForce);
						}
					}
				}
				
				if(target.Level < actor.Level)	//Target.y is lower than Actor.y
				{
					if(tmpDist <= fallDist)		//Fall distant detection
					{
						actor.EC.EnemAct = ActorAction.Fall;
					}
				}
				
				if( actor.EC.EnemAct != ActorAction.Jump && actor.EC.EnemAct != ActorAction.Fall && actor.EC.EnemAct != ActorAction.Attack)	//Actor running animation condition check
				{
					actor.EC.EnemAct = ActorAction.Running;
				}
			}
		}
		else if( mFacing == 1 )		//Facing right
		{
			act.localRotation = Quaternion.identity;	//Rotation is done in respect to texture orientation
			
			if(mActor == 1)		//Actor is Hero
			{
				if(target.Level > actor.Level)		//Target is above Actor
				{
					if(actor.PC.Jumpable)			//Determine if Actor is in a state that is permitted to jump
					{
						if(tmpDist <= jumpDist)		//Jump distant detection
						{
							float applyForce = jumpForce * tmpDist / jumpDist;
							actor.PC.Jumpable = false;
							actor.PC.HeroAct = ActorAction.Jump;
							actor.PC.rigidbody2D.AddForce(Vector2.up * jumpForce);
//							actor.PC.rigidbody2D.AddForce(Vector2.up * applyForce);
						}
					}
				}
				
				if(target.Level < actor.Level)		//Target is below Actor 
				{
					if(tmpDist <= fallDist)			//Fall distant detection
					{
						actor.PC.HeroAct = ActorAction.Fall;
					}
				}
				
				if( actor.PC.HeroAct != ActorAction.Jump && actor.PC.HeroAct != ActorAction.Fall && actor.PC.HeroAct != ActorAction.Attack)
				{
					actor.PC.HeroAct = ActorAction.Running;
				}
			}
			
			if(mActor == -1)	//Actor is Enemy
			{
				if(target.Level > actor.Level)		//Target is above Actor
				{
					if(actor.EC.Jumpable)			//Determine if Actor is in a state that is permitted to jump
					{
						if(tmpDist <= jumpDist)		//Jump distant detection
						{
							float applyForce = jumpForce * tmpDist / jumpDist;
							actor.EC.Jumpable = false;
							actor.EC.EnemAct = ActorAction.Jump;
							actor.EC.rigidbody2D.AddForce(Vector2.up * jumpForce);
//							actor.EC.rigidbody2D.AddForce(Vector2.up * applyForce);
						}
					}
				}
				
				if(target.Level < actor.Level)		//Target is below Actor
				{
					if(tmpDist <= fallDist)			//Fall distant detection
					{
						actor.EC.EnemAct = ActorAction.Fall;
					}
				}
				
				if( actor.EC.EnemAct != ActorAction.Jump && actor.EC.EnemAct != ActorAction.Fall && actor.EC.EnemAct != ActorAction.Attack)	//Actor running animation condition check
				{
					actor.EC.EnemAct = ActorAction.Running;
				}
			}
		}
		act.position = new Vector2(mDistX, act.position.y);		//*** Actor Move Function ***//
	}

	/// <summary>
	/// Reads the actor.
	/// </summary>
	/// <param name="act">Act. "Hero" or "Enemy"</param>
	void ReadActor(Transform act)
	{
		if(act.tag == "Hero")
		{
			actor.Level = PlayerControl.instance.Level;
			actor.Trans = act;
			actor.PC = PlayerControl.instance;
			mActor = 1;
		}
		
		if(act.tag == "Enemy")
		{
			actor.Level = EnemyControl.instance.Level;
			actor.Trans = act;
			actor.EC = EnemyControl.instance;
			mActor = -1;
		}
	}

	/// <summary>
	/// Reads the target.
	/// </summary>
	/// <param name="tar">Tar. "Card" or "Hero"</param>
	void ReadTarget(Transform tar)
	{
		if(tar.tag == "Card")	//Hero's target
		{
			target.Level = tar.gameObject.GetComponent<BaseCard>().Level;
			target.Trans = tar;
		}
		
		if(tar.tag == "Hero")	//Enemy's target
		{
			target.Level = PlayerControl.instance.Level;
			target.Trans = tar;
		}
	}

	void ReadTarget(Point tar)
	{
		target.Level = tar.GetLevel();
	}
}
