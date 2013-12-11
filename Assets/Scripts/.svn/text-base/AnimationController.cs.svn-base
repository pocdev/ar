using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(SJTrackableEventHandler))]
public class AnimationController : MonoBehaviour 
{	
	public float desiredAnimationLength = 60f; //Makes the text labels display larger values as if the animation was longer but time scaled.
	private float animationGUIScaleFactor;
	
	public float animationStartTime = 0f;
	
	private Slider _slider;
	private TimeLabels _timeLabels;
	private AnimToggleButton _animToggleButton;
	
	private bool _autoPlay = false;
	private float _animationDuration = 0f;
	
	private List<AnimationState> _animStates;
	
	void Start()
	{
		_animStates = new List<AnimationState>();
		foreach(Animation anim in GetComponentsInChildren<Animation>())
		{
			_animStates.Add(anim[anim.clip.name]);
			anim.Play();
		}
				
		ResetAnimations();
	}
	
	void ResetAnimations()
	{
		foreach(AnimationState state in _animStates)
		{
			state.speed = 0f;
			state.time = animationStartTime;
			
			if(state.length > _animationDuration)
				_animationDuration = state.length;
		}
		
		foreach(MegaPointCache MPC in GetComponentsInChildren<MegaPointCache>())
		{
			MPC.time = animationStartTime;
			MPC.animated = false;
			
			if(MPC.maxtime > _animationDuration)
				_animationDuration = MPC.maxtime;
		}
		
		animationGUIScaleFactor = desiredAnimationLength / (_animationDuration-animationStartTime);
	}
	
	public void Subscribe()
	{
		_slider = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
		_animToggleButton = GameObject.FindGameObjectWithTag("AnimToggleButton").GetComponent<AnimToggleButton>();
		
		_slider.onSliderChanged += onSliderChanged;
		_slider.onSliderPressed += onSliderPressed;
		_slider.onSliderReleased += onSliderReleased;
		
		_animToggleButton.onToggleChanged += onToggleChanged;
		
		_timeLabels = GameObject.FindGameObjectWithTag("TimeLabels").GetComponent<TimeLabels>();
		_timeLabels.setDuration(desiredAnimationLength);
		//_timeLabels.setDuration(Mathf.Round(_animationDuration - animationStartTime));
		_timeLabels.setCurrentTime(0f);
	}
	
	void onSliderPressed()
	{
		_animToggleButton.pauseAnimation();
	}
	
	void onSliderReleased()
	{
	}
		
	void onSliderChanged(float val)
	{	
		float time = Mathf.Clamp(_animationDuration * val,animationStartTime,_animationDuration);
		
		foreach(AnimationState state in _animStates)
		{
			state.time = time;
		}
		
		foreach(MegaPointCache MPC in GetComponentsInChildren<MegaPointCache>())
		{
			MPC.time = time;
		}
	}
	
	void onToggleChanged(bool autoPlay)
	{
		_autoPlay = autoPlay;
		
		if(autoPlay)
		{
			foreach(AnimationState state in _animStates)
			{
				if(state.normalizedTime >= 1f)
				{
					ResetAnimations();
					break;
				}
			}
		}
		
		foreach(AnimationState state in _animStates)
		{
			state.speed = _autoPlay ? 1f : 0f;
		}
		
		foreach(MegaPointCache MPC in GetComponentsInChildren<MegaPointCache>())
		{
			MPC.animated = _autoPlay;
		}
	}
	
	void Update()
	{
		if(_autoPlay)
		{
			float normalizedTime = 0f;
			
			foreach(AnimationState state in _animStates)
			{
				if(state.normalizedTime > normalizedTime)
					normalizedTime = state.normalizedTime;
			}
			
			if(normalizedTime >= 1f)
			{
				_animToggleButton.pauseAnimation();
			}
			else
				_slider.setSliderValue(normalizedTime);
		}
		
		if(_timeLabels != null)
		{
			float time = Mathf.Clamp(_animStates[1].time - animationStartTime,0f,_animationDuration - animationStartTime);
			_timeLabels.setCurrentTime(time * animationGUIScaleFactor);
		}
	}
	
}
