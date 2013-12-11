using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Keds_SoleState
{
	RUBBER = 0,
	TWILL,
};

public class KedsCustomizer : MonoBehaviour {
	
	private Keds_SoleState _solestate = Keds_SoleState.RUBBER;
	private Dictionary<Keds_SoleState,MeshRenderer> _soleStyles;
	
	void Start()
	{
		_soleStyles = new Dictionary<Keds_SoleState, MeshRenderer>();
		_soleStyles[Keds_SoleState.RUBBER] = transform.FindChild("sneaker_sole").GetComponent<MeshRenderer>();
		_soleStyles[Keds_SoleState.TWILL] = transform.FindChild("sneaker_sole_twill").GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			RaycastHit RayHit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch(0).position);

			if(Physics.Raycast(ray, out RayHit))
			{
				if(RayHit.transform.parent.name.Contains("keds"))
				{
					_soleStyles[_solestate].enabled = false;

					switch(_solestate)
					{						
						case Keds_SoleState.RUBBER:
							_solestate = Keds_SoleState.TWILL;	
						break;
						case Keds_SoleState.TWILL:
							_solestate = Keds_SoleState.RUBBER;
						break;
					}
					
					_soleStyles[_solestate].enabled = true;
				}
			}
		}
	}
}