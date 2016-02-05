using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Define : MonoBehaviour {

	private static string m_strAppVersion = "0.0.0";			// ハードコード
	public static string APP_VERSION{
		get{ 
			return m_strAppVersion;
		}
	}

	public enum WHERE_PATTERN {
		NONE					= 0,
		SERIAL_EQUAL			,
		SERIAL_NOT_EQUAL		,
		RATE					,
		ITEM_STATUS_NOT_EQUAL	,



	}



	public enum ENVIROMENT
	{
		PRODUCTION			= 0,
		STREAMING_ASSETS	,
		LOCAL				,
		MAX					,
	}

	#if UNITY_ANDROID
	public const string ASSET_BUNDLE_PREFIX             = "android";
	public const string ASSET_BUNDLES_ROOT              = "AssetBundles/Android";
	public const string S3_SERVER_HEADER = "http://ad.xnosserver.com/apps/myzoo_data/bokunosuizokukan/Android/assets/assetbundleresource";
	#else
	public const string ASSET_BUNDLE_PREFIX             = "iphone";
	public const string ASSET_BUNDLES_ROOT              = "AssetBundles/iOS";
	public const string S3_SERVER_HEADER = "http://ad.xnosserver.com/apps/myzoo_data/bokunosuizokukan/iOS/assets/assetbundleresource";
	#endif

	//public const string S3_SERVER_HEADER = "https://s3-ap-northeast-1.amazonaws.com/every-studio/xnos/osakanamikke/assetbundleresource";

	public const string PLAYREPREFS_KEY_IS_ENC  = "playerprefs_is_enc";
	public const string PLAYREPREFS_KEY_DB_VER  = "playerprefs_db_ver";
	public const string READ_ERROR_STRING = "sql_datamanager_read_error";

	static public readonly string DB_PASSWORD						= "nCGuUssjthjK4HgYh94t";
	static public readonly string DB_FILE_DIRECTORY				= "databases/";
	static public readonly string DB_NAME_DOUBTSUEN				= "doubutsuen.db";
	static public readonly string DB_NAME_DOUBTSUEN_BK				= "doubutsuen.db.backup";
	static public readonly string DB_NAME_DOUBTSUEN_BK2				= "doubutsuen.db.backup2";

	static public readonly string DB_TABLE_ASYNC_KVS 				= "kvs_async";
	static public readonly string DB_TABLE_ASYNC_ITEM				= "item_async";
	static public readonly string DB_TABLE_ASYNC_ITEM_BK			= "item_async_bk";
	static public readonly string DB_TABLE_ASYNC_ITEM_MASTER		= "item_master_async";
	static public readonly string DB_TABLE_ASYNC_WORK				= "work_async";
	static public readonly string DB_TABLE_ASYNC_MONSTER			= "monster_async";
	static public readonly string DB_TABLE_ASYNC_MONSTER_MASTER		= "monster_master_async";
	static public readonly string DB_TABLE_ASYNC_STAFF				= "staff_async";

	static public readonly string USER_SYAKKIN						= "user_syakkin";
	static public readonly string USER_URIAGE_PAR_HOUR				= "user_uriage_par_hour";
	static public readonly string USER_SHISYUTU_PAR_HOUR			= "user_shisyutu_par_hour";
	static public readonly string USER_SYOJIKIN						= "user_syojikin";
	static public readonly string USER_TICKET						= "user_ticket";
	static public readonly string USER_LEVEL						= "user_level";
	static public readonly string USER_NEXT_EXP						= "user_next_exp";
	static public readonly string USER_TOTAL_EXP					= "user_total_exp";
	static public readonly string USER_COLLECT_COUNT				= "user_collect_count";
	static public readonly string USER_TWEET_COUNT					= "user_tweet_count";
	static public readonly string USER_LOGIN_COUNT					= "user_login_count";

	static public readonly string USER_WIDTH						= "user_width";
	static public readonly string USER_HEIGHT						= "user_height";

	static public readonly int DEFUALT_USER_WIDTH					= 20;
	static public readonly int DEFUALT_USER_HEIGHT					= 20;
	static public readonly int EXPAND_FIELD							=  5;

	static public readonly int USER_LEVEL_MAX						=100;

	static public readonly string KEY_TUTORIAL_PARENT_ID			= "key_tutorial_parent_id";


	public class Item
	{
		public enum Category
		{
			NONE	= 0,
			CAGE	,
			OFFICE	,
			SHOP	,
			EXPAND	,
			TICKET	,
			GOLD	,
			MAX		,
		};

		public enum Type{
			NONE		= 0,
			ROAD		,
			SHOP		,
			CAGE		,
			OFFICE		,
			SYMBOL		,
			COIN		= 97,
			EXPAND		= 98,
			TICKET		= 99,
			MAX			,

		};

		public enum Status{
			NONE	= 0,
			SETTING	,
			MAX		,
		};
	};

	public class Work
	{
		public enum STATUS {
			NONE		= 0,
			APPEAR		,
			CLEARD		,
			MAX			,
		}
	}

	public class Monster{
		public enum CONDITION{
			NONE		= 0,
			FINE		,
			SICK		,
			MAX			,
		}
	}
	public class Staff{
		public enum EFFECT_PARAM{
			NONE		= 0,
			MEAL		,
			CLEAN		,
			MEAL_CLEAN	,
			MAX			,
		}
	}

	public enum ROAD
	{
		NO_CHECK		= 0,
		DISCONNECT		,
		CONNECTION		,
		CONNECTION_SHOP	,		// SHOPだけじゃないけどね
		MAX				,
	}
	static public int ITEM_ID_ROAD = 15;

	static public string GetProductId( int _iItemId , ref int _iTicketNum ){
		string strRet = "";
		_iTicketNum = 0;
		switch (_iItemId) {
		case 30:
			_iTicketNum = 10;
			strRet = PurchasesManager.TICKET_010;
			break;
		case 31:
			_iTicketNum = 55;
			strRet = PurchasesManager.TICKET_055;
			break;
		case 32:
			_iTicketNum = 125;
			strRet = PurchasesManager.TICKET_125;
			break;
		case 33:
			_iTicketNum = 350;
			strRet = PurchasesManager.TICKET_350;
			break;
		case 34:
			_iTicketNum = 800;
			strRet = PurchasesManager.TICKET_800;
			break;
		default:
			break;
		}
		return strRet;
	}

	static public readonly Vector3 CELL_X_DIR = new Vector3 (80.0f, 40.0f, 0.0f);
	static public readonly Vector3 CELL_Y_DIR = new Vector3 (-80.0f, 40.0f, 0.0f);
	static private float cell_x_length = 0.0f;
	static private float cell_y_length = 0.0f;
	static public float CELL_X_LENGTH {
		get{
			if (cell_x_length == 0.0f) {
				cell_x_length = CELL_X_DIR.magnitude;
			}
			return cell_x_length;
		}
	}
	static public float CELL_Y_LENGTH {
		get{
			if (cell_y_length == 0.0f) {
				cell_y_length = CELL_Y_DIR.magnitude;
			}
			return cell_y_length;
		}
	}
	static public readonly float LONG_TAP_TIME = 0.5f;

	static public float GetValue( int _iNow , int _iMin , int _iMax ){

		if (_iMax < _iMin) {
			Debug.LogError (" set value if Error Pattern!" + "max:"+ _iMax.ToString() + " min:" + _iMin.ToString());
		}

		float fRet = (float)(_iNow - _iMin) / (float)(_iMax-_iMin);
		return fRet;
	}

	static public string GetWorkNewKey( int _iWorkId ){
		string strRet = string.Format ("work_new_{0}", _iWorkId);
		return strRet;
	}
}

public class Grid {
	public int x;
	public int y;

	public Grid( int _iX , int _iY ){
		x = _iX;
		y = _iY;
		return;
	}
	public Grid(){
		x = 0;
		y = 0;
	}

	static  public bool Equals( Grid _a , Grid _b ){
		return (_a.x == _b.x && _a.y == _b.y);
	}
	public bool Equals( int _x , int _y ){
		if (x == _x && y == _y) {
			return true;
		} else {
			return false;
		}
	}

	static private void setUsingGrid( ref List<Grid> _gridList , int _iX , int _iY , int _iWidth , int _iHeight ){
		for (int x = 0; x < _iWidth; x++) {
			for (int y = 0; y < _iHeight; y++) {
				Grid grid = new Grid ( _iX + x, _iY + y);
				_gridList.Add (grid);
			}
		}
		return;
	}

	static public void SetUsingGrid( ref List<Grid> _gridList , DataItem _dataItem ){
		setUsingGrid (ref _gridList, _dataItem.x, _dataItem.y, _dataItem.width, _dataItem.height);
		return;
	}

	static public void SetUsingGrid( ref List<Grid> _gridList , List<DataItem> _dataItemList ){
		_gridList.Clear ();
		foreach (DataItem data in _dataItemList) {
			List<Grid> grid_list = new List<Grid> ();
			SetUsingGrid (ref grid_list, data);
			foreach (Grid grid in grid_list) {
				_gridList.Add (grid);
			}
		}
		return;
	}

	static public bool AbleSettingItem( DataItemMaster _dataItem , int _iX , int _iY , List<Grid> _gridList ){
		bool bRet = true;

		List<Grid> useGrid = new List<Grid> ();
		setUsingGrid (ref useGrid, _iX, _iY, _dataItem.size, _dataItem.size);

		foreach (Grid check_grid in useGrid) {
			if (check_grid.x < 0) {
				bRet = false;
			} else if (check_grid.y < 0) {
				bRet = false;
			} else if (DataManager.user.m_iWidth <= check_grid.x) {
				bRet = false;
			} else if (DataManager.user.m_iHeight <= check_grid.y) {
				bRet = false;
			} else {
			}
		}


		foreach (Grid check_grid in useGrid) {
			foreach (Grid field_grid in _gridList) {

				if (Grid.Equals (check_grid, field_grid) == true) {
					//Debug.Log ("you cant setting here!");
					return false;
				}
			}
		}
		//Debug.Log ("able set GOOD POSITION");
		return bRet;
	}


}

public class SearchData {
	public GameMain.TABLE_TYPE m_eTableType;
	public string m_strWhere;

	Define.WHERE_PATTERN m_eWherePattern;
	List<int> int_list = new List<int> ();

	public BannerBase.MODE m_eBannerMode;

	public SearchData(){
	}
	public SearchData( GameMain.TABLE_TYPE _eTableType , Define.WHERE_PATTERN _eWherePattern , List<int> _int_list  , BannerBase.MODE _eBannerMode = BannerBase.MODE.NONE ){
		m_eTableType = _eTableType;
		m_eWherePattern = _eWherePattern;
		int_list = _int_list;
		m_strWhere = "";
		m_eBannerMode = _eBannerMode;
	}
	public SearchData( GameMain.TABLE_TYPE _eTableType , string _strWhere  , BannerBase.MODE _eBannerMode = BannerBase.MODE.NONE ){
		m_eTableType = _eTableType;
		m_strWhere = _strWhere;
		m_eBannerMode = _eBannerMode;
	}
}

public class Tab {

	public enum TYPE{
		WORK_OSUSUME	,
		WORK_BUILD		,
		WORK_MONSTER	,
		WORK_STAFF		,
		WORK_OTHER		,

		ITEM_GAUGE		,
		ITEM_OFFICE		,
		ITEM_SHOP		,
		ITEM_EXTEND		,
		ITEM_TICKET		,

		MAX				,
	}

	public TYPE m_eType;
	public string m_strMessageKey;
	public string m_strButtonHeader;
	public string m_strSwitchHeader;
	public string m_strSubPrefabName;
	public string m_strWordKey;

	public List<SearchData> m_SearchDataList = new List<SearchData>();

	public Tab( TYPE _eType , string _strMessageKey , string _strButtonHeader , string _strSwitchHeader , SearchData [] _searchDataList , string _strWordKey , string _strSubPrefabName = "" ){
		m_eType = _eType;
		m_strMessageKey = _strMessageKey;
		m_strButtonHeader = _strButtonHeader;
		m_strSwitchHeader = _strSwitchHeader;
		m_strWordKey = _strWordKey;

		foreach (SearchData data in _searchDataList) {
			m_SearchDataList.Add( data );
		}
		m_strSubPrefabName = _strSubPrefabName;
	}
}

public class UtilString{

	static public string GetSyuunyuu( int _iGold , int _iIntervalSec ){

		string strRet = _iGold.ToString() + "G/";

		if (60 <= _iIntervalSec) {
			int iMinute = _iIntervalSec / 60;
			strRet += iMinute.ToString () + "分";
		} else {
			strRet += _iIntervalSec.ToString () + "秒";
		}
		return strRet;

	}

}


