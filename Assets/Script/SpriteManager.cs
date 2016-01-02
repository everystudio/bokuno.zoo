using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UtilAssetBundleSprite))]
public class SpriteManager : MonoBehaviourEx {

	[System.Serializable]
	public class TTexture2DPair {
		public string		strSpriteName;
		public Texture2D	texture2D;
	}
	[System.Serializable]
	public class TSpritePair {
		public string		strSpriteName;
		public Sprite	sprSprite;
	}

	public UtilAssetBundleSprite m_csUtilAssetBundleSprite;

	public Sprite Get( string _strName ){

		_strName = _strName.ToLower ();
		//Debug.Log (m_LoadedSpriteList.Count);

		foreach (TSpritePair data in m_LoadedSpriteList) {
			//Debug.Log (data.strSpriteName);
			if (data.strSpriteName.Equals (_strName) == true) {
				return data.sprSprite;
			}
		}
		return null;
	}

	public Texture2D GetTexture( string _strName ){
		_strName = _strName.ToLower ();
		//Debug.Log (m_LoadedSpriteList.Count);

		foreach (TTexture2DPair data in m_LoadedTexture2DList) {
			//Debug.Log (data.strSpriteName);
			if (data.strSpriteName.Equals (_strName) == true) {
				return data.texture2D;
			}
		}
		return null;
	}

	public bool IsExistSprite( string _strName ){
		_strName = _strName.ToLower ();
		foreach (TSpritePair data in m_LoadedSpriteList) {
			if (data.strSpriteName.Equals (_strName) == true) {
				return true;
			}
		}
		return false;
	}

	// 閲覧用
	public List<TSpritePair> m_LoadedSpriteList = new List<TSpritePair>();
	public List<TTexture2DPair> m_LoadedTexture2DList = new List<TTexture2DPair>();

	public bool Add( string _strAssetBundleName , Texture2D _texture2D ){

		foreach (TTexture2DPair data in m_LoadedTexture2DList) {
			if (data.strSpriteName.Equals (_strAssetBundleName) == true) {
				return false;
			}
		}
		//Debug.Log ("insert sprite:" + _strAssetBundleName);

		TTexture2DPair insertData = new TTexture2DPair ();
		insertData.strSpriteName = _strAssetBundleName;
		insertData.texture2D = _texture2D;

		m_LoadedTexture2DList.Add (insertData);

		Sprite tempSprite = Sprite.Create (_texture2D, new Rect (0, 0, _texture2D.width, _texture2D.height), Vector2.zero);
		tempSprite.name = string.Format ("{0}(fromTexture2D)", _strAssetBundleName);

		Add (_strAssetBundleName, tempSprite);

		return true;	
	}

	public bool Add( string _strAssetBundleName , Sprite _sprite ){
		bool bRet = false;

		foreach (TSpritePair data in m_LoadedSpriteList) {
			if (data.strSpriteName.Equals (_strAssetBundleName) == true) {
				return false;
			}
		}
		//Debug.Log ("insert sprite:" + _strAssetBundleName);

		TSpritePair insertData = new TSpritePair ();
		insertData.strSpriteName = _strAssetBundleName;
		insertData.sprSprite = _sprite;

		m_LoadedSpriteList.Add (insertData);
		return true;
	}

	static SpriteManager instance = null;
	public static SpriteManager Instance
	{
		get
		{
			if(instance == null)
			{
				GameObject obj = GameObject.Find("SpriteManager");
				if( obj == null )
				{
					obj = new GameObject("SpriteManager");
				}

				instance = obj.GetComponent<SpriteManager>();

				if(instance == null)
				{
					instance = obj.AddComponent<SpriteManager>() as SpriteManager;
				}
				instance.initialize ();
			}
			return instance;
		}
	}
	private void initialize(){
		DontDestroyOnLoad(gameObject);

		m_csUtilAssetBundleSprite = GetComponent<UtilAssetBundleSprite> ();
		m_csUtilAssetBundleSprite.SetParent (gameObject);
		return;
	}
	public Queue<string > m_LoadFileName = new Queue<string > ();
	public string m_strLoadingFileName;
	public bool LoadAssetBundleQueue( string _strAssetName ){
		_strAssetName = _strAssetName.ToLower ();
		if ( IsExistSprite(_strAssetName) == true ) {
			return false;
		}
		m_LoadFileName.Enqueue (_strAssetName);
		return true;
	}
	public bool IsIdle(){
		if (0 < m_LoadFileName.Count) {
			return false;
		}
		return (m_eStep == STEP.IDLE);
	}

	public enum STEP {
		NONE		= 0,
		IDLE		,
		LOAD_START	,
		LOADING		,
		LOAD_END	,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	void Update(){
		bool bInit = false;

		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}
		switch (m_eStep) {
		case STEP.NONE:
		case STEP.IDLE:
			if (0 < m_LoadFileName.Count) {
				m_eStep = STEP.LOADING;
			}
			break;

		case STEP.LOADING:
			if( bInit ){
				m_strLoadingFileName = m_LoadFileName.Dequeue ();

				m_csUtilAssetBundleSprite.Load (m_strLoadingFileName );
			}
			if( m_csUtilAssetBundleSprite.IsLoaded()){
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.MAX:
		default:
			break;

		}
	}





}













