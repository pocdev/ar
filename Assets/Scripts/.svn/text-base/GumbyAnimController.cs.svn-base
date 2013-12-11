using UnityEngine;
using System.Collections;

public class GumbyAnimController : MonoBehaviour {
	
	private Animation _gumbyAnimation;
	
	// Use this for initialization
	void Start () {
		_gumbyAnimation = GetComponent<Animation>();
	}
	
	void Update()
	{		
		if(Input.touchCount == 1)
		{
			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{				
				RaycastHit RayHit = new RaycastHit();
				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				
				if(Physics.Raycast(ray, out RayHit) )
				{
					Debug.Log(RayHit.transform.name + "was hit");
					
					if(RayHit.transform.name == "Gumby")
					{
						Debug.Log("You clicked gumby!");
			
						_gumbyAnimation.CrossFadeQueued("Gumby_Poke",0.3f,QueueMode.PlayNow);
						_gumbyAnimation.CrossFadeQueued("Gumby_Wave",0.3f,QueueMode.CompleteOthers);
					}
				}
			}
		}
	}
}
