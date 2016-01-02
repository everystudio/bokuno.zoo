using UnityEngine;
using System.Collections;

public class UtilSwitchSprite : MonoBehaviourEx {

	public bool IsIdle(){
		if (m_eStep == STEP.IDLE) {
			return true;
		}
		return false;
	}

	public void Clear(){
		//Debug.LogError ( string.Format("clear:{0}" , m_strSpriteName ));
		if (m_sprite2D != null) {
			m_sprite2D.sprite2D = null;
		}
	}

	public bool SetSprite( string _strName , int _iPosX , int _iPosY , int _iDepth ){
		myTransform.localPosition = new Vector3 ((float)_iPosX, (float)_iPosY, 0.0f);
		if (m_sprite2D != null) {
			m_sprite2D.depth = _iDepth;
		}
		return SetSprite (_strName);
	}

	public bool SetSprite( string _strName ){

		bool bRet = false;

		if (m_sprite == null && m_sprite2D == null) {
			//Debug.LogError ("no set sprite or sprite 2d");
			return false;
		}

		if (m_eStep == STEP.NONE || m_eStep == STEP.IDLE ) {
			enabled = true;
			m_eStep = STEP.LOADING;
			m_strSpriteName = _strName;
			bRet = true;
		}
		return bRet;
	}

	public bool SetSprite( ref UI2DSprite _sprite , string _strName ){
		m_sprite2D = _sprite;
		m_sprite = null;
		return SetSprite (_strName);
	}

	public bool SetSprite( ref Sprite _sprite , string _strName ){
		m_sprite = _sprite;
		m_sprite2D = null;
		return SetSprite (_strName);
	}

	public enum STEP
	{
		NONE		= 0,
		IDLE		,
		LOADING		,
		SET_SPRITE	,
		END			,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	public Sprite m_sprite;						// 何か名前に困った
	public UI2DSprite m_sprite2D;				// 何か名前に困った
	public string m_strSpriteName;		// アセットバンドルの名前

	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}
		switch (m_eStep) {
		case STEP.IDLE:
			if (bInit) {
				// 自分で停止させる
				enabled = false;
			}
			break;

		case STEP.LOADING:
			if (bInit) {
				//Debug.Log (m_strSpriteName);
				SpriteManager.Instance.LoadAssetBundleQueue (m_strSpriteName);
			}
			if (SpriteManager.Instance.IsIdle ()) {
				m_eStep = STEP.SET_SPRITE;
			}
			break;

		case STEP.SET_SPRITE:
			if (SpriteManager.Instance.IsExistSprite (m_strSpriteName)) {
				//Debug.LogError ("isexistsprite:[" + m_strSpriteName + "]" );
				Sprite tempSprite = SpriteManager.Instance.Get (m_strSpriteName);
				if (m_sprite != null) {
					m_sprite = tempSprite;
					//Debug.LogError ("seta" );
				}
				if (m_sprite2D != null) {
					m_sprite2D.sprite2D = tempSprite;
					m_sprite2D.width = tempSprite.texture.width;
					m_sprite2D.height = tempSprite.texture.height;
					//Debug.LogError ("setb" );
				}
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.END:
		case STEP.MAX:
		default:
			m_eStep = STEP.IDLE;
			break;
		}
	
	}
}
