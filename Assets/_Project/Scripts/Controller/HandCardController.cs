using UnityEngine;
using System.Collections;
using System.Collections.Generic ;

public class HandCardController : MonoBehaviour 
{
	private List<BaseCard> mHandCards ;
	private List<BaseCard> mDeckCards ;
	private BaseCard mCurrentSelectedCard ;

	public float width ;

	public static HandCardController instance ;

	public BaseCard CurrentCard
	{
		get { return mCurrentSelectedCard ; }
		set
		{
			if ( mCurrentSelectedCard != value )
			{
				if ( value && value.InWorld ) return ;

				if ( mCurrentSelectedCard ) 
				{
					mCurrentSelectedCard.IsSelected = false ;
				}

				mCurrentSelectedCard = value ;

				if ( mCurrentSelectedCard ) 
				{
					mCurrentSelectedCard.IsSelected = true ;
				}
			}
		}
	}
	
	public List<BaseCard> handCards
	{
		get { return mHandCards ; }
	}

	public List<BaseCard> deckCards
	{
		get { return mDeckCards ; }
	}

	void Awake ()
	{
		if ( !instance ) instance = this ;
		Init () ;
	}

	void Init ()
	{
		mHandCards = new List<BaseCard> () ;
		mDeckCards = new List<BaseCard> () ;
	}

	void Start ()
	{
		StartHand () ;
	}

	public void StartHand ()
	{
		mDeckCards = PlayerController.instance.currentDeck ;
		//List<BaseCard> mList = new List<BaseCard> () ;

		for ( int i=0; i<5; i++ )
		{
			BaseCard tmpSelectedCard = mDeckCards [ Random.Range ( 0, mDeckCards.Count ) ] ;

			BaseCard tmpNewCard = Controller.cardGenerator.InstantitateNewCard ( i.ToString (), transform, tmpSelectedCard.GetType () ) ;
				tmpNewCard.InitPosition ( CalcPositionByID ( i, i * 0.001f ) ) ;

			mHandCards.Add ( tmpNewCard ) ;
			mDeckCards.Remove ( tmpSelectedCard ) ;

			if ( mDeckCards.Count == 0 ) break ;
		}
	}

	public void Remove ( BaseCard p_card )
	{
		mHandCards.Remove ( p_card ) ;
		Rearrange () ;
	}

	public void Add ( BaseCard p_card )
	{
		if ( mHandCards.Contains ( p_card ) ) return ;

		mHandCards.Add ( p_card ) ;
		Rearrange () ;
	}

	public void Rearrange ()
	{
		for ( int i=0; i<mHandCards.Count; i++ )
		{
			mHandCards[i].InitPosition ( CalcPositionByID ( i, i * 0.001f ), true ) ;
		}
	}

	Vector3 CalcPositionByID ( int id, float depth )
	{
		Vector3 retval = Vector3.zero ;
		retval.x = id * width ;
		retval.z = depth ;
		return retval ;
	}
}
