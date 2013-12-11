using UnityEngine;
using System.Collections;

public class CDVideo : MonoBehaviour {

	public string MovieName;
	public string iTunesLink;

	public void PlayMovie()
	{		
		PlayStreamingMovie PSM = GetComponent<PlayStreamingMovie>();
		
		if(PSM)
			PSM.ResumeMovie();
		
		else
		{
			PSM = gameObject.AddComponent<PlayStreamingMovie>();
			
			if(ForwardiOSMessages.movie.Count == 0)
				ForwardiOSMessages.movie.Add(PSM);
			
			PSM.PlayMovie(MovieName);
			PSM.AudioLevel(.75f);
		}
	}
	
	public void StopMovie()
	{
		PlayStreamingMovie PSM = GetComponent<PlayStreamingMovie>();
		if(PSM)
			PSM.PauseMovie();
	}
	
	public void clearMovieQueue()
	{
		foreach(PlayStreamingMovie PSM in GameObject.Find("Panel Manager").GetComponentsInChildren<PlayStreamingMovie>())
		{
			PSM.AudioLevel(0f);
			PSM.StopMovie();
		}
		
		ForwardiOSMessages.movie.Clear();
	}
}
