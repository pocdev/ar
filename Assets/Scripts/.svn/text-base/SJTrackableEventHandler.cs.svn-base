using UnityEngine;
using System.Collections;

public class SJTrackableEventHandler : DefaultTrackableEventHandler 
{
	public string levelToLoad;
	
	bool _doLoading = true;
	
	// Use this for initialization
	protected override void Start () 
	{
		if(Application.platform != RuntimePlatform.OSXEditor)
			levelToLoad = "Sunjammer_" + (SystemInfo.deviceModel.Contains("iPhone") ? "iPhone" : "iPad");
				
		base.Start();
	}
	
	protected override void OnTrackingFound()
	{
		StartCoroutine(loadLevel());
	}
	
	protected override void OnTrackingLost()
	{
		base.OnTrackingLost();
	}
	
	IEnumerator loadLevel()
	{
		if(_doLoading)
		{	
			AsyncOperation async = Application.LoadLevelAdditiveAsync(levelToLoad);
			yield return async;
			
			GetComponent<AnimationController>().Subscribe();
			
			_doLoading = false;
		}
		
		base.OnTrackingFound();
	}
}
