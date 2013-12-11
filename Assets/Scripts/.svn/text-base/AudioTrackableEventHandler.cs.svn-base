using UnityEngine;
using System.Collections;

public class AudioTrackableEventHandler : DefaultTrackableEventHandler, IVirtualButtonEventHandler
{
	public AudioClip clipToPlay = null;
	public float fadeTimer = 4.0f;
	
	public string albumCoverURL = "";
	public string publicFacebookTitle = "";
	public string purchaseURL = "";
	
	public string gameCenterAchievementID = "";
				
	protected override void Start()
	{		
		VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < vbs.Length; ++i)
        {
            vbs[i].RegisterEventHandler(this);
			vbs[i].UnregisterOnDestroy = false;
        }
	}
	
	protected override void OnTrackingFound()
	{
		//FBLikeGUI.Instance.onClickedLikeButton += onClickedLikeButton;
		GameCenterBinding.reportAchievement(gameCenterAchievementID, 100.0f);
		
		GameObject ARCamera = GameObject.Find("ARCamera");
		AudioSource audio = ARCamera.GetComponent<AudioSource>();
		
		if(audio == null)
		{
			if(clipToPlay != null)
			{
				audio = ARCamera.AddComponent<AudioSource>();
			
				audio.clip = clipToPlay;
				audio.volume = 1.0f;
				audio.Play();
			}

			base.OnTrackingFound();
		}
	}
	
	protected override void OnTrackingLost()
	{
		GameObject ARCamera = GameObject.Find("ARCamera");
		AudioSource audio = ARCamera.GetComponent<AudioSource>();
		
		//FBLikeGUI.Instance.onClickedLikeButton -= onClickedLikeButton;
		
		if(audio != null && clipToPlay != null && audio.clip.name == clipToPlay.name)
		{
			StartCoroutine("fadeAudio");		
		}
		
		base.OnTrackingLost();
	}
	
	// Called when the virtual button has just been pressed.
    public void OnButtonPressed(VirtualButtonBehaviour vb)
	{
		/*if(mTrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.TRACKED)
		{
			Debug.Log(vb.VirtualButtonName + " Pressed!");
			transform.FindChild("Cube").renderer.material.color = Color.red;
		}*/
	}
		
    // Called when the virtual button has just been released.
    public void OnButtonReleased(VirtualButtonBehaviour vb)
	{
		/*if(mTrackableBehaviour.CurrentStatus == TrackableBehaviour.Status.TRACKED)
		{
			Debug.Log(vb.VirtualButtonName + " Released!");
			transform.FindChild("Cube").renderer.material.color = Color.white;
		}*/
	}
	
	protected IEnumerator fadeAudio()
	{		
		float start = 1.0F;
   	 	float end = 0.0F;
   	 	float i = 0.0F;
   	 	float step = 1.0F/fadeTimer;
		
		AudioSource audio = GameObject.Find("ARCamera").GetComponent<AudioSource>();
			
		while (i <= 1.0F) 
		{
			i += step * Time.deltaTime;
  	    	 	audio.volume = Mathf.Lerp(start, end, i);
   	    		yield return new WaitForSeconds(step * Time.deltaTime);
		}
		
		audio.Stop();
		Destroy(audio);
	}		
	
	protected void onClickedLikeButton(object sender, System.EventArgs e)
	{
		//Debug.Log(string.Format("User clicked FB Like Button for Target {0}", base.mTrackableBehaviour.name));		
		
		string message = string.Format("I just found {0} using Concord AR!", publicFacebookTitle);
		
		Hashtable ht = new Hashtable();
		ht.Add("message",message);
		ht.Add("link", purchaseURL);
		ht.Add("picture", albumCoverURL);
		FacebookBinding.graphRequest("me/feed","POST",ht);
	}
}
