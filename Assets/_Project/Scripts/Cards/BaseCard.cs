using UnityEngine;
using System.Collections;
using Holoville.HOTween ;

[RequireComponent ( typeof ( CardTrigger ) ) ]
public class BaseCard : MonoBehaviour 
{
	public int Level = 0;
	private Vector3 	mDeltaPosition ;
	private Vector3 	mFixPosition ;
	private Vector3 	mPickedPosition ;
	private bool 		mIsSelected ;
	private bool 		mInWorld ;

	public bool IsSelected
	{
		get { return mIsSelected ; }
		set
		{
			SetDragEvent ( value ) ;
			SetPositionTweening ( value ) ;
			mIsSelected = value ;
		}
	}

	public bool InWorld
	{
		get { return mInWorld; }
		set
		{
			if ( mInWorld != value )
			{
				//string tmpStr = value? "Dragged to world" : "Dragged to hand" ;	
				mInWorld = value ;

			}
		}
	}

	void OnEnable ()
	{
		EasyTouch.On_SimpleTap += Click;
	}

	void OnDisable ()
	{
		EasyTouch.On_SimpleTap -= Click;
		SetDragEvent ( false ) ;
	}

	public void InitPosition ( Vector3 p_position, bool p_animate = false )
	{
		if ( p_animate ) TweenPosition ( p_position ) ;
		else transform.localPosition = p_position ;

		mFixPosition			= p_position ;
		mPickedPosition			= p_position + new Vector3 ( 0, -0.5f, -1 ) ;
	}

	public void TweenPosition ( Vector3 p_targetPos )
	{
		HOTween.Init ( true, true, true ) ;
		HOTween.To ( transform, 0.5f, new TweenParms ()
		            .Prop ( "localPosition", p_targetPos ) ) ;
	}

	public Vector3 Depth ( float depth )
	{
		Vector3 retval = transform.localPosition ;
		retval.z = depth ;
		return retval ;
	}

	void Click (Gesture gesture)
	{
		if ( gesture.pickObject == gameObject )
		{
			HandCardController.instance.CurrentCard = this ;
		}
	}

	void DragStart (Gesture gesture)
	{
		if ( gesture.pickObject == gameObject )
		{
			mDeltaPosition = gesture.GetTouchToWorldPoint ( 0 ) - transform.position ;
		}
	}

	void DragToMove (Gesture gesture)
	{
		if ( gesture.pickObject == gameObject )
		{
			Vector3 tmpTouchPosition = gesture.GetTouchToWorldPoint ( 0 ) ;
			transform.position = tmpTouchPosition - mDeltaPosition ;
		}
	}
	
	void DragEnd (Gesture gesture)
	{
		if ( gesture.pickObject == gameObject )
		{
			if ( mInWorld ) 
			{
				HandCardController.instance.Remove ( this ) ;
				//TODO: The card is added to the map now, Do Something

				CardController.instance.AddCard(this);

				SetDragEvent ( true ) ;
			}
			else
			{
				HandCardController.instance.Add ( this ) ;
				SetDragEvent ( false ) ;
			}

			HandCardController.instance.CurrentCard = null ;
		}
	}

	void OnTriggerEnter2D (Collider2D hit)
	{
		switch(hit.transform.tag)	//Determine the Level card is being placed
		{
		case "Level_0":
			Level = 0;
			break;
		case "Level_1":
			Level = 1;
			break;
		case "Level_2":
			Level = 2;
			break;
		case "Level_3":
			Level = 3;
			break;
		}
	}

	void SetPositionTweening ( bool switchOn )
	{
		if ( mInWorld ) return ;

		float tmpDepth		 = switchOn ? mPickedPosition.z : mFixPosition.z ;
		transform.localPosition = Depth ( tmpDepth ) ;

		Vector3 tmpTargetPos = switchOn ? mPickedPosition : mFixPosition ;
		TweenPosition ( tmpTargetPos ) ;
	}
	
	void SetDragEvent ( bool switchOn )
	{
		if ( switchOn )
		{
			EasyTouch.On_DragStart += DragStart;
			EasyTouch.On_Drag += DragToMove;
			EasyTouch.On_DragEnd += DragEnd;
		}
		else
		{
			EasyTouch.On_DragStart -= DragStart;
			EasyTouch.On_Drag -= DragToMove;
			EasyTouch.On_DragEnd -= DragEnd;
		}
	}

}
