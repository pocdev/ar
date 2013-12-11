using UnityEngine;
using System.Collections;

[System.Serializable]
public class AnimationDetails
{
	public AnimationClip animClip;
	public int weight;
	public AudioSource audioSource;
}

public class MinionAnimationController : MonoBehaviour {
	
	public AnimationDetails[] animationDetails;
	public AnimationDetails _onClickAnim;
	
	void Start () 
	{
		transform.parent.GetComponent<UIButton3D>().SetValueChangedDelegate(onClick);
		animation.CrossFadeQueued(animationDetails[0].animClip.name,0f,QueueMode.PlayNow);	
	}
	
	void onClick(IUIObject obj)
	{
		if(!animation.IsPlaying(_onClickAnim.animClip.name))
		{
		 	animation.CrossFadeQueued(_onClickAnim.animClip.name,0.3f,QueueMode.PlayNow);
			if(_onClickAnim.audioSource != null)
				_onClickAnim.audioSource.Play();
		}
	}
	
	void onAnimEnd()
	{
		animation.CrossFadeQueued(animationDetails[0].animClip.name,3.0f,QueueMode.PlayNow);
	}
}
