using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	public tk2dCamera gameCamera ;
	public static GameController instance ;

	//private bool mPlayerTurn = true;

	void Awake ()
	{
		if ( !instance ) instance = this ;
		Application.targetFrameRate = 60;
	}

	public Vector3 GetWorldPosition ( Vector3 screenPosition )
	{	
		return gameCamera.camera.ScreenToWorldPoint ( screenPosition ) ;
	}

	public void StartTurn()
	{
		StartCoroutine(PlayerTurn());		//Start Player Turn
	}

	public void Update ()
	{
//		Debug.Watcher("Player Velocity", PlayerControl.instance.rigidbody2D.velocity);
//		Debug.Watcher("Player Action", PlayerControl.instance.HeroAct);
//		Debug.Watcher("TurnEnd", PlayerControl.instance.TurnEnd);
//		Debug.Watcher("Player IsPlaying", PlayerControl.instance.IsPlaying);
//		Debug.Watcher("Enemy IsPlaying", EnemyControl.instance.IsPlaying);
//		Debug.Watcher("Player Jump", PlayerControl.instance.Jumpable);
//		Debug.Watcher("AltCheck", NodeController.instance.AltCheck);
		Debug.Watcher("pLevel", PlayerControl.instance.Level);

		if(PlayerControl.instance.HeroTarget.Count > 0)
		{
			Debug.Watcher("tLevel", PlayerControl.instance.HeroTarget[0].Level);
		}

//		Debug.Watcher("Enemy Level", EnemyControl.instance.Level);
	}

	/// <summary>
	/// Player's turn.
	/// </summary>
	/// <returns>The turn.</returns>
	IEnumerator PlayerTurn()
	{
		PlayerControl.instance.HeroStart (CardController.instance.cardList);

		while(!PlayerControl.instance.TurnEnd)
		{
			yield return 1;
		}

		StartCoroutine(EnemyTurn());
	}

	/// <summary>
	/// Enemy's turn.
	/// </summary>
	/// <returns>The turn.</returns>
	IEnumerator EnemyTurn()
	{
		if(!PlayerControl.instance.IsPlaying)
		{
			EnemyControl.instance.EnemStart(PlayerControl.instance);
		}

		while(EnemyControl.instance.IsPlaying)
		{
			yield return 1;
		}

		StartCoroutine(ResetTurn());
	}

	/// <summary>
	/// Resets the turn.
	/// </summary>
	/// <returns>The turn.</returns>
	IEnumerator ResetTurn()
	{
		PlayerControl.instance.TurnEnd = false;

		yield return 1;
	}
}
