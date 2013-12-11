using UnityEngine;
using System.Collections;

public class RepublicTrackableEventHandler : StarbucksTrackableEventHandler {

	// Use this for initialization
	protected override void Start () {
		base.Start();
		
		ForwardiOSMessages.onMovieFinished += delegate() { SwipeHandler(SWIPETYPE.RIGHT); };
	}
	
	protected override void OnTrackingFound()
	{
		base.OnTrackingFound();
		
		GetComponent<SceneController>().playMovie(_panelManager.CurrentPanel.index);
	}
	
	protected override void OnTrackingLost()
	{		
		_panelManager.CurrentPanel.transform.Rotate(Vector3.up);
		_panelManager.CurrentPanel.transform.localScale = Vector3.zero;
		base.OnTrackingLost();
		
		GetComponent<SceneController>().stopMovie();
	}	
	
	protected override void OnGUI()
	{
		base.OnGUI();
	}
	
	protected override void SwipeHandler(SWIPETYPE ST)
	{		
		if(ST == SWIPETYPE.LEFT)
			_panelManager.MoveBack();
		else if(ST == SWIPETYPE.RIGHT)
			_panelManager.MoveForward();
		
		StartCoroutine(delayMovie(.8f));
	}
	
	IEnumerator delayMovie(float time)
	{		
		yield return new WaitForSeconds(time);
		_panelManager.CurrentPanel.transform.Find("CD").transform.localRotation = Quaternion.Euler(310,0,330);
		GetComponent<SceneController>().playMovie(_panelManager.CurrentPanel.index);		
	}
	
	/*IEnumerator test()
	{
		Color temp = _panelManager.CurrentPanel.transform.Find("CD/CD_center").renderer.material.color;
		
		yield return new WaitForSeconds(.1f);
		
		temp.a = .5f;
		_panelManager.CurrentPanel.transform.Find("CD/CD_center").renderer.material.color = temp;
	}*/
}
