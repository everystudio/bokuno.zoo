using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[System.Serializable]
public class DataItem : SearchBase{

	public int m_item_serial;
	public int m_item_id;
	public int m_category;
	public int m_level;
	public int m_status;
	public int m_cost;
	public int m_cost_max;
	public int m_revenue_rate;
	public int m_x;
	public int m_y;
	public int m_width;
	public int m_height;
	public string m_collect_time;
	public string m_create_time;

	public int item_serial { get{ return m_item_serial;} set{m_item_serial = value; } }
	public int item_id { get{ return m_item_id;} set{m_item_id = value; } }
	public int category { get{ return m_category;} set{m_category = value; } }
	public int level { get{ return m_level;} set{m_level = value; } }
	public int status { get{ return m_status;} set{m_status = value; } }
	public int cost { get{ return m_cost;} set{m_cost = value; } }
	public int cost_max { get{ return m_cost_max;} set{m_cost_max = value; } }
	public int revenue_rate { get{ return m_revenue_rate;} set{m_revenue_rate = value; } }
	public int x { get{ return m_x;} set{m_x = value; } }
	public int y { get{ return m_y;} set{m_y = value; } }
	public int width { get{ return m_width;} set{m_width = value; } }
	public int height { get{ return m_height;} set{m_height = value; } }
	public string collect_time { get{ return m_collect_time;} set{m_collect_time = value; } }
	public string create_time { get{ return m_create_time;} set{m_create_time = value; } }

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
				} else if (strJudge.Equals ("<")) {
					if (intcheck <= intparam ) {
						bRet = false;
					}
				} else {
				}
			}
		}
		return bRet;
	}

	public bool Equals( Define.WHERE_PATTERN _ePattern , List<int> _iList ){

		switch (_ePattern) {

		case Define.WHERE_PATTERN.SERIAL_EQUAL:
			if (item_serial == _iList[0]) {
				return true;
			}
			break;
		case Define.WHERE_PATTERN.SERIAL_NOT_EQUAL:
			if (item_serial != _iList [0]) {
				return true;
			}
			break;
		case Define.WHERE_PATTERN.RATE:
			if ((item_id == 23 || item_id == 24) && status == 1) {
				return true;
			}
			break;

		case Define.WHERE_PATTERN.ITEM_STATUS_NOT_EQUAL:
			if (status != _iList [0]) {
				return true;
			}
			break;

		default:
			break;
		}

		return false;

	}
	/*
	public void Set(Dictionary<string , string > _dict){

		foreach (string key in _dict.Keys) {
			switch (key) {
			case "item_id":
				item_id = int.Parse (_dict [key]);
				break;
			case "category":
				category = int.Parse (_dict [key]);
				break;
			case "level":
				level = int.Parse (_dict [key]);
				break;
			case "status":
				status = int.Parse (_dict [key]);
				break;
			case "cost":
				cost = int.Parse (_dict [key]);
				break;
			case "cost_max":
				cost_max = int.Parse (_dict [key]);
				break;
			case "revenue_rate":
				revenue_rate = int.Parse (_dict [key]);
				break;
			case "x":
				x = int.Parse (_dict [key]);
				break;
			case "y":
				y = int.Parse (_dict [key]);
				break;
			case "width":
				width = int.Parse (_dict [key]);
				break;
			case "height":
				height = int.Parse (_dict [key]);
				break;
			case "collect_time":
				collect_time = _dict [key];
				break;
			case "create_time":
				create_time = _dict [key];
				break;
			}
		}
	}
	*/

	public DataItem(){
	}
	public DataItem( MapChipCSV _mapChip , CsvItem _csvItem ){

		CsvItemData item_data = new CsvItemData ();
		foreach (CsvItemData data in _csvItem.All) {
			if (data.item_id == _mapChip.item_id) {
				item_data = data;
			}
		}

		item_serial = _mapChip.serial;
		item_id = _mapChip.item_id;
		category = item_data.category;
		x = _mapChip.x;
		y = _mapChip.y;
		width = item_data.size;//_mapChip.width
		height = item_data.size;//_mapChip.height
		status = 1;
		level = 1;
		collect_time = TimeManager.StrNow ();;
		create_time = TimeManager.StrNow ();;
	}

	static public float GetSymbolRate(){
		float fTotalRate = 1.0f;
		//List<DataItem> symbol_list = GameMain.dbItem.Select (" type = " + ((int)(Define.Item.Type.SYMBOL)).ToString () + " ");
		List<DataItem> symbol_list = GameMain.dbItem.Select ( Define.WHERE_PATTERN.RATE );
		foreach( DataItem symbol in symbol_list ){
			CsvItemData csv_symbol_item_data = DataManager.GetItem (symbol.item_id);
			if ((int)(fTotalRate * 100.0f) < csv_symbol_item_data.revenue_up) {
				fTotalRate = csv_symbol_item_data.revenue_up * 0.01f;
			}
		}

		return fTotalRate;
	}

	public int GetCollect( bool _bCollect , out int _iCollectGold , out int _iCollectExp){

		float fTotalRate = GetSymbolRate();

		int iCollectGold = 0;
		int iCollectExp = 0;
		List<DataMonster> monster_list = GameMain.dbMonster.Select (" item_serial = " + item_serial.ToString ());
		foreach (DataMonster monster in monster_list) {
			int iTempGold = 0;
			int iTempExp = 0;

			monster.GetCollect (_bCollect , out iTempGold , out iTempExp );

			iCollectGold += iTempGold;
			iCollectExp += iTempExp;
		}

		int iShopCollectGold = 0;
		CsvItemData csv_item_data = DataManager.GetItem (item_id);
		if (0 < csv_item_data.revenue) {
			// お店自体金額回収
			double diffSec = TimeManager.Instance.GetDiffNow (collect_time).TotalSeconds * -1.0d;
			double dCount = diffSec / csv_item_data.revenue_interval;
			if (1 < dCount) {
				dCount = 1;
			}

			iShopCollectGold = (int)dCount * csv_item_data.revenue;

			/*
			Debug.LogError (collect_time);
			Debug.LogError (diffSec);
			Debug.LogError (dCount);
			Debug.LogError (iShopCollectGold);
			*/
			if (_bCollect) {
				double amari = diffSec % csv_item_data.revenue_interval;
				string strNow = TimeManager.StrGetTime (-1* (int)amari);
				Dictionary< string , string > dict = new Dictionary< string , string > ();
				dict.Add ("collect_time", "\"" + strNow + "\"");
				GameMain.dbItem.Update (item_serial, dict);
			}
		}

		if (0 < iShopCollectGold) {
			iCollectGold = iShopCollectGold;
		} else {
			CsvItemDetailData item_detail_data = DataManager.GetItemDetail (item_id, level);

			iCollectGold = (iCollectGold * item_detail_data.revenue_rate) / 100;
		}
		// お店のぶんも計上

		CtrlFieldItem ctrlFieldItem = GameMain.ParkRoot.GetFieldItem (item_serial);

		if (ctrlFieldItem != null && ctrlFieldItem.m_eRoad != Define.ROAD.CONNECTION_SHOP) {
			//iCollectGold/= 2;
			//iCollectExp /= 2;
		} else {
			iCollectGold +=iCollectGold/ 2;
			iCollectExp  +=iCollectExp/ 2;
		}

		//Debug.Log( string.Format( "rate:{0}" ,fTotalRate ));
		iCollectGold = (int)(iCollectGold * fTotalRate);

		_iCollectGold = iCollectGold;
		_iCollectExp = iCollectExp;

		return iCollectGold;
	}

	public int GetUriagePerHour(){

		float fTotalRate = GetSymbolRate();

		int iRet = 0;
		bool bHalf = true;
		CtrlFieldItem ctrlFieldItem = GameMain.ParkRoot.GetFieldItem (item_serial);

		//Debug.LogError (item_serial);
		if (ctrlFieldItem != null) {
			//Debug.LogError (ctrlFieldItem.m_dataItem.category);
		}
		if (ctrlFieldItem != null && ctrlFieldItem.m_eRoad == Define.ROAD.CONNECTION_SHOP) {
			bHalf = false;
			//Debug.LogError ("here ");
		}
		//Debug.LogError (bHalf);

		// 例外?処理
		CsvItemData csv_item_data = DataManager.GetItem (item_id);
		if (0 < csv_item_data.revenue) {
			int iCount = 3600 / csv_item_data.revenue_interval;
			// お店自体金額回収

			iRet = iCount * csv_item_data.revenue;
			if (bHalf) {
				iRet /= 2;
			}
			if (status == 0) {
				iRet = 0;
			}

			iRet = (int)(iRet * fTotalRate);

			return iRet;
		}

		List<DataMonster > monster_list = GameMain.dbMonster.Select (" item_serial = " + item_serial.ToString ());
		int iUriageMax = 0;
		foreach (DataMonster monster in monster_list) {
			CsvMonsterData monster_csv = DataManager.GetMonster (monster.monster_id);

			int iCount = 3600 / monster_csv.revenew_interval;
			iUriageMax += monster_csv.revenew_coin * iCount;
		}
		CsvItemDetailData item_detail_data = DataManager.GetItemDetail (item_id, level);
		iUriageMax = (iUriageMax * item_detail_data.revenue_rate) / 100;
		iRet = iUriageMax;
		if (bHalf) {
			iRet /= 2;
		}
		iRet = (int)(iRet * fTotalRate);

		return iRet;
	}

	public int GetShiSyutsuPerHour(){
		int iShisyutsu = 0;
		List<DataStaff > staff_list = GameMain.dbStaff.Select (" item_serial = " + item_serial.ToString ());

		foreach (DataStaff staff in staff_list) {
			iShisyutsu += staff.GetShisyutsuParHour ();
		}
		return iShisyutsu;
	}

	static public void OpenNewItem( int _iKeyItemId ){

		List<DataItemMaster> open_item_list = GameMain.dbItemMaster.Select (string.Format (" status = {0} and open_item_id = {1} ", (int)Define.Item.Status.NONE, _iKeyItemId));
		foreach (DataItemMaster open_item in open_item_list) {
			Dictionary<string , string > update_value = new Dictionary<string , string > ();
			update_value.Add ("status", string.Format ( "{0}" , (int)Define.Item.Status.SETTING ));
			GameMain.dbItemMaster.Update ( open_item.item_id , update_value);
		}
		return;
	}


}

[System.Serializable]
public class DataItemMaster :SODataParam{

	public int m_item_id;
	public int m_status;
	public string m_name;
	public int m_category;
	public int m_type;
	public int m_cell_type;
	public string m_description;
	public int m_need_coin;
	public int m_need_ticket;
	public int m_need_money;
	public int m_size;
	public int m_cost;
	public int m_area;
	public int m_revenue;
	public int m_revenue_interval;
	public int m_revenue_up;
	public int m_production_time;
	public int m_setting_limit;
	public int m_sub_parts_id;
	public int m_open_item_id;
	public int m_revenue_up2;
	public int m_add_coin;

	public int item_id { get{ return m_item_id;} set{m_item_id = value; } }
	public int status { get{ return m_status;} set{m_status = value; } }
	public string name { get{ return m_name;} set{m_name = value; } }
	public int category { get{ return m_category;} set{m_category = value; } }
	public int type { get{ return m_type;} set{m_type = value; } }
	public int cell_type { get{ return m_cell_type;} set{m_cell_type = value; } }
	public string description { get{ return m_description;} set{m_description = value; } }
	public int need_coin { get{ return m_need_coin;} set{m_need_coin = value; } }
	public int need_ticket { get{ return m_need_ticket;} set{m_need_ticket = value; } }
	public int need_money { get{ return m_need_money;} set{m_need_money = value; } }
	public int size { get{ return m_size;} set{m_size = value; } }
	public int cost { get{ return m_cost;} set{m_cost = value; } }
	public int area { get{ return m_area;} set{m_area = value; } }
	public int revenue { get{ return m_revenue;} set{m_revenue = value; } }
	public int revenue_interval { get{ return m_revenue_interval;} set{m_revenue_interval = value; } }
	public int revenue_up { get{ return m_revenue_up;} set{m_revenue_up = value; } }
	public int production_time { get{ return m_production_time;} set{m_production_time = value; } }
	public int setting_limit { get{ return m_setting_limit;} set{m_setting_limit = value; } }
	public int sub_parts_id { get{ return m_sub_parts_id;} set{m_sub_parts_id = value; } }
	public int open_item_id { get{ return m_open_item_id;} set{m_open_item_id = value; } }
	public int revenue_up2 { get{ return m_revenue_up2;} set{m_revenue_up2 = value; } }
	public int add_coin { get{ return m_add_coin;} set{m_add_coin = value; } }

	public DataItemMaster(){
		category = 0;
		size = 1;
		area = 1;
	}

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

	public void Copy( DataItemMaster _data ){
		item_id = _data.item_id;
		status = 0;			// 通常は利用できるとして扱う
		name = _data.name;
		category = _data.category;
		type = _data.type;
		cell_type = _data.cell_type;
		description = _data.description;
		need_coin = _data.need_coin;
		need_ticket = _data.need_ticket;
		need_money = _data.need_money;
		size = _data.size;
		cost = _data.cost;
		area = _data.area;
		revenue = _data.revenue;
		revenue_interval = _data.revenue_interval;
		revenue_up = _data.revenue_up;
		production_time = _data.production_time;
		setting_limit = _data.setting_limit;
		sub_parts_id = _data.sub_parts_id;
		open_item_id = _data.open_item_id;
		revenue_up2 = _data.revenue_up2;
		add_coin = _data.add_coin;
		item_id = _data.item_id;
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

	public DataItemMaster( CsvItemData _data ){
		int count = 0;
		item_id = _data.item_id;
		status = 0;			// 通常は利用できるとして扱う
		name = _data.name;
		category = _data.category;
		type = _data.type;
		cell_type = _data.cell_type;
		description = _data.description;
		need_coin = _data.need_coin;
		need_ticket = _data.need_ticket;
		need_money = _data.need_money;
		size = _data.size;
		cost = _data.cost;
		area = _data.area;
		revenue = _data.revenue;
		revenue_interval = _data.revenue_interval;
		revenue_up = _data.revenue_up;
		production_time = _data.production_time;
		setting_limit = _data.setting_limit;
		sub_parts_id = _data.sub_parts_id;
		open_item_id = _data.open_item_id;
		revenue_up2 = _data.revenue_up2;
		add_coin = _data.add_coin;
		item_id = _data.item_id;

	}


}









