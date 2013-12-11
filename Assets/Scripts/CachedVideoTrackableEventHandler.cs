using UnityEngine;
using System.Collections;

public class CachedVideoTrackableEventHandler : DefaultTrackableEventHandler {

	public string streamingVideoURL = "";
	public string movieCoverURL = "";
	public string publicFacebookTitle = "";
	public string purchaseURL = "";
	public string gameCenterAchievementID = "";
	
	public GameObject liveTrailer;
	public GameObject objectToAnimate = null;
	
	PlayCachedStreamingMovie _PSM;
	
	
	protected override void Start()
	{
		_PSM = liveTrailer.GetComponent<PlayCachedStreamingMovie>();
		
		ForwardiOSMessages.movie.Clear();
		ForwardiOSMessages.movie.Add(_PSM);
		
		_PSM.InitMovie(streamingVideoURL);
		
		base.Start();
		Home.onClickedHomeButton += clearMovieQueue;
	}
	
	protected override void OnTrackingFound()
	{
		base.OnTrackingFound();
		GameCenterBinding.reportAchievement(gameCenterAchievementID, 100.0f);			
		
		_PSM.Play();
		_PSM.AudioLevel(0.75f);
		
		if(objectToAnimate != null)
		{
			objectToAnimate.animation.Play();
		}
	}
	
	protected override void OnTrackingLost()
	{									
		_PSM.StopMovie();
		_PSM.AudioLevel(0.0f);
				
		base.OnTrackingLost();
	}
	
	void onClickedLikeButton(object sender, System.EventArgs e)
	{
		//Debug.Log(string.Format("User clicked FB Like Button for Target {0}", base.mTrackableBehaviour.name));		
		
		string message = string.Format("I just found {0} using Concord AR!", publicFacebookTitle);
		
		Hashtable ht = new Hashtable();
		ht.Add("message",message);
		ht.Add("link", purchaseURL);
		ht.Add("picture", movieCoverURL);
		FacebookBinding.graphRequest("me/feed","POST",ht);
	}
	
	void clearMovieQueue()
	{
		Home.onClickedHomeButton -= clearMovieQueue;
		
		_PSM.AudioLevel(0.0f);
		_PSM.StopMovie();
		Destroy(_PSM);
		
		ForwardiOSMessages.movie.Clear();				
	}
}
