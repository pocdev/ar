using UnityEngine;
using System.Collections;
using System;

public class StarbucksTrackableEventHandler : DefaultTrackableEventHandler 
{
	
	public GameObject panelManager;
	protected UIPanelManager _panelManager;
	protected UIPanelManager _textPanelManager;
	
	protected override void Start()
	{
		_panelManager = panelManager.GetComponent<UIPanelManager>();
		_textPanelManager = GameObject.Find("Text Panel Manager").GetComponent<UIPanelManager>();
		
		SB_SwipeDrag swipeController = GetComponent<SB_SwipeDrag>();
		swipeController.SwipeHandler += SwipeHandler;
		
		base.Start();
	}
	
	protected override void OnTrackingFound()
	{
		base.OnTrackingFound();
		
		panelManager.active = true;
		_panelManager.CurrentPanel.BringIn();
		_textPanelManager.CurrentPanel.BringIn();
		
		SB_Album currentAlbum = _panelManager.CurrentPanel.gameObject.GetComponent<SB_Album>();
		
		if(currentAlbum)
			currentAlbum.startStream();
	}
	
	protected override void OnTrackingLost()
	{				
		try
		{
			SB_Album currentAlbum = _panelManager.CurrentPanel.gameObject.GetComponent<SB_Album>();
	
			if(currentAlbum)
				currentAlbum.stopStream();
		
			base.OnTrackingLost();
		}
		catch(Exception)
		{
			
		}
	}	
	
	protected virtual void SwipeHandler(SWIPETYPE ST)
	{	
		if(GetComponent<ImageTargetBehaviour>().CurrentStatus != TrackableBehaviour.Status.TRACKED)
			return;
		
		SB_Album currentAlbum = _panelManager.CurrentPanel.gameObject.GetComponent<SB_Album>();
		
		if(currentAlbum != null)	
			currentAlbum.stopStream();
		
		if(ST == SWIPETYPE.LEFT)
			_panelManager.MoveBack();
		else if(ST == SWIPETYPE.RIGHT)
			_panelManager.MoveForward();
		
		currentAlbum = _panelManager.CurrentPanel.gameObject.GetComponent<SB_Album>();
				
		if(currentAlbum != null)
			currentAlbum.startStream();		
	}
	
	protected virtual void OnGUI()
	{
		if(Application.platform == RuntimePlatform.OSXEditor)
		{
			if(GUI.Button(new Rect(0,0,Screen.width * 0.1f, Screen.height*0.1f),"L"))
				SwipeHandler(SWIPETYPE.LEFT);
			else if(GUI.Button(new Rect(Screen.width * 0.2f,0,Screen.width * 0.1f, Screen.height*0.1f),"R"))
				SwipeHandler(SWIPETYPE.RIGHT);
		}
	}
}
