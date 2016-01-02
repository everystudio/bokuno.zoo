using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParkMain : PageBase {

	public enum STEP
	{
		NONE			= 0,
		SETUP_DB		,
		SETUP			,
		IDLE			,

		SWIPE			,

		SETTING_ITEM	,

		END			,
		MAX			,
	}
	public STEP m_eStep;
	private STEP m_eStepPre;

	#region SerializeField
	[SerializeField]
	private GameObject m_goParkRoot;
	public GameObject goParkRoot{
		get{ return m_goParkRoot; }
	}
	[SerializeField]
	private Camera m_cameraUI;
	public Camera cameraUI{
		get{ return m_cameraUI; }
	}

	[SerializeField]
	private UISprite m_sprBackground;
	#endregion

	#region 各スクリプト(インスペクターで設定)
	public CtrlParkRoot m_csParkRoot;
	#endregion

	#region フッター関連
	[SerializeField]
	private CtrlParkMainFooter m_footer;
	#endregion

	private ParkMainController m_parkMainController;

	public List<CtrlFieldItem> m_csFieldItemList = new List<CtrlFieldItem>();

	public enum EDIT_MODE
	{
		NONE		= 0,
		NORMAL		,
		MOVE		,
		MAX			,
	}

	public EDIT_MODE m_eEditMode;

	// 警告つぶしとりあえず宣言
	protected override void initialize(){

		m_sprBackground.spriteName = "bg_main";
		m_eStep = STEP.SETUP_DB;
		m_eStepPre = STEP.MAX;
		m_footer.TriggerClearAll ();
		m_footer.gameObject.SetActive (false);

		if (m_parkMainController != null) {
			m_parkMainController.enabled = false;
		}
		AdsManager.Instance.ShowIcon (gameObject, true);
		//Debug.LogError ("initialize");

	}
	protected override void close(){
		m_sprBackground.spriteName = "bg_table";
		AdsManager.Instance.ShowIcon (gameObject, false);
		//Debug.LogError ("close");

	}

	public bool m_bInputTrigger;

	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
			Debug.Log (m_eStep);
		}

		switch (m_eStep) {
		case STEP.SETUP_DB:
			if (bInit) {
			}
			if (GameMain.Instance.IsIdle ()) {
				m_eStep = STEP.SETUP;
			}
			break;
		case STEP.SETUP:
			if (bInit) {

				m_csFieldItemList.Clear ();
				DataManager.Instance.m_ItemDataList = GameMain.dbItem.Select ( " status != 0 " );
				//Debug.LogError (DataManager.Instance.m_ItemDataList.Count);
				m_csParkRoot.Initialize (DataManager.Instance.m_ItemDataList );
				m_csParkRoot.ConnectingRoadCheck ();

				// ここで仕事のチェックしますか
				DataWork.WorkCheck ();
			}
			if (InputManager.Instance.m_TouchInfo.TouchON) {
				m_eStep = STEP.IDLE;
			}

			if (GameMain.Instance.m_bStartSetting) {
				m_eStep = STEP.SETTING_ITEM;
			} else {
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.IDLE:
			if (bInit) {
				SoundManager.Instance.PlayBGM (SoundName.BGM_MAIN);

				m_eEditMode = ParkMain.EDIT_MODE.NORMAL;
				m_footer.gameObject.SetActive (true);

				if (m_parkMainController != null) {
					m_parkMainController.enabled = false;
				}
				m_parkMainController = GetComponent<ParkMainIdle> () as ParkMainController;
				if (m_parkMainController == null) {
					m_parkMainController = gameObject.AddComponent<ParkMainIdle> () as ParkMainController;
				}
				m_parkMainController.Initialize (this);
			}
			if (m_parkMainController.IsEnd ()) {
				m_eEditMode = ParkMain.EDIT_MODE.MOVE;;
				m_eStep = STEP.SETTING_ITEM;
			}
			break;

		case STEP.SETTING_ITEM:
			if (bInit) {
				// 念のため
				m_footer.gameObject.SetActive (false);

				if (m_parkMainController != null) {
					m_parkMainController.enabled = false;
				}
				m_parkMainController = GetComponent<ParkMainSettingItem> () as ParkMainController;
				if (m_parkMainController == null) {
					m_parkMainController = gameObject.AddComponent<ParkMainSettingItem> () as ParkMainController;
				}
				m_parkMainController.Initialize (this);
			}
			if (m_parkMainController.IsEnd ()) {
				m_parkMainController.Clear ();
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.MAX:
		default:
			break;
		}
	
	}



	/*
	public bool GetGrid( Vector2 _inputPoint , out int _iX , out int _iY ){
		bool bRet = false;
		RaycastHit hit;
		Ray ray = m_cameraUI.ScreenPointToRay (new Vector3 (_inputPoint.x, _inputPoint.y, 0.0f));
		float fDistance = 100.0f;

		_iX = 0;
		_iY = 0;

			//レイを投射してオブジェクトを検出
		if (Physics.Raycast (ray, out hit, fDistance)) {

			Vector3 v3Dir = hit.point - m_goParkRoot.transform.position;
			GameObject objPoint = new GameObject ();
			objPoint.transform.position = hit.point;
			objPoint.transform.parent = m_goParkRoot.transform;

			// ここの計算式は後で見直します
			int calc_x = Mathf.FloorToInt ((objPoint.transform.localPosition.x + (objPoint.transform.localPosition.y * 2.0f)) / 160.0f);
			int calc_y = Mathf.FloorToInt (((objPoint.transform.localPosition.y * 2.0f) - objPoint.transform.localPosition.x) / 160.0f);
			//Debug.Log ("calc_x=" +  calc_x.ToString () + " calc_y=" +  calc_y.ToString ());

			bRet = true;
			_iX = calc_x;
			_iY = calc_y;
			Destroy (objPoint);
		}
		return bRet;
	}
	*/





}
