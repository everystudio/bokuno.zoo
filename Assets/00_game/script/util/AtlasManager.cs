using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AtlasManager : MonoBehaviour {

	static AtlasManager instance = null;

	public static AtlasManager Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("AtlasManager");
				if (obj == null) {
					obj = new GameObject("AtlasManager");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<AtlasManager> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<AtlasManager>() as AtlasManager;
				}
				instance.initialize ();
			}
			return instance;
		}
	}

	private void initialize ()
	{
		DontDestroyOnLoad(gameObject);
		return;
	}

	[SerializeField]
	private List<UIAtlas> m_atlasList = new List<UIAtlas> ();
	public UIAtlas GetAtlas( string _strName ){
		return getAtlas (_strName, m_atlasList);
	}


	private UIAtlas getAtlas( string _strName , List<UIAtlas> _lstAtlas ){
		foreach (UIAtlas atlas in _lstAtlas) {
			if (atlas.GetSprite (_strName) != null) {
				return atlas;
			}
		}
		return null;
	}

	/*
	[SerializeField]
	private List<UIAtlas> m_atlasUIList = new List<UIAtlas> ();
	public UIAtlas GetAtlasUI( string _strName ){
		return getAtlas (_strName, m_atlasUIList);
	}

	[SerializeField]
	private List<UIAtlas> m_atlasMapList = new List<UIAtlas> ();
	public UIAtlas GetAtlasMap( string _strName ){
		return getAtlas (_strName, m_atlasMapList);
	}
	*/




}
