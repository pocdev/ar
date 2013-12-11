using UnityEngine;
using System.Collections;

public class Crosshairs : MonoBehaviour {

	public Texture2D crosshairTexture;
	public bool crosshairEnabled = true;
	public Animation gunAnimation;
	
	private Rect _position;
	private GameController _gameController;
	
	void Start () 
	{
		 _position = new Rect((Screen.width - crosshairTexture.width) / 2, (Screen.height -crosshairTexture.height) /2, crosshairTexture.width, crosshairTexture.height);
		_gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	void OnGUI()
	{
		if(crosshairEnabled)
		{
			GUI.DrawTexture(_position, crosshairTexture);
		
			if(GUI.Button(new Rect(0f,0f,200f,200f),"FIRE!") && !gunAnimation.isPlaying)
				fireTest();
		}
	}
	
	void fireTest()
	{
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Debug.DrawRay(transform.position,fwd);
		
		RaycastHit hit;
		if(Physics.Raycast(transform.position,fwd,out hit))
		{
			GameObject GO = hit.transform.gameObject;
			if(GO.GetComponent<Huntable>())
			{
				Huntable hitHuntable = GO.GetComponent<Huntable>();
				hitHuntable.OnHit();
				_gameController.Score += hitHuntable.PointValue;
			}
		}
		
		gunAnimation.Play();
	}
	
}
