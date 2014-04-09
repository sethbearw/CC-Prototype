using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour 
{
	public static CardGenerator cardGenerator ;

	void Awake ()
	{
		Init () ;
	}

	void Init ()
	{
		cardGenerator = GetComponent <CardGenerator> () ;
	}
}
