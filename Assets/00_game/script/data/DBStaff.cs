using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DBStaff : DBDataBase {
	//テーブル名
	public const string TABLE_NAME = "staff";
	public const string FILE_NAME = "SODataStaff";
	public SODataStaff m_soDataStaff;

	private bool m_bDebugLog = false;

	public List<DataStaff> data_list = new List<DataStaff>();

	public DBStaff( SQLiteAsync _Async ):base(_Async){
	}
	public DBStaff( string _strAsyncName ):base(_strAsyncName){
		m_soDataStaff = PrefabManager.Instance.PrefabLoadInstance (FILE_NAME).GetComponent<SODataStaff> ();
	}
	public string READ_ERROR_STRING = "sql_datamanager_read_error";


	// 新規購入の場合
	// とり得る数からシリアルを返すようにする
	public DataStaff Insert( int _iStaffId , int _iOfficeSerial , int _iItemSerial ){
		//data_list = SelectAll ();
		//int topIndex = data_list.Count + 1;
		int topIndex = m_soDataStaff.list.Count + 1;
		string strNow = TimeManager.StrNow ();
		//データの上書きのコマンドを設定する　

		DataStaff insert_data = new DataStaff ();
		insert_data.staff_serial = topIndex;
		insert_data.staff_id = _iStaffId;
		insert_data.office_serial = _iOfficeSerial;
		insert_data.item_serial = _iItemSerial;
		insert_data.setting_time = strNow;
		insert_data.create_time = strNow;
		m_soDataStaff.list.Add (insert_data);
		return insert_data;
		/*
		string strQuery = "insert into " + TABLE_NAME + " (staff_serial,staff_id,office_serial,item_serial,setting_time,create_time) values( '" +
		                  topIndex.ToString () + "','" +
		                  _iStaffId.ToString () + "','" +
		                  _iOfficeSerial.ToString () + "','" +
		                  _iItemSerial.ToString () + "','" +

		                  strNow + "','" +
		                  strNow + "');";

		Debug.Log ("DBMonster Insert : "+strQuery);
		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);
		query.Step ();      //
		query.Release ();   //解放
		return Select (topIndex);
		*/
	}

	public DataStaff Update( int _iSerial , Dictionary<string , string > _dict ){

		foreach (DataStaff data in m_soDataStaff.list) {
			if (data.staff_serial == _iSerial) {
				data.Set (_dict);
				return data;
			}
		}
		return new DataStaff ();
		/*
		string strQuery = "update " + TABLE_NAME + " set ";
		bool bHead = true;
		foreach (string key in _dict.Keys) {
			if (bHead) {
				strQuery += "";		// なにもしません
				bHead = false;
			} else {
				strQuery += ",";		// なにもしません
			}
			strQuery += key + " = " + _dict[key] + " ";
		}
		strQuery += " where staff_serial = " + _iSerial.ToString () + ";";
		Debug.Log( "DBStaff.Update:"+strQuery);
		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);
		query.Step ();      //
		query.Release ();   //解放

		return Select (_iSerial);
		*/
	}

	public DataStaff Select( int _iStaffSerial ){
		foreach (DataStaff data in m_soDataStaff.list) {
			if (data.staff_serial == _iStaffSerial) {
				return data;
			}
		}
		return new DataStaff();
		/*
		DataStaff ret;
		string strQuery = "select * from " + TABLE_NAME + " where staff_serial = '" + _iStaffSerial + "';";

		SQLiteQuery query = new SQLiteQuery(m_sqlDB , strQuery );
		if( query.Step() ){
			ret = MakeData (query);
		}
		else {
			ret = new DataStaff();
		}
		return ret;
		*/
	}

	public List<DataStaff> Select(string _strWhere = null ){

		List<DataStaff> ret_list = new List<DataStaff> ();
		foreach (DataStaff data in m_soDataStaff.list) {
			if (data.Equals( _strWhere )) {
				ret_list.Add( data);
			}
		}
		return ret_list;
		/*
		string strQuery = "select * from " + TABLE_NAME;

		if (_strWhere != null) {
			strQuery += " where " + _strWhere;
		}

		//Debug.Log ("DBStaff SelectQuery : "+strQuery);

		//m_sqlDBはDBDataBaseのプロテクト変数
		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);

		List<DataStaff> ret = new List<DataStaff> ();

		//テーブルからデータを取ってくる
		while (query.Step ()) {
			DataStaff data = MakeData (query);
			ret.Add (data);
		}
		query.Release ();

		return ret;
		*/
	}

	//テーブル以下全て取ってくる
	public List<DataStaff> SelectAll()
	{
		return m_soDataStaff.list;
		/*
		//データをクリア
		data_list.Clear ();

		string strQuery = "SELECT * FROM "+TABLE_NAME;

		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);

		//テーブルからデータを取ってくる
		while (query.Step ()) {
			DataStaff data = MakeData (query);
			data_list.Add (data);
		}
		query.Release ();

		return data_list;
		*/
	}

	public DataStaff MakeData( SQLiteQuery _qr ){
		DataStaff data = new DataStaff();

		if( m_bDebugLog ){
			//Debug.Log( "key  :" + strKey );
			//Debug.Log( "value:" + strValue );
		}
		data.staff_serial	= _qr.GetInteger("staff_serial");
		data.office_serial	= _qr.GetInteger("office_serial");
		data.staff_id		= _qr.GetInteger("staff_id");
		data.item_serial	= _qr.GetInteger("item_serial");
		data.setting_time	= _qr.GetString ("setting_time");
		data.create_time	= _qr.GetString ("create_time");

		return data;

	}

	protected override void pickup_data( SQLiteQuery _qr ){
		data_list.Add( MakeData(_qr) );
		return;
	}



}















