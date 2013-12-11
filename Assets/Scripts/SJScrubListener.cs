using UnityEngine;
using System.Collections;

public class SJScrubListener : MonoBehaviour {
	
	private Scrubber _scrubber;
	
	public float animStartTime = 0f;
	private float _animLoopTime = 0f;
	
	GameObject _sail;
	AnimationState _animState;
	PlayButton _playButton;
	
	bool manualPlayMode = false;
	
	// Use this for initialization
	void Start () 
	{
		_scrubber = GameObject.FindGameObjectWithTag("Scrubber").GetComponent<Scrubber>();
		_scrubber.OnScrubValueChanged += OnScrubValueChanged;
		
		_sail = GameObject.FindGameObjectWithTag("Sail");
		_animState = _sail.animation[_sail.animation.clip.name];
		_animState.speed = 0.0f;
		_sail.animation.Play();
		
		_playButton = GameObject.Find("PlayButton").GetComponent<PlayButton>();
		
		OnScrubValueChanged(0f);
		
		foreach(MegaPointCache MPC in _sail.GetComponentsInChildren<MegaPointCache>())
		{
			if(MPC.maxtime >= _animLoopTime)
				_animLoopTime = MPC.maxtime;
		}
		
		if(_animState.length > _animLoopTime)
			_animLoopTime = _animState.length;
	}
	
	void OnScrubValueChanged(float percentage)
	{	
		float time = manualPlayMode ? _animState.time : animStartTime + (_animLoopTime * percentage);
		
		foreach(MegaPointCache MPC in _sail.GetComponentsInChildren<MegaPointCache>())			
			MPC.time = /*(percentage == 1f) ? 0f :*/ time;

		_animState.time = time;
	}
	
	public void startAnim(float animSpeed)
	{
		if(_animState.normalizedTime >= 1f)
		{
			_scrubber.setScrollPosition(0f);
			OnScrubValueChanged(0f);
		}
		
		manualPlayMode = true;
		_animState.speed = animSpeed;
		_sail.animation.Play();
	}
	
	public void stopAnim()
	{
		manualPlayMode = false;
		_animState.speed = 0f;
	}		
	
	void Update()
	{
		if(manualPlayMode)
		{
			if(_animState.time >= _animLoopTime + animStartTime)
			{
				_playButton.manualOverride();
			}
			else
			{
				_scrubber.setScrollPosition(_animState.normalizedTime - (animStartTime / _animLoopTime));
			}
		}
	}
}
