using UnityEngine;
using System.Collections;

public class UsherAnimController : MonoBehaviour {
	
	private bool _bFadedIn = false;
	
	void Start()
	{
		StartCoroutine(waitForFadeIn());
	}
	
	IEnumerator waitForFadeIn()
	{
		while(renderer.materials[0].color.a < 1.0f)
		{
			yield return new WaitForEndOfFrame();
		}
		
		_bFadedIn = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.touchCount == 1 && _bFadedIn)
		{
			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{				
				RaycastHit RayHit = new RaycastHit();
				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				
				if(Physics.Raycast(ray, out RayHit) )
				{
					if(RayHit.transform.name == "UsherText")
					{
						Application.OpenURL("http://www.belvederevodka.com/collection/belvedere-red");
					}
				}
			}
		}
	}
}
