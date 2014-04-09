using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyControl : MonoBehaviour {
	
	public static EnemyControl instance;
	public float tmpForce = 0 ;
	public bool isAttacking = false;
	public PlayerControl EnemTarget;
	
	public float Speed = 10;
	public int Level = 0;
	public bool Jumpable = false;
	public GameObject[] gPlatforms;
	private Animator mAnimControl;
	private ActorAction mEnemAct ;
	private Transform mEnemObj;
	private BaseCard target;
	private bool mTurnEnd = false;
	private bool mIsPlaying = false;
	private int mMaxLevel = 0;
	private List<Point> movePath = new List<Point>();
	
	public bool TurnEnd
	{
		get
		{
			return mTurnEnd;
		}
		set
		{
			mTurnEnd = value;
		}
	}
	
	public bool IsPlaying
	{
		get
		{
			return mIsPlaying;
		}
	}
	
	public ActorAction EnemAct
	{
		get
		{
			return mEnemAct ;
		}
		set
		{
			SwitchActorAction ( value ) ;
			mEnemAct = value;
		}
	}
	
	void Awake()
	{
		instance = this;
		mEnemObj = this.transform;
		mAnimControl = GetComponent<Animator> ();
		
		gPlatforms = GameObject.FindGameObjectsWithTag("Platforms");
		
		mMaxLevel = gPlatforms.Length;
	}
	
	void Start () {
		SwitchActorAction (ActorAction.Idle);
	}

	/// <summary>
	/// Resets the attack.
	/// Called from the last Keyframe in Punch Animation Clip from Mecanim
	/// As of now, shares the same Mecanim as Hero
	/// </summary>
	public void resetAttack()	//
	{
		EnemAct = ActorAction.Idle;
	}

	/// <summary>
	/// Enemy Activates
	/// Start moving, Get PlayerControl, ET from GameController
	/// </summary>
	/// <param name="ET">E.</param>
	public void EnemStart(PlayerControl ET)
	{
		EnemTarget = ET;
		NodePlacer.instance.Initialize(EnemTarget.transform.position);
		mIsPlaying = true;

		StartCoroutine (EnemyAnimControl ());
	}

	/// <summary>
	/// Enemies the animation control.
	/// </summary>
	/// <returns>The animation control.</returns>
	IEnumerator EnemyAnimControl()
	{
		while (mIsPlaying) 
		{	
			DetectIdle();
			DetectJumpable();
			
			if(movePath.Count == 0)
			{
				movePath = NodeController.instance.Path(mEnemObj.position, EnemTarget.transform.position, Level);
			}
			
			float tarDist = Vector2.Distance(mEnemObj.position, movePath[1].GetPos());

			if(tarDist < 1)
			{
				if(mEnemObj.rigidbody2D.velocity == Vector2.zero)
				{
					movePath = NodeController.instance.Path(mEnemObj.position, EnemTarget.transform.position, Level);
				}
			}
			
			for (int i = 0; i < movePath.Count - 1; i++)
			{
				Debug.DrawLine(movePath[i].GetPos(), movePath[i + 1].GetPos(), Color.red, 0.01f);
			}
			
			if(EnemAct != ActorAction.Attack)
			{
				ActorsAI.instance.StartMove(mEnemObj, movePath[1], 1);
			}
			
			yield return null;
		}
	}

	/// <summary>
	/// Determines if the Enemy should be in Idle State
	/// </summary>
	void DetectIdle()
	{
		if(mEnemObj.rigidbody2D.velocity.y <= 0  && EnemAct == ActorAction.Jump)
		{
			EnemAct = ActorAction.Idle;
		}
	}
	
	/// <summary>
	/// Determines if Enemy is a state permitted to jump
	/// </summary>
	void DetectJumpable()
	{
		if(mEnemAct == ActorAction.Idle || mEnemAct == ActorAction.Running)
		{
			if(mEnemObj.rigidbody2D.velocity.y <= 0)
			{
				Jumpable = true;
			}
		}
	}

	/// <summary>
	/// When Enemy Collider Enters Hero
	/// </summary>
	/// <param name="hit">Hit.</param>
	void OnCollisionEnter2D(Collision2D hit)
	{
		if (hit.transform.tag == "Hero" && mIsPlaying) 		//If Collide with Hero
		{
			this.GetComponent<BoxCollider2D>().isTrigger = false;
			mEnemObj.rigidbody2D.velocity = new Vector2(0, 0);
			mEnemObj.position = new Vector2 (mEnemObj.position.x, EnemTarget.transform.position.y);
			
			if(Mathf.Abs((int)mEnemObj.position.x) == Mathf.Abs((int)EnemTarget.transform.position.x));
			{
				StartCoroutine(AnimAttack());
			}
		}
	}

	/// <summary>
	/// If Enemy Collider is already in contact with Hero's Collider
	/// </summary>
	/// <param name="hit">Hit.</param>
	void OnCollisionStay2D(Collision2D hit)
	{
		if (hit.transform.tag == "Hero" && mIsPlaying) 		//If Collide with Hero
		{
			this.GetComponent<BoxCollider2D>().isTrigger = false;
			mEnemObj.rigidbody2D.velocity = new Vector2(0, 0);
			mEnemObj.position = new Vector2 (mEnemObj.position.x, EnemTarget.transform.position.y);
			
			if(Mathf.Abs((int)mEnemObj.position.x) == Mathf.Abs((int)EnemTarget.transform.position.x));
			{
				StartCoroutine(AnimAttack());
			}
		}
	}

	void OnTriggerEnter2D(Collider2D hit)
	{
		if (hit.transform.tag == "Hero" && mIsPlaying) 
		{
			this.GetComponent<BoxCollider2D>().isTrigger = false;
			mEnemObj.rigidbody2D.velocity = new Vector2(0, 0);
			mEnemObj.position = new Vector2 (mEnemObj.position.x, EnemTarget.transform.position.y);

			if(Mathf.Abs((int)mEnemObj.position.x) == Mathf.Abs((int)EnemTarget.transform.position.x));
			{
				StartCoroutine(AnimAttack());
			}
		}

		for (int k = 0; k <= mMaxLevel; k++)	//Determine the Level of Enemy
		{
			string bTag = "Level_" + k;
			
			if(hit.transform.tag == bTag)
			{
				Level = k;
			}
		}

		if(EnemAct == ActorAction.Fall)			//Determines which Level to stop Fall State
		{
			switch(movePath[1].GetLevel())
			{
			case 0:
				if(hit.transform.tag == "Platform_0")
				{
					EnemAct = ActorAction.Idle;
				}
				break;
			case 1:
				if(hit.transform.tag == "Platform_1")
				{
					EnemAct = ActorAction.Idle;
				}
				break;
			case 2:
				if(hit.transform.tag == "Platform_2")
				{
					EnemAct = ActorAction.Idle;
				}
				break;
			}
		}
	}

	/// <summary>
	/// Perform Attack
	/// </summary>
	/// <returns>The attack.</returns>
	IEnumerator AnimAttack()
	{
		EnemAct = ActorAction.Attack;
		mIsPlaying = false;

		yield return 1;
	}


	/// <summary>
	/// Handles the State Control of Enemy
	/// </summary>
	/// <param name="act">Act.</param>
	void SwitchActorAction ( ActorAction act ) {
		switch(act)
		{
		case ActorAction.Idle:
			this.GetComponent<BoxCollider2D>().isTrigger = false;
			mAnimControl.SetBool("isIdle" ,true);
			mAnimControl.SetBool("isRunning", false);
			mAnimControl.SetBool("isJumping", false);
			mAnimControl.SetBool("isAttacking", false);
			break;
		case ActorAction.Running:
			this.GetComponent<BoxCollider2D>().isTrigger = false;
			mAnimControl.SetBool("isIdle" , false);
			mAnimControl.SetBool("isRunning", true);
			mAnimControl.SetBool("isJumping", false);
			mAnimControl.SetBool("isAttacking", false);
			break;
		case ActorAction.Jump:
			this.GetComponent<BoxCollider2D>().isTrigger = true;
			Jumpable = false;
			mAnimControl.SetBool("isIdle" ,false);
			mAnimControl.SetBool("isRunning", false);
			mAnimControl.SetBool("isJumping", true);
			mAnimControl.SetBool("isAttacking", false);
			break;
		case ActorAction.Fall:
			this.GetComponent<BoxCollider2D>().isTrigger = true;
			Jumpable = false;
			mAnimControl.SetBool("isIdle" ,false);
			mAnimControl.SetBool("isRunning", false);
			mAnimControl.SetBool("isJumping", true);
			mAnimControl.SetBool("isAttacking", false);
			break;
		case ActorAction.Attack:
			this.GetComponent<BoxCollider2D>().isTrigger = false;
			Jumpable = false;
			mAnimControl.SetBool("isIdle" ,false);
			mAnimControl.SetBool("isRunning", false);
			mAnimControl.SetBool("isJumping", false);
			mAnimControl.SetBool("isAttacking", true);
			break;
		}
	}
}
