using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager : MonoBehaviour {


	static DataManager instance = null;

	public static DataManager Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("DataManager");
				if (obj == null) {
					obj = new GameObject("DataManager");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<DataManager> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<DataManager>() as DataManager;
				}
				instance.initialize ();
			}
			return instance;
		}
	}

	public void DummyCall(){
		return;
	}

	private void initialize(){
		Application.targetFrameRate = 60;
		QualitySettings.vSyncCount = 0;

		DontDestroyOnLoad(gameObject);

		if (PlayerPrefs.HasKey (Define.USER_WIDTH) == false) {
			PlayerPrefs.SetInt (Define.USER_WIDTH, Define.DEFUALT_USER_WIDTH);
			PlayerPrefs.SetInt (Define.USER_HEIGHT, Define.DEFUALT_USER_WIDTH);
		}

		int iWidth = PlayerPrefs.GetInt (Define.USER_WIDTH);
		int iHeight= PlayerPrefs.GetInt (Define.USER_HEIGHT);

		//m_tDataUser.Initialize (iWidth,iHeight);

		m_masterTableAudio.Load ();
		m_masterTablePrefab.Load ();
		m_masterTableSprite.Load ();

		m_csvItem.Load ();
		m_csvMonster.Load ();
		m_csvItemDetail.Load ();
		m_csvWork.Load ();
		m_csvStaff.Load ();
		m_csvLevel.Load ();
		m_csvTime.Load ();
		m_csvWord.Load ();
		m_csvTutorial.Load ();
		m_csvLocalNotification.Load ();
		foreach( CsvLocalNotificationData data in csv_localNotification ){
			LocalNotificationManager.Instance.Add (data);
		}
		return;
	}

	private DataUser m_tDataUser = new DataUser ();
	static public DataUser user{
		get{
			return Instance.m_tDataUser;
		}
		set {
			Instance.m_tDataUser = value;
		}
	}

	public List<DataItem> m_ItemDataList = new List<DataItem>();
	public List<DataItemMaster> m_ItemMasterList = new List<DataItemMaster>();
	static public List<DataItemMaster> itemMaster {
		get{ return Instance.m_ItemMasterList; }
		set{ Instance.m_ItemMasterList = value; }
	}
	static public DataItemMaster GetItemMaster( int _iItemId ){
		foreach( DataItemMaster data in itemMaster ){
			if (data.item_id == _iItemId) {
				return data;
			}
		}
		return new DataItemMaster ();
	}

	#region For CSV
	public MasterTableAudio m_masterTableAudio = new MasterTableAudio();
	static public List<MasterAudioCSV> master_audio_list {
		get{ 
			return Instance.m_masterTableAudio.All;
		}
	}
	public MasterTablePrefab m_masterTablePrefab = new MasterTablePrefab();
	static public List<MasterPrefabCSV> master_prefab_list {
		get{ 
			return Instance.m_masterTablePrefab.All;
		}
	}
	public MasterTableSprite m_masterTableSprite = new MasterTableSprite();
	static public List<MasterSpriteCSV> master_sprite_list {
		get{ 
			return Instance.m_masterTableSprite.All;
		}
	}
	public CsvItem m_csvItem = new CsvItem();
	static public List<CsvItemData> csv_item {
		get{ 
			return Instance.m_csvItem.All;
		}
	}
	static public CsvItemData GetItem( int _iItemId ){
		foreach (CsvItemData data in csv_item) {
			if (_iItemId == data.item_id) {
				return data;
			}
		}
		return new CsvItemData ();
	}
	public CsvMonster m_csvMonster = new CsvMonster();
	static public List<CsvMonsterData> csv_monster {
		get{ 
			return Instance.m_csvMonster.All;
		}
	}
	static public CsvMonsterData GetMonster( int _iMonsterId ){
		foreach (CsvMonsterData data in csv_monster) {
			if (_iMonsterId == data.monster_id) {
				return data;
			}
		}
		return new CsvMonsterData ();
	}
	public CsvItemDetal m_csvItemDetail = new CsvItemDetal();
	static public List<CsvItemDetailData> csv_item_detail {
		get{ 
			return Instance.m_csvItemDetail.All;
		}
	}
	static public CsvItemDetailData GetItemDetail( int _iItemId , int _iLevel ){
		foreach (CsvItemDetailData data in csv_item_detail) {
			if (data.item_id == _iItemId && data.level == _iLevel) {
				return data;
			}
		}
		return new CsvItemDetailData ();
	}

	public CsvWork m_csvWork = new CsvWork();
	static public List<CsvWorkData> csv_work {
		get{ 
			return Instance.m_csvWork.All;
		}
	}
	static public CsvWorkData GetWork( int _iWorkId ){
		foreach (CsvWorkData work in csv_work) {
			if (work.work_id == _iWorkId) {
				return work;
			}
		}
		Debug.LogError ("ignore staff_id:" + _iWorkId.ToString ());
		return new CsvWorkData ();
	}

	public CsvStaff m_csvStaff = new CsvStaff();
	static public List<CsvStaffData> csv_staff {
		get{ 
			return Instance.m_csvStaff.All;
		}
	}
	static public CsvStaffData GetStaff( int _iStaffId ){
		foreach (CsvStaffData staff in csv_staff) {
			if (staff.staff_id == _iStaffId) {
				return staff;
			}
		}
		Debug.LogError ("ignore staff_id:" + _iStaffId.ToString ());
		return new CsvStaffData ();
	}
	public CsvLevel m_csvLevel = new CsvLevel();
	static public List<CsvLevelData> csv_level {
		get{ 
			return Instance.m_csvLevel.All;
		}
	}
	public CsvTime m_csvTime = new CsvTime();
	static public List<CsvTimeData> csv_time {
		get{ 
			return Instance.m_csvTime.All;
		}
	}
	public CsvWord m_csvWord = new CsvWord();
	static public List<CsvWordData> csv_word {
		get{ 
			return Instance.m_csvWord.All;
		}
	}
	public string GetWord( string _strKey ){
		foreach (CsvWordData data in csv_word) {
			if (_strKey.Equals (data.key) == true) {
				return data.word;
			}
		}
		return "-------";
	}

	public CsvTutorial m_csvTutorial = new CsvTutorial();
	static public List<CsvTutorialData> csv_tutorial {
		get{ 
			return Instance.m_csvTutorial.All;
		}
	}

	public CsvLocalNotification m_csvLocalNotification = new CsvLocalNotification();
	static public List<CsvLocalNotificationData> csv_localNotification {
		get{ 
			return Instance.m_csvLocalNotification.All;
		}
	}

	#endregion

}



















