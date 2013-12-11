
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MegaMorphAnimClip
{
	public string			name;
	public float			start;
	public float			end;
	public MegaRepeatMode	loop;

	public MegaMorphAnimClip(string _name, float _start, float _end, MegaRepeatMode _loop)
	{
		name = _name;
		start = _start;
		end = _end;
		loop = _loop;
	}
}

//public delegate bool MegaMorphAnimClipEvent(int clip, int id);

[AddComponentMenu("Modifiers/Morph Animator")]
[ExecuteInEditMode]
public class MegaMorphAnimator : MonoBehaviour
{
	public MegaMorphBase	morph;

	public MegaMorphBase[]	morphs;	

	public List<MegaMorphAnimClip>	clips = new List<MegaMorphAnimClip>();

	public int current = 0;
	public float t = -1.0f;	// Current clip time
	public float at = 0.0f;
	//MegaMorphAnimClipEvent	listener;

	public int sourceFPS = 30;
	public bool	useFrames = true;

	//public void SetListener(MegaMorphAnimClipEvent listen)
	//{
	//	listener = listen;
	//}

	[ContextMenu("Help")]
	public void Help()
	{
		Application.OpenURL("http://www.west-racing.com/mf/?page_id=1108");
	}

	public bool IsPlaying()
	{
		if ( t >= 0.0f )
			return true;

		return false;
	}

	public void SetTime(float time)
	{
		t = time;
	}

	public float GetTime()
	{
		return at;
	}

	public void PlayClipEvent(int i)
	{
		PlayClip(i);
	}

	public void PlayClipNameEvent(string name)
	{
		PlayClip(name);
	}

	public void PlayClip(int i)
	{
		if ( i < clips.Count )
		{
			current = i;
			t = 0.0f;
		}
	}

	public void PlayClip(string name)
	{
		for ( int i = 0; i < clips.Count; i++ )
		{
			if ( clips[i].name == name )
			{
				current = i;
				t = 0.0f;
				return;
			}
		}
	}

	public void Stop()
	{
		t = -1.0f;
	}

	public int AddClip(string name, float start, float end, MegaRepeatMode loop)
	{
		MegaMorphAnimClip clip = new MegaMorphAnimClip(name, start, end, loop);
		clips.Add(clip);
		return clips.Count - 1;
	}

	public string[] GetClipNames()
	{
		string[] names = new string[clips.Count];

		for ( int i = 0; i < clips.Count; i++ )
		{
			names[i] = clips[i].name;
		}

		return names;
	}

	void Start()
	{
		if ( PlayOnStart )
			t = 0.0f;
		else
			t = -1.0f;
	}

	void Update()
	{
		if ( MultipleMorphs )
		{
			if ( morphs == null )
				morphs = GetComponentsInChildren<MegaMorphBase>();
		}
		else
		{
			if ( morph == null )
				morph = (MegaMorphBase)GetComponent<MegaMorphBase>();
		}

		if ( LinkedUpdate )
		{
			DoLinkedUpdate();
		}
		else
		{
			//if ( morph && clips.Count > 0 && current < clips.Count )
			if ( clips.Count > 0 && current < clips.Count )
			{
				if ( t >= 0.0f )
				{
					t += Time.deltaTime;
					float dt = clips[current].end - clips[current].start;

					switch ( clips[current].loop )
					{
						case MegaRepeatMode.Loop:		at = Mathf.Repeat(t, dt);		break;
						case MegaRepeatMode.PingPong:	at = Mathf.PingPong(t, dt);		break;
						case MegaRepeatMode.Clamp:		at = Mathf.Clamp(t, 0.0f, dt);	break;
					}

					at += clips[current].start;

					if ( MultipleMorphs )
					{
						if ( morphs != null )
						{
							for ( int i = 0; i < morphs.Length; i++ )
							{
								morphs[i].SetAnim(at);
							}
						}
					}
					else
					{
						if ( morph )
							morph.SetAnim(at);
					}
				}
			}
		}
	}

	public bool MultipleMorphs = false;
	public bool LinkedUpdate = false;
	public bool PlayOnStart = true;

	void DoLinkedUpdate()
	{
		if ( animation != null )
		{
			foreach ( AnimationState state in animation )
			{
				if ( state.enabled )
				{
					AnimationClip clip = state.clip;

					if ( clip != null )
					{
						for ( int i = 0; i < clips.Count; i++ )
						{
							if ( clips[i].name == clip.name )
							{
								current = i;
								float ct = state.time;

								WrapMode wm = clip.wrapMode;

								if ( wm == WrapMode.Default )
								{
									wm = animation.wrapMode;
								}

								switch ( clip.wrapMode )
								{
									case WrapMode.Loop:
										ct = Mathf.Repeat(ct, clip.length);
										break;

									case WrapMode.PingPong:
										ct = Mathf.PingPong(ct, clip.length);
										break;

									case WrapMode.ClampForever:
										ct = Mathf.Clamp(ct, 0.0f, clip.length);
										break;

									case WrapMode.Once:
										if ( ct > clip.length )
											ct = 0.0f;
										break;
								}
								ct += clips[current].start;

								if ( MultipleMorphs )
								{
									if ( morphs != null )
									{
										for ( int m = 0; m < morphs.Length; m++ )
										{
											morphs[m].SetAnim(at);
										}
									}
								}
								else
								{
									if ( morph )
										morph.SetAnim(ct);	//state.time + clips[current].start);
								}
								return;
							}
						}
					}
				}
			}
		}
	}
}