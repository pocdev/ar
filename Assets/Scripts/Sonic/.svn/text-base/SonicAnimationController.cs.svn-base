using UnityEngine;
using System.Collections;

public class SonicAnimationController : MonoBehaviour {
	
	public GameObject rings;
	
	void onHit()
	{
		if(!animation.IsPlaying("Sonic_Hit") && !rings.animation.IsPlaying("Sonic_Rings"))
		{
			animation.CrossFadeQueued("Sonic_Hit",0f,QueueMode.PlayNow);
			
			rings.SetActiveRecursively(true);
			rings.animation.Play();
		}
	}
	
	void hitAnimEnd()
	{
		animation.CrossFadeQueued("Sonic_Wave",0f,QueueMode.PlayNow);
	}
}
