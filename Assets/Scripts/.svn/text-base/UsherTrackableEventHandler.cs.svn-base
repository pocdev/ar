using UnityEngine;
using System.Collections;

public class UsherTrackableEventHandler : DefaultTrackableEventHandler {
	
	private MegaPointCache _usherFacialAnimation;
	private Animation _usherTextAnimation;
	private MeshRenderer _usherTextRenderer;
	
	public AudioClip clipToPlay;
	private AudioSource _audioSource;
	
	protected override void Start()
	{
		base.Start();
		_usherTextAnimation = GameObject.Find("UsherText_animation").GetComponent<Animation>();
		_usherTextRenderer = GameObject.Find("UsherText").GetComponent<MeshRenderer>();
		
		_audioSource = Camera.mainCamera.GetComponent<AudioSource>();
		if(_audioSource == null)
			_audioSource = Camera.mainCamera.gameObject.AddComponent<AudioSource>();
	}
	
	protected override void OnTrackingFound()
	{
		_usherFacialAnimation = GameObject.Find("usher_mesh").GetComponent<MegaPointCache>();
			
		doUsherFadeIn();
			
		base.OnTrackingFound();
			
		_audioSource.clip = clipToPlay;
		_audioSource.volume = 1.0f;
		_audioSource.Play();
		_usherFacialAnimation.time = 0;
		_usherFacialAnimation.animated = true;			
			
		_usherTextAnimation.Play();
		
	}
	
	void doUsherFadeIn()
	{
		foreach(Material mat in _usherFacialAnimation.gameObject.renderer.materials)
		{
			Color tempColor = mat.color;
			tempColor.a = 0;
			mat.color = tempColor;
		}
		
		StartCoroutine(usherFadeInCoroutine());
	}
	
	IEnumerator usherFadeInCoroutine()
	{
		float tempAlpha = 0;
		
		while(tempAlpha < 1)
		{
			tempAlpha += 10.0f/255.0f;
			
			for(int i = 0; i<=3; i++)
			{
				Color tempColor = _usherFacialAnimation.gameObject.renderer.materials[i].color;
				tempColor.a = tempAlpha;
				_usherFacialAnimation.gameObject.renderer.materials[i].color = tempColor;
			}
				yield return new WaitForEndOfFrame();
		}
		
		Material mat;// = _usherFacialAnimation.gameObject.renderer.materials[0];
		//mat.shader = Shader.Find("Diffuse");
		
		mat = _usherFacialAnimation.gameObject.renderer.materials[3];
		mat.shader = Shader.Find("Diffuse");
		
		for(int i = 4; i < _usherFacialAnimation.gameObject.renderer.materials.Length; i++)
		{
			mat = _usherFacialAnimation.gameObject.renderer.materials[i];
			Color tempColor = mat.color;
			tempColor.a = 1;
			mat.color = tempColor;
			mat.color = tempColor;
			mat.shader = Shader.Find("Diffuse");
		}
	}
	
	protected override void OnTrackingLost()
	{		
		if(_audioSource != null && _audioSource.clip.name == clipToPlay.name)
		{
			_audioSource.Stop();
		}
		
		if(_usherFacialAnimation && _usherFacialAnimation.gameObject.renderer.materials.Length > 0)
		{
			foreach(Material mat in _usherFacialAnimation.gameObject.renderer.materials)
			{
				if(mat.name.Contains("dark_glasses"))
					continue;
				
				Color tempColor = mat.color;
				tempColor.a = 0;
				mat.color = tempColor;
				mat.shader = Shader.Find("Transparent/Diffuse");
			}
			
			_usherTextAnimation.Stop();
			_usherTextAnimation.Rewind();
			
			foreach(Material mat in _usherTextRenderer.materials)
			{
				Color tempColor = mat.color;
				tempColor.a = 0;
				mat.color = tempColor;
			}
		}
		
		base.OnTrackingLost();
	}
}
