using UnityEngine;
using System.Collections;

public class Pick : MonoBehaviour {
	
	public static string URL;
	
	public GameObject baseObject;
	
	// Use this for initialization
	void Start () 
	{
		ChangeColor.onClick = onClick;
	}
	

	public void onClick(Transform t)
	{
		URL = t.gameObject.GetComponent<ChangeColor>().URL;
		StartCoroutine(LerpCoroutine(t));
	}
	
	IEnumerator LerpCoroutine(Transform t)
	{
		Vector3 temp = transform.position;
		float speedBoost = 1.0f;
		
		while(transform.position.x != t.position.x)
		{
			temp.x = Mathf.Lerp(temp.x, t.position.x, Time.deltaTime * speedBoost);
			transform.position = temp;
			speedBoost *= 1.1f;
			yield return new WaitForEndOfFrame();
		}
	}
	
	public void Reset()
	{
		onClick(baseObject.transform);
	}
}
