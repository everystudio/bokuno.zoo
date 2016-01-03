using UnityEngine;
using System.Collections;

public class DataSave : MonoBehaviour {

	protected static DataSave instance = null;
	public static DataSave Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("DataSave");
				if (obj == null) {
					obj = new GameObject("DataSave");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<DataSave> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<DataSave>() as DataSave;
				}
				instance.Initialize ();
			}
			return instance;
		}
	}

	public float m_fInterval;
	public const float INTERVAL = 10.0f;
	#region DB関係
	DBKvs m_dbKvs;
	DBItem m_dbItem;
	DBItemMaster m_dbItemMaster;
	DBWork m_dbWork;
	DBMonster m_dbMonster;
	DBMonsterMaster m_dbMonsterMaster;
    DBStaff m_dbStaff;
	#endregion

	public bool m_bInitialized = false;
	private void Initialize(){
		if (m_bInitialized == false) {
			m_bInitialized = true;
			DontDestroyOnLoad (gameObject);
			m_dbItem = new DBItem (Define.DB_TABLE_ASYNC_ITEM);
			m_dbItemMaster = new DBItemMaster (Define.DB_TABLE_ASYNC_ITEM_MASTER);
			m_dbWork = new DBWork (Define.DB_TABLE_ASYNC_WORK);
			m_dbMonster = new DBMonster (Define.DB_TABLE_ASYNC_MONSTER);
			m_dbMonsterMaster = new DBMonsterMaster (Define.DB_TABLE_ASYNC_MONSTER_MASTER);
			m_dbKvs = new DBKvs (Define.DB_TABLE_ASYNC_KVS);
            m_dbStaff = new DBStaff(Define.DB_TABLE_ASYNC_STAFF);
            m_fInterval = 0.0f;
		}
	}

	public void save(){
		m_dbItem.m_soDataItem.Save ();
		m_dbItemMaster.m_soDataItemMaster.Save ();
		m_dbWork.m_soDataWork.Save ();
		m_dbMonster.m_soDataMonster.Save ();
		m_dbMonsterMaster.m_soDataMonsterMaster.Save ();
		m_dbKvs.m_soDataKvs.Save ();
        m_dbStaff.m_soDataStaff.Save();

    }

	// 個人的には邪道なんだけど
	void Start(){
		Debug.LogError ("start");
		Initialize ();
		m_dbItem.m_soDataItem.Load (DBItem.FILE_NAME);
		m_dbItemMaster.m_soDataItemMaster.Load (DBItemMaster.FILE_NAME);
		m_dbWork.m_soDataWork.Load (DBWork.FILE_NAME);
		m_dbMonster.m_soDataMonster.Load (DBMonster.FILE_NAME);
		m_dbMonsterMaster.m_soDataMonsterMaster.Load (DBMonsterMaster.FILE_NAME);
		m_dbKvs.m_soDataKvs.Load (DBKvs.FILE_NAME);
        m_dbStaff.m_soDataStaff.Load(DBStaff.FILE_NAME);
	}

	void OnApplicationPause(bool pauseStatus) {
		///Debug.LogError ("here");
		Initialize ();
		if (pauseStatus) {
			save ();

		} else {
		}
	}
	#if UNITY_EDITOR
	void Update(){

		m_fInterval += Time.deltaTime;

		if (INTERVAL < m_fInterval) {
			m_fInterval -= INTERVAL;
			save ();
		}

	}
	#endif

}












