using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

// See base class for pause/resumer etc .....
public class PlayStreamingMovie : PlayHardwareMovieClassPro {
	
	public GUIText pauseIndictator; // need to display something if movie pauses with stalled stream
	bool doneReady; //check done textures
	bool streamEarly; // if streamReady before movieReady -- stream most probaly was already a file (or short stream)
	
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
	
	[DllImport("__Internal")]
    public static extern void UnityMovieInitStream(string url);
	
	public override void PlayMovie (string movie)
	{
		currentMovie=movie;
		doneReady=false; 
		streamEarly=false;
		if (Application.platform != RuntimePlatform.OSXEditor) {
			UnityMovieInitStream(movie);// use audio, start at beginning.
		}
		if (Application.platform == RuntimePlatform.OSXEditor) {
			playMovie=true;
		}
	}
	
	public override void ReadyMovie (string str)
	{
		if (Application.platform != RuntimePlatform.OSXEditor) {
			if(!doneReady) {
				Debug.Log("ReadyMovie");
				doneReady=true;
				int textureWidth=OpenGLMovieTextureWidthIndex(movieIndex);
				int textureHeight=OpenGLMovieTextureHeightIndex(movieIndex);
				MakeTextures(textureWidth,textureHeight);
				setTiling(1.0f);
			}
		}
		if(streamEarly) {
			playMovie = true; // only play once stream is ready.
		} 
		// do not start to play yet ... wait for streamReady
	}
	
	public override void streamReadFail () // only one stream
	{
		pauseIndictator.text="StreamRead Fail";
	}
	
	public override void ReadyStream () // only one stream
	{
		if(!doneReady) {
			streamEarly=true;
		} else {
			playMovie = true; // only play once stream is ready.
		}
	}
	
	public override void streamPause (bool isPaused) // only one stream
	{
		if(isPaused) {
			pauseIndictator.text="Stalled";
		} else {
			pauseIndictator.text="";
		}
	}
}
