using UnityEngine;
using System.Collections;
using System.Collections.Generic ;

public class PlayerController : MonoBehaviour 
{
	public List<BaseCard> currentDeck = new List<BaseCard> () ;
	public static PlayerController instance ;

	void Awake ()
	{
		if ( !instance ) instance = this ;
	}

	void Start ()
	{
		currentDeck.Add ( new BaseCard () ) ;
		currentDeck.Add ( new PhysicCard () ) ;
		currentDeck.Add ( new PhysicCard () ) ;
		currentDeck.Add ( new PhysicCard () ) ;
		currentDeck.Add ( new PhysicCard () ) ;
		currentDeck.Add ( new PhysicCard () ) ;
	}
}
