using UnityEngine;
using System.Collections;

public class AnimationInitializer : MonoBehaviour {

	public GameObject[] objectsToAnimate;
	
	public void Animate()
	{
		Rewind();
		
		foreach(GameObject GO in objectsToAnimate)
		{
			Animation anim = GO.GetComponent<Animation>();
			
			if(anim == null)
				continue;
			
			anim.Play();
		}
	}
	
	void Rewind()
	{
		Debug.Log("Rewinding");
		foreach(GameObject GO in objectsToAnimate)
		{
			Animation anim = GO.GetComponent<Animation>();
			
			if(anim == null)
				continue;
			
			anim.Stop();
			anim.Rewind();
		}
	}
}
