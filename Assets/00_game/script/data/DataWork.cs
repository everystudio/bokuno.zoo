using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DataWork : SearchBase{

	public int m_work_id;
	public int m_status;
	public string m_title;
	public string m_description;
	public int m_type;
	public int m_level;
	public int m_appear_work_id;
	public int m_exp;
	public string m_difficulty;
	public int m_prize_ticket;
	public int m_prize_coin;
	public int m_prize_monster;

	public int m_mission_level;
	public int m_mission_monster;
	public int m_mission_staff;
	public int m_mission_item;
	public int m_mission_collect;
	public int m_mission_tweet;
	public int m_mission_login;
	public int m_mission_num;

	public int work_id{ get{ return m_work_id;} set{m_work_id = value; } }
	public int status{ get{ return m_status;} set{m_status = value; } }
	public string title{ get{ return m_title;} set{m_title = value; } }
	public string description{ get{ return m_description;} set{m_description = value; } }
	public int type{ get{ return m_type;} set{m_type = value; } }
	public int level{ get{ return m_level;} set{m_level = value; } }
	public int appear_work_id{ get{ return m_appear_work_id;} set{m_appear_work_id = value; } }
	public int exp{ get{ return m_exp;} set{m_exp = value; } }
	public string difficulty{ get{ return m_difficulty;} set{m_difficulty = value; } }
	public int prize_ticket{ get{ return m_prize_ticket;} set{m_prize_ticket = value; } }
	public int prize_coin{ get{ return m_prize_coin;} set{m_prize_coin = value; } }
	public int prize_monster{ get{ return m_prize_monster;} set{m_prize_monster = value; } }

	public int mission_level{ get{ return m_mission_level;} set{m_mission_level = value; } }
	public int mission_monster{ get{ return m_mission_monster;} set{m_mission_monster = value; } }
	public int mission_staff{ get{ return m_mission_staff;} set{m_mission_staff = value; } }
	public int mission_item{ get{ return m_mission_item;} set{m_mission_item = value; } }
	public int mission_collect{ get{ return m_mission_collect;} set{m_mission_collect = value; } }
	public int mission_tweet{ get{ return m_mission_tweet;} set{m_mission_tweet = value; } }
	public int mission_login{ get{ return m_mission_login;} set{m_mission_login = value; } }
	public int mission_num{ get{ return m_mission_num;} set{m_mission_num = value; } }

	public DataWork(){
	}
	public DataWork( CsvWorkData _work ){
		work_id = _work.work_id;
		status = 0;
		title = _work.title;
		description = _work.description;
		type = _work.type;
		level = _work.level;
		appear_work_id = _work.appear_work_id;
		exp = _work.exp;
		difficulty = _work.difficulty;
		prize_ticket = _work.prize_ticket;
		prize_coin = _work.prize_coin;
		prize_monster = _work.prize_monster;
		mission_level = _work.mission_level;
		mission_monster = _work.mission_monster;
		mission_staff= _work.mission_staff;
		mission_item= _work.mission_item;
		mission_collect= _work.mission_collect;
		mission_tweet= _work.mission_tweet;
		mission_login= _work.mission_login;
		mission_num= _work.mission_num;
	}

	static public void WorkCheck(){

		if (TutorialManager.Instance.IsTutorial ()) {
			;// チュートリアル中はチェックしない
		} else {
			List<DataWork> check_work_list = GameMain.dbWork.Select (string.Format (" status = {0} ", (int)Define.Work.STATUS.APPEAR));
			foreach (DataWork work in check_work_list) {
				if (work.ClearCheck ()) {
					work.MissionClear ();
				}
			}
			DataWork.WorkOpen ();
		}
	}

	static public void WorkOpen(){

		Dictionary< string , string > dict_appear = new Dictionary< string , string > ();
		dict_appear.Add( "status" , ((int)Define.Work.STATUS.APPEAR).ToString() ); 

		List<DataWork> list_work = GameMain.dbWork.Select ( string.Format( " status = {0} and appear_work_id = 0 " , (int)Define.Work.STATUS.NONE ));
		foreach (DataWork appear_work in list_work) {
			if (appear_work.level <= GameMain.dbKvs.ReadInt (Define.USER_LEVEL)) {
				GameMain.dbWork.Update ( appear_work.work_id , dict_appear );

				// NEWの表示を出す
				PlayerPrefs.SetInt (Define.GetWorkNewKey (appear_work.work_id), 1);
			}
		}
		return;
	}

	// ミッションをクリア状態にして、他のミッションが開放されないか確認する
	public void MissionClear(){

		//Debug.LogError (string.Format( "mission clear work_id:{0}", work_id));
		//string strNow = TimeManager.StrNow ();
		Dictionary< string , string > dict = new Dictionary< string , string > ();
		dict.Add( "status" , ((int)Define.Work.STATUS.CLEARD).ToString() ); 
		GameMain.dbWork.Update ( work_id , dict );

		// ふういんされているモンスターを解き放つ
		List<DataMonsterMaster> list_monster = GameMain.dbMonsterMaster.Select ( string.Format(" status = 0 and open_work_id = {0} " , work_id ));
		foreach (DataMonsterMaster data_monster_master in list_monster) {
			Dictionary< string , string > monster_master_dict = new Dictionary< string , string > ();
			monster_master_dict.Add ("status", "1");
			GameMain.dbMonsterMaster.Update (data_monster_master.monster_id , monster_master_dict);
		}


		GameMain.Instance.AddFukidashi ( work_id , title + " を[FF0000]達成![-]");

		if (0 < prize_coin) {
			DataManager.user.AddGold (prize_coin);
		}
		if (0 < prize_ticket) {
			DataManager.user.AddTicket (prize_ticket);
		}
		if (0 < prize_monster) {
			GameMain.dbMonster.Insert (prize_monster, 0);
		}

		// お仕事の開放は絞られた仕事idと、レベルによって開放されます。
		// 先に仕事idの制限を切って,レベルでの制限に引っかかってないか確認する

		// この完了した仕事につられて起きる仕事
		List<DataWork> list_work = GameMain.dbWork.Select (" status = 0 and appear_work_id = " + work_id.ToString () + " ");
		Dictionary< string , string > dict_appear = new Dictionary< string , string > ();
		dict_appear.Add( "appear_work_id" , "0" ); 
		foreach (DataWork appear_work in list_work) {
			GameMain.dbWork.Update ( appear_work.work_id , dict_appear );
		}

		DataWork.WorkOpen ();

		return;
	}

	// とりあえずチェックしたかったらこれ！
	public bool ClearCheck(){
		bool bRet = false;

		if (0 < mission_level) {
			int iLevel = GameMain.dbKvs.ReadInt (Define.USER_LEVEL);

			//Debug.LogError (mission_level);
			if (mission_level <= iLevel) {
				bRet = true;
			}
		} else if (0 < mission_monster) {
			// 確認したいモンスターを何体持っているか
			string strSql = string.Format (" monster_id = {0} and item_serial != {1} ", mission_monster, 0);
			List<DataMonster> monster_list = GameMain.dbMonster.Select (strSql);
			/*
			Debug.LogError (strSql);
			foreach (DataMonster monst in monster_list) {
				Debug.LogError (string.Format ("monster_id:{0} , monster_serial={1} , item_serial={2}" , monst.monster_id,monst.monster_serial,monst.item_serial));
			}
			*/
			int monster_num = monster_list.Count;

			//Debug.LogError (string.Format ("norma={0} count={1}",mission_num,monster_num));
			if (mission_num <= monster_num) {
				bRet = true;
			}
		} else if (0 < mission_staff) {
			int staff_num = GameMain.dbStaff.Select ( string.Format(  " staff_id = {0} and item_serial != {1} " , mission_staff , 0 ) ).Count;
			if (mission_num <= staff_num) {
				bRet = true;
			}
		} else if (0 < mission_item) {

			string strWhere = string.Format (" item_id = {0} and status = {1} " , mission_item , (int)Define.Item.Status.SETTING);
			//Debug.LogError (" item_id = " + mission_item.ToString () + " ");
			List<DataItem> item_list = GameMain.dbItem.Select (" item_id = " + mission_item.ToString () + " ");
			foreach (DataItem item in item_list) {
				//Debug.LogError (item.item_id);
			}
			int item_num = GameMain.dbItem.Select (" item_id = " + mission_item.ToString () + " ").Count;
			if (mission_num <= item_num) {
				bRet = true;
			}
		} else if (0 < mission_collect) {
			int collect_count = GameMain.dbKvs.ReadInt (Define.USER_COLLECT_COUNT);

			//Debug.LogError (string.Format ("here:{0}", collect_count));
			// 注意！ここはmission_numじゃないです
			if (mission_collect <= collect_count) {
				bRet = true;
			}
		} else if (0 < mission_tweet) {
			int tweet_count = GameMain.dbKvs.ReadInt (Define.USER_TWEET_COUNT);
			if (mission_tweet < tweet_count) {
				bRet = true;
			}
		} else if (0 < mission_login) {
			int login_count = GameMain.dbKvs.ReadInt (Define.USER_LOGIN_COUNT);
			if (mission_login < login_count) {
				bRet = true;
			}
		} else {
		}

		return bRet;
	}




	/*
	public bool Level( int _iLevel){
		bool bRet = false;
		if (mission_level <= _iLevel) {
			bRet = true;
		}
		return bRet;
	}

	public bool Monster( int _iMonsterId ){
		bool bRet = false;
		if (mission_level <= _iMonsterId) {
			bRet = true;
		}
		return bRet;
	}
	public bool Staff( int _iMonsterId ){
	}
	public bool Item( int _iMonsterId ){
	}
	public bool Collect( int _iMonsterId ){
	}
	public bool Tweet( int _iMonsterId ){
	}
	public bool Login( int _iMonsterId ){
	}
	*/



}










