using UnityEngine;
using System.Collections;
using System;

public class VideoTrackableEventHandler : DefaultTrackableEventHandler {

	public string streamingVideoURL = "";
	public string movieCoverURL = "";
	public string publicFacebookTitle = "";
	public string purchaseURL = "";
	public string gameCenterAchievementID = "";
	
	public GameObject liveTrailer;
	public GameObject objectToAnimate = null;
	
	protected override void Start()
	{
		base.Start();
		Home.onClickedHomeButton += clearMovieQueue;
	}
	
	protected override void OnTrackingFound()
	{
		base.OnTrackingFound();
		//FBLikeGUI.Instance.onClickedLikeButton += onClickedLikeButton;
		GameCenterBinding.reportAchievement(gameCenterAchievementID, 100.0f);
					
		if(liveTrailer)
		{
			PlayStreamingMovie PSM = liveTrailer.GetComponent<PlayStreamingMovie>();
		
			if(!PSM)
			{
				PSM = liveTrailer.AddComponent<PlayStreamingMovie>();
				
				ForwardiOSMessages.movie.Clear();
				ForwardiOSMessages.movie.Add(PSM);
			}
			
			PSM.PlayMovie(streamingVideoURL);			
			liveTrailer.GetComponent<PlayStreamingMovie>().AudioLevel(0.75f);
			
			if(objectToAnimate != null)
			{
				objectToAnimate.animation.Play();
			}	
		}
	}
	
	protected override void OnTrackingLost()
	{									
		if(liveTrailer)
		{
			PlayStreamingMovie PSM = liveTrailer.GetComponent<PlayStreamingMovie>();
			if(PSM)
			{
				PSM.StopMovie();
				PSM.AudioLevel(0.0f);
				ForwardiOSMessages.movie.Clear();
				Destroy(PSM);
			}
		}
				
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
		PlayStreamingMovie PSM = liveTrailer.GetComponent<PlayStreamingMovie>();
		if(PSM)
		{	
			PSM.AudioLevel(0.0f);
			PSM.StopMovie();			
		}
		
		ForwardiOSMessages.movie.Clear();
						
		Home.onClickedHomeButton -= clearMovieQueue;
	}
}
