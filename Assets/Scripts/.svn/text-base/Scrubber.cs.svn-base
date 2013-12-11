using UnityEngine;
using System.Collections;

public class Scrubber : MonoBehaviour {
	
	public float inertiaDuration = .5f;
	public float dampening = 100f;
	public float maxScrub = 5;
	public float minScrub = -5;
	public float maxScrollVelocity = 750;
	
	public delegate void ScrubValueChangedEventHandler(float val); 
	public event ScrubValueChangedEventHandler OnScrubValueChanged;
	
	private bool _scrubberActive = false;
	private float scrollVelocity = 0f;
	private float timeTouchPhaseEnded = 0f;
	private Vector2 scrollPosition;
	
	private float _scrollPercentage = 0f;
	
	private PlayButton _playButton;
	
	public void Start()
	{
		scrollPosition.y = minScrub;
		renderer.material.mainTextureOffset = scrollPosition;
		_playButton = GameObject.Find("PlayButton").GetComponent<PlayButton>();
	}
	
	public void onPress()
	{
		_scrubberActive = true;
		scrollVelocity = 0.0f;
	}
		
	void Update()
	{
		//Debug.Log(string.Format("SCROLLPOSITION: {0},{1}",scrollPosition.x, scrollPosition.y));
		
		if (Input.touchCount != 1)
		{
			_scrubberActive = false;

			if ( scrollVelocity != 0.0f )
			{
				// slow down over time
				float t = (Time.time - timeTouchPhaseEnded) / inertiaDuration;
				float frameVelocity = Mathf.Lerp(scrollVelocity, 0, t);
				scrollPosition.y -= frameVelocity * Time.deltaTime / dampening;
				
				// after N seconds, we've stopped
				if (t >= inertiaDuration) scrollVelocity = 0.0f;
				
				scrollPosition.y = Mathf.Clamp(scrollPosition.y,minScrub,maxScrub);
				renderer.material.mainTextureOffset = scrollPosition;
				
				_scrollPercentage = ((scrollPosition.y - (minScrub)) /  (maxScrub - (minScrub)));
				
				if(OnScrubValueChanged != null)
					OnScrubValueChanged(_scrollPercentage);
			}
			return;
		}
		
		Touch touch = Input.touches[0];
		if (touch.phase == TouchPhase.Canceled)
		{
			_scrubberActive = false;
		}
		
		if(!_scrubberActive) 
			return;
		else if (touch.phase == TouchPhase.Moved)
		{
			_playButton.manualOverride();
			
			// dragging
			scrollPosition.y -= touch.deltaPosition.y / dampening;
			
			scrollPosition.y = Mathf.Clamp(scrollPosition.y,minScrub,maxScrub);
			renderer.material.mainTextureOffset = scrollPosition;
			
			if(OnScrubValueChanged != null)
				OnScrubValueChanged( ((scrollPosition.y - (minScrub)) /  (maxScrub - (minScrub))));
		}
		else if (touch.phase == TouchPhase.Ended)
		{
            _scrubberActive = false;
			
			// impart momentum, using last delta as the starting velocity
			// ignore delta < 10; precision issues can cause ultra-high velocity
			if (Mathf.Abs(touch.deltaPosition.y) >= 10) 
				scrollVelocity = Mathf.Clamp((int)(touch.deltaPosition.y / touch.deltaTime),-maxScrollVelocity,maxScrollVelocity);
				
			timeTouchPhaseEnded = Time.time;
		}		
	}
	
	public void setScrollPosition(float percentage)
	{
		if(OnScrubValueChanged != null)
			OnScrubValueChanged(percentage);
		
		scrollPosition.y = (percentage*(maxScrub-minScrub))+minScrub;
		renderer.material.mainTextureOffset = scrollPosition;
	}
}
