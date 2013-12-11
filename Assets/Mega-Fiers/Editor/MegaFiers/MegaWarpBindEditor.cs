
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MegaWarpBind))]
public class MegaWarpBindEditor : MegaModifierEditor
{
	public override string GetHelpString() { return "Warp Bind Modifier by Chris West"; }

	public GameObject	SourceWarpObj;	// TODO: or point at mod on the warp
	GameObject			current;
	public float		decay = 0.0f;

	public override bool Inspector()
	{
		MegaWarpBind mod = (MegaWarpBind)target;

		EditorGUIUtility.LookLikeControls();
		mod.SourceWarpObj = (GameObject)EditorGUILayout.ObjectField("Warp Object", mod.SourceWarpObj, typeof(GameObject), true);
		mod.decay = EditorGUILayout.FloatField("Decay", mod.decay);
		return false;
	}
}
