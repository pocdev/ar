using UnityEngine;
using System.Collections;

public class MarioAnimationController : MonoBehaviour {
	
	public GameObject box;
	public GameObject coin;
	
	void onHit()
	{
		if(!animation.IsPlaying("Mario_Jump"))
			animation.CrossFade("Mario_Jump");
	}
	
	void onJumpEnd()
	{
		animation.CrossFadeQueued("Mario_Wave",0.3f,QueueMode.PlayNow);
		box.active = false;
	}
	
	void onBoxHit()
	{
		box.SetActiveRecursively(true);
	}
	
	void coinEnd()
	{
		coin.active = false;
	}
}
