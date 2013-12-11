
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

// TODO: Button to recalc lengths

[CustomEditor(typeof(MegaShape))]
public class MegaShapeEditor : Editor
{
	int		selected = -1;
	Vector3 pm = new Vector3();
	Vector3 delta = new Vector3();

	bool showsplines = false;
	bool showknots = false;
	bool showlabels = true;

	float ImportScale = 1.0f;
	static public Vector3 CursorPos = Vector3.zero;
	static public Vector3 CursorSpline = Vector3.zero;
	static public Vector3 CursorTangent = Vector3.zero;
	static public int		CursorKnot = 0;
	//static public float		CursorPercent = 0.0f;

	public virtual bool Params()	{ return false; }

	public bool showcommon = true;

	public float outline = 0.0f;
	bool hidewire = false;

	public override void OnInspectorGUI()
	{
		//undoManager.CheckUndo();
		bool buildmesh = false;
		MegaShape shape = (MegaShape)target;

		EditorGUILayout.BeginHorizontal();

		int curve = shape.selcurve;

		if ( GUILayout.Button("Add Knot") )
		{
			if ( shape.splines == null || shape.splines.Count == 0 )
			{
				MegaSpline spline = new MegaSpline();	// Have methods for these
				shape.splines.Add(spline);
			}

			//Undo.RegisterUndo(target, "Add Knot");

			MegaKnot knot = new MegaKnot();
			// Add a point at CursorPos

			//sp = selected + 1;
			//Debug.Log("CursorPos " + CursorPos + " CursorKnot " + CursorKnot);
			float per = shape.CursorPercent * 0.01f;

			CursorTangent = shape.splines[curve].Interpolate(per + 0.01f, true, ref CursorKnot);	//this.GetPositionOnSpline(i) - p;
			CursorPos = shape.splines[curve].Interpolate(per, true, ref CursorKnot);	//this.GetPositionOnSpline(i) - p;

			knot.p = CursorPos;
			//CursorTangent = 
			//Vector3 t = shape.splines[0].knots[selected].Interpolate(0.51f, shape.splines[0].knots[0]);
			knot.outvec = (CursorTangent - knot.p);
			knot.outvec.Normalize();
			knot.outvec *= shape.splines[curve].knots[CursorKnot].seglength * 0.25f;
			knot.invec = -knot.outvec;
			knot.invec += knot.p;
			knot.outvec += knot.p;

			shape.splines[curve].knots.Insert(CursorKnot + 1, knot);
			shape.CalcLength();	//10);
			EditorUtility.SetDirty(target);
			buildmesh = true;
		}

		if ( GUILayout.Button("Delete Knot") )
		{
			if ( selected != -1 )
			{
				//Undo.RegisterUndo(target, "Delete Knot");
				shape.splines[curve].knots.RemoveAt(selected);
				selected--;
				shape.CalcLength();	//10);
			}
			EditorUtility.SetDirty(target);
			buildmesh = true;
		}
		
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();

		if ( GUILayout.Button("Match Handles") )
		{
			if ( selected != -1 )
			{
				//Undo.RegisterUndo(target, "Match Handles");

				Vector3 p = shape.splines[curve].knots[selected].p;
				Vector3 d = shape.splines[curve].knots[selected].outvec - p;
				shape.splines[curve].knots[selected].invec = p - d;
				shape.CalcLength();	//10);
			}
			EditorUtility.SetDirty(target);
			buildmesh = true;
		}

		if ( GUILayout.Button("Load") )
		{
			// Load a spl file from max, so delete everything and replace
			LoadShape(ImportScale);
			buildmesh = true;
		}

		if ( GUILayout.Button("Load SXL") )
		{
			// Load a spl file from max, so delete everything and replace
			LoadSXL(ImportScale);
			buildmesh = true;
		}

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		if ( GUILayout.Button("AutoCurve") )
		{
			shape.AutoCurve();
			EditorUtility.SetDirty(target);
			buildmesh = true;
		}

		if ( GUILayout.Button("Reverse") )
		{
			shape.Reverse(curve);
			EditorUtility.SetDirty(target);
			buildmesh = true;
		}

		EditorGUILayout.EndHorizontal();

		if ( GUILayout.Button("Apply Scaling") )
		{
			shape.Scale(shape.transform.localScale);
			EditorUtility.SetDirty(target);
			shape.transform.localScale = Vector3.one;
			buildmesh = true;
		}



		//outline = EditorGUILayout.FloatField("Outline", outline);

		//if ( GUILayout.Button("Outline") )
		//{
		//	shape.OutlineSpline(shape, shape.selcurve, outline, true);
		//	EditorUtility.SetDirty(target);
		//	buildmesh = true;
		//}

		if ( GUILayout.Button("Import SVG") )
		{
			LoadSVG(ImportScale);
			buildmesh = true;
		}

		showcommon = EditorGUILayout.Foldout(showcommon, "Common Params");

		bool rebuild = false;	//Params();

		if ( showcommon )
		{
			//CursorPos = EditorGUILayout.Vector3Field("Cursor", CursorPos);
			shape.CursorPercent = EditorGUILayout.FloatField("Cursor", shape.CursorPercent);
			shape.CursorPercent = Mathf.Repeat(shape.CursorPercent, 100.0f);

			ImportScale = EditorGUILayout.FloatField("Import Scale", ImportScale);

			MegaAxis av = (MegaAxis)EditorGUILayout.EnumPopup("Axis", shape.axis);
			if ( av != shape.axis )
			{
				shape.axis = av;
				rebuild = true;
			}

			if ( shape.splines.Count > 1 )
			{
				//shape.selcurve = EditorGUILayout.IntField("Curve", shape.selcurve);
				shape.selcurve = EditorGUILayout.IntSlider("Curve", shape.selcurve, 0, shape.splines.Count - 1);
			}

			if ( shape.selcurve < 0 )
				shape.selcurve = 0;

			if ( shape.selcurve > shape.splines.Count - 1 )
				shape.selcurve = shape.splines.Count - 1;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Colors");
			//shape.col1 = EditorGUILayout.ColorField("Col 1", shape.col1);
			//shape.col2 = EditorGUILayout.ColorField("Col 2", shape.col2);
			shape.col1 = EditorGUILayout.ColorField(shape.col1);
			shape.col2 = EditorGUILayout.ColorField(shape.col2);
			EditorGUILayout.EndHorizontal();


			//shape.KnotCol = EditorGUILayout.ColorField("Knot Col", shape.KnotCol);
			//shape.HandleCol = EditorGUILayout.ColorField("Handle Col", shape.HandleCol);
			shape.VecCol = EditorGUILayout.ColorField("Vec Col", shape.VecCol);

			shape.KnotSize = EditorGUILayout.FloatField("Knot Size", shape.KnotSize);
			shape.stepdist = EditorGUILayout.FloatField("Step Dist", shape.stepdist);

			MegaSpline spline = shape.splines[shape.selcurve];

			if ( shape.stepdist < 0.01f )
				shape.stepdist = 0.01f;

			shape.dolateupdate = EditorGUILayout.Toggle("Do Late Update", shape.dolateupdate);
			shape.normalizedInterp = EditorGUILayout.Toggle("Normalized Interp", shape.normalizedInterp);

			spline.constantSpeed = EditorGUILayout.Toggle("Constant Speed", spline.constantSpeed);
			int subdivs = EditorGUILayout.IntField("Calc Subdivs", spline.subdivs);

			if ( subdivs < 2 )
				subdivs = 2;
			if ( subdivs != spline.subdivs )
			{
				//spline.subdivs = subdivs;
				spline.CalcLength(subdivs);
			}

			shape.drawHandles = EditorGUILayout.Toggle("Draw Handles", shape.drawHandles);
			shape.drawKnots = EditorGUILayout.Toggle("Draw Knots", shape.drawKnots);
			shape.drawspline = EditorGUILayout.Toggle("Draw Spline", shape.drawspline);
			shape.lockhandles = EditorGUILayout.Toggle("Lock Handles", shape.lockhandles);
			shape.autosmooth = EditorGUILayout.Toggle("Auto Smooth", shape.autosmooth);
			shape.handleType = (MegaHandleType)EditorGUILayout.EnumPopup("Handle Type", shape.handleType);

			showlabels = EditorGUILayout.Toggle("Labels", showlabels);

			hidewire = EditorGUILayout.Toggle("Hide Wire", hidewire);

			EditorUtility.SetSelectedWireframeHidden(shape.renderer, hidewire);

			shape.animate = EditorGUILayout.Toggle("Animate", shape.animate);
			if ( shape.animate )
			{
				shape.time = EditorGUILayout.FloatField("Time", shape.time);
				shape.MaxTime = EditorGUILayout.FloatField("Loop Time", shape.MaxTime);
				shape.speed = EditorGUILayout.FloatField("Speed", shape.speed);
				shape.LoopMode = (MegaRepeatMode)EditorGUILayout.EnumPopup("Loop Mode", shape.LoopMode);
			}

			if ( shape.splines.Count > 0 )
			{
				//shape.selcurve = EditorGUILayout.IntField("Curve", shape.selcurve);
				if ( spline.outlineSpline != -1 )
				{
					int outlineSpline = EditorGUILayout.IntSlider("Outline Spl", spline.outlineSpline, 0, shape.splines.Count - 1);
					float outline = EditorGUILayout.FloatField("Outline", spline.outline);

					if ( outline != spline.outline || outlineSpline != spline.outlineSpline )
					{
						spline.outlineSpline = outlineSpline;
						spline.outline = outline;
						if ( outlineSpline != shape.selcurve )
						{
							shape.OutlineSpline(shape.splines[spline.outlineSpline], spline, spline.outline, true);
							spline.CalcLength();	//10);
							EditorUtility.SetDirty(target);
							buildmesh = true;
						}
					}
				}
				else
				{
					outline = EditorGUILayout.FloatField("Outline", outline);

					if ( GUILayout.Button("Outline") )
					{
						shape.OutlineSpline(shape, shape.selcurve, outline, true);
						shape.splines[shape.splines.Count - 1].outline = outline;
						shape.splines[shape.splines.Count - 1].outlineSpline = shape.selcurve;
						shape.selcurve = shape.splines.Count - 1;
						EditorUtility.SetDirty(target);
						buildmesh = true;
					}
				}
			}

			// Mesher
			shape.makeMesh = EditorGUILayout.Toggle("Make Mesh", shape.makeMesh);

			if ( shape.makeMesh )
			{
				shape.meshType = (MeshShapeType)EditorGUILayout.EnumPopup("Mesh Type", shape.meshType);
				shape.Pivot = EditorGUILayout.Vector3Field("Pivot", shape.Pivot);

				shape.CalcTangents = EditorGUILayout.Toggle("Calc Tangents", shape.CalcTangents);
				shape.GenUV = EditorGUILayout.Toggle("Gen UV", shape.GenUV);

				EditorGUILayout.BeginVertical("Box");
				switch ( shape.meshType )
				{
					case MeshShapeType.Fill:
						shape.DoubleSided = EditorGUILayout.Toggle("Double Sided", shape.DoubleSided);
						shape.Height = EditorGUILayout.FloatField("Height", shape.Height);
						//shape.HeightSegs = EditorGUILayout.IntField("HeightSegs", shape.HeightSegs);
						shape.UseHeightCurve = EditorGUILayout.Toggle("Use Height Crv", shape.UseHeightCurve);
						if ( shape.UseHeightCurve )
						{
							shape.heightCrv = EditorGUILayout.CurveField("Height Curve", shape.heightCrv);
							shape.heightOff = EditorGUILayout.Slider("Height Off", shape.heightOff, -1.0f, 1.0f);
						}
						shape.mat1 = (Material)EditorGUILayout.ObjectField("Top Mat", shape.mat1, typeof(Material), true);
						shape.mat2 = (Material)EditorGUILayout.ObjectField("Bot Mat", shape.mat2, typeof(Material), true);
						shape.mat3 = (Material)EditorGUILayout.ObjectField("Side Mat", shape.mat3, typeof(Material), true);

						shape.PhysUV = EditorGUILayout.Toggle("Physical UV", shape.PhysUV);
						shape.UVOffset = EditorGUILayout.Vector2Field("UV Offset", shape.UVOffset);
						shape.UVRotate = EditorGUILayout.Vector2Field("UV Rotate", shape.UVRotate);
						shape.UVScale = EditorGUILayout.Vector2Field("UV Scale", shape.UVScale);
						shape.UVOffset1 = EditorGUILayout.Vector2Field("UV Offset1", shape.UVOffset1);
						shape.UVRotate1 = EditorGUILayout.Vector2Field("UV Rotate1", shape.UVRotate1);
						shape.UVScale1 = EditorGUILayout.Vector2Field("UV Scale1", shape.UVScale1);

						break;

					//case MeshShapeType.Line:
						//shape.DoubleSided = EditorGUILayout.Toggle("Double Sided", shape.DoubleSided);
						//shape.Height = EditorGUILayout.FloatField("Height", shape.Height);
						//shape.HeightSegs = EditorGUILayout.IntField("HeightSegs", shape.HeightSegs);
						//shape.heightCrv = EditorGUILayout.CurveField("Height Curve", shape.heightCrv);
						//shape.Start = EditorGUILayout.FloatField("Start", shape.Start);
						//shape.End = EditorGUILayout.FloatField("End", shape.End);
						//shape.Rotate = EditorGUILayout.FloatField("Rotate", shape.Rotate);
						//break;

					case MeshShapeType.Tube:
						shape.TubeStart = EditorGUILayout.Slider("Start", shape.TubeStart, -1.0f, 2.0f);
						shape.TubeLength = EditorGUILayout.Slider("Length", shape.TubeLength, 0.0f, 1.0f);
						shape.rotate = EditorGUILayout.FloatField("Rotate", shape.rotate);
						shape.tsides = EditorGUILayout.IntField("Sides", shape.tsides);
						shape.tradius = EditorGUILayout.FloatField("Radius", shape.tradius);
						shape.offset = EditorGUILayout.FloatField("Offset", shape.offset);
						//shape.SegsPerUnit = EditorGUILayout.FloatField("Segs", shape.SegsPerUnit);

						shape.scaleX = EditorGUILayout.CurveField("Scale X", shape.scaleX);
						shape.unlinkScale = EditorGUILayout.BeginToggleGroup("unlink Scale", shape.unlinkScale);
						shape.scaleY = EditorGUILayout.CurveField("Scale Y", shape.scaleY);
						EditorGUILayout.EndToggleGroup();

						shape.strands = EditorGUILayout.IntField("Strands", shape.strands);
						if ( shape.strands > 1 )
						{
							shape.strandRadius = EditorGUILayout.FloatField("Strand Radius", shape.strandRadius);
							shape.TwistPerUnit = EditorGUILayout.FloatField("Twist", shape.TwistPerUnit);
							shape.startAng = EditorGUILayout.FloatField("Start Twist", shape.startAng);
						}
						shape.UVOffset = EditorGUILayout.Vector2Field("UV Offset", shape.UVOffset);
						shape.uvtilex = EditorGUILayout.FloatField("UV Tile X", shape.uvtilex);
						shape.uvtiley = EditorGUILayout.FloatField("UV Tile Y", shape.uvtiley);

						shape.cap = EditorGUILayout.Toggle("Cap", shape.cap);
						shape.RopeUp = (MegaAxis)EditorGUILayout.EnumPopup("Up", shape.RopeUp);
						shape.mat1 = (Material)EditorGUILayout.ObjectField("Mat", shape.mat1, typeof(Material), true);
						//shape.Sides = EditorGUILayout.IntField("Sides", shape.Sides);
						//shape.TubeStep = EditorGUILayout.FloatField("TubeStep", shape.TubeStep);
						//shape.Start = EditorGUILayout.FloatField("Start", shape.Start);
						//shape.End = EditorGUILayout.FloatField("End", shape.End);
						break;

					case MeshShapeType.Ribbon:
						shape.TubeStart = EditorGUILayout.Slider("Start", shape.TubeStart, -1.0f, 2.0f);
						shape.TubeLength = EditorGUILayout.Slider("Length", shape.TubeLength, 0.0f, 1.0f);
						shape.boxwidth = EditorGUILayout.FloatField("Width", shape.boxwidth);
						shape.raxis = (MegaAxis)EditorGUILayout.EnumPopup("Axis", shape.raxis);
						shape.rotate = EditorGUILayout.FloatField("Rotate", shape.rotate);
						shape.ribsegs = EditorGUILayout.IntField("Segs", shape.ribsegs);
						if ( shape.ribsegs < 1 )
							shape.ribsegs = 1;
						shape.offset = EditorGUILayout.FloatField("Offset", shape.offset);

						shape.scaleX = EditorGUILayout.CurveField("Scale X", shape.scaleX);

						shape.strands = EditorGUILayout.IntField("Strands", shape.strands);
						if ( shape.strands > 1 )
						{
							shape.strandRadius = EditorGUILayout.FloatField("Strand Radius", shape.strandRadius);
							shape.TwistPerUnit = EditorGUILayout.FloatField("Twist", shape.TwistPerUnit);
							shape.startAng = EditorGUILayout.FloatField("Start Twist", shape.startAng);
							//shape.SegsPerUnit = EditorGUILayout.FloatField("Segs", shape.SegsPerUnit);
						}

						shape.UVOffset = EditorGUILayout.Vector2Field("UV Offset", shape.UVOffset);
						shape.uvtilex = EditorGUILayout.FloatField("UV Tile X", shape.uvtilex);
						shape.uvtiley = EditorGUILayout.FloatField("UV Tile Y", shape.uvtiley);

						shape.RopeUp = (MegaAxis)EditorGUILayout.EnumPopup("Up", shape.RopeUp);
						shape.mat1 = (Material)EditorGUILayout.ObjectField("Mat", shape.mat1, typeof(Material), true);
						break;

					case MeshShapeType.Box:
						shape.TubeStart = EditorGUILayout.Slider("Start", shape.TubeStart, -1.0f, 2.0f);
						shape.TubeLength = EditorGUILayout.Slider("Length", shape.TubeLength, 0.0f, 1.0f);
						shape.rotate = EditorGUILayout.FloatField("Rotate", shape.rotate);
						shape.boxwidth = EditorGUILayout.FloatField("Box Width", shape.boxwidth);
						shape.boxheight = EditorGUILayout.FloatField("Box Height", shape.boxheight);
						shape.offset = EditorGUILayout.FloatField("Offset", shape.offset);
						//shape.strandRadius = EditorGUILayout.FloatField("Strand Radius", shape.strandRadius);
						//shape.strandRadius = EditorGUILayout.FloatField("Strand Radius", shape.strandRadius);
						//shape.SegsPerUnit = EditorGUILayout.FloatField("Segs", shape.SegsPerUnit);

						shape.scaleX = EditorGUILayout.CurveField("Scale X", shape.scaleX);
						shape.unlinkScale = EditorGUILayout.BeginToggleGroup("unlink Scale", shape.unlinkScale);
						shape.scaleY = EditorGUILayout.CurveField("Scale Y", shape.scaleY);
						EditorGUILayout.EndToggleGroup();

						shape.strands = EditorGUILayout.IntField("Strands", shape.strands);
						if ( shape.strands > 1 )
						{
							shape.tradius = EditorGUILayout.FloatField("Radius", shape.tradius);
							shape.TwistPerUnit = EditorGUILayout.FloatField("Twist", shape.TwistPerUnit);
							shape.startAng = EditorGUILayout.FloatField("Start Twist", shape.startAng);
						}

						shape.UVOffset = EditorGUILayout.Vector2Field("UV Offset", shape.UVOffset);
						shape.uvtilex = EditorGUILayout.FloatField("UV Tile X", shape.uvtilex);
						shape.uvtiley = EditorGUILayout.FloatField("UV Tile Y", shape.uvtiley);

						shape.cap = EditorGUILayout.Toggle("Cap", shape.cap);
						shape.RopeUp = (MegaAxis)EditorGUILayout.EnumPopup("Up", shape.RopeUp);
						shape.mat1 = (Material)EditorGUILayout.ObjectField("Mat", shape.mat1, typeof(Material), true);
						//shape.Sides = EditorGUILayout.IntField("Sides", shape.Sides);
						//shape.TubeStep = EditorGUILayout.FloatField("TubeStep", shape.TubeStep);
						//shape.Start = EditorGUILayout.FloatField("Start", shape.Start);
						//shape.End = EditorGUILayout.FloatField("End", shape.End);
						break;
				}

				if ( shape.strands < 1 )
					shape.strands = 1;

				EditorGUILayout.EndVertical();
			}
			else
			{
				//shape.shapemesh = null;
				shape.ClearMesh();
			}

			showsplines = EditorGUILayout.Foldout(showsplines, "Spline Data");

			if ( showsplines )
			{
				EditorGUILayout.BeginVertical("Box");
				if ( shape.splines != null && shape.splines.Count > 0 )
					DisplaySpline(shape, shape.splines[shape.selcurve]);
				EditorGUILayout.EndVertical();
#if false
				for ( int i = 0; i < shape.splines.Count; i++ )
				{
					// We should only show the selected curve
					EditorGUILayout.BeginVertical("Box");
					DisplaySpline(shape, shape.splines[i]);
					EditorGUILayout.EndVertical();
				}
#endif
			}

			EditorGUILayout.BeginHorizontal();

			Color col = GUI.backgroundColor;
			GUI.backgroundColor = Color.green;
			if ( GUILayout.Button("Add") )
			{
				// Create a new spline in the shape
				MegaSpline spl = MegaSpline.Copy(shape.splines[shape.selcurve]);

				shape.splines.Add(spl);
				shape.selcurve = shape.splines.Count - 1;
				EditorUtility.SetDirty(shape);
				buildmesh = true;
			}

			if ( shape.splines.Count > 1 )
			{
				GUI.backgroundColor = Color.red;
				if ( GUILayout.Button("Delete") )
				{
					// Delete current spline
					shape.splines.RemoveAt(shape.selcurve);


					for ( int i = 0; i < shape.splines.Count; i++ )
					{
						if ( shape.splines[i].outlineSpline == shape.selcurve )
							shape.splines[i].outlineSpline = -1;

						if ( shape.splines[i].outlineSpline > shape.selcurve )
							shape.splines[i].outlineSpline--;
					}

					shape.selcurve--;
					if ( shape.selcurve < 0 )
						shape.selcurve = 0;

					EditorUtility.SetDirty(shape);
					buildmesh = true;
				}
			}
			GUI.backgroundColor = col;
			EditorGUILayout.EndHorizontal();
		}

		if ( !shape.imported )
		{
			if ( Params() )
			{
				rebuild = true;
			}
		}

		if ( GUI.changed )
		{
			EditorUtility.SetDirty(target);
			//shape.CalcLength(10);
			buildmesh = true;
		}

		if ( rebuild )
		{
			shape.MakeShape();
			EditorUtility.SetDirty(target);
			buildmesh = true;
		}

		if ( buildmesh )
		{
			if ( shape.makeMesh )
			{
				shape.SetMats();
				shape.BuildMesh();
			}
		}

		//undoManager.CheckDirty();
	}

	void DisplayKnot(MegaShape shape, MegaSpline spline, MegaKnot knot)
	{
		bool recalc = false;

		Vector3 p = EditorGUILayout.Vector3Field("Pos", knot.p);
		delta = p - knot.p;

		knot.invec += delta;
		knot.outvec += delta;

		if ( knot.p != p )
		{
			recalc = true;
			knot.p = p;
		}

		if ( recalc )
		{
			shape.CalcLength();	//10);
		}
	}

	void DisplaySpline(MegaShape shape, MegaSpline spline)
	{
		bool closed = EditorGUILayout.Toggle("Closed", spline.closed);

		if ( closed != spline.closed )
		{
			spline.closed = closed;
			shape.CalcLength();	//10);
			EditorUtility.SetDirty(target);
			//shape.BuildMesh();
		}

		spline.reverse = EditorGUILayout.Toggle("Reverse", spline.reverse);

		EditorGUILayout.LabelField("Length ", spline.length.ToString("0.000"));

		//if ( GUILayout.Button("Outline") )
		//{
		//	shape.OutlineSpline(shape, shape.selcurve, outline, true);
		//	EditorUtility.SetDirty(target);
		//	buildmesh = true;
		//}



		showknots = EditorGUILayout.Foldout(showknots, "Knots");

		if ( showknots )
		{
			for ( int i = 0; i < spline.knots.Count; i++ )
			{
				DisplayKnot(shape, spline, spline.knots[i]);
				//EditorGUILayout.Separator();
			}
		}
	}

#if false
	public void OnSceneGUI()
	{
		//Undo.RegisterUndo(target, "Move Shape Points");
		//undoManager.CheckUndo(target);
		//Undo.CreateSnapshot();

		MegaShape shape = (MegaShape)target;

		Handles.matrix = shape.transform.localToWorldMatrix;

		Quaternion rot = shape.transform.rotation;
		Vector3 trans = shape.transform.position;
		Handles.matrix = Matrix4x4.TRS(trans, rot, Vector3.one);

		if ( shape.selcurve > shape.splines.Count - 1 )
			shape.selcurve = 0;

		bool recalc = false;

		Vector3 dragplane = Vector3.one;

		Color nocol = new Color(0, 0, 0, 0);

		for ( int s = 0; s < shape.splines.Count; s++ )
		{
			for ( int p = 0; p < shape.splines[s].knots.Count; p++ )
			{
				if ( shape.drawKnots && s == shape.selcurve )
				{
					pm = shape.splines[s].knots[p].p;

					if ( showlabels )
					{
						if ( p == selected && s == shape.selcurve )
						{
							Handles.color = Color.white;
							Handles.Label(pm, " Selected\n" + pm.ToString("0.000"));
						}
						else
						{
							Handles.color = shape.KnotCol;
							Handles.Label(pm, " " + p);
						}
					}

					Handles.color = nocol;
					Vector3 newp = Handles.PositionHandle(pm, Quaternion.identity);
					if ( newp != pm )
					{
						Undo.SetSnapshotTarget(shape, "Knot Move");
					}
					shape.splines[s].knots[p].p += Vector3.Scale(newp - pm, dragplane);

					delta = shape.splines[s].knots[p].p - pm;

					shape.splines[s].knots[p].invec += delta;
					shape.splines[s].knots[p].outvec += delta;

					if ( shape.splines[s].knots[p].p != pm )
					{
						selected = p;
						recalc = true;
					}

					pm = shape.splines[s].knots[p].p;
				}

				if ( shape.drawHandles && s == shape.selcurve )
				{
					Handles.color = shape.VecCol;
					pm = shape.splines[s].knots[p].p;

					Vector3 ip = shape.splines[s].knots[p].invec;
					Vector3 op = shape.splines[s].knots[p].outvec;
					Handles.DrawLine(pm, ip);
					Handles.DrawLine(pm, op);

					Handles.color = shape.HandleCol;

					Vector3 invec = shape.splines[s].knots[p].invec;
					Handles.color = nocol;
					Vector3 newinvec = Handles.PositionHandle(shape.splines[s].knots[p].invec, Quaternion.identity);

					if ( newinvec != shape.splines[s].knots[p].invec )
					{
						Undo.SetSnapshotTarget(shape, "Handle Move");
					}
					invec += Vector3.Scale(newinvec - invec, dragplane);
					if ( invec != shape.splines[s].knots[p].invec )
					{
						if ( shape.lockhandles )
						{
							Vector3 d = invec - shape.splines[s].knots[p].invec;
							shape.splines[s].knots[p].outvec -= d;
						}

						shape.splines[s].knots[p].invec = invec;
						selected = p;
						recalc = true;
					}
					Vector3 outvec = shape.splines[s].knots[p].outvec;

					Vector3 newoutvec = Handles.PositionHandle(shape.splines[s].knots[p].outvec, Quaternion.identity);
					if ( newoutvec != shape.splines[s].knots[p].outvec )
					{
						Undo.SetSnapshotTarget(shape, "Handle Move");
					}
					outvec += Vector3.Scale(newoutvec - outvec, dragplane);

					if ( outvec != shape.splines[s].knots[p].outvec )
					{
						if ( shape.lockhandles )
						{
							Vector3 d = outvec - shape.splines[s].knots[p].outvec;
							shape.splines[s].knots[p].invec -= d;
						}

						shape.splines[s].knots[p].outvec = outvec;
						selected = p;
						recalc = true;
					}
					Vector3 hp = shape.splines[s].knots[p].invec;
					if ( selected == p )
						Handles.Label(hp, " " + p);

					hp = shape.splines[s].knots[p].outvec;
					
					if ( selected == p )
						Handles.Label(hp, " " + p);
				}
			}
		}

		// Draw nearest point (use for adding knot)
		CursorPos = Handles.PositionHandle(CursorPos, Quaternion.identity);
		float calpha = 0.0f;
		CursorPos = shape.FindNearestPoint(CursorPos, 5, ref CursorKnot, ref CursorTangent, ref calpha);
		CursorPercent = calpha * 100.0f;
		Handles.Label(CursorPos, "Cursor " + CursorPercent.ToString("0.00") + "% - " + CursorPos);

		if ( recalc )
		{
			shape.CalcLength(10);
			shape.BuildMesh();
		}

		Handles.matrix = Matrix4x4.identity;
		//undoManager.CheckDirty(target);

		if ( GUI.changed )
		{
			//Undo.RegisterCreatedObjectUndo(shape, "plop");
			Undo.CreateSnapshot();
			Undo.RegisterSnapshot(); 
		}

		Undo.ClearSnapshotTarget();
	}
#else

	public void OnSceneGUI()
	{
#if false
		if ( Event.current.type == EventType.mouseDown )
		{
			tl = Tools.current;
			Tools.current = Tool.None;

			Debug.Log("d");
			//if ( Event.current.modifiers == EventModifiers.Shift )
			{
				Debug.Log("Mod");
				// Start a selection box
				mdown = true;
				mstart = Event.current.mousePosition;
				Event.current.Use();
			}
		}

		if ( Event.current.type == EventType.mouseMove && mdown )
		{
			//Debug.Log("Do select");
			Event.current.Use();
		}

		if ( Event.current.type == EventType.mouseUp )
		{
			Tools.current = tl;
			Debug.Log("p");
			if ( mdown )
			{
				mdown = false;
				mend = Event.current.mousePosition;
				//Event.current.Use();
				Debug.Log("DragBox " + mstart + " to " + mend);
			}
		}
#endif
		//Undo.RegisterUndo(target, "Move Shape Points");
		//undoManager.CheckUndo(target);
		//Undo.CreateSnapshot();

		MegaShape shape = (MegaShape)target;

		//Handles.matrix = Matrix4x4.identity;	//shape.transform.localToWorldMatrix;
		Handles.matrix = shape.transform.localToWorldMatrix;

		//Quaternion rot = shape.transform.rotation;
		//Vector3 trans = shape.transform.position;
		//Matrix4x4 tm = shape.transform.localToWorldMatrix;	//Matrix4x4.TRS(trans, rot, Vector3.one);

		if ( shape.selcurve > shape.splines.Count - 1 )
			shape.selcurve = 0;

		bool recalc = false;

		Vector3 dragplane = Vector3.one;

		Color nocol = new Color(0, 0, 0, 0);

		for ( int s = 0; s < shape.splines.Count; s++ )
		{
			for ( int p = 0; p < shape.splines[s].knots.Count; p++ )
			{
				if ( shape.drawKnots && s == shape.selcurve )
				{
					//pm = tm.MultiplyPoint(shape.splines[s].knots[p].p);
					pm = shape.splines[s].knots[p].p;

					if ( showlabels )
					{
						if ( p == selected && s == shape.selcurve )
						{
							Handles.color = Color.white;
							Handles.Label(pm, " Selected\n" + pm.ToString("0.000"));
						}
						else
						{
							Handles.color = shape.KnotCol;
							Handles.Label(pm, " " + p);
						}
					}

					Handles.color = nocol;
					//Vector3 newp = Handles.PositionHandle(pm, Quaternion.identity);
					Vector3 newp = PosHandles(shape, pm, Quaternion.identity);
					if ( newp != pm )
					{
						Undo.SetSnapshotTarget(shape, "Knot Move");
					}

					Vector3 dl = Vector3.Scale(newp - pm, dragplane);

					//dl = shape.transform.worldToLocalMatrix.MultiplyPoint(dl);
					//Debug.Log("dl " + dl);

					shape.splines[s].knots[p].p += dl;	//Vector3.Scale(newp - pm, dragplane);

					//delta = shape.splines[s].knots[p].p - pm;

					shape.splines[s].knots[p].invec += dl;	//delta;
					shape.splines[s].knots[p].outvec += dl;	//delta;

					//if ( shape.splines[s].knots[p].p != pm )
					if ( newp != pm )
					{
						selected = p;
						recalc = true;
					}

					//pm = shape.splines[s].knots[p].p;
				}

#if true
				if ( shape.drawHandles && s == shape.selcurve )
				{
					Handles.color = shape.VecCol;
					//pm = tm.MultiplyPoint(shape.splines[s].knots[p].p);
					pm = shape.splines[s].knots[p].p;

					//Vector3 ip = tm.MultiplyPoint(shape.splines[s].knots[p].invec);
					//Vector3 op = tm.MultiplyPoint(shape.splines[s].knots[p].outvec);
					Vector3 ip = shape.splines[s].knots[p].invec;
					Vector3 op = shape.splines[s].knots[p].outvec;
					Handles.DrawLine(pm, ip);
					Handles.DrawLine(pm, op);

					Handles.color = shape.HandleCol;

					//Vector3 invec = tm.MultiplyPoint(shape.splines[s].knots[p].invec);
					Vector3 invec = shape.splines[s].knots[p].invec;
					Handles.color = nocol;
					//Vector3 newinvec = Handles.PositionHandle(invec, Quaternion.identity);
					Vector3 newinvec = PosHandles(shape, invec, Quaternion.identity);

					if ( newinvec != invec )	//shape.splines[s].knots[p].invec )
					{
						Undo.SetSnapshotTarget(shape, "Handle Move");
					}
					Vector3 dl = Vector3.Scale(newinvec - invec, dragplane);
					shape.splines[s].knots[p].invec += dl;	//Vector3.Scale(newinvec - invec, dragplane);
					if ( invec != newinvec )	//shape.splines[s].knots[p].invec )
					{
						if ( shape.lockhandles )
						{
							//Vector3 d = invec - shape.splines[s].knots[p].invec;
							shape.splines[s].knots[p].outvec -= dl;
						}

						//shape.splines[s].knots[p].invec = invec;
						selected = p;
						recalc = true;
					}
					//Vector3 outvec = tm.MultiplyPoint(shape.splines[s].knots[p].outvec);
					Vector3 outvec = shape.splines[s].knots[p].outvec;

					//Vector3 newoutvec = Handles.PositionHandle(outvec, Quaternion.identity);
					Vector3 newoutvec = PosHandles(shape, outvec, Quaternion.identity);
					if ( newoutvec != outvec )	//shape.splines[s].knots[p].outvec )
					{
						Undo.SetSnapshotTarget(shape, "Handle Move");
					}
					dl = Vector3.Scale(newoutvec - outvec, dragplane);
					//outvec += dl;	//Vector3.Scale(newoutvec - outvec, dragplane);
					shape.splines[s].knots[p].outvec += dl;
					if ( outvec != newoutvec )	//shape.splines[s].knots[p].outvec )
					{
						if ( shape.lockhandles )
						{
							//Vector3 d = outvec - shape.splines[s].knots[p].outvec;
							shape.splines[s].knots[p].invec -= dl;
						}

						//shape.splines[s].knots[p].outvec = outvec;
						selected = p;
						recalc = true;
					}
					//Vector3 hp = tm.MultiplyPoint(shape.splines[s].knots[p].invec);
					Vector3 hp = shape.splines[s].knots[p].invec;
					if ( selected == p )
						Handles.Label(hp, " " + p);

					//hp = tm.MultiplyPoint(shape.splines[s].knots[p].outvec);
					hp = shape.splines[s].knots[p].outvec;

					if ( selected == p )
						Handles.Label(hp, " " + p);
				}
#endif
			}
		}

		// Draw nearest point (use for adding knot)
		//Vector3 wcp = tm.MultiplyPoint(CursorPos);
		Vector3 wcp = CursorPos;
		//Vector3 newCursorPos = Handles.PositionHandle(wcp, Quaternion.identity);
		Vector3 newCursorPos = PosHandles(shape, wcp, Quaternion.identity);
		
		if ( newCursorPos != wcp )
		{
			Vector3 cd = newCursorPos - wcp;

			CursorPos += cd;

			float calpha = 0.0f;
			CursorPos = shape.FindNearestPoint(CursorPos, 5, ref CursorKnot, ref CursorTangent, ref calpha);
			shape.CursorPercent = calpha * 100.0f;
		}

		//Handles.Label(tm.MultiplyPoint(CursorPos), "Cursor " + CursorPercent.ToString("0.00") + "% - " + CursorPos);
		Handles.Label(CursorPos, "Cursor " + shape.CursorPercent.ToString("0.00") + "% - " + CursorPos);

		if ( recalc )
		{
			shape.CalcLength();	//10);
			shape.BuildMesh();
		}

		Handles.matrix = Matrix4x4.identity;
		//undoManager.CheckDirty(target);

		if ( GUI.changed )
		{
			//Undo.RegisterCreatedObjectUndo(shape, "plop");
			Undo.CreateSnapshot();
			Undo.RegisterSnapshot();
		}

		Undo.ClearSnapshotTarget();
	}
#endif

	Vector3 PosHandles(MegaShape shape, Vector3 pos, Quaternion q)
	{
		switch ( shape.handleType )
		{
			case MegaHandleType.Position:
				pos = Handles.PositionHandle(pos, q);
				break;

			case MegaHandleType.Free:
				//Handles.SphereCap(0, shape.transform.TransformPoint(pos), q, shape.KnotSize * 0.01f);
				pos = Handles.FreeMoveHandle(pos, q, shape.KnotSize * 0.01f, Vector3.zero, Handles.CircleCap);
				break;
		}
		return pos;
	}
#if false
	[DrawGizmo(GizmoType.NotSelected | GizmoType.Pickable)]
	static void RenderGizmo(MegaShape shape, GizmoType gizmoType)
	{
		if ( (gizmoType & GizmoType.NotSelected) != 0 )
		{
			if ( (gizmoType & GizmoType.Active) != 0 )
			{
				DrawGizmos(shape, new Color(1.0f, 1.0f, 1.0f, 1.0f));
				Color col = Color.yellow;
				col.a = 0.5f;
				Gizmos.color = col;	//Color.yellow;
				CursorPos = shape.InterpCurve3D(shape.selcurve, CursorPercent * 0.01f, true);
				Gizmos.DrawSphere(shape.transform.TransformPoint(CursorPos), shape.KnotSize * 0.01f);
				Handles.color = Color.white;
				//Handles.Label(shape.transform.TransformPoint(CursorPos), "Cursor " + CursorPercent.ToString("0.00") + "% - " + CursorPos);
			}
			else
				DrawGizmos(shape, new Color(1.0f, 1.0f, 1.0f, 0.25f));
		}
		Gizmos.DrawIcon(shape.transform.position, "MegaSpherify icon.png", false);
		Handles.Label(shape.transform.position, " " + shape.name);
	}
#endif
	[DrawGizmo(GizmoType.NotSelected | GizmoType.Pickable | GizmoType.Selected)]
	static void RenderGizmo(MegaShape shape, GizmoType gizmoType)
	{
		//if ( (gizmoType & GizmoType.NotSelected) != 0 )
		{
			if ( (gizmoType & GizmoType.Active) != 0 )
			{
				if ( shape.splines == null || shape.splines.Count == 0 )
					return;

				DrawGizmos(shape, new Color(1.0f, 1.0f, 1.0f, 1.0f));
				Color col = Color.yellow;
				col.a = 0.5f;
				Gizmos.color = col;	//Color.yellow;
				CursorPos = shape.InterpCurve3D(shape.selcurve, shape.CursorPercent * 0.01f, true);
				Gizmos.DrawSphere(shape.transform.TransformPoint(CursorPos), shape.KnotSize * 0.01f);
				Handles.color = Color.white;
				//Handles.Label(shape.transform.TransformPoint(CursorPos), "Cursor " + CursorPercent.ToString("0.00") + "% - " + CursorPos);

				if ( shape.handleType == MegaHandleType.Free )
				{
					//for ( int s = 0; s < shape.splines.Count; s++ )
					int s = shape.selcurve;
					{
						for ( int p = 0; p < shape.splines[s].knots.Count; p++ )
						{
							if ( shape.drawKnots )	//&& s == shape.selcurve )
							{
								Gizmos.color = Color.green;
								Gizmos.DrawSphere(shape.transform.TransformPoint(shape.splines[s].knots[p].p), shape.KnotSize * 0.01f);
							}

							if ( shape.drawHandles )
							{
								Gizmos.color = Color.red;
								Gizmos.DrawSphere(shape.transform.TransformPoint(shape.splines[s].knots[p].invec), shape.KnotSize * 0.01f);
								Gizmos.DrawSphere(shape.transform.TransformPoint(shape.splines[s].knots[p].outvec), shape.KnotSize * 0.01f);
							}
						}
					}
				}
			}
			else
				DrawGizmos(shape, new Color(1.0f, 1.0f, 1.0f, 0.25f));
		}
		Gizmos.DrawIcon(shape.transform.position, "MegaSpherify icon.png", false);
		Handles.Label(shape.transform.position, " " + shape.name);
	}

	// Dont want this in here, want in editor
	// If we go over a knot then should draw to the knot
#if false
	static void DrawGizmos(MegaShape shape, Color modcol)
	{
		if ( ((1 << shape.gameObject.layer) & Camera.current.cullingMask) == 0 )
			return;

		if ( !shape.drawspline )
			return;

		Matrix4x4 tm = shape.transform.localToWorldMatrix;

		for ( int s = 0; s < shape.splines.Count; s++ )
		{
			float ldist = shape.stepdist * 0.1f;
			if ( ldist < 0.01f )
				ldist = 0.01f;

			if ( shape.splines[s].length / ldist > 500.0f )
			{
				ldist = shape.splines[s].length / 500.0f;
			}

			float ds = shape.splines[s].length / (shape.splines[s].length / ldist);

			if ( ds > shape.splines[s].length )
			{
				ds = shape.splines[s].length;
			}

			int c	= 0;
			int k	= -1;
			int lk	= -1;

			Vector3 first = shape.splines[s].Interpolate(0.0f, shape.normalizedInterp, ref lk);

			for ( float dist = ds; dist < shape.splines[s].length; dist += ds )
			{
				float alpha = dist / shape.splines[s].length;
				Vector3 pos = shape.splines[s].Interpolate(alpha, shape.normalizedInterp, ref k);

				if ( (c & 1) == 1 )
					Gizmos.color = shape.col1 * modcol;
				else
					Gizmos.color = shape.col2 * modcol;

				if ( k != lk )
				{
					for ( lk = lk + 1; lk <= k; lk++ )
					{
						Gizmos.DrawLine(shape.transform.TransformPoint(first), shape.transform.TransformPoint(shape.splines[s].knots[lk].p));
						first = shape.splines[s].knots[lk].p;
					}
				}

				lk = k;

				Gizmos.DrawLine(shape.transform.TransformPoint(first), shape.transform.TransformPoint(pos));

				c++;

				first = pos;
			}

			if ( (c & 1) == 1 )
				Gizmos.color = shape.col1 * modcol;
			else
				Gizmos.color = shape.col2 * modcol;

			Vector3 lastpos;
			if ( shape.splines[s].closed )
				lastpos = shape.splines[s].Interpolate(0.0f, shape.normalizedInterp, ref k);
			else
				lastpos = shape.splines[s].Interpolate(1.0f, shape.normalizedInterp, ref k);

			Gizmos.DrawLine(shape.transform.TransformPoint(first), shape.transform.TransformPoint(lastpos));
		}
	}
#else
	static void DrawGizmos(MegaShape shape, Color modcol1)
	{
		if ( ((1 << shape.gameObject.layer) & Camera.current.cullingMask) == 0 )
			return;

		if ( !shape.drawspline )
			return;

		Matrix4x4 tm = shape.transform.localToWorldMatrix;

		for ( int s = 0; s < shape.splines.Count; s++ )
		{
			float ldist = shape.stepdist * 0.1f;
			if ( ldist < 0.01f )
				ldist = 0.01f;

			Color modcol = modcol1;

			if ( s != shape.selcurve && modcol1.a == 1.0f )
			{
				modcol.a *= 0.5f;
			}

			if ( shape.splines[s].length / ldist > 500.0f )
			{
				ldist = shape.splines[s].length / 500.0f;
			}

			float ds = shape.splines[s].length / (shape.splines[s].length / ldist);

			if ( ds > shape.splines[s].length )
			{
				ds = shape.splines[s].length;
			}

			int c	= 0;
			int k	= -1;
			int lk	= -1;

			Vector3 first = shape.splines[s].Interpolate(0.0f, shape.normalizedInterp, ref lk);

			for ( float dist = ds; dist < shape.splines[s].length; dist += ds )
			{
				float alpha = dist / shape.splines[s].length;
				Vector3 pos = shape.splines[s].Interpolate(alpha, shape.normalizedInterp, ref k);

				if ( (c & 1) == 1 )
					Gizmos.color = shape.col1 * modcol;
				else
					Gizmos.color = shape.col2 * modcol;

				if ( k != lk )
				{
					for ( lk = lk + 1; lk <= k; lk++ )
					{
						Gizmos.DrawLine(tm.MultiplyPoint(first), tm.MultiplyPoint(shape.splines[s].knots[lk].p));
						first = shape.splines[s].knots[lk].p;
					}
				}

				lk = k;

				Gizmos.DrawLine(tm.MultiplyPoint(first), tm.MultiplyPoint(pos));

				c++;

				first = pos;
			}

			if ( (c & 1) == 1 )
				Gizmos.color = shape.col1 * modcol;
			else
				Gizmos.color = shape.col2 * modcol;

			Vector3 lastpos;
			if ( shape.splines[s].closed )
				lastpos = shape.splines[s].Interpolate(0.0f, shape.normalizedInterp, ref k);
			else
				lastpos = shape.splines[s].Interpolate(1.0f, shape.normalizedInterp, ref k);

			Gizmos.DrawLine(tm.MultiplyPoint(first), tm.MultiplyPoint(lastpos));
		}
	}
#endif

	// Load stuff
	string lastpath = "";

	public delegate bool ParseBinCallbackType(BinaryReader br, string id);
	public delegate void ParseClassCallbackType(string classname, BinaryReader br);

	void LoadSVG(float scale)
	{
		MegaShape ms = (MegaShape)target;

		string filename = EditorUtility.OpenFilePanel("SVG File", lastpath, "svg");

		if ( filename == null || filename.Length < 1 )
			return;

		lastpath = filename;

		bool opt = true;
		if ( ms.splines != null && ms.splines.Count > 0 )
			opt = EditorUtility.DisplayDialog("Spline Import Option", "Splines already present, do you want to 'Add' or 'Replace' splines with this file?", "Add", "Replace");

		int startspline = 0;
		if ( opt )
			startspline = ms.splines.Count;
		//else
		//	ms.splines.Clear();

		//FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, System.IO.FileShare.Read);

		StreamReader streamReader = new StreamReader(filename);
		string text = streamReader.ReadToEnd();
		streamReader.Close();
		MegaShapeSVG svg = new MegaShapeSVG();
		svg.importData(text, ms, scale, opt, startspline);	//.splines[0]);
		ms.imported = true;
	}

	void LoadSXL(float scale)
	{
		MegaShape ms = (MegaShape)target;

		string filename = EditorUtility.OpenFilePanel("SXL File", lastpath, "sxl");

		if ( filename == null || filename.Length < 1 )
			return;

		lastpath = filename;

		bool opt = true;
		if ( ms.splines != null && ms.splines.Count > 0 )
			opt = EditorUtility.DisplayDialog("Spline Import Option", "Splines already present, do you want to 'Add' or 'Replace' splines with this file?", "Add", "Replace");

		int startspline = 0;
		if ( opt )
			startspline = ms.splines.Count;
		//else
		//	ms.splines.Clear();

		//FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, System.IO.FileShare.Read);

		StreamReader streamReader = new StreamReader(filename);
		string text = streamReader.ReadToEnd();
		streamReader.Close();
		MegaShapeSXL sxl = new MegaShapeSXL();
		sxl.importData(text, ms, scale, opt, startspline);	//.splines[0]);
		ms.imported = true;
	}

	void LoadShape(float scale)
	{
		MegaShape ms = (MegaShape)target;
		//Modifiers mod = mr.GetComponent<Modifiers>();	// Do this at start and store

		string filename = EditorUtility.OpenFilePanel("Shape File", lastpath, "spl");

		if ( filename == null || filename.Length < 1 )
			return;

		lastpath = filename;

		// Clear what we have
		bool opt = true;
		if ( ms.splines != null && ms.splines.Count > 0 )
			opt = EditorUtility.DisplayDialog("Spline Import Option", "Splines already present, do you want to 'Add' or 'Replace' splines with this file?", "Add", "Replace");

		int startspline = 0;
		if ( opt )
			startspline = ms.splines.Count;
		else
			ms.splines.Clear();

		ParseFile(filename, ShapeCallback);

		ms.Scale(scale, startspline);

		ms.MaxTime = 0.0f;

		for ( int s = 0; s < ms.splines.Count; s++ )
		{
			if ( ms.splines[s].animations != null )
			{
				for ( int a = 0; a < ms.splines[s].animations.Count; a++ )
				{
					MegaControl con = ms.splines[s].animations[a].con;
					if ( con != null )
					{
						float t = con.Times[con.Times.Length - 1];
						if ( t > ms.MaxTime )
							ms.MaxTime = t;
					}
				}
			}
		}
		ms.imported = true;
	}

	public void ShapeCallback(string classname, BinaryReader br)
	{
		switch ( classname )
		{
			case "Shape": LoadShape(br); break;
		}
	}

	public void LoadShape(BinaryReader br)
	{
		//MegaMorphEditor.Parse(br, ParseShape);
		MegaParse.Parse(br, ParseShape);
	}

	public void ParseFile(string assetpath, ParseClassCallbackType cb)
	{
		FileStream fs = new FileStream(assetpath, FileMode.Open, FileAccess.Read, System.IO.FileShare.Read);

		BinaryReader br = new BinaryReader(fs);

		bool processing = true;

		while ( processing )
		{
			string classname = MegaParse.ReadString(br);

			if ( classname == "Done" )
				break;

			int	chunkoff = br.ReadInt32();
			long fpos = fs.Position;

			cb(classname, br);

			fs.Position = fpos + chunkoff;
		}

		br.Close();
	}

	static public Vector3 ReadP3(BinaryReader br)
	{
		Vector3 p = Vector3.zero;

		p.x = br.ReadSingle();
		p.y = br.ReadSingle();
		p.z = br.ReadSingle();

		return p;
	}

	bool SplineParse(BinaryReader br, string cid)
	{
		MegaShape ms = (MegaShape)target;
		MegaSpline ps = ms.splines[ms.splines.Count - 1];

		switch ( cid )
		{
			case "Transform":
				Vector3 pos = ReadP3(br);
				Vector3 rot = ReadP3(br);
				Vector3 scl = ReadP3(br);
				rot.y = -rot.y;
				ms.transform.position = pos;
				ms.transform.rotation = Quaternion.Euler(rot * Mathf.Rad2Deg);
				ms.transform.localScale = scl;
				break;

			case "Flags":
				int count = br.ReadInt32();
				ps.closed = (br.ReadInt32() == 1);
				count = br.ReadInt32();
				ps.knots = new List<MegaKnot>(count);
				ps.length = 0.0f;
				break;

			case "Knots":
				for ( int i = 0; i < ps.knots.Capacity; i++ )
				{
					MegaKnot pk = new MegaKnot();

					pk.p = ReadP3(br);
					pk.invec = ReadP3(br);
					pk.outvec = ReadP3(br);
					pk.seglength = br.ReadSingle();

					ps.length += pk.seglength;
					pk.length = ps.length;
					ps.knots.Add(pk);
				}
				break;
		}
		return true;
	}

	MegaKnotAnim ma;

	bool AnimParse(BinaryReader br, string cid)
	{
		MegaShape ms = (MegaShape)target;

		switch ( cid )
		{
			case "V":
				int v = br.ReadInt32();
				ma = new MegaKnotAnim();
				int s = ms.GetSpline(v, ref ma);	//.s, ref ma.p, ref ma.t);

				if ( ms.splines[s].animations == null )
					ms.splines[s].animations = new List<MegaKnotAnim>();

				ms.splines[s].animations.Add(ma);
				break;

			case "Anim":
				ma.con = MegaParseBezVector3Control.LoadBezVector3KeyControl(br);
				break;
		}
		return true;
	}

	bool ParseShape(BinaryReader br, string cid)
	{
		MegaShape ms = (MegaShape)target;

		switch ( cid )
		{
			case "Num":
				int count = br.ReadInt32();
				ms.splines = new List<MegaSpline>(count);
				break;

			case "Spline":
				MegaSpline spl = new MegaSpline();
				ms.splines.Add(spl);
				MegaParse.Parse(br, SplineParse);
				break;

			case "Anim":
				MegaParse.Parse(br, AnimParse);
				break;
		}

		return true;
	}

#if false
	// Prefab creation
	[MenuItem("GameObject/Create MegaShape Prefab")]
	static void DoCreateSimplePrefab()
	{
#if true
		if ( Selection.activeGameObject != null )
		{
			if ( !Directory.Exists("Assets/MegaPrefabs") )
			{
				AssetDatabase.CreateFolder("Assets", "MegaPrefabs");
				//string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
				//Debug.Log("folder path " + newFolderPath);
			}

			GameObject prefab = PrefabUtility.CreatePrefab("Assets/MegaPrefabs/" + Selection.activeGameObject.name + ".prefab", Selection.activeGameObject);

			MeshFilter mf = Selection.activeGameObject.GetComponent<MeshFilter>();
			if ( mf )
			{
				MeshFilter newmf = prefab.GetComponent<MeshFilter>();

				//Mesh mesh = CloneMesh(mf.sharedMesh);

				newmf.sharedMesh = CloneMesh(mf.sharedMesh);
			}
			//PrefabUtility.DisconnectPrefabInstance(prefab);
		}
#else
		Transform[] transforms = Selection.transforms;
		foreach ( Transform t in transforms )
		{
			//Object prefab = EditorUtility.CreateEmptyPrefab("Assets/Temporary/" + t.gameObject.name + ".prefab");
			Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/Temporary/" + t.gameObject.name + ".prefab");

			MeshFilter mf = t.gameObject.GetComponent<MeshFilter>();
			Mesh ms = mf.sharedMesh;
			Debug.Log("Mesh " + ms);
			GameObject newgo = EditorUtility.ReplacePrefab(t.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);

			MeshFilter newmf = newgo.GetComponent<MeshFilter>();
			//Mesh newms = newmf.sharedMesh;
			//Debug.Log("Mesh " + newms);
			newmf.sharedMesh = CloneMesh(ms);
		}
#endif
	}
#endif

	[MenuItem("GameObject/Create MegaShape Prefab")]
	static void DoCreateSimplePrefabNew()
	{
#if true
		if ( Selection.activeGameObject != null )
		{
			if ( !Directory.Exists("Assets/MegaPrefabs") )
			{
				AssetDatabase.CreateFolder("Assets", "MegaPrefabs");
				//string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
				//Debug.Log("folder path " + newFolderPath);
			}

			GameObject obj = Selection.activeGameObject;

			GameObject prefab = PrefabUtility.CreatePrefab("Assets/MegaPrefabs/" + Selection.activeGameObject.name + ".prefab", Selection.activeGameObject);

			MeshFilter mf = obj.GetComponent<MeshFilter>();

			if ( mf )
			{
				MeshFilter newmf = prefab.GetComponent<MeshFilter>();

				Mesh mesh = CloneMesh(mf.sharedMesh);

				mesh.name = obj.name + " copy";
				AssetDatabase.AddObjectToAsset(mesh, prefab);
				//AssetDatabase.CreateAsset(mesh, "Assets/MegaPrefabs/" + Selection.activeGameObject.name + ".asset");
				newmf.sharedMesh = mesh;

				MeshCollider mc = prefab.GetComponent<MeshCollider>();
				if ( mc )
				{
					mc.sharedMesh = null;
					mc.sharedMesh = mesh;
				}
			}


			//PrefabUtility.DisconnectPrefabInstance(prefab);
		}
#else
		Transform[] transforms = Selection.transforms;
		foreach ( Transform t in transforms )
		{
			//Object prefab = EditorUtility.CreateEmptyPrefab("Assets/Temporary/" + t.gameObject.name + ".prefab");
			Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/Temporary/" + t.gameObject.name + ".prefab");

			MeshFilter mf = t.gameObject.GetComponent<MeshFilter>();
			Mesh ms = mf.sharedMesh;
			Debug.Log("Mesh " + ms);
			GameObject newgo = EditorUtility.ReplacePrefab(t.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);

			MeshFilter newmf = newgo.GetComponent<MeshFilter>();
			//Mesh newms = newmf.sharedMesh;
			//Debug.Log("Mesh " + newms);
			newmf.sharedMesh = CloneMesh(ms);
		}
#endif
	}



	static Mesh CloneMesh(Mesh mesh)
	{
		Mesh clonemesh = new Mesh();
		clonemesh.vertices = mesh.vertices;
		clonemesh.uv1 = mesh.uv1;
		clonemesh.uv2 = mesh.uv2;
		clonemesh.uv = mesh.uv;
		clonemesh.normals = mesh.normals;
		clonemesh.tangents = mesh.tangents;
		clonemesh.colors = mesh.colors;

		clonemesh.subMeshCount = mesh.subMeshCount;

		for ( int s = 0; s < mesh.subMeshCount; s++ )
		{
			clonemesh.SetTriangles(mesh.GetTriangles(s), s);
		}

		//clonemesh.triangles = mesh.triangles;

		clonemesh.boneWeights = mesh.boneWeights;
		clonemesh.bindposes = mesh.bindposes;
		clonemesh.name = mesh.name + "_copy";
		clonemesh.RecalculateBounds();

		return clonemesh;
	}
}
