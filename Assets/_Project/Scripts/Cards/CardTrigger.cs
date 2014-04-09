using UnityEngine;
using System.Collections;

public class CardTrigger : MonoBehaviour 
{
	private BaseCard mBaseCard ;
	public BaseCard Card
	{
		get 
		{ 
			if ( mBaseCard == null ) mBaseCard = GetComponent <BaseCard> () ;
			return mBaseCard;
		}
	}
	
	void OnTriggerExit2D ( Collider2D p_collider )
	{
		if ( p_collider.gameObject.layer == 14 )
		{
			Card.InWorld = true ;
		}
	}

	void OnTriggerEnter2D ( Collider2D p_collider )
	{
		if ( p_collider.gameObject.layer == 14 )
		{
			Card.InWorld = false ;
		}
	}

}
