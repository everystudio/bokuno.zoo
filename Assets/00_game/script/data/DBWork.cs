using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DBWork : DBDataBase {
	//テーブル名
	public const string TABLE_NAME = "work";
	public const string FILE_NAME = "SODataWork";
	public SODataWork m_soDataWork;

	private bool m_bDebugLog = false;

	public List<DataWork> data_list = new List<DataWork>();

	public DBWork( SQLiteAsync _Async ):base(_Async){
	}
	public DBWork( string _strAsyncName ):base(_strAsyncName){
		m_soDataWork = PrefabManager.Instance.PrefabLoadInstance (FILE_NAME).GetComponent<SODataWork> ();
	}
	public string READ_ERROR_STRING = "sql_datamanager_read_error";

	/*
		ここのWriteは即書き込み
	*/

	public void Update( int _iWorkId , Dictionary<string , string > _dict ){

		foreach (DataWork data in m_soDataWork.list) {
			if (data.work_id == _iWorkId) {
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
		strQuery += " where work_id = " + _iWorkId.ToString () + ";";
		Debug.Log( "DBWorkUpdate:"+strQuery);
		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);
		query.Step ();      //
		query.Release ();   //解放
		return;
		*/
	}



	//DBへ保存
	public void Replace(DataWork _replocalData)
	{
		m_soDataWork.list.Add (_replocalData);
		return;
		/*
		//データの上書きのコマンドを設定する　
		string strQuery = "replace into " + TABLE_NAME + " (work_id, status, title, description, type, level, appear_work_id, exp, difficulty, prize_ticket, prize_coin, prize_monster, mission_level, mission_monster, mission_staff, mission_item, mission_collect, mission_tweet, mission_login, mission_num) values( '" +
		                  _replocalData.work_id.ToString () + "','" +
		                  _replocalData.status.ToString () + "',\"" +
		                  _replocalData.title.ToString () + "\",\"" +
		                  _replocalData.description.ToString () + "\",'" +
		                  _replocalData.type.ToString () + "','" +
		                  _replocalData.level.ToString () + "','" +
		                  _replocalData.appear_work_id.ToString () + "','" +
		                  _replocalData.exp.ToString () + "','" +
		                  _replocalData.difficulty.ToString () + "','" +
		                  _replocalData.prize_ticket.ToString () + "','" +
		                  _replocalData.prize_coin.ToString () + "','" +
		                  _replocalData.prize_monster.ToString () + "','" +
		                  _replocalData.mission_level.ToString () + "','" +
		                  _replocalData.mission_monster.ToString () + "','" +
		                  _replocalData.mission_staff.ToString () + "','" +
		                  _replocalData.mission_item.ToString () + "','" +
		                  _replocalData.mission_collect.ToString () + "','" +
		                  _replocalData.mission_tweet.ToString () + "','" +
		                  _replocalData.mission_login.ToString () + "','" +
		                  _replocalData.mission_num.ToString () + "');";

		Debug.Log( "DBPark:"+strQuery);

		//m_sqlDBはDBDataBaseのプロテクト変数
		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);

		query.Step ();      //
		query.Release ();   //解放
		*/
	}

	public DataWork Select( int _iWorkId ){
		foreach (DataWork data in m_soDataWork.list) {
			if (data.work_id == _iWorkId) {
				return data;
			}
		}
		return new DataWork ();
		/*
		DataWork ret;
		string strQuery = "select * from " + TABLE_NAME + " where work_id = '" + _iWorkId + "';";

		SQLiteQuery query = new SQLiteQuery(m_sqlDB , strQuery );
		if( query.Step() ){
			ret = MakeData (query);
		}
		else {
			ret = new DataWork();
		}
		return ret;
		*/
	}


	public List<DataWork> Select( string _strWhere = null , bool _bDebugLog = true ){

		List<DataWork> ret_list = new List<DataWork> ();
		foreach (DataWork data in m_soDataWork.list) {
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

		if (_bDebugLog) {
			Debug.Log ("DBWork SelectQuery : " + strQuery);
		}

		//m_sqlDBはDBDataBaseのプロテクト変数
		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);

		List<DataWork> ret = new List<DataWork> ();

		//テーブルからデータを取ってくる
		while (query.Step ()) {
			DataWork data = MakeData (query);
			ret.Add (data);
		}
		query.Release ();
		return ret;
		*/
	}

	public List<DataWork> Select(List<string> _whereList ){

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

	/*
	public List<DataWork> Select2(Dictionary<string,object> _where = null){

		//テーブルからデータを調べる為のコマンドを設定
		string strQuery = "SELECT * FROM " + TABLE_NAME;
		if (_where != null)
		{
			strQuery += " WHERE ";	//コマンドをループ設定にする
			int loopCount = 0;
			int keyCount = _where.Keys.Count;
			foreach(var key in _where.Keys)
			{
				strQuery += key + " = ";		//条件追加
				//変数の型を調べる
				if (_where [key].GetType () == typeof(string)) {
					strQuery+= "\""+(string)_where [key]+"\"";
				} else if (_where [key].GetType () == typeof(int)) {
					strQuery+= ""+(int)_where [key];
				} else if (_where [key].GetType () == typeof(double)) {
					strQuery+= ""+(double)_where [key];
				}
				else {
					Debug.LogError (" [string] [int] [double]以外のタイプです : " + _where [key].GetType ());
				}
				loopCount++;
				if (keyCount != loopCount) {
					strQuery += " AND ";			//AND追加
				}
			}
		}

		Debug.Log ("DBWork SelectQuery : "+strQuery);

		//m_sqlDBはDBDataBaseのプロテクト変数
		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);

		data_list.Clear ();

		//テーブルからデータを取ってくる
		while (query.Step ()) {
			DataWork data = MakeData (query);
			data_list.Add (data);
		}
		query.Release ();

		return data_list;
	}
	*/


	//テーブル以下全て取ってくる
	public List<DataWork> SelectAll()
	{
		return m_soDataWork.list;
		/*
		List<DataWork> ret = new List<DataWork> ();

		string strQuery = "SELECT * FROM "+TABLE_NAME;

		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);

		//テーブルからデータを取ってくる
		while (query.Step ()) {
			DataWork data = MakeData (query);
			ret.Add (data);
		}
		query.Release ();

		return ret;
		*/
	}


	public DataWork MakeData( SQLiteQuery _qr ){
		DataWork data = new DataWork();

		if( m_bDebugLog ){
			//Debug.Log( "key  :" + strKey );
			//Debug.Log( "value:" + strValue );
		}
		data.work_id	= _qr.GetInteger("work_id");
		data.status		= _qr.GetInteger("status");
		data.title			= _qr.GetString("title");
		data.description			= _qr.GetString("description");
		data.type 				= _qr.GetInteger ("type");
		data.level 				= _qr.GetInteger ("level");
		data.appear_work_id			= _qr.GetInteger("appear_work_id");
		data.exp			= _qr.GetInteger("exp");
		data.difficulty		= _qr.GetString("difficulty");
		data.prize_ticket			= _qr.GetInteger("prize_ticket");
		data.prize_coin		= _qr.GetInteger("prize_coin");
		data.prize_monster		= _qr.GetInteger("prize_monster");
		data.mission_level		= _qr.GetInteger("mission_level");
		data.mission_monster		= _qr.GetInteger("mission_monster");
		data.mission_staff		= _qr.GetInteger("mission_staff");
		data.mission_item		= _qr.GetInteger("mission_item");
		data.mission_collect		= _qr.GetInteger("mission_collect");
		data.mission_tweet		= _qr.GetInteger("mission_tweet");
		data.mission_login		= _qr.GetInteger("mission_login");
		data.mission_num		= _qr.GetInteger("mission_num");

		return data;

	}

	protected override void pickup_data( SQLiteQuery _qr ){
		data_list.Add( MakeData(_qr) );
		return;
	}

}



