
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MegaWarp))]
public class MegaWarpEditor : Editor
{
	public Texture			image;
	public bool				showhelp = false;
	public bool				showmodparams = true;
	public virtual Texture	LoadImage() { return null; }
	public virtual string	GetHelpString() { return "Warp Modifer by Chris West"; }
	public virtual bool		Inspector() { return true; }
	public virtual bool		DisplayCommon() { return true; }
	private MegaWarp		src;
	private MegaUndo		undoManager;

	[DrawGizmo(GizmoType.NotSelected | GizmoType.Pickable)]
	static void RenderGizmo(MegaWarp warp, GizmoType gizmoType)
	{
		if ( MegaModifiers.GlobalDisplay && warp.DisplayGizmo )
		{
			if ( (gizmoType & GizmoType.NotSelected) != 0 )
			{
				if ( (gizmoType & GizmoType.Active) != 0 )
				{
					if ( warp.Enabled )
						warp.DrawGizmo(Color.white);
					else
						warp.DrawGizmo(new Color(1.0f, 1.0f, 0.0f, 0.75f));
				}
				else
				{
					if ( warp.Enabled )
						warp.DrawGizmo(new Color(0.0f, 1.0f, 0.0f, 0.5f));
					else
						warp.DrawGizmo(new Color(1.0f, 0.0f, 0.0f, 0.25f));
				}
			}
			Gizmos.DrawIcon(warp.transform.position, warp.GetIcon(), false);
		}
	}

	//[DrawGizmo(GizmoType.SelectedOrChild)]
	//static void RenderGizmoSelected(Warp warp, GizmoType gizmoType)
	//{
		//if ( Modifiers.GlobalDisplay && warp.DisplayGizmo )
		//{
			//if ( warp.Enabled )
				//warp.DrawGizmo(Color.white);
			//else
				//warp.DrawGizmo(new Color(1.0f, 1.0f, 0.0f, 0.75f));

			//Gizmos.DrawIcon(warp.transform.position, warp.GetIcon());
		//}
	//}

	private void OnEnable()
	{
		src = target as MegaWarp;
		undoManager = new MegaUndo(src, src.WarpName() + " change");
	}

	public void CommonModParamsBasic(MegaWarp mod)
	{
		mod.Enabled = EditorGUILayout.Toggle("Enabled", mod.Enabled);
		mod.DisplayGizmo = EditorGUILayout.Toggle("Display Gizmo", mod.DisplayGizmo);

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Gizmo Col");
		mod.GizCol1 = EditorGUILayout.ColorField(mod.GizCol1);
		mod.GizCol2 = EditorGUILayout.ColorField(mod.GizCol2);
		EditorGUILayout.EndHorizontal();

		//mod.steps = EditorGUILayout.IntField("Gizmo Detail", mod.steps);
		//if ( mod.steps < 1 )
		//	mod.steps = 1;
	}

	public void CommonModParams(MegaWarp mod)
	{
		showmodparams = EditorGUILayout.Foldout(showmodparams, "Warp Common Params");

		if ( showmodparams )
		{
			EditorGUILayout.BeginVertical("Box");
			CommonModParamsBasic(mod);
			mod.Width = EditorGUILayout.FloatField("Width", mod.Width);
			mod.Height = EditorGUILayout.FloatField("Height", mod.Height);
			mod.Length = EditorGUILayout.FloatField("Length", mod.Length);
			mod.Decay = EditorGUILayout.FloatField("Decay", mod.Decay);
			EditorGUILayout.EndVertical();
		}
	}

	public virtual void DrawGUI()
	{
		MegaWarp mod = (MegaWarp)target;

		if ( DisplayCommon() )
			CommonModParams(mod);

		if ( GUI.changed )
			EditorUtility.SetDirty(target);

		if ( Inspector() )
			DrawDefaultInspector();
	}

	public virtual void DrawSceneGUI()
	{
		MegaWarp mod = (MegaWarp)target;

		if ( mod.Enabled && mod.DisplayGizmo && showmodparams )
		{
		}
	}

	public override void OnInspectorGUI()
	{
		undoManager.CheckUndo();
		DrawGUI();

		if ( GUI.changed )
			EditorUtility.SetDirty(target);
		undoManager.CheckDirty();
	}

	public void OnSceneGUI()
	{
		DrawSceneGUI();
	}
}
