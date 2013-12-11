    ///////////////////////////////////////////////////////////
    //
    //   Author  : Alexander Orozco
    //   Email   : [email]alex@rozgo.com[/email]
    //   License : Keep this notice around. Otherwise, enjoy!
    //
    ///////////////////////////////////////////////////////////
     
    using UnityEngine;
    using System.Collections;
     
    public class Gestures : MonoBehaviour {
    
		public bool isLandscape = false;
	
        // adjust accordingly in the inspector
        public float orbitScreenToWorldRatio = 1.0f;
		public float dampening = 8.0f;
       
        // don't change these
        Vector3 orbitSpeed = Vector3.zero;
        float distWeight;
        float zoomDistance;
	
		private bool _isRotating = false;
		private GameObject _sail;
	
		void Start()
		{
			_sail = GameObject.FindGameObjectWithTag("Sail");
		
			if(SystemInfo.deviceModel.Contains("iPad"))
				dampening *= 1.5f;
		}
	
		public void onBeginTouch()
		{
			Debug.Log("GESTURES ONBEGINTOUCH");
			_isRotating = true;
		}
       
        void Update () 
		{
     
		// one finger gestures
            if (Input.touchCount == 1 && _isRotating) 
			{
               
                // finger data
                Touch f0 = Input.GetTouch(0);
               
                // finger delta
                Vector3 f0Delta = new Vector3(f0.deltaPosition.x, -f0.deltaPosition.y, 0);
               
                // if finger moving
                if (f0.phase == TouchPhase.Moved) {
                   
                    // compute orbit speed
                    orbitSpeed += (f0Delta + f0Delta * distWeight) * orbitScreenToWorldRatio * Time.deltaTime;
                }
            }
			else 
			{
				_isRotating = false;
			}
                      
            // decelerate orbit speed
            orbitSpeed = orbitSpeed * (1 - Time.deltaTime * 5);
		
			if(orbitSpeed == Vector3.zero)
				return;	
		
			
		
			bool flipRot = false;
			if(_sail.transform.rotation.eulerAngles.y <= 180 && (_sail.transform.eulerAngles.x >= 90 && _sail.transform.eulerAngles.x <= 270))
				flipRot = true;
		
			if(isLandscape)
			{
				_sail.transform.RotateAround(flipRot ? Vector3.right : Vector3.left, orbitSpeed.y/dampening);
				_sail.transform.RotateAround(Vector3.down, orbitSpeed.x/dampening);
			}
			else
			{
				_sail.transform.RotateAround(Vector3.left, orbitSpeed.y/dampening);
				_sail.transform.RotateAround(flipRot ? Vector3.up : Vector3.down, orbitSpeed.x/dampening);
			}
        }
    }