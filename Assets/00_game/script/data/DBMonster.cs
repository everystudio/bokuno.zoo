using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DBMonster : DBDataBase {
	//テーブル名
	public const string TABLE_NAME = "monster";
	public const string FILE_NAME = "SODataMonster";
	public SODataMonster m_soDataMonster;

	private bool m_bDebugLog = false;

	public List<DataMonster> data_list = new List<DataMonster>();

	public DBMonster( SQLiteAsync _Async ):base(_Async){
	}
	public DBMonster( string _strAsyncName ):base(_strAsyncName){
		m_soDataMonster = PrefabManager.Instance.PrefabLoadInstance (FILE_NAME).GetComponent<SODataMonster> ();
	}
	public string READ_ERROR_STRING = "sql_datamanager_read_error";

	/*
		ここのWriteは即書き込み
	*/

	//DBへ保存
	public void Replace(DataMonster _replocalData)
	{
		// ここ、最初しか呼ばれてないのでもうチェックしない
		m_soDataMonster.list.Add (_replocalData);
		return;
		/*
		//データの上書きのコマンドを設定する　
		string strQuery = "replace into " + TABLE_NAME + " (monster_serial,monster_id,item_serial,condition,collect_time,meal_time,clean_time) values( '" +
		                  _replocalData.monster_serial.ToString () + "','" +
		                  _replocalData.monster_id.ToString () + "','" +
		                  _replocalData.item_serial.ToString () + "','" +
		                  _replocalData.condition.ToString () + "','" +
		                  _replocalData.collect_time.ToString () + "','" +
		                  _replocalData.meal_time.ToString () + "','" +
		                  _replocalData.clean_time.ToString () + "');";

		Debug.Log( "DBMonster:"+strQuery);

		//m_sqlDBはDBDataBaseのプロテクト変数
		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);

		query.Step ();      //
		query.Release ();   //解放
		*/
	}

	public void Update( int _iSerial , Dictionary<string , string > _dict ){
		//Debug.LogError (_iSerial);
		foreach (DataMonster data in m_soDataMonster.list) {
			//Debug.LogError (data.item_serial);
			if (data.monster_serial == _iSerial) {
				data.Set (_dict);
			}
		}
		return;
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
		strQuery += " where monster_serial = " + _iSerial.ToString () + ";";
		Debug.Log( "DBMonster.Update:"+strQuery);
		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);
		query.Step ();      //
		query.Release ();   //解放
		return;
		*/
	}


	public void Update( int _iMonsterSerial , int _iItemSerial , int _iCondition = -1 ){

		foreach (DataMonster data in m_soDataMonster.list) {
			if (data.monster_serial == _iMonsterSerial) {
				data.item_serial = _iItemSerial;
				if (_iCondition != -1) {
					data.condition = _iCondition;
				}
			}
		}
		return;
		/*
		string strQuery = "update " + TABLE_NAME +
		                  " set item_serial = " + _iItemSerial.ToString ();
		if (_iCondition != -1) {
			strQuery += " , " + " condition = " + _iCondition.ToString ();
		}
		strQuery += " where monster_serial = " + _iMonsterSerial.ToString () + ";";
		Debug.Log( "DBMonster.Update:"+strQuery);
		//m_sqlDBはDBDataBaseのプロテクト変数
		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);

		query.Step ();      //
		query.Release ();   //解放
		*/
	}

	// 新規購入の場合
	// とり得る数からシリアルを返すようにする
	public DataMonster Insert( int _iMonsterId , int _iItemSerial ){

		int topIndex = m_soDataMonster.list.Count + 1;

		string strNow = TimeManager.StrNow ();
		int iStartCondition = (int)Define.Monster.CONDITION.FINE;

		DataMonster insert_data = new DataMonster ();
		insert_data.monster_id = _iMonsterId;
		insert_data.monster_serial = topIndex;
		insert_data.item_serial = _iItemSerial;
		insert_data.condition = iStartCondition;
		insert_data.collect_time = strNow;
		insert_data.meal_time = strNow;
		insert_data.clean_time = strNow;
		m_soDataMonster.list.Add (insert_data);

		return insert_data;
		/*
		//データの上書きのコマンドを設定する　
		string strQuery = "insert into " + TABLE_NAME + " (monster_serial,monster_id,item_serial,condition,collect_time,meal_time,clean_time) values( '" +
		                  topIndex.ToString () + "','" +
		                  _iMonsterId.ToString () + "','" +
		                  _iItemSerial.ToString () + "','" +
		                  iStartCondition.ToString () + "','" +
		                  strNow + "','" +
		                  strNow + "','" +
		                  strNow + "');";

		Debug.Log ("DBMonster Insert : "+strQuery);

		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);

		query.Step ();      //
		query.Release ();   //解放

		DataMonster retMonster = Select (topIndex);

		return retMonster;
		*/
	}

	public DataMonster Select( int _iSerial ){
		foreach (DataMonster data in m_soDataMonster.list) {
			if (data.monster_serial == _iSerial) {
				return data;
			}
		}
		return new DataMonster ();
		/*
		DataMonster ret;
		string strQuery = "select * from " + TABLE_NAME + " where monster_serial = '" + _iSerial + "';";

		SQLiteQuery query = new SQLiteQuery(m_sqlDB , strQuery );
		if( query.Step() ){
			ret = MakeData (query);
		}
		else {
			ret = new DataMonster();
		}
		return ret;
		*/
	}


	//テーブル以下全て取ってくる
	public List<DataMonster> SelectAll()
	{
		return m_soDataMonster.list;
		/*
		//データをクリア
		data_list.Clear ();

		string strQuery = "SELECT * FROM "+TABLE_NAME;

		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);

		//テーブルからデータを取ってくる
		while (query.Step ()) {
			DataMonster data = MakeData (query);
			data_list.Add (data);
		}
		query.Release ();

		return data_list;
		*/
	}

	public List<DataMonster> Select( string _strWhere = null ){
		List<DataMonster> ret_list = new List<DataMonster> ();

		foreach (DataMonster data in m_soDataMonster.list) {
			if (data.Equals (_strWhere)) {
				ret_list.Add (data);
			}
		}
		return ret_list;
		/*
		string strQuery = "select * from " + TABLE_NAME;

		if (_strWhere != null) {
			strQuery += " where " + _strWhere;
		}
		//Debug.Log ("DBMonster SelectQuery : "+strQuery);
		//m_sqlDBはDBDataBaseのプロテクト変数
		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);
		List<DataMonster> ret = new List<DataMonster> ();
		//テーブルからデータを取ってくる
		while (query.Step ()) {
			DataMonster data = MakeData (query);
			ret.Add (data);
		}
		query.Release ();
		return ret;
		*/
	}

	public List<DataMonster> Select(List<string> _whereList ){

		string strWhere = "";

		int iWhereCount = 0;
		if (_whereList != null) {
			foreach (string temp in _whereList) {
				if (0 < iWhereCount ) {
					strWhere += " and ";
				}
				strWhere += temp;
			}
		}
		return Select( strWhere );
	}


	public DataMonster MakeData( SQLiteQuery _qr ){
		DataMonster data = new DataMonster();

		if( m_bDebugLog ){
			//Debug.Log( "key  :" + strKey );
			//Debug.Log( "value:" + strValue );
		}
		data.monster_serial	= _qr.GetInteger("monster_serial");
		data.monster_id		= _qr.GetInteger("monster_id");
		data.item_serial			= _qr.GetInteger("item_serial");
		data.condition		= _qr.GetInteger("condition");
		data.collect_time		= _qr.GetString ("collect_time");
		data.meal_time	= _qr.GetString ("meal_time");
		data.clean_time		= _qr.GetString ("clean_time");

		return data;

	}

	protected override void pickup_data( SQLiteQuery _qr ){
		data_list.Add( MakeData(_qr) );
		return;
	}

}



