using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMain : MonoBehaviour {

	public UniWebView m_csUniWebView;

	public bool TutorialInputLock;
	private int m_iTutorialIndex;
	public int TutorialIndex {
		get{ return m_iTutorialIndex; }
		set{ m_iTutorialIndex = value; }
	}

	public int SwitchItemSerial;
	public int SwitchClean;
	public int SwitchFood;
	public int SwitchSetting;
	public int SwitchClose;
	public bool bSwitchTab;
	public int SwitchTabIndex;
	public bool TutorialBuildup;
	public int TutorialMonster;
	public bool bOjisanCheck;
	public int OjisanCheckIndex;
	public int PreSettingItemId;

	#region DB関係
	DBKvs m_dbKvs;
	DBItem m_dbItem;
	DBItemMaster m_dbItemMaster;
	DBWork m_dbWork;
	DBMonster m_dbMonster;
	DBMonsterMaster m_dbMonsterMaster;
	DBStaff m_dbStaff;
	ThreadQueue.TaskControl m_tkKvsOpen;
	ThreadQueue.TaskControl m_tkBackupCheck;
	#endregion

	#region SerializeField
	[SerializeField]
	private CtrlParkRoot m_ctrlParkRoot;
	static public CtrlParkRoot ParkRoot{
		get{ return Instance.m_ctrlParkRoot; }
	}

	[SerializeField]
	private GameObject m_goPanelFront;
	static public GameObject PanelFront{
		get{ return Instance.m_goPanelFront; }
	}

	[SerializeField]
	private Camera m_cameraUI;

	[SerializeField]
	private CtrlHeader m_header;
	[SerializeField]
	private CtrlCollectGold m_collectGold;
	[SerializeField]
	private CtrlPopupWork m_popupWork;
	[SerializeField]
	private CtrlFukidashiWork m_fukidashiWork;
	#endregion



	private int m_iPushedBuildingSerial;
	public int BuildingSerial{
		get{ return m_iPushedBuildingSerial; }
		set{ m_iPushedBuildingSerial = value; }
	}



	public void AddFukidashi( int _iWorkId , string _strMessage ){
		m_popupWork.AddCleardWorkId (_iWorkId);
		m_fukidashiWork.AddMessage (_strMessage);
		//m_fukidashiWork.m_MessageQueue.Enqueue ( string.Format( "[000000]{0}[-]" , _strMessage ) );
	}

	static public DBWork dbWork{
		get{
			return instance.m_dbWork;
		}
	}
	static public DBItem dbItem{
		get{
			return instance.m_dbItem;
		}
	}
	static public DBItemMaster dbItemMaster{
		get{
			return instance.m_dbItemMaster;
		}
	}
	static public DBMonster dbMonster{
		get{
			return instance.m_dbMonster;
		}
	}
	static public DBMonsterMaster dbMonsterMaster{
		get{
			return instance.m_dbMonsterMaster;
		}
	}
	static public DBStaff dbStaff{
		get{
			return instance.m_dbStaff;
		}
	}
	static public DBKvs dbKvs{
		get{
			/*
			if (Instance.m_dbKvs == null) {
				Instance.m_dbKvs = new DBKvs (Define.DB_TABLE_ASYNC_KVS);
			}
			*/
			return Instance.m_dbKvs;
		}
	}

	private DBItem m_dbItemBackup;


	private bool m_bListRefresh;
	static public bool ListRefresh{
		get{ 
			bool bRet = Instance.m_bListRefresh;
			Instance.m_bListRefresh = false;
			return bRet;
		}
		set {
			Instance.m_bListRefresh = value;
		}
	}

	public enum TABLE_TYPE {
		NONE			= 0,
		WORK			,
		ITEM			,
		ITEM_MASTER		,
		MONSTER			,
		MONSTER_MASTER	,
		STAFF			,
		STAFF_MASTER	,
		MAX				,
	}

	protected static GameMain instance = null;
	protected static bool m_bInitialized = false;

	public static bool IsInstance(){
		return instance != null;
	}
	public static GameMain Instance
	{
		get
		{
			// InputManagerの唯一のインスタンスを生成
			if (instance == null)
			{
				GameObject obj = GameObject.Find("GameMain");

				if (obj == null)
				{
					obj = new GameObject("GameMain");
				}
				instance = obj.GetComponent<GameMain>();
				if (instance == null)
				{
					instance = obj.AddComponent<GameMain>();
				}
				instance.initialize();
			}
			if (m_bInitialized == false)
			{
				instance.initialize();
			}
			return instance;
		}
	}
	private void initialize(){
		m_eMoveStatus = STATUS.NONE;
		m_iMoveTab = 0;
		if (m_bInitialized == false) {
			int iWidth = PlayerPrefs.GetInt (Define.USER_WIDTH);
			int iHeight= PlayerPrefs.GetInt (Define.USER_HEIGHT);
			DataManager.user.Initialize (iWidth, iHeight);

			foreach (PageBase page in m_PageList) {
				page.Close ();
			}
			m_eStatus = STATUS.NONE;
			m_eStep = STEP.DB_SETUP;

			/*
			m_csUniWebView.OnLoadComplete += OnLoadComplete;
			m_csUniWebView.OnReceivedMessage += OnReceivedMessage;
			//m_csUniWebView.OnEvalJavaScriptFinished += OnEvalJavaScriptFinished;

			m_csUniWebView.insets = new UniWebViewEdgeInsets( UniWebViewHelper.screenHeight - 50,0,0,UniWebViewHelper.screenWidth - 320);
			Debug.Log ("helper width :" + UniWebViewHelper.screenWidth );
			Debug.Log ("helper height:" + UniWebViewHelper.screenHeight );
			m_csUniWebView.HideToolBar (false);
			//m_csUniWebView.Load(Define.strFooterAdUrl);
			m_csUniWebView.Load( "http://ad.xnosserver.com/apps/bokunodoubutsuen_ios/ad.html" );
			*/
			SetStatus (STATUS.PARK);
		}

		//Debug.Log (TutorialManager.Instance.m_eStep);

		m_bInitialized = true;
		return;
	}
	// The listening method of OnLoadComplete method.
	void OnLoadComplete(UniWebView webView, bool success, string errorMessage) {
		if (success) {
			// Great, everything goes well. Show the web view now.
			webView.Show();
		} else {
			// Oops, something wrong.
			Debug.LogError("Something wrong in webview loading: " + errorMessage);
		}
	}

	void OnReceivedMessage(UniWebView webView, UniWebViewMessage message) {
		//Debug.Log(message.rawMessage);
		if (string.Equals(message.path, "move")) {
			// It is time to move!

			// In this example:
			// message.args["direction"] = "up"
			// message.args["distance"] = "1"
		}
	}

	public enum STEP {
		NONE			= 0,
		DB_SETUP		,
		IDLE			,
		REVIEW			,
		BACKUP_CHECK	,
		MAX			,
	}
	public STEP m_eStep;
	private STEP m_eStepPre;

	public float m_fBackupInterval;
	public float m_fBackupIntervalTimer;
	public CtrlReviewWindow m_reviewWindow;


	public enum STATUS{
		NONE			= 0,
		PARK			,
		BOOK			,
		WORK			,
		ITEM			,
		CAGE_DETAIL		,
		OFFICE_DETAIL	,
		MAX				,
	}

	public STATUS m_eStatus;
	private STATUS m_eStatusPre;

	public List<PageBase> m_PageList = new List<PageBase>();

	public bool IsIdle(){
		return (m_eStep == STEP.IDLE);
	}

	public void SetStatus( STATUS _eStatus ){

		if (m_eStatus != _eStatus) {

			m_PageList [(int)m_eStatus].Close ();
			m_PageList [(int)_eStatus].Initialize ();
			m_eStatus = _eStatus;
		}
		return;
	}

	// Use this for initialization
	void Start () {
		initialize ();
		AdsManager.Instance.ShowAdBanner (true);
	}
	
	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {
		case STEP.DB_SETUP:
			if (bInit) {
				m_dbItem = new DBItem (Define.DB_TABLE_ASYNC_ITEM);
				m_dbItemMaster = new DBItemMaster (Define.DB_TABLE_ASYNC_ITEM_MASTER);
				m_dbWork = new DBWork (Define.DB_TABLE_ASYNC_WORK);
				m_dbMonster = new DBMonster (Define.DB_TABLE_ASYNC_MONSTER);
				m_dbMonsterMaster = new DBMonsterMaster (Define.DB_TABLE_ASYNC_MONSTER_MASTER);
				m_dbStaff = new DBStaff (Define.DB_TABLE_ASYNC_STAFF);
				if (m_dbKvs == null) {
					m_dbKvs = new DBKvs (Define.DB_TABLE_ASYNC_KVS);
				}
				/*
				m_dbItem.Open (Define.DB_NAME_DOUBTSUEN, Define.DB_FILE_DIRECTORY, "");
				m_dbItemMaster.Open (Define.DB_NAME_DOUBTSUEN, Define.DB_FILE_DIRECTORY, "");
				m_dbWork.Open (Define.DB_NAME_DOUBTSUEN, Define.DB_FILE_DIRECTORY, "");
				m_dbMonster.Open (Define.DB_NAME_DOUBTSUEN, Define.DB_FILE_DIRECTORY, "");
				m_dbMonsterMaster.Open (Define.DB_NAME_DOUBTSUEN, Define.DB_FILE_DIRECTORY, "");
				m_dbStaff.Open (Define.DB_NAME_DOUBTSUEN, Define.DB_FILE_DIRECTORY, "");
				m_tkKvsOpen = m_dbKvs.Open (Define.DB_NAME_DOUBTSUEN, Define.DB_FILE_DIRECTORY, ""); // DEFINE.DB_PASSWORD
				*/
			}
			//if (m_tkKvsOpen.Completed) {
			if (true) {
				DataManager.itemMaster = m_dbItemMaster.SelectAll ();

				m_header.Initialize ();
				m_header.RefleshNum ();
				m_collectGold.Initialize ();
				m_fukidashiWork.Initialize ();


				m_eStep = STEP.IDLE;
			}
			break;
		case STEP.IDLE:
			if (bInit) {
				m_fBackupInterval = 10.0f;
				m_fBackupIntervalTimer = 0.0f;
			}
			/*
			m_fBackupIntervalTimer += Time.deltaTime;
			if (m_fBackupInterval < m_fBackupIntervalTimer) {
				m_fBackupIntervalTimer -= m_fBackupInterval;

				m_eStep = STEP.BACKUP_CHECK;
			}
			*/
			if (TutorialManager.Instance.IsTutorial () == false  && ReviewManager.Instance.IsReadyReview()) {
				m_eStep = STEP.REVIEW;
			}
			break;

		case STEP.REVIEW:
			if (bInit) {
				GameObject obj = PrefabManager.Instance.MakeObject ("prefab/CtrlReviewWindow", m_goPanelFront);
				m_reviewWindow = obj.GetComponent<CtrlReviewWindow> ();
				m_reviewWindow.Initialize ();

			}
			if (m_reviewWindow.IsEnd ()) {
				Destroy (m_reviewWindow.gameObject);;
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.BACKUP_CHECK:
			if (bInit) {

				string sourceDB = System.IO.Path.Combine (Application.persistentDataPath, Define.DB_NAME_DOUBTSUEN );
				string backup2DB = System.IO.Path.Combine (Application.persistentDataPath, Define.DB_NAME_DOUBTSUEN_BK2 );
				System.IO.File.Copy (sourceDB, backup2DB, true);

				m_dbItemBackup = new DBItem (Define.DB_NAME_DOUBTSUEN_BK2);
				m_tkBackupCheck = new ThreadQueue.TaskControl ();
				m_tkBackupCheck = m_dbItemBackup.Open (Define.DB_NAME_DOUBTSUEN_BK2, Define.DB_FILE_DIRECTORY, "");
				//Debug.Log ("STEP.BACKUP_CHECK");

			}
			//Debug.Log ("frame");
			if (m_tkBackupCheck.Completed) {

				m_eStep = STEP.IDLE;
				try
				{
					// DBおかしくなってたらここでThrowされる
					List<DataItem> check = m_dbItemBackup.SelectAll ();

					//Debug.Log( "Copy" );
					//string sourcePath = System.IO.Path.Combine (Application.streamingAssetsPath, Define.DB_FILE_DIRECTORY + Define.DB_NAME_DOUBTSUEN );
					string backupDB = System.IO.Path.Combine (Application.persistentDataPath, Define.DB_NAME_DOUBTSUEN_BK );
					string backup2DB = System.IO.Path.Combine (Application.persistentDataPath, Define.DB_NAME_DOUBTSUEN_BK2 );
					if (System.IO.File.Exists (backup2DB)) {
						System.IO.File.Copy (backup2DB, backupDB, true);
					}
				}catch{
					//Debug.LogError ("まずー");
				}
			}
			break;

		default:
			break;
		}
	}

	public bool m_bStartSetting;
	public int m_iSettingItemId;
	public int m_iSettingItemSerial;
	public STATUS m_eMoveStatus;
	public int m_iMoveTab;
	public void SettingItem( int _iItemId , int _iItemSerial ){
		m_bStartSetting = true;
		m_iSettingItemId = _iItemId;
		m_iSettingItemSerial = _iItemSerial;
		return;
	}
	public int m_iCostNow;
	public int m_iCostMax;
	public int m_iCostNokori;

	public void HeaderRefresh( bool _bForce = false ){

		ParkRoot.ConnectingRoadCheck ();

		// 一時間あたりの売上
		int iUriagePerHour = 0;
		List<DataItem> item_list = GameMain.dbItem.Select (" item_serial != 0 ");
		foreach (DataItem item in item_list) {
			iUriagePerHour += item.GetUriagePerHour ();
		}
		GameMain.dbKvs.WriteInt (Define.USER_URIAGE_PAR_HOUR, iUriagePerHour);

		// 一時間あたりの支出
		int iShisyutsuHour = 0;
		foreach (DataItem item in item_list) {
			iShisyutsuHour += item.GetShiSyutsuPerHour ();
		}
		GameMain.dbKvs.WriteInt (Define.USER_SHISYUTU_PAR_HOUR, iShisyutsuHour);

		m_header.RefleshNum (_bForce);
	}

	static public bool GetGrid( GameObject _goRoot , Vector2 _inputPoint , out int _iX , out int _iY , Camera _camera ){
		_iX = 0;
		_iY = 0;

		if (GameMain.instance.TutorialInputLock == true ) {
			return false;
		}

		if (DebugRoot.Instance.IsDebugMode ()) {
			return false;
		}

		bool bRet = false;
		RaycastHit hit;

		//Debug.Log (_camera);
		Ray ray = _camera.ScreenPointToRay (new Vector3 (_inputPoint.x, _inputPoint.y, 0.0f));
		float fDistance = 100.0f;

		_iX = 0;
		_iY = 0;

		//レイを投射してオブジェクトを検出
		if (Physics.Raycast (ray, out hit, fDistance)) {

			Vector3 v3Dir = hit.point - _goRoot.transform.position;
			GameObject objPoint = new GameObject ();
			objPoint.transform.position = hit.point;
			objPoint.transform.parent = _goRoot.transform;

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

	static public bool GetGrid( GameObject _goRoot , Vector2 _inputPoint , out int _iX , out int _iY ){
		return GetGrid (_goRoot, _inputPoint, out _iX, out _iY, Instance.m_cameraUI );
	}
	static public bool GetGrid( Vector2 _inputPoint , out int _iX , out int _iY ){
		return GetGrid (ParkRoot.gameObject, _inputPoint, out _iX, out _iY );
	}

	static public bool GridHit( int _iX , int _iY , DataItem _dataItem , out int _iOffsetX , out int _iOffsetY ){
		_iOffsetX = 0;
		_iOffsetY = 0;

		//Debug.Log ("x:" + _dataItem.x.ToString () + " y:" + _dataItem.y.ToString () + " w:" + _dataItem.width.ToString () + " h:" + _dataItem.height.ToString ());

		bool bHit = false;
		for (int x = _dataItem.x; x < _dataItem.x + _dataItem.width; x++) {
			for (int y = _dataItem.y; y < _dataItem.y + _dataItem.height; y++) {
				if (_iX == x && _iY == y) {

					_iOffsetX = x - _dataItem.x;
					_iOffsetY = y - _dataItem.y;
					bHit = true;
					break;
				}
			}
		}
		return bHit;
	}

	static public bool GridHit( int _iX , int _iY , DataItem _dataItem ){

		int iOffsetX = 0;
		int iOffsetY = 0;

		return GridHit (_iX, _iY, _dataItem, out iOffsetX, out iOffsetY);
	}



}










