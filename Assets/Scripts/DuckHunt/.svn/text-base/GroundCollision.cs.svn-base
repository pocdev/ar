using UnityEngine;
using System.Collections;

public class GroundCollision : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.gameObject.name + " was the thing that hit the ground");
		
		Huntable huntable = other.gameObject.GetComponent<Huntable>();
		if(huntable)
		{
			huntable.OnHitGround();	
		}
	}
}
