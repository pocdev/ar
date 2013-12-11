using UnityEngine;
using System.Collections;

public class RingAnimController : MonoBehaviour {

	void startRings()
	{
		foreach(MeshRenderer MR in gameObject.GetComponentsInChildren<MeshRenderer>())
		{
			MR.enabled = true;
		}
	}
	
	void endRingAnim()
	{	
		foreach(MeshRenderer MR in gameObject.GetComponentsInChildren<MeshRenderer>())
		{
			MR.enabled = false;
			MR.gameObject.active = false;
		}
	}
	
}
