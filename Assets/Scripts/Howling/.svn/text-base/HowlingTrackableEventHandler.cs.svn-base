using UnityEngine;
using System.Collections;

public class HowlingTrackableEventHandler : DefaultTrackableEventHandler {
	
	public Animation screenAnimation;
	public GameObject videoScreen;
	public string moviePath;
	public Material textMaterial;
	
	private VideoTexture _videoTexture = null;
	
	// Use this for initialization
	protected override void Start() 
	{
		base.Start();
		createMeshUVs();
		
		Color resetColor = textMaterial.color;
		resetColor.a = 1f;
		textMaterial.color = resetColor;
	}
	
	protected override void OnTrackingFound()
	{
		screenAnimation.Play();
		
		if(_videoTexture == null)
			createVideoTexture();
		else
			_videoTexture.unpause();
		
		base.OnTrackingFound();
		
		StartCoroutine("fadeOutText");
	}
	
	IEnumerator fadeOutText()
	{
		yield return new WaitForSeconds(3.5f);
		
		Color newColor = textMaterial.color;
		float speedBoost = 1.0f;
		
		while(textMaterial.color.a > 0.1f)
		{
			newColor.a = Mathf.Lerp(newColor.a, 0f, Time.deltaTime * speedBoost);
			textMaterial.color = newColor;
			speedBoost *= 1.1f;
			yield return new WaitForEndOfFrame();
		}
		
		newColor.a = 0f;
		textMaterial.color = newColor;
	}
	
	protected override void OnTrackingLost()
	{
		screenAnimation.Rewind();
		base.OnTrackingLost();
		
		if(_videoTexture != null)
			_videoTexture.pause();
	}
	
	void createVideoTexture()
	{
		_videoTexture = new VideoTexture(moviePath, 256, 256, false, 0f, true );
			
		// apply the texture to a material and set the UVs
	    videoScreen.renderer.sharedMaterial.mainTexture = _videoTexture.texture;
	    //LiveTextureBinding.updateMaterialUVScaleForTexture(videoScreen.renderer.sharedMaterial, _videoTexture.texture );
			
		
		// add some event handlers
		_videoTexture.videoDidStartEvent = () =>
		{
			Debug.Log( "Video one started" );
		};
		
		_videoTexture.videoDidFinishEvent = () =>
		{
			// when the video finishes if we are not set to loop this instance is no longer valid
			Debug.Log( "Video one finished" );
		};
		
		Home.onClickedHomeButton += destroyVideoTexture;
	}
	
	void destroyVideoTexture()
	{
		_videoTexture.stop();
		_videoTexture = null;
		
		Home.onClickedHomeButton -= destroyVideoTexture;
	}
	
	void createMeshUVs()
	{
		Mesh mesh = videoScreen.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];
        int i = 0;
        while (i < uvs.Length) 
		{
            uvs[i] = new Vector2(vertices[uvs.Length - i - 1].x/10, vertices[i].z/10);
            i++;
        }
        mesh.uv = uvs;
	}
	
}
