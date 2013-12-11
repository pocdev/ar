using UnityEngine;
using System.Collections;

public class DHTrackableEventHandler : DefaultTrackableEventHandler {

	public Terrain terrain;
	public Animation[] objectsToAnimate;
	
	protected override void Start()
	{
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
	}
	
	protected override void OnTrackingFound()
	{
		base.OnTrackingFound();
		terrain.enabled = true;
		
		foreach(Animation anim in objectsToAnimate)
			anim.Play();
	}
	
	protected override void OnTrackingLost()
	{
		terrain.enabled = false;
		base.OnTrackingLost();
		
		foreach(Animation anim in objectsToAnimate)
		{
			if(anim.isPlaying)
				anim.Stop();
			anim.Rewind();
		}
	}
}
