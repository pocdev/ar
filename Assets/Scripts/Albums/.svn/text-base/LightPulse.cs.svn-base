using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PulsingLight
{
	public Light light;
	public bool isPulsing;
};

public class LightPulse : MonoBehaviour {

	private static int LOW_INTENSITY = 2;
    private static int HIGH_INTENSITY = 8;
	
	List<PulsingLight> lights;
	
	// Use this for initialization
	void Start () {
		
		lights = new List<PulsingLight>();
		foreach(Light L in GetComponentsInChildren<Light>())
		{
			PulsingLight temp = new PulsingLight();
			temp.light = L;
			temp.isPulsing = false;
			
			lights.Add(temp);
		}
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < lights.Count; i++)
		{
			if(!lights[i].isPulsing && Random.Range(0.0f,1.0f) > 0.33f)
			{
				lights[i].isPulsing = true;
				StartCoroutine(Pulse(lights[i]));
			}
		}
	}
	
	IEnumerator Pulse(PulsingLight PL)
	{
		float waitBeforePulse = Random.Range(0,3);
		yield return new WaitForSeconds(waitBeforePulse);
		
		int step = Random.Range(1,3);
		for(int i = 0; i <= 90; i+= step)
		{
			PL.light.intensity = LOW_INTENSITY + (Mathf.Cos(Mathf.Deg2Rad*i) * (HIGH_INTENSITY - LOW_INTENSITY));
			yield return new WaitForEndOfFrame();
		}
		
		waitBeforePulse = Random.Range(0,2);
		yield return new WaitForSeconds(waitBeforePulse);
		
		step = Random.Range(1,5);
		for(int i = 90; i >= 0; i-= step)
		{
			PL.light.intensity = LOW_INTENSITY + (Mathf.Cos(Mathf.Deg2Rad*i) * (HIGH_INTENSITY - LOW_INTENSITY));
			yield return new WaitForEndOfFrame();
		}
	
		PL.isPulsing = false;
	}
}
