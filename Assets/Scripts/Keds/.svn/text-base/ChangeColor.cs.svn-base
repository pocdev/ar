using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour {
	
	public delegate void _onClick(Transform t);
	public static _onClick onClick;
	
	static bool _isLerping = false;
	
	public GameObject target;
	public Color color;
	
	public string URL;
	
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
					if(!_isLerping)
					{
						StartCoroutine(doColor());
					
						if(onClick != null)
							onClick(this.transform);
					}
				}
			}
		}
	}
	
	IEnumerator doColor()
	{
		_isLerping = true;
		Debug.Log("Started lerping color");
		
		float speedBoost = 1.0f;
		while(target.renderer.material.color != color)
		{
			Color tempColor = Color.Lerp(target.renderer.material.color, color, Time.deltaTime * speedBoost);
			target.renderer.material.color = tempColor;
			speedBoost *= 1.1f;
			yield return new WaitForEndOfFrame();
		}
		
		Debug.Log("finished lerping color");
		_isLerping = false;
	}
}
