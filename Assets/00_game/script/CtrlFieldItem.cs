using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlFieldItem : MonoBehaviourEx {

	public enum STEP
	{
		NONE		= 0,
		INIT		,			// 主ににチェック
		READY		,			// 設置に時間がかかるやつ
		IDLE		,

		EDITTING	,

		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;
	public bool m_bEditting;

	public Define.ROAD m_eRoad;

	public List<CtrlIconRoot> m_iconRootList = new List<CtrlIconRoot> ();

	public DataItem m_dataItem = new DataItem();
	public DataItemMaster m_dataItemMaster;
	[SerializeField]
	private UISprite m_sprItem;

	public UISprite ItemSprite {
		get{ return m_sprItem; }
	}

	private int m_iNokoriTime;
	public int GetNokoriTime(){
		return m_iNokoriTime;
	}
	private float m_fCheckBuildTime;
	private float m_fCheckBuildInterval = 0.5f;
	private CtrlFieldItemBuildTime m_csBuildTime;

	public void SetColor( Color _color ){
		m_sprItem.color = _color;
	}

	public bool Clean(){
		bool bRet = false;
		foreach (CtrlIconRoot iconRoot in m_iconRootList) {
			if (iconRoot.Clean ()) {
				bRet = true;
			}
		}
		return bRet;
	}
	public bool Meal(){
		bool bRet = false;
		foreach (CtrlIconRoot iconRoot in m_iconRootList) {
			if (iconRoot.Meal ()) {
				bRet = true;
			}
		}
		return bRet;
	}

	public void Add( CtrlIconRoot _iconRoot ){

		_iconRoot.SetDepth (m_sprItem.depth);
		_iconRoot.m_iSize = m_dataItem.width;

		m_iconRootList.Add (_iconRoot);
		return;
	}

	public void Remove(){
		Debug.Log ("Remove:" + gameObject.name);
		if (0 < m_dataItem.item_serial) {
			List<DataMonster> monster_list = GameMain.dbMonster.Select (" item_serial = " + m_dataItem.item_serial.ToString ());
			foreach (DataMonster monster in monster_list) {
				GameMain.dbMonster.Update (monster.monster_serial, 0);
			}
		
			List<DataStaff> staff_list = GameMain.dbStaff.Select (" item_serial = " + m_dataItem.item_serial.ToString ());
			foreach (DataStaff staff in staff_list) {
				Dictionary< string , string > dict = new Dictionary< string , string > ();
				dict.Add ("office_serial", "0"); 
				dict.Add ("item_serial", "0"); 
				GameMain.dbStaff.Update (staff.staff_serial, dict);
			}
		}

		Destroy (gameObject);

	}

	public void RemoveMonster( int _iSerial ){
		int iIndex = 0;
		foreach (CtrlIconRoot iconRoot in m_iconRootList) {
			if (iconRoot.EqualMonsterSerial (_iSerial) == true) {
				iconRoot.Clean ();
				Destroy (iconRoot.gameObject);
				break;
			}
			iIndex += 1;
		}
		// リストからも除外
		Debug.Log (iIndex);
		m_iconRootList.RemoveAt (iIndex);

	}
	public void RemoveStaff( int _iSerial ){
		int iIndex = 0;
		foreach (CtrlIconRoot iconRoot in m_iconRootList) {
			if (iconRoot.EqualStaffSerial (_iSerial) == true) {
				Destroy (iconRoot.gameObject);
				break;
			}
			iIndex += 1;
		}
		// リストからも除外
		Debug.Log (iIndex);
		m_iconRootList.RemoveAt (iIndex);

	}


	private void change_sprite( UISprite _spr , string _strName ){
		_spr.atlas = AtlasManager.Instance.GetAtlas (_strName);
		_spr.spriteName = _strName;

		UISpriteData spriteData = _spr.GetAtlasSprite ();

		_spr.width = spriteData.width;
		_spr.height = spriteData.height;
	}

	private void change_sprite( UISprite _spr , int _iItemId ){
		string strName = "item" + _iItemId.ToString ();

		change_sprite (_spr, strName);

		return;
	}

	public void Init( int _iX , int _iY , int _iItemId , ParkMain.EDIT_MODE _eEditMode = ParkMain.EDIT_MODE.NORMAL){

		m_eStep = STEP.INIT;
		m_eStepPre = STEP.MAX;


		if (_eEditMode == ParkMain.EDIT_MODE.MOVE) {
		} else {
			m_iconRootList.Clear ();
		}

		m_dataItem.x = _iX;
		m_dataItem.y = _iY;

		change_sprite (m_sprItem, _iItemId);
		DataItemMaster master = GameMain.dbItemMaster.Select (_iItemId);

		m_dataItem.width = master.size;
		m_dataItem.height= master.size;
		m_dataItem.item_id = _iItemId;
		m_dataItem.category = master.category;

		m_dataItemMaster = master;

		SetPos (_iX, _iY);

		return;
	}

	public void EditEnd( int _iSerial){
		m_bEditting = false;
		m_eStep = STEP.INIT;

		m_dataItem = GameMain.dbItem.Select (_iSerial);

		Debug.LogError (string.Format ("serial:{0} x:{1} y:{2}", _iSerial, m_dataItem.x, m_dataItem.y));
		SetPos (m_dataItem.x, m_dataItem.y);
		CheckAroundConnectRoad ();

		return;
	}

	public void InitEdit( int _iX , int _iY , int _iItemId  , ParkMain.EDIT_MODE _eEditMode ){
		m_bEditting = true;
		Init (_iX, _iY, _iItemId , _eEditMode );
		m_eStep = STEP.EDITTING;
		return;
	}


	public void Init( DataItem _cell ){
		Init (_cell.x, _cell.y, _cell.item_id);

		// マスター経由の方で書き込まれてるのもあるけどこっちで再上書き
		m_dataItem = _cell;
	}

	// 自分おお店の周りに接続された道路があるかチェック＆セット
	public void CheckAroundConnectRoad(){
		//Debug.Log ("CheckAroundConnectRoad start");
		CtrlFieldItem fieldItem = null;
		for (int x = m_dataItem.x; x < m_dataItem.x + m_dataItem.width; x++) {
			int iYMin = m_dataItem.y - 1;
			int iYMax = m_dataItem.y + m_dataItem.height;

			fieldItem = GameMain.ParkRoot.GetFieldItem (x, iYMin);

			//Debug.Log (string.Format ("(x,y)=({0},{1})", x, iYMin));
			if ( fieldItem != null && fieldItem.IsConnectingRoad ()) {
				m_eRoad = Define.ROAD.CONNECTION_SHOP;
				return;
			}
			fieldItem = GameMain.ParkRoot.GetFieldItem (x, iYMax);
			//Debug.Log (string.Format ("(x,y)=({0},{1})", x, iYMax));
			if ( fieldItem != null && fieldItem.IsConnectingRoad ()) {
				m_eRoad = Define.ROAD.CONNECTION_SHOP;
				return;
			}
		}
		for (int y = m_dataItem.y; y < m_dataItem.y + m_dataItem.height; y++) {
			int iXMin = m_dataItem.x - 1;
			int iXMax = m_dataItem.x + m_dataItem.width;

			fieldItem = GameMain.ParkRoot.GetFieldItem (iXMin, y);
			//Debug.Log (string.Format ("(x,y)=({0},{1})", iXMin, y));
			if ( fieldItem != null && fieldItem.IsConnectingRoad ()) {
				m_eRoad = Define.ROAD.CONNECTION_SHOP;
				return;
			}
			fieldItem = GameMain.ParkRoot.GetFieldItem (iXMax, y);
			//Debug.Log (string.Format ("(x,y)=({0},{1})", iXMax, y));
			if (fieldItem != null && fieldItem.IsConnectingRoad ()) {
				m_eRoad = Define.ROAD.CONNECTION_SHOP;
				return;
			}
		}
		//Debug.Log ("CheckAroundConnectRoad end nohit");
		return;
	}

	protected bool IsConnectingRoad(){

		if (m_dataItem.item_id == Define.ITEM_ID_ROAD && m_eRoad == Define.ROAD.CONNECTION ) {
			return true;
		}
		return false;


	}

	public void ResetPos(){

		//Debug.Log ("x=" + m_dataItem.x.ToString () + " y=" + m_dataItem.y.ToString ());
		SetPos (m_dataItem.x, m_dataItem.y);
	}

	public void SetPos( int _iX , int _iY ){
		myTransform.localPosition = (Define.CELL_X_DIR.normalized * Define.CELL_X_LENGTH * _iX) + (Define.CELL_Y_DIR.normalized * Define.CELL_Y_LENGTH * _iY);

		int iDepth = 100 - (_iX + _iY) - (m_dataItem.width-1);// + (m_dataItem.height-1));

		if (m_bEditting) {
			iDepth += 10;		// こんだけ上なら
		}

		//Debug.LogError (iDepth);

		m_sprItem.depth = iDepth;
		return;
	}

	public void SetEditAble( bool _bFlag ){

		if (_bFlag) {
			TweenColorAll (gameObject, 0.025f, Color.green);
			TweenAlphaAll (gameObject, 0.025f, 0.75f);
		} else {
			TweenColorAll (gameObject, 0.025f, Color.red);
			TweenAlphaAll (gameObject, 0.025f, 0.75f);
		}


	}

	public string GetReadyItemName( int _iSize ){
		return "item_processing" + _iSize.ToString ();
	}

	public bool IsReady(){
		if (m_eStep == STEP.READY) {
			return true;
		}
		return false;
	}

	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}
		switch (m_eStep) {
		case STEP.INIT:


			m_eStep = STEP.READY;
			if (m_dataItemMaster.category == (int)Define.Item.Category.NONE) {
				m_eStep = STEP.IDLE;
			} else {
				double diff = TimeManager.Instance.GetDiffNow (m_dataItem.create_time).TotalSeconds;
				//Debug.Log ( m_dataItem.item_id.ToString() + ":" + m_dataItem.item_serial.ToString() + ":" + diff.ToString ());

				// 絶対かこなので　
				diff *= -1;

				if (m_dataItemMaster.production_time < diff) {
					m_eStep = STEP.IDLE;
				} else if (0 == m_dataItemMaster.production_time) {
					m_eStep = STEP.IDLE;
				} else {
				}
			}
			break;

		case STEP.READY:
			if (bInit) {

				m_fCheckBuildTime = 0.0f;
				m_fCheckBuildInterval = 0.5f;
				change_sprite (m_sprItem, GetReadyItemName (m_dataItemMaster.size));

				double diff = TimeManager.Instance.GetDiffNow (m_dataItem.create_time).TotalSeconds;
				diff *= -1;
				m_iNokoriTime = m_dataItemMaster.production_time - (int)diff;

				GameObject obj = PrefabManager.Instance.MakeObject ("Prefab/PrefFieldItemBuildTime", gameObject);
				obj.transform.localPosition = new Vector3 (0.0f, 40.0f * m_dataItemMaster.size, 0.0f);
				m_csBuildTime = obj.GetComponent<CtrlFieldItemBuildTime> ();
				m_csBuildTime.Init (m_iNokoriTime, m_sprItem.depth, m_dataItemMaster.item_id);
				m_csBuildTime.TriggerClear ();
			}
			m_fCheckBuildTime += Time.deltaTime;
			if (m_fCheckBuildInterval < m_fCheckBuildTime) {
				m_fCheckBuildTime -= m_fCheckBuildInterval;

				// データ撮り直し
				m_dataItem = GameMain.dbItem.Select (m_dataItem.item_serial);

				double diff = TimeManager.Instance.GetDiffNow (m_dataItem.create_time).TotalSeconds;
				diff *= -1;
				m_iNokoriTime = m_dataItemMaster.production_time - (int)diff;

				if (0 < m_iNokoriTime) {
					m_csBuildTime.SetNokoriSec (m_iNokoriTime);
				} else {
					Destroy (m_csBuildTime.gameObject);
					m_csBuildTime = null;
					m_eStep = STEP.IDLE;
					DataWork.WorkCheck ();
				}
			}
			if (m_csBuildTime) {
				if (m_csBuildTime.ButtonPushed) {
					m_csBuildTime.TriggerClear ();
					GameMain.Instance.BuildingSerial = m_dataItem.item_serial;
				}
			}
			break;

		case STEP.IDLE:
			if (bInit) {
				change_sprite (m_sprItem, m_dataItem.item_id);
				m_fCheckBuildTime = 0.0f;
			}

			if (0 < m_iconRootList.Count) {
				m_fCheckBuildTime += Time.deltaTime;
				if (1.0f < m_fCheckBuildTime) {
					m_fCheckBuildTime -= 1.0f;

					int iIndex = 0;

					List<UISprite> sprList = new List<UISprite> ();
					foreach (CtrlIconRoot icon in m_iconRootList) {
						sprList.Add (icon.m_sprIcon);
					}
					sprList.Sort (CompareByID);

					int iLoop = 2;
					foreach (UISprite sprite in sprList) {
						sprite.depth = m_sprItem.depth + iLoop;
						iLoop += 1;
					}
				}
			}
			break;

		case STEP.EDITTING:
			if (bInit) {
				change_sprite (m_sprItem, m_dataItem.item_id);
			}
			break;
		case STEP.MAX:
		default:
			break;
		}
	}

	private static int CompareByID (UISprite a, UISprite b)
	{
		if (a.transform.position.y > b.transform.position.y) {
			return -1;
		}

		if (a.transform.position.y < b.transform.position.y) {
			return 1;
		}

		return 0;
	}

}





























