using UnityEngine;
using System.Collections;

public class KedsTrackableEventHandler : DefaultTrackableEventHandler {
	
	public GameObject kedsRoot;
	public GameObject kedsPick;
	
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
	}
	
	protected override void OnTrackingFound()
	{
		
		kedsRoot.SetActiveRecursively(true);
		kedsRoot.GetComponent<Animation>().Play();
		
		base.OnTrackingFound();
		
		//GetComponent<MeshRenderer>().enabled = false;
	}
	
	protected override void OnTrackingLost()
	{
		Animation anim = kedsRoot.GetComponent<Animation>();
		
		if(anim != null)
		{
			anim.Stop();
			anim.Rewind();
		}
		
		kedsPick.GetComponent<Pick>().Reset();
		
		kedsRoot.SetActiveRecursively(false);
		
		base.OnTrackingLost();
	}
}
