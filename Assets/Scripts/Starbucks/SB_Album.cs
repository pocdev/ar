using UnityEngine;
using System.Collections;

public class SB_Album : MonoBehaviour {
	
	public string streamURL;
	public bool streamEarly = false;
	
	private SimpleRotate _rotation;
	private AudioSource _audioSource;
	
	private AudioClip _clip;
	
	// Use this for initialization
	void Start () {
		_rotation = gameObject.GetComponentInChildren<SimpleRotate>();	
		_audioSource = GameObject.Find("ARCamera").GetComponent<AudioSource>();
		
		if(streamEarly)
			StartCoroutine("startStreamCoroutine");
	}
	
	public void startStream()
	{
		if(_rotation)
			_rotation.rotate = true;	
		
		StartCoroutine(streamEarly ? "playStreamCoroutine" : "startStreamCoroutine");
	}
	
	public void stopStream()
	{
		if(_rotation)
			_rotation.rotate = false;
		
		StopCoroutine("startStream");
		StopCoroutine("playStream");
		_audioSource.Stop();
	}
	
	private IEnumerator startStreamCoroutine()
	{		
		WWW audioStream = new WWW(streamURL);
		_clip = audioStream.GetAudioClip(true,true);
				
		if(!streamEarly)
		{
			while(!_clip.isReadyToPlay)
				yield return new WaitForSeconds(0.5f);	
			
			_audioSource.clip = _clip;	
			_audioSource.Play();
		}
	}
	
	private IEnumerator playStreamCoroutine()
	{
		while(!_clip.isReadyToPlay)
				yield return new WaitForSeconds(0.5f);	
			
			_audioSource.clip = _clip;	
			_audioSource.Play();	
	}
}
