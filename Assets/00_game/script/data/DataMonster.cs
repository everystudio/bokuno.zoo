using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[System.Serializable]
public class DataMonster : SODataParam{

	public int m_monster_serial;
	public int m_monster_id;
	public int m_item_serial;
	public int m_condition;
	public string m_collect_time;
	public string m_meal_time;
	public string m_clean_time;

	public int monster_serial { get{ return m_monster_serial;} set{m_monster_serial = value; } }
	public int monster_id { get{ return m_monster_id;} set{m_monster_id = value; } }
	public int item_serial { get{ return m_item_serial;} set{m_item_serial = value; } }
	public int condition { get{ return m_condition;} set{m_condition = value; } }
	public string collect_time { get{ return m_collect_time;} set{m_collect_time = value; } }
	public string meal_time { get{ return m_meal_time;} set{m_meal_time = value; } }
	public string clean_time { get{ return m_clean_time;} set{m_clean_time = value; } }


	public void Set(Dictionary<string , string > _dict){

		foreach (string key in _dict.Keys) {
			Debug.LogError (key);
			PropertyInfo propertyInfo = GetType ().GetProperty (key);
			if (propertyInfo.PropertyType == typeof(int)) {
				int iValue = int.Parse (_dict [key]);
				propertyInfo.SetValue (this, iValue, null);
			} else if (propertyInfo.PropertyType == typeof(string)) {
				Debug.LogError (_dict [key].Replace ("\"", ""));
				propertyInfo.SetValue (this, _dict [key].Replace ("\"", ""), null);
			} else if (propertyInfo.PropertyType == typeof(double)) {
				propertyInfo.SetValue (this, double.Parse (_dict [key]), null);
			} else if (propertyInfo.PropertyType == typeof(float)) {
				propertyInfo.SetValue (this, float.Parse (_dict [key]), null);
			}
			else {
				Debug.LogError ("error type unknown");
			}
		}

	}

	public int GetCollect( bool _bCollect , out int _iCollectGold , out int _iCollectExp){
		double diffSec = TimeManager.Instance.GetDiffNow (collect_time).TotalSeconds * -1.0d;
		//Debug.Log (diffSec.ToString() + ":" + condition.ToString() );
		CsvMonsterData csvMonster = DataManager.GetMonster (monster_id);
		double dCount = diffSec / csvMonster.revenew_interval;
		int iCollectGold = (int)dCount * csvMonster.revenew_coin;
		int iCollectExp = (int)dCount * csvMonster.revenew_exp;
		if (_bCollect) {
			string strNow = TimeManager.StrNow ();
			Dictionary< string , string > dict = new Dictionary< string , string > ();
			dict.Add ("collect_time", "\"" + strNow + "\"");
			GameMain.dbMonster.Update (monster_serial, dict );
		}

		_iCollectGold = iCollectGold;
		_iCollectExp = iCollectExp;

		// 健全なのは1
		if ( (int)Define.Monster.CONDITION.SICK  == condition ) {
			iCollectGold = 0;
		}
		return (int)iCollectGold;
	}

	public void GetConditions( ref int _iCleanLevel , ref int _iMealLevel ){

		_iCleanLevel = 0;
		_iMealLevel = 0;

		double d_clean_time = TimeManager.Instance.GetDiffNow (clean_time).TotalSeconds * -1.0d;
		double d_meal_time = TimeManager.Instance.GetDiffNow (meal_time).TotalSeconds * -1.0d;

		foreach (CsvTimeData time_data in DataManager.csv_time) {
			if (time_data.type == 1) {
				if (d_clean_time < time_data.delta_time) {
					if (_iCleanLevel < time_data.now) {
						_iCleanLevel = time_data.now;
					}
				}

			} else if (time_data.type == 2) {
				if (d_meal_time < time_data.delta_time) {
					if (_iMealLevel < time_data.now) {
						_iMealLevel = time_data.now;
					}
				}
			} else {
			}
		}
		return;
	}

	public bool Equals( string _strWhere ){

		//Debug.Log (_strWhere);
		string[] test = _strWhere.Trim().Split (' ');
		int count = 0;
		foreach (string check in test) {
			//Debug.Log (string.Format ("{0}:{1}", count, check));
			count += 1;
		}

		bool bRet = true;

		for (int i = 0; i < test.Length; i+=4 ) {
			//Debug.Log (test [i]);
			PropertyInfo propertyInfo = GetType ().GetProperty (test [i]);
			if (propertyInfo.PropertyType == typeof(int)) {
				int intparam = (int)propertyInfo.GetValue (this, null);
				string strJudge = test [i + 1];
				int intcheck = int.Parse(test[i+2]);
				if (strJudge.Equals ("=")) {
					if (intparam != intcheck) {
						bRet = false;
					}
				} else if (strJudge.Equals ("!=")) {
					if (intparam == intcheck) {
						bRet = false;
					}
				} else {
				}
			}
		}
		return bRet;
	}
}


[System.Serializable]
public class DataMonsterMaster : SODataParam {
	public int m_monster_id;
	public string m_name;
	public string m_description_cell;
	public int m_cost;
	public int m_fill;
	public int m_dust;
	public int m_coin;
	public int m_ticket;
	public int m_revenew_coin;
	public int m_revenew_exp;
	public int m_revenew_interval;
	public int m_open_work_id;
	public string m_description_book;
	public int m_size;
	public int m_rare;
	public int m_status;

	public int monster_id { get{ return m_monster_id;} set{m_monster_id = value; } }
	public string name { get{ return m_name;} set{m_name = value; } }
	public string description_cell { get{ return m_description_cell;} set{m_description_cell = value; } }
	public int cost { get{ return m_cost;} set{m_cost = value; } }
	public int fill { get{ return m_fill;} set{m_fill = value; } }
	public int dust { get{ return m_dust;} set{m_dust = value; } }
	public int coin { get{ return m_coin;} set{m_coin = value; } }
	public int ticket { get{ return m_ticket;} set{m_ticket = value; } }
	public int revenew_coin { get{ return m_revenew_coin;} set{m_revenew_coin = value; } }
	public int revenew_exp { get{ return m_revenew_exp;} set{m_revenew_exp = value; } }
	public int revenew_interval { get{ return m_revenew_interval;} set{m_revenew_interval = value; } }
	public int open_work_id { get{ return m_open_work_id;} set{m_open_work_id = value; } }
	public string description_book { get{ return m_description_book;} set{m_description_book = value; } }
	public int size { get{ return m_size;} set{m_size = value; } }
	public int rare { get{ return m_rare;} set{m_rare = value; } }
	public int status { get{ return m_status;} set{m_status = value; } }

	public void Set(Dictionary<string , string > _dict){

		foreach (string key in _dict.Keys) {
			PropertyInfo propertyInfo = GetType ().GetProperty (key);
			if (propertyInfo.PropertyType == typeof(int)) {
				int iValue = int.Parse (_dict [key]);
				propertyInfo.SetValue (this, iValue, null);
			} else if (propertyInfo.PropertyType == typeof(string)) {
				propertyInfo.SetValue (this, _dict [key].Replace ("\"", ""), null);
			} else if (propertyInfo.PropertyType == typeof(double)) {
				propertyInfo.SetValue (this, double.Parse (_dict [key]), null);
			} else if (propertyInfo.PropertyType == typeof(float)) {
				propertyInfo.SetValue (this, float.Parse (_dict [key]), null);
			}
			else {
				Debug.LogError ("error type unknown");
			}
		}
	}


	public bool Equals( string _strWhere ){
		//Debug.Log (_strWhere);
		string[] test = _strWhere.Trim().Split (' ');
		int count = 0;
		foreach (string check in test) {
			//Debug.Log (string.Format ("{0}:{1}", count, check));
			count += 1;
		}

		bool bRet = true;

		for (int i = 0; i < test.Length; i+=4 ) {
			//Debug.Log (test [i]);
			PropertyInfo propertyInfo = GetType ().GetProperty (test [i]);
			if (propertyInfo.PropertyType == typeof(int)) {
				int intparam = (int)propertyInfo.GetValue (this, null);
				string strJudge = test [i + 1];
				int intcheck = int.Parse(test[i+2]);
				if (strJudge.Equals ("=")) {
					if (intparam != intcheck) {
						bRet = false;
					}
				} else if (strJudge.Equals ("!=")) {
					if (intparam == intcheck) {
						bRet = false;
					}
				} else {
				}
			}
		}
		return bRet;
	}

}










