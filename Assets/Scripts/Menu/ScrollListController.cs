using UnityEngine;
using System.Collections;

public class ScrollListController : MonoBehaviour {
	
	private UIScrollList list;
	
	private Transform previouslyClickedTransform = null;
	
	// Use this for initialization
	void Start () {
	
		list = GetComponent<UIScrollList>();
		list.SetValueChangedDelegate(scrollList_OnClick);
		
		StartCoroutine(waitForList());
	}
	
	IEnumerator waitForList()
	{
		while(list.Count == 0)
			yield return new WaitForEndOfFrame();
		
		Debug.Log("list has count = " + list.Count);
		
		for(int i = 0; i < list.Count; i++)
		{
			list.GetItem(i).Data = i;
			Debug.Log("adding data: " + i);
		}
	}	
	
	public void scrollList_OnClick(IUIObject obj)
	{
		Transform parentTransform = list.LastClickedControl.transform.parent;
		parentTransform.FindChild("RadioButton").GetComponent<UIRadioBtn>().Value = true;
		
		if(parentTransform == previouslyClickedTransform)
		{
			LaunchButtonController.Instance.onClick(null);
		}
		else
		{
			previouslyClickedTransform = parentTransform;
		}
	}
}
