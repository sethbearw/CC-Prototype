using UnityEngine;
using System.Collections;

public class PlatformDetection : MonoBehaviour {

	public static PlatformDetection instance;

	private bool mLevel1 = false;
	private bool mLevel2 = false;
	private bool mLevel3 = false;

	void Awake()
	{
		instance = this;
	}

	void Update()
	{
		Debug.Watcher("mLevel1", mLevel1);
		Debug.Watcher("mLevel2", mLevel2);
		Debug.Watcher("mLevel3", mLevel3);
		Debug.Watcher("Target Count", PlayerControl.instance.HeroTarget.Count);
		Debug.Watcher("IsPlaying", PlayerControl.instance.IsPlaying);
	}

	void OnTriggerStay2D (Collider2D hit)
	{
		if(PlayerControl.instance.Jumpable == false)
		{
			switch(hit.transform.tag)
			{
			case "Platform_1":
				if(PlayerControl.instance.Level == 0 && mLevel1 == false)
				{
					mLevel1 = true;
					PlayerControl.instance.Jumpable = true;
				}
				break;
			case "Platform_2":
				if(PlayerControl.instance.Level == 1 && mLevel2 == false)
				{
					mLevel2 = true;
					PlayerControl.instance.Jumpable = true;
				}
				break;
			case "Platform_3":
				if(PlayerControl.instance.Level == 2 && mLevel3 == false)
				{
					mLevel3 = true; 
					PlayerControl.instance.Jumpable = true;
				}
				break;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D hit)
	{
		switch(hit.transform.tag)
		{
		case "Platform_0":
			mLevel1 = false;
			mLevel2 = false;
			mLevel3 = false;
			break;
		case "Platform_1":
			mLevel1 = false;
			break;
		case "Platform_2":
			mLevel2 = false;
			break;
		case "Platform_3":
			mLevel3 = false;
			break;
		}
	}
}
