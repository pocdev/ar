using UnityEngine;
using System.Collections;

public class AnimationTrackableEventHandler : DefaultTrackableEventHandler {

	protected override void OnTrackingFound()
	{
		base.OnTrackingFound();
		
		AnimationInitializer AI = GetComponentInChildren<AnimationInitializer>();
		if(AI)
			AI.Animate();
	}
}
