using UnityEngine;
using System.Collections;

public class StreamingAudioTrackableEventHandler : DefaultTrackableEventHandler 
{
	public string audioStreamSource;
	public string gameCenterAchievementID = "";
	
	public float fadeTimer = 4.0f;
	
	public string albumCoverURL = "";
	public string publicFacebookTitle = "";
	public string purchaseURL = "";
	
	public bool streamEarly = false;
	
	public GameObject[] objectsToAnimate;
	
	private AudioClip _clipToStream;
	private AudioSource _audioSource;
	
	protected override void Start()
	{
		_audioSource = Camera.mainCamera.GetComponent<AudioSource>();
		if(_audioSource == null)
			_audioSource = Camera.mainCamera.gameObject.AddComponent<AudioSource>();		
	
		base.Start();
		
		if(audioStreamSource != null && audioStreamSource != "" && streamEarly)
			StartCoroutine(startStream());
	}
		
	protected override void OnTrackingFound()
	{
		base.OnTrackingFound();
		//FBLikeGUI.Instance.onClickedLikeButton += onClickedLikeButton;
		if(audioStreamSource != null && audioStreamSource != "")
			StartCoroutine( streamEarly ? playStream() : startStream() );
		
		foreach(GameObject GO in objectsToAnimate)
		{
			Animation anim = GO.GetComponent<Animation>();
			
			if(anim == null)
				continue;
			
			anim.Play();
		}
	}
	
	protected override void OnTrackingLost()
	{
		if(_audioSource != null)
			_audioSource.Stop();
		
		//FBLikeGUI.Instance.onClickedLikeButton -= onClickedLikeButton;
		
		foreach(GameObject GO in objectsToAnimate)
		{
			if(GO == null)
				continue;
			
			Animation anim = GO.GetComponent<Animation>();
			
			if(anim == null)
				continue;
			
			anim.Stop();
			anim.Rewind();
		}
		
		base.OnTrackingLost();
	}
	
	IEnumerator startStream()
	{
		_audioSource.Stop();
		
		WWW audioStream = new WWW(audioStreamSource);
		AudioClip clip = audioStream.GetAudioClip(true,true);
		_audioSource.clip = clip;
		
		if(!streamEarly)
		{
			while(!clip.isReadyToPlay)
				yield return new WaitForSeconds(0.5f);	
			
			_audioSource.Play();
		}
	}
	
	IEnumerator playStream()
	{
		while(!_audioSource.clip.isReadyToPlay)
			yield return new WaitForEndOfFrame();
		
		_audioSource.Play();
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
