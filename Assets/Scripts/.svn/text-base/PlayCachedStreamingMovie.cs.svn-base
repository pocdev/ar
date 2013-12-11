using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class PlayCachedStreamingMovie : PlayHardwareMovieClassPro {
	
	bool texturesReady; //check done textures
	bool streamReady; // if streamReady before movieReady -- stream most probaly was already a file (or short stream)
	
	// Use this for initialization
	protected override void Awake () {
		base.Awake();
	}

	protected override void Start ()
	{
		base.Start();
		movieIndex=0; // only one stream and at index 0.
	}
	
	public override void FinishedMovie(string str)
	{
		OpenGLMovieRewindIndex(movieIndex);
		Debug.Log("Half volume");
		OpenGLMovieVolumeIndex(movieIndex,0.5f); // half volume;
	}
	
	//[DllImport("__Internal")]
    //protected static extern void UnityMovieInitStream(string url);
	
	public void InitMovie(string movie)
	{
		currentMovie=movie;
		texturesReady=false; 
		streamReady=false;
		
		PlayStreamingMovie.UnityMovieInitStream(movie);
	}
	
	public void Play()
	{
		StartCoroutine(playMovieCoroutine());
	}
	
	IEnumerator playMovieCoroutine()
	{
		while(!texturesReady || !streamReady)
			yield return new WaitForEndOfFrame();
		
		playMovie = true;
	}
	
	public override void ReadyMovie (string str)
	{
		if (Application.platform != RuntimePlatform.OSXEditor) {
			if(!texturesReady) {
				Debug.Log("ReadyMovie");
				texturesReady=true;
				int textureWidth=OpenGLMovieTextureWidthIndex(movieIndex);
				int textureHeight=OpenGLMovieTextureHeightIndex(movieIndex);
				MakeTextures(textureWidth,textureHeight);
				setTiling(1.0f);
			}
		}
	}
	
	public override void streamReadFail () // only one stream
	{
		Debug.Log("StreamRead Fail");
	}
	
	public override void ReadyStream () // only one stream
	{
		streamReady=true;
	}
	
	public override void streamPause (bool isPaused) // only one stream
	{
		if(isPaused)
			Debug.Log("Stalled");
	}
}
