using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using Prime31;

public class InitialMain : MonoBehaviour {

	public enum STEP
	{
		NONE				= 0,
		DATAMANAGER_SETUP	,
		SOUND_LOAD			,
		REVIEW				,

		IDLE				,
		DB_SETUP			,
		INPUT_WAIT			,

		DB_BACKUP_NOEXIST	,
		DB_BACKUP_CHECK		,
		DB_BACKUP			,
		END					,
		MAX					,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	public GameObject m_goRoot;
	public GameObject m_goStartButton;
	public ButtonBase m_btnStart;
	public ButtonBase m_btnBackup;
	public CtrlOjisanCheck m_ojisanCheck;
	public UITexture m_texBack;
	public UtilSwitchSprite m_SwitchSpriteBack;

	public ButtonBase m_btnTutorialReset;
	public ButtonBase m_btnCacheClear;

	public CtrlReviewWindow m_reviewWindow;

	public CtrlLoading m_csLoading;
	[SerializeField]
	private GameObject m_posDisplay;
	#region DB関係
	DBKvs m_dbKvs;
	DBItem m_dbItem;
	DBItemMaster m_dbItemMaster;
	DBWork m_dbWork;
	DBMonster m_dbMonster;
	DBMonsterMaster m_dbMonsterMaster;
	ThreadQueue.TaskControl m_tkKvsOpen;
	#endregion



	// Use this for initialization
	void Start () {
	
		Application.targetFrameRate = 60;
		QualitySettings.vSyncCount = 0;

		m_eStep = STEP.IDLE;
		m_eStep = STEP.DATAMANAGER_SETUP;
		m_eStepPre = STEP.MAX;

		//m_SwitchSpriteBack.SetSprite ("garalley_003");
		m_SwitchSpriteBack.SetSprite ("bg001");
		//m_SwitchSpriteBack.SetSprite ("tutorial777");

		SoundManager.Instance.PlayBGM ("bgm_opening");

		#if UNITY_ANDROID
		GoogleIAB.enableLogging (true);
		string key = "your public key from the Android developer portal here";
		key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsqFXrg2t62dru/VFQYyxd2m1kORBbrAGxDxiSAkh3ybaXJtJWNcej/YAxKx7Orrtfq+pU965U2FnU3K54xddts2UGCI9O6TSU0AoKbwFYj+okfF21firsEqZd4aYtVYQ471flWj3ZEG9u2YpIzjGykUQadsxO4Y/OcRbdUn9289Mc0JAbdepmN9yRnvgBJWKZF/c0mBrM4ISfF5TVip2Tp+BXACqblOb+TQZjOB0OeVPxYpdy5k3eJTcQuwiLmYxgpEBL3tIT7grxVROgk8YYncncaZR7Q/wWlsFgFTNMRaF2bPI8apLiA7eIyKv5zbmhbE7YLBXUvkuoHbAqDQrLQIDAQAB";
		Debug.Log( key );
		//下はテスト用
		//key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArGLKSb92Imt43S40ArCXfTmQ31c+pFQTM0Dza3j/Tn4cqjwccjQ/jej68GgVyGXGC2gT/EtbcVVA+bHugXmyv73lGBgmQlzBL41WYTKolO8Z6pVWTeHBtsT7RcHKukoKiONZ7NiQ9P5t6CCPBB2sXQOp1y3ryVbv01xXlM+hB6HkkKxrT6lIjTbtiVXCHAJvqPexPbqVIfGjBaXH/oHKxEBxYDaa6PTUsU3OP3MTx63ycTEnEMsQlA1W6ZuTFIa5Jd3cVlfQI7uovEzAbIlUfwcnxVOUWASiYe81eQiD1BMl+JeCRhfd5e8D4n0LOA12rHm1F3fC9ZoIEjpNB+BRhwIDAQAB";
		GoogleIAB.init( key );
		#endif


	}
	
	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {

		case STEP.DATAMANAGER_SETUP:
			if (bInit) {
				/*
				GameObject pref = PrefabManager.Instance.PrefabLoadInstance ("test");
				paramtest script = pref.GetComponent<paramtest> ();
				Debug.Log (script.list.Count);
				script.list.Add (new DataItem ());
				*/
			}


			if (SpriteManager.Instance.IsIdle ()) {
				m_goRoot.SetActive (true);
				m_btnStart.gameObject.SetActive (false);
				m_eStep = STEP.SOUND_LOAD;
			}
			if (m_csLoading != null) {
				m_csLoading.ViewPercent (0.0f);
			}
			break;
		case STEP.SOUND_LOAD:
			if (bInit) {
				foreach (MasterAudioCSV data in DataManager.master_audio_list) {
					if (data.audio_type != 1) {
						SoundManager.Instance.AddLoadFile (data.filename);
					}
				}
			}
			if (m_csLoading != null) {
				m_csLoading.ViewPercent ( 0.0f );
			}
			if (SoundManager.Instance.IsIdle ()) {
				m_btnStart.gameObject.SetActive (true);
				m_eStep = STEP.IDLE;

				if (ReviewManager.Instance.IsReadyReview ()) {
					m_eStep = STEP.REVIEW;
				}
			}
			break;

		case STEP.REVIEW:
			if (bInit) {
				GameObject obj = PrefabManager.Instance.MakeObject ("prefab/CtrlReviewWindow", m_goRoot.transform.parent.gameObject);
				m_reviewWindow = obj.GetComponent<CtrlReviewWindow> ();
				m_reviewWindow.Initialize ();

				m_goRoot.SetActive (false);

			}
			if (m_reviewWindow.IsEnd ()) {
				m_goRoot.SetActive (true);
				Destroy (m_reviewWindow.gameObject);;
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.IDLE:
			if (bInit) {
				m_btnStart.TriggerClear ();
				m_btnBackup.TriggerClear ();
			}
			if (m_btnStart.ButtonPushed) {
				m_eStep = STEP.DB_SETUP;
				SoundManager.Instance.PlaySE ("se_cleanup");
			} else if (m_btnBackup.ButtonPushed) {

				string backupDB = System.IO.Path.Combine (Application.persistentDataPath, Define.DB_NAME_DOUBTSUEN_BK );
				if (System.IO.File.Exists (backupDB) == false ) {
					m_eStep = STEP.DB_BACKUP_NOEXIST;
				} else {
					m_eStep = STEP.DB_BACKUP_CHECK;
				}
			} else {
			}

			break;

		case STEP.DB_SETUP:
			if (bInit) {

				m_dbItem = new DBItem (Define.DB_TABLE_ASYNC_ITEM);
				m_dbItemMaster = new DBItemMaster (Define.DB_TABLE_ASYNC_ITEM_MASTER);
				m_dbWork = new DBWork (Define.DB_TABLE_ASYNC_WORK);
				m_dbMonster = new DBMonster (Define.DB_TABLE_ASYNC_MONSTER);
				m_dbMonsterMaster = new DBMonsterMaster (Define.DB_TABLE_ASYNC_MONSTER_MASTER);
				m_dbKvs = new DBKvs (Define.DB_TABLE_ASYNC_KVS);
				/*
				m_dbItem.Open (Define.DB_NAME_DOUBTSUEN, Define.DB_FILE_DIRECTORY, "");
				m_dbItemMaster.Open (Define.DB_NAME_DOUBTSUEN, Define.DB_FILE_DIRECTORY, "");
				m_dbWork.Open (Define.DB_NAME_DOUBTSUEN, Define.DB_FILE_DIRECTORY, "");
				m_dbMonster.Open (Define.DB_NAME_DOUBTSUEN, Define.DB_FILE_DIRECTORY, "");
				m_dbMonsterMaster.Open (Define.DB_NAME_DOUBTSUEN, Define.DB_FILE_DIRECTORY, "");
				m_tkKvsOpen = m_dbKvs.Open (Define.DB_NAME_DOUBTSUEN, Define.DB_FILE_DIRECTORY, ""); // DEFINE.DB_PASSWORD
				*/
			}
			if (true) {
				//if (m_tkKvsOpen.Completed) {

				if (m_dbKvs.ReadInt (Define.USER_SYOJIKIN) == 0) {
					m_dbKvs.WriteInt (Define.USER_SYOJIKIN, 1000);
				}

				List<DataItem> data_item_list =  m_dbItem.SelectAll ();
				// 最初しか通らない
				if (data_item_list.Count == 0) {
					Debug.LogError ("here");
					m_dbKvs.WriteInt (Define.USER_SYAKKIN,300000000);
					var skitMasterTable = new MasterTableMapChip ();
					skitMasterTable.Load ();
					var csvItem = new CsvItem ();
					csvItem.Load ();
					foreach (MapChipCSV csvMapChip in skitMasterTable.All) {
						DataItem data = new DataItem (csvMapChip , csvItem );
						m_dbItem.Replace (data);

					}
				}

				List<DataWork> data_work_list = m_dbWork.SelectAll ();
				if (data_work_list.Count == 0) {
					var csvWork = new CsvWork ();
					csvWork.Load ();
					foreach (CsvWorkData csv_work_data in csvWork.All) {
						DataWork data = new DataWork (csv_work_data);
						// 最初に出現していいのはappear_work_id== 0とlevel<=1のものだけ
						if (data.appear_work_id == 0 && data.level <= 1 ) {
							data.status = 1;
						}
						m_dbWork.Replace (data);
					}
				}

				List<DataMonster> data_monster_list = m_dbMonster.SelectAll ();
				//Debug.LogError( string.Format( "data_monster_list.Count:{0}" , data_monster_list.Count ));
				if (data_monster_list.Count == 0) {
					DataMonster monster = new DataMonster ();
					monster.monster_serial = 1;
					monster.monster_id = 1;
					monster.item_serial = 12;
					monster.condition = (int)Define.Monster.CONDITION.FINE;
					monster.collect_time = TimeManager.StrNow ();

					string strHungry = TimeManager.StrGetTime (-1 * 60 * 30);
					monster.meal_time = strHungry;
					monster.clean_time = strHungry;
					m_dbMonster.Replace (monster);
				}

				List<DataMonsterMaster> data_monster_master_list = m_dbMonsterMaster.SelectAll ();
				if (data_monster_master_list.Count == 0) {
					var csvMonsterMaster = new CsvMonster ();
					csvMonsterMaster.Load ();
					foreach (CsvMonsterData csv_monster_master_data in csvMonsterMaster.All) {
						m_dbMonsterMaster.Replace (csv_monster_master_data);
					}
				}

				List<DataItemMaster> data_item_master = m_dbItemMaster.SelectAll ();
				//Debug.LogError (string.Format ("count:{0}", data_item_master.Count));
				if (data_item_master.Count == 0) {
					var csvItem = new CsvItem ();
					csvItem.Load ();
					foreach (CsvItemData csv_item_data in csvItem.All) {
						DataItemMaster data = new DataItemMaster (csv_item_data);
						if (data.open_item_id == 0) {
							data.status = 1;
						}
						m_dbItemMaster.Replace (data);
					}
				}
				m_eStep = STEP.INPUT_WAIT;
			} else {
				//Debug.Log ("wait");
			}

			break;

		case STEP.INPUT_WAIT:
			if (bInit) {
				m_btnStart.TriggerClear ();
			}
			if (true) {

				// とりあえず全部調べる
				List<DataWork> cleared_work_list = m_dbWork.Select ( string.Format(" status = {0} " , (int)Define.Work.STATUS.CLEARD ));
				foreach (DataWork work in cleared_work_list) {
					List<DataMonsterMaster> list_monster = m_dbMonsterMaster.Select ( string.Format(" status = 0 and open_work_id = {0} " , work.work_id ));
					foreach (DataMonsterMaster data_monster_master in list_monster) {
						Dictionary< string , string > monster_master_dict = new Dictionary< string , string > ();
						monster_master_dict.Add ("status", "1");
						m_dbMonsterMaster.Update (data_monster_master.monster_id , monster_master_dict);
					}

				}

				m_btnStart.TriggerClear ();
				Application.LoadLevel ("park_main");
			}
			break;

		case STEP.DB_BACKUP_NOEXIST:
			if (bInit) {
				GameObject objOjisan = PrefabManager.Instance.MakeObject ("prefab/PrefOjisanCheck", m_posDisplay);
				m_ojisanCheck = objOjisan.GetComponent<CtrlOjisanCheck> ();
				m_ojisanCheck.Initialize ("バックアップファイルが\n存在しません", true);
			}
			if (m_ojisanCheck.IsYes ()) {
				Destroy (m_ojisanCheck.gameObject);
				m_eStep = STEP.IDLE;
			}
			break;
		case STEP.DB_BACKUP_CHECK:
			if (bInit) {
				GameObject objOjisan = PrefabManager.Instance.MakeObject ("prefab/PrefOjisanCheck", m_posDisplay );
				m_ojisanCheck = objOjisan.GetComponent<CtrlOjisanCheck> ();
				m_ojisanCheck.Initialize ("自動保存されたデータ\nを利用して\nバックアップを行います\n\nよろしいですか");
			}
			if (m_ojisanCheck.IsYes ()) {
				//SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);
				Destroy (m_ojisanCheck.gameObject);
				m_eStep = STEP.DB_BACKUP;
			} else if (m_ojisanCheck.IsNo ()) {
				//SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);
				Destroy (m_ojisanCheck.gameObject);
				m_eStep = STEP.IDLE;
			} else {
			}
			break;
		case STEP.DB_BACKUP:
			if (bInit) {
				GameObject objOjisan = PrefabManager.Instance.MakeObject ("prefab/PrefOjisanCheck", m_posDisplay);
				m_ojisanCheck = objOjisan.GetComponent<CtrlOjisanCheck> ();
				m_ojisanCheck.Initialize ("完了しました\nゲームをスタートしてください", true);

				string sourceDB = System.IO.Path.Combine (Application.persistentDataPath, Define.DB_NAME_DOUBTSUEN );
				string dummyDB = System.IO.Path.Combine (Application.persistentDataPath, Define.DB_NAME_DOUBTSUEN + "." + TimeManager.StrGetTime() );
				string backupDB = System.IO.Path.Combine (Application.persistentDataPath, Define.DB_NAME_DOUBTSUEN_BK );
				string backup2DB = System.IO.Path.Combine (Application.persistentDataPath, Define.DB_NAME_DOUBTSUEN_BK2 );
				if (System.IO.File.Exists (sourceDB)) {
					System.IO.File.Copy (sourceDB, dummyDB, true);
				}
				if (System.IO.File.Exists (backupDB)) {
					System.IO.File.Copy (backupDB, sourceDB, true);
				}
			}
			if (m_ojisanCheck.IsYes ()) {
				Destroy (m_ojisanCheck.gameObject);
				m_eStep = STEP.IDLE;
			}
			break;

		default:
			break;
		}

		if (m_btnTutorialReset.ButtonPushed) {
			PlayerPrefs.DeleteAll ();

			string full_path = System.IO.Path.Combine (Application.persistentDataPath , Define.DB_NAME_DOUBTSUEN );
			System.IO.File.Delete( full_path );

			m_btnTutorialReset.TriggerClear ();
		}
		if (m_btnCacheClear.ButtonPushed) {
			Caching.CleanCache();
			m_btnCacheClear.TriggerClear ();
		}
	
	}
}

























