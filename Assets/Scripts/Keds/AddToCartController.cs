using UnityEngine;
using System.Collections;

public class AddToCartController : MonoBehaviour {

	void Update()
	{
		if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			RaycastHit RayHit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch(0).position);

			if(Physics.Raycast(ray, out RayHit))
			{
				if(RayHit.transform == this.transform)
				{
					if(Pick.URL != null && Pick.URL != "")
						Application.OpenURL(Pick.URL);
				}
			}
		}
	}
}
