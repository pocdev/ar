using UnityEngine;
using System.Collections;

public class ParticleSystemTrackableEventHandler : DefaultTrackableEventHandler {
	
	public GameObject particleRoot;
	
	protected override void Start () 
	{
		base.Start();
	}

	protected override void OnTrackingFound()
	{
		base.OnTrackingFound();
		
		foreach(ParticleSystem PS in particleRoot.GetComponentsInChildren<ParticleSystem>())
		{
			PS.Play();
		}		
	}
	
	protected override void OnTrackingLost()
	{
		base.OnTrackingLost();
		
				
		foreach(ParticleSystem PS in particleRoot.GetComponentsInChildren<ParticleSystem>())
		{
			PS.Stop();
			PS.time = 0f;
		}
	}
	
}
