using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardGenerator : MonoBehaviour {

	public GameObject cardPrefab ;

	public BaseCard InstantitateNewCard ( string p_name, Transform p_parent, System.Type p_type )
	{
		GameObject 	tmpGO 					= Instantiate ( cardPrefab.gameObject ) as GameObject ;
					tmpGO.name 				= p_name ;
					tmpGO.transform.parent 	= p_parent ;

		return tmpGO.AddComponent ( p_type ) as BaseCard ;
	}
}
