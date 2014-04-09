using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardController : MonoBehaviour {

	public static CardController instance;
	private List<BaseCard> mCardList = new List<BaseCard> () ;

	public List<BaseCard> cardList
	{
		get { return mCardList; }
	}

	public void AddCard(BaseCard value)
	{
		cardList.Add (value);
	}

	public void RemoveCard(BaseCard value)
	{
		if (cardList.Contains (value))
			cardList.Remove (value);
	}

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
}
