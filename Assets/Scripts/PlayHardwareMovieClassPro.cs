using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class PlayHardwareMovieClassPro : MonoBehaviour
{
	public int movieIndex;
	
	protected int currentTextureWidth=0;
	protected int currentTextureHeight=0;
	protected Texture2D m_Texture;
	
	protected Color movieColor = Color.white;
	protected Color movieEnd = Color.clear;
	protected string currentMovie;
	
	protected bool playMovie;
	protected int movieColorSet;
	
	
	// Use this for initialization
	protected virtual void Awake () {
		// so we can see thing in editor
		if (Application.platform == RuntimePlatform.OSXEditor) {
			 movieColor=Color.green;
			 movieEnd=Color.gray;
		}
	}
	
	// Link to the UnityTexture plugin and call the UpdateTexture function there.
	[DllImport("__Internal")]
	private static extern void UnityMovieInitIndex(int index,string file, bool audio, float seek);

	protected virtual void Start ()
	{
		playMovie = false;
		MakeTextures(16,16);
		//renderer.enabled=false;
	}
	
	protected void MakeTextures(int textWidth, int textHeight) {
		//if(textWidth != currentTextureWidth || textHeight != currentTextureHeight ) {
			currentTextureWidth=textWidth;
			currentTextureHeight=textHeight;
			//Debug.Log("Texture Size " + currentTextureWidth + " " + currentTextureHeight);
			// Create texture that will be updated in the plugin
			m_Texture = new Texture2D (currentTextureWidth, currentTextureHeight, TextureFormat.BGRA32, false);
		
			// Assign texture to the renderer
			if (renderer) {
				if (Application.platform != RuntimePlatform.OSXEditor) {
				Debug.Log("Assigning new texture to renderer.sharedMaterial");
					renderer.sharedMaterial.SetTexture ("_MainTex", m_Texture);
				}
				movieColorSet = 0;
				renderer.sharedMaterial.SetColor ("_Color", movieEnd);
			
			} else {
				Debug.Log ("Game object has no renderer to assign the generated texture to!");
			}
		/*} else {
			movieColorSet = 0;
			renderer.sharedMaterial.SetColor ("_Color", movieEnd);
		}*/
		
	}
	
	[DllImport("__Internal")]
	protected static extern int OpenGLMovieTextureWidthIndex(int index);
	
	[DllImport("__Internal")]
	protected static extern int OpenGLMovieTextureHeightIndex(int index);
   
	[DllImport("__Internal")]
    protected static extern int OpenGLMovieWidthIndex(int index);
    
	[DllImport("__Internal")]
    protected static extern int OpenGLMovieHeightIndex(int index);
	
	// heightfactor can be used to remove black borders 
	protected virtual void setTiling(float heightFactor) {
		float width=(float) OpenGLMovieWidthIndex(movieIndex);
		float height=(float) OpenGLMovieHeightIndex(movieIndex);
		//Debug.Log("tiling Size " + width + " " + height);
		float scaleW=width/currentTextureWidth;
		float scaleH=(height*heightFactor)/currentTextureHeight;
		float remove=(height - height*heightFactor)/currentTextureHeight;
		float heightAmount=remove/2.0f;
		float widthAmount=(1.0f - scaleW);
		renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(-widthAmount,heightAmount));
		renderer.sharedMaterial.SetTextureScale("_MainTex", new Vector2(-scaleW,scaleH));
		//Debug.Log("offset " + heightAmount + " scale " + scaleH);
	}

	[DllImport("__Internal")]
	private static extern void OpenGLMoviePauseIndex (int index);
	[DllImport("__Internal")]
	private static extern float OpenGLMovieResumeIndex (int index);
	
	public void ResumeMovie ()
	{
		if(playMovie) {
			//float resumeTime=0.0f;
			if (Application.platform != RuntimePlatform.OSXEditor) {
				//resumeTime=
					OpenGLMovieResumeIndex(movieIndex);
			}
		}
	}
	
	void OnApplicationPause(bool pause)
	{
		if(pause) {
			PauseMovie(); 
		} else {
			ResumeMovie();// MUST resume after application paase
		}
	}
	
	public void PauseMovie ()
	{
		if (playMovie) {
			if (Application.platform != RuntimePlatform.OSXEditor) {
				OpenGLMoviePauseIndex(movieIndex);
			}
		}
	}
	
	[DllImport("__Internal")]
	protected static extern void OpenGLMovieVolumeIndex(int index, float volume);
	
	public void AudioLevel(float level) {
		if (Application.platform != RuntimePlatform.OSXEditor) {
			if(level < 0f) level=0f;
			if(level > 1f) level=1f;
			OpenGLMovieVolumeIndex(movieIndex, level);
		}
	}
	
	[DllImport("__Internal")]
	protected static extern void OpenGLMovieRewindIndex(int index);
	
	public virtual void FinishedMovie(string str)
	{
		OpenGLMovieRewindIndex(movieIndex);
	}
	
	public virtual void PlayMovie (string movie)
	{
		currentMovie=movie;
		bool audio=true;
		if (Application.platform != RuntimePlatform.OSXEditor) {
			UnityMovieInitIndex(movieIndex,movie,audio,-1.0f);// use audio, start at beginning.
		}
		if (Application.platform == RuntimePlatform.OSXEditor) {
			playMovie=true;
		}
	}
	
	public virtual void PlayMovieAt (string movie, float startTime)
	{
		currentMovie=movie;
		bool audio=true;
		if (Application.platform != RuntimePlatform.OSXEditor) {
			UnityMovieInitIndex(movieIndex,movie,audio,startTime);// use audio, start at beginning.
		}
		if (Application.platform == RuntimePlatform.OSXEditor) {
			playMovie=true;
		}
	}
	
	public virtual void ReadyMovie (string str)
	{
		if (Application.platform != RuntimePlatform.OSXEditor) {
			int textureWidth=OpenGLMovieTextureWidthIndex(movieIndex);
			int textureHeight=OpenGLMovieTextureHeightIndex(movieIndex);
			MakeTextures(textureWidth,textureHeight);
			setTiling(1.0f);
		}
	}
	
	public virtual void ReadyStream () // only one stream
	{
		// not in this class
	}
	
	public virtual void streamReadFail () // only one stream
	{
		// not in this class
	}
	
	public virtual void streamPause (bool isPaused) // only one stream
	{
		// not in this class
	}

	public void StopMovie ()
	{
		//print ("stop movie");
		playMovie = false;
		renderer.sharedMaterial.SetColor ("_Color", movieEnd);
		movieColorSet = 0;
	}
		
	// Link to the UnityTexture plugin and call the UpdateTexture function there.
	[DllImport("__Internal")]
	private static extern bool OpenGLMovieUpdateTextureIndex(int index,int textureID);
	
	// Now we can simply call UpdateTexture which gets routed directly into the plugin
	void Update ()
	{
		if (playMovie) {
			if (Application.platform != RuntimePlatform.OSXEditor) {
				Debug.Log("movie Playing, updating texture");
				OpenGLMovieUpdateTextureIndex(movieIndex,m_Texture.GetNativeTextureID ());
			}
			if (movieColorSet == 3) {
				// wait a couple of frames as there seems to be a display lag
				renderer.sharedMaterial.SetColor ("_Color", movieColor);
				renderer.enabled=true;
			}
			movieColorSet++;
		}
	}

		
}
