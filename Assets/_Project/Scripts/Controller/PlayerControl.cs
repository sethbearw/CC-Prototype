using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {

	public static PlayerControl instance;
	public float tmpForce = 0 ;
	public bool isAttacking = false;
	public List<BaseCard> HeroTarget;

	public float Speed = 10;
	public int Level = 0;
	public bool Jumpable = false;
	public GameObject[] gPlatforms;
	private Animator mAnimControl;
	private ActorAction mHeroAct ;
	private Transform mHeroObj;
	private BaseCard target;
	private bool mTurnEnd = false;
	private bool mIsPlaying = false;
	private int mMaxLevel = 0;
	private Vector3 mDeltaPosition;
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

	public ActorAction HeroAct
	{
		get
		{
			return mHeroAct ;
		}
		set
		{
			SwitchActorAction ( value ) ;
			mHeroAct = value;
		}
	}

	void Awake()
	{
		instance = this;
		mHeroObj = this.transform;
		mAnimControl = GetComponent<Animator> ();
	
		gPlatforms = GameObject.FindGameObjectsWithTag("Platforms");

		mMaxLevel = gPlatforms.Length;
	}

	void Start () {
		SwitchActorAction (ActorAction.Idle);
	}

	/// <summary>
	/// Resets the attack.
	/// Calls from the last key frame from Attack Animation Clip from Mecanim
	/// </summary>
	public void resetAttack()
	{
		HeroAct = ActorAction.Idle;
	}

	/// <summary>
	/// Heros Activates
	/// Start moving, Get List of BaseCard, HT from GameController
	/// </summary>
	/// <param name="HT">H.</param>
	public void HeroStart(List<BaseCard> HT)
	{
		HeroTarget = HT;
		mIsPlaying = true;

		foreach (BaseCard c in HeroTarget)		//Enable all Colliders in List
		{
			float xPos = c.transform.position.x;
			float yPos = c.transform.position.y;
			c.collider2D.enabled = false;
			c.transform.position = new Vector3(xPos, yPos, 0f);
		}

		StartCoroutine (HeroAnimControl ());
	}

	/// <summary>
	/// Animation Control for Hero
	/// </summary>
	/// <returns>The animation control.</returns>
	IEnumerator HeroAnimControl()
	{
		while (HeroTarget.Count > 0) 	//If Hero still has target
		{	
			DetectIdle(); 
			DetectJumpable();
			DetectNodes();

			HeroTarget[0].collider2D.enabled = true;

			if(movePath.Count == 0)
			{
				movePath = NodeController.instance.Path(mHeroObj.position, HeroTarget[0].transform.position, Level);
			}

			float tarDist = Vector2.Distance(mHeroObj.position, movePath[1].GetPos());

			if(tarDist < 1)
			{
				if(mHeroObj.rigidbody2D.velocity == Vector2.zero)
				{
					movePath = NodeController.instance.Path(mHeroObj.position, HeroTarget[0].transform.position, Level);
				}
			}
			
			for (int i = 0; i < movePath.Count - 1; i++)
			{
				Debug.DrawLine(movePath[i].GetPos(), movePath[i + 1].GetPos(), Color.white, 0.01f);
			}
			
			if(HeroAct != ActorAction.Attack)
			{
				ActorsAI.instance.StartMove(mHeroObj, movePath[1], 1);
			}

			yield return 1;
		}
		StartCoroutine(DetectTurnEnd());
	}

	IEnumerator DetectTurnEnd()
	{
		while(!mTurnEnd)
		{
			if(HeroTarget.Count <= 0)	//Turn End Trigger Variables
			{
				if(HeroAct != ActorAction.Attack)
				{
					mIsPlaying = false;
					mTurnEnd = true;
				}
			}
			yield return 1;
		}
	}
	/// <summary>
	/// Detects if Hero should be in Idle State.
	/// </summary>
	void DetectIdle()
	{
		if(mHeroObj.rigidbody2D.velocity.y <= 0  && HeroAct == ActorAction.Jump)
		{
			HeroAct = ActorAction.Idle;
		}
	}

	/// <summary>
	/// Determines if the Hero is in a state permitted to jump
	/// </summary>
	void DetectJumpable()
	{
		if(mHeroAct == ActorAction.Idle || mHeroAct == ActorAction.Running)
		{
			if(mHeroObj.rigidbody2D.velocity.y <= 0)
			{
				Jumpable = true;
			}
		}
	}

	void DetectNodes()
	{
		GameObject[] nodes = GameObject.FindGameObjectsWithTag(NodeController.instance.nodeTag);
		bool isExist = false;

		foreach(GameObject go in nodes)
		{
			if(HeroTarget[0].transform.position == go.transform.position)
			{
				isExist = true;
			}
		}

		if(!isExist)
		{
			NodePlacer.instance.Initialize(HeroTarget[0].transform.position);
		}
	}

	void OnTriggerEnter2D(Collider2D hit)
	{
		if (hit.transform.tag == "Card" && mIsPlaying) 		//Collide with BaseCard
		{
			this.GetComponent<BoxCollider2D>().isTrigger = false;
			mHeroObj.rigidbody2D.velocity = new Vector2(0, 0);

			if(Mathf.Abs((int)mHeroObj.position.x) == Mathf.Abs((int)HeroTarget[0].transform.position.x));
			{
				StartCoroutine(EaseTo(hit));		//Move closer into Target to perform Attack
			}
		}

		for (int k = 0; k <= mMaxLevel; k++)	//Determines the Height Level of the Hero
		{
			string bTag = "Level_" + k;

			if(hit.transform.tag == bTag)
			{
				Level = k;
			}
		}

		if(HeroAct == ActorAction.Fall)			//Determines which Level to stop Fall State
		{
			switch(movePath[1].GetLevel())
			{
			case 0:
				if(hit.transform.tag == "Platform_0")
				{
					HeroAct = ActorAction.Idle;
				}
				break;
			case 1:
				if(hit.transform.tag == "Platform_1")
				{
					HeroAct = ActorAction.Idle;
				}
				break;
			case 2:
				if(hit.transform.tag == "Platform_2")
				{
					HeroAct = ActorAction.Idle;
				}
				break;
			}
		}

		if(hit.transform.tag == "Platform_0")	//Hero always will be able to land on Level 0
		{
			HeroAct = ActorAction.Idle;
		}

//		for( int j = 0; j <= mMaxLevel; j++ )			//*** Might Be Important ***//
//		{
//			string pTag = "Platform_" + j;
//
//			if(hit.transform.tag == pTag && HeroAct != ActorAction.Jump)
//			{
//				this.GetComponent<BoxCollider2D>().isTrigger = false;
//			}
//		}
	}

	/// <summary>
	/// Move Closer into target to perform attack
	/// </summary>
	/// <returns>The to.</returns>
	/// <param name="hit">Hit.</param>
	IEnumerator EaseTo(Collider2D hit)
	{
		while(mHeroObj.position != hit.transform.position)
		{
			mHeroObj.position = Vector3.Lerp(transform.position, hit.transform.position, 10 * Time.deltaTime);

		}
		yield return StartCoroutine(AnimAttack(hit));
	}

	/// <summary>
	/// Performs Attack
	/// </summary>
	/// <returns>The attack.</returns>
	/// <param name="hit">Hit.</param>
	IEnumerator AnimAttack(Collider2D hit)		//Performs Attack Animation
	{
		BaseCard tmpCard = hit.gameObject.GetComponent<BaseCard>() ;
		
		HeroAct = ActorAction.Attack;		//Change this line to change attack animation
		
		if ( tmpCard == HeroTarget[0] )
		{
			GameObject[] nodes = GameObject.FindGameObjectsWithTag(NodeController.instance.nodeTag);

			foreach(GameObject go in nodes)
			{
				if(hit.transform.position == go.transform.position)
				{
					Destroy(go);
				}
			}

			HeroTarget.RemoveAt ( 0 ) ;
			Destroy(hit.gameObject);
		}

		yield return 1;
	}

	/// <summary>
	/// Handles the State Control of Hero
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
