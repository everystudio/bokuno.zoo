using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour {

	static TutorialManager instance = null;
	public static TutorialManager Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("TutorialManager");
				if (obj == null) {
					obj = new GameObject("TutorialManager");
				}
				instance = obj.GetComponent<TutorialManager> ();
				if (instance == null) {
					instance = obj.AddComponent<TutorialManager>() as TutorialManager;
				}
				instance.initialize ();
			}
			return instance;
		}
	}

	public bool IsTutorial(){
		return m_eStep != STEP.END;
	}
	public void initialize(){
		DontDestroyOnLoad(gameObject);
		//PlayerPrefs.SetInt (Define.KEY_TUTORIAL_PARENT_ID , 3);
		//PlayerPrefs.DeleteKey(Define.KEY_TUTORIAL_PARENT_ID);
		int iParentTutorialId = 0;
		if (PlayerPrefs.HasKey (Define.KEY_TUTORIAL_PARENT_ID)) {
			iParentTutorialId = PlayerPrefs.GetInt (Define.KEY_TUTORIAL_PARENT_ID);
		} else {
			iParentTutorialId = 1;
			PlayerPrefs.SetInt (Define.KEY_TUTORIAL_PARENT_ID, iParentTutorialId);
		}


		m_iParentTutorialId = PlayerPrefs.GetInt (Define.KEY_TUTORIAL_PARENT_ID);
		m_iChildTutorialId = 1;
		m_iTutorialIndex = 0;

		m_eStep = STEP.END;
		foreach (CsvTutorialData data in DataManager.csv_tutorial) {
			if (data.tutorial_parent_id == m_iParentTutorialId) {
				m_eStep = STEP.COMMAND;
			}
		}
		//m_eStep = STEP.END;


		return;
	}


	public CsvTutorialData m_CurrentData;

	public enum STEP {
		NONE			= 0,
		INIT			,
		COMMAND			,
		INPUT_LIMITED	,
		CHECK			,
		WHITE			,
		IMAGE			,
		TOUCH_SCREEN	,
		TOUCH_RECT		,
		WAIT			,
		END_PARENT		,
		BANNER			,
		TOUCH_BANNER	,
		FINISH			,
		END				,
		MAX				,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;
	public STEP m_eStepNext;

	public float m_fWaitTimer;

	public ButtonBase m_btnHitRect;

	public GameObject m_RectAll;
	public GameObject m_RectMonster;

	public int m_iParentTutorialId;
	public int m_iChildTutorialId;
	public int m_iTutorialIndex;
	//public List<UITexture> m_TextureList = new List<UITexture> ();
	public List<UtilSwitchSprite> m_SpriteList = new List<UtilSwitchSprite> ();
	public int m_iSpriteIndex;

	public CsvTutorialData GetTutorialData( int _iParentId , int _iChildId 	, int _iIndex ){
		int iCount = 0;
		foreach (CsvTutorialData data in DataManager.csv_tutorial) {
			if (data.tutorial_parent_id == _iParentId && data.tutorial_child_id == _iChildId) {
				if (_iIndex == iCount) {
					return data;
				} else {
					iCount += 1;
				}
			}
		}
		return new CsvTutorialData ();
	}

	private STEP GetStep( CsvTutorialData _data ){
		switch (_data.command) {
		case "input_limited":
			return STEP.INPUT_LIMITED;
			break;
		case "white":
			return STEP.WHITE;
			break;
		case "image":
			return STEP.IMAGE;
			break;
		case "touch_screen":
			return STEP.TOUCH_SCREEN;
			break;
		case "touch_rect":
			return STEP.TOUCH_RECT;
			break;
		case "wait":
			return STEP.WAIT;
			break;
		case "end_parent":
			return STEP.END_PARENT;
			break;
		case "banner":
			return STEP.BANNER;
			break;
		case "touch_banner":
			return STEP.TOUCH_BANNER;
			break;
		case "finish":
			return STEP.FINISH;
			break;
		default:
			break;

		}
		return STEP.MAX;
	}

	private void ActionTouchRect( CsvTutorialData _data ){
		switch (_data.string_param) {
		case "item_serial":
			GameMain.Instance.SwitchItemSerial = _data.param5;
			break;
		case "clean":
			GameMain.Instance.SwitchClean = 1;
			break;
		case "food":
			GameMain.Instance.SwitchFood = 1;
			break;
		case "close":
			GameMain.Instance.SwitchClose = 1;
			break;
		case "tab_index":
			GameMain.Instance.bSwitchTab = true;
			GameMain.Instance.SwitchTabIndex = _data.param5;
			break;
		case "buildup":
			GameMain.Instance.TutorialBuildup = true;
			break;
		case "ojisan_check":
			GameMain.Instance.bOjisanCheck = true;
			GameMain.Instance.OjisanCheckIndex = _data.param5;
			break;
		default:
			break;
		}
	}



	public UISprite m_sprWhite;

	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {
		case STEP.NONE:
			break;

		case STEP.INIT:
			if (bInit) {
				m_iParentTutorialId = PlayerPrefs.GetInt (Define.KEY_TUTORIAL_PARENT_ID);
				m_iChildTutorialId = 1;
				m_iTutorialIndex = 0;
			}
			m_eStep = STEP.END;
			foreach (CsvTutorialData data in DataManager.csv_tutorial) {
				if (data.tutorial_parent_id == m_iParentTutorialId) {
					m_eStep = STEP.COMMAND;
				}
			}
			break;

		case STEP.CHECK:

			if (0 < m_CurrentData.next_tutorial_child_id) {
				//Debug.Log( string.Format( "{0}:{1}" , m_eStep , m_CurrentData.next_tutorial_child_id ));
				m_iChildTutorialId = m_CurrentData.next_tutorial_child_id;
				m_iTutorialIndex = 0;
				m_iSpriteIndex = 0;

				foreach (UtilSwitchSprite switch_sprite in m_SpriteList) {
					switch_sprite.Clear ();
				}

			}
			m_eStep = STEP.COMMAND;
			break;

		case STEP.COMMAND:
			if (bInit) {
				m_CurrentData = GetTutorialData (m_iParentTutorialId, m_iChildTutorialId, m_iTutorialIndex);

				//あんま良くないけどいったんここでインクリメント
				m_iTutorialIndex += 1;

			}
			m_eStepNext = GetStep (m_CurrentData);

			m_eStep = m_eStepNext;//= GetStep (m_CurrentData);
			break;
		case STEP.INPUT_LIMITED:
			GameMain.Instance.TutorialInputLock = true;

			if (m_CurrentData.string_param.Equals ("monster")) {
				m_RectAll.SetActive (false);
				m_RectMonster.SetActive (true);
			} else {
				m_RectAll.SetActive (true);
				m_RectMonster.SetActive (false);
			}

			m_eStep = STEP.CHECK;

			break;
		case STEP.WHITE:

			if (m_CurrentData.string_param.Equals ("display")) {
				m_sprWhite.alpha = 0.3f;
			} else {
				m_sprWhite.alpha = 0.0f;
			}
			m_eStep = STEP.CHECK;
			break;
		case STEP.IMAGE:
			if (bInit) {
				UtilSwitchSprite switch_sprite = m_SpriteList [m_iSpriteIndex];
				switch_sprite.gameObject.SetActive (true);
				switch_sprite.GetComponent<Fuwafuwa> ().enabled = false;
				switch_sprite.GetComponent<Chikachika> ().enabled = false;

				switch_sprite.SetSprite (m_CurrentData.string_param ,
					m_CurrentData.param1,
					m_CurrentData.param2,
					m_CurrentData.param3);

				if (m_CurrentData.param4 == 1) {
					switch_sprite.GetComponent<Chikachika> ().enabled = true;
				} else if (m_CurrentData.param4 == 2) {
					switch_sprite.GetComponent<Fuwafuwa> ().enabled = true;
				} else {
				}
				m_iSpriteIndex += 1;
			}
			m_eStep = STEP.CHECK;

			break;
		case STEP.TOUCH_SCREEN:

			bool btsEnd = true;

			foreach (UtilSwitchSprite sws in m_SpriteList) {
				if (sws.IsIdle () == false) {
					btsEnd = false;
				}
			}
			if (btsEnd) {
				if (Input.GetMouseButtonUp (0) && SpriteManager.Instance.IsIdle ()) {
					m_eStep = STEP.CHECK;
				}
			}
			break;
		case STEP.TOUCH_RECT:
			if (bInit) {
				//m_btnHitRect.GetComponent<BoxCollider> ().bounds.center = new Vector3 ((float)m_CurrentData.param1, (float)m_CurrentData.param2, 0.0f);
				//m_btnHitRect.GetComponent<BoxCollider> ().bounds.size = new Vector3 ((float)m_CurrentData.param3, (float)m_CurrentData.param4, 0.0f);
				m_btnHitRect.GetComponent<BoxCollider> ().center = new Vector3 ((float)m_CurrentData.param1, (float)m_CurrentData.param2, -100.0f);
				m_btnHitRect.GetComponent<BoxCollider> ().size = new Vector3 ((float)m_CurrentData.param3, (float)m_CurrentData.param4, 0.0f);
				m_btnHitRect.TriggerClear ();
			}


			if (m_btnHitRect.ButtonPushed) {


				bool btsEnd2 = true;

				foreach (UtilSwitchSprite sws in m_SpriteList) {
					if (sws.IsIdle () == false) {
						btsEnd2 = false;
					}
				}
				m_btnHitRect.TriggerClear ();
				if (btsEnd2) {
					ActionTouchRect (m_CurrentData);
					m_eStep = STEP.CHECK;
				}
			}
			break;

		case STEP.WAIT:
			if (bInit) {
				m_fWaitTimer = 0.0f;
			}
			m_fWaitTimer += Time.deltaTime;
			if ((float)m_CurrentData.param1 < m_fWaitTimer) {
				m_eStep = STEP.CHECK;
			}
			break;

		case STEP.BANNER:
			GameMain.Instance.SwitchSetting = 1;
			if (m_CurrentData.string_param.Equals ("monster") == true) {
				GameMain.Instance.TutorialMonster = m_CurrentData.param1;
			}
			m_eStep = STEP.CHECK;
			break;

		case STEP.END_PARENT:
			int iParentTutorialId = 0;
			if (PlayerPrefs.HasKey (Define.KEY_TUTORIAL_PARENT_ID)) {
				iParentTutorialId = PlayerPrefs.GetInt (Define.KEY_TUTORIAL_PARENT_ID);
			} else {
				iParentTutorialId = 1;		// ない場合は
			}
			PlayerPrefs.SetInt (Define.KEY_TUTORIAL_PARENT_ID, iParentTutorialId + 1);
			m_eStep = STEP.INIT;
			break;

		case STEP.FINISH:
			PlayerPrefs.SetInt (Define.KEY_TUTORIAL_PARENT_ID, 99);
			m_eStep = STEP.INIT;
			break;

		case STEP.END:
			// STAY!
			GameMain.Instance.TutorialInputLock = false;

			gameObject.SetActive (false);
			break;

		default:
			break;
		}



	
	}

	// 乱用禁止
	public void Next(){
		m_eStep = STEP.CHECK;
	}


}












