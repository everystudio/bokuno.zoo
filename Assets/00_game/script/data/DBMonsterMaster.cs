using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DBMonsterMaster : DBDataBase {
	//テーブル名
	public const string TABLE_NAME = "monster_master";
	public const string FILE_NAME = "SODataMonsterMaster";
	public SODataMonsterMaster m_soDataMonsterMaster;

	private bool m_bDebugLog = false;

	public List<DataMonsterMaster> data_list = new List<DataMonsterMaster>();

	public DBMonsterMaster( SQLiteAsync _Async ):base(_Async){
	}
	public DBMonsterMaster( string _strAsyncName ):base(_strAsyncName){
		m_soDataMonsterMaster = PrefabManager.Instance.PrefabLoadInstance (FILE_NAME).GetComponent<SODataMonsterMaster> ();
	}
	public string READ_ERROR_STRING = "sql_datamanager_read_error";

	/*
		ここのWriteは即書き込み
	*/

	public void Update( int _iMonsterId , Dictionary<string , string > _dict ){

		foreach (DataMonsterMaster data in m_soDataMonsterMaster.list) {
			if (data.monster_id == _iMonsterId) {
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
		strQuery += " where monster_id = " + _iMonsterId.ToString () + ";";
		Debug.Log( "DBMonster.Update:"+strQuery);
		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);
		query.Step ();      //
		query.Release ();   //解放
		return;
		*/
	}


	//DBへ保存
	public void Replace(CsvMonsterData _replocalData)
	{
		int iInitStatus = 0;
		if (_replocalData.open_work_id == 0) {
			iInitStatus = 1;
		}

		string strName = CommonConvertToSaveDb (_replocalData.name);
		string strDescriptionBook = CommonConvertToSaveDb (_replocalData.description_book);
		string strDescriptionCell = CommonConvertToSaveDb (_replocalData.description_cell);

		DataMonsterMaster data_replace = new DataMonsterMaster ();

		data_replace.monster_id = _replocalData.monster_id;
		data_replace.name = strName;
		data_replace.description_cell = _replocalData.description_cell;
		data_replace.cost = _replocalData.cost;
		data_replace.fill = _replocalData.fill;
		data_replace.dust = _replocalData.dust;
		data_replace.coin = _replocalData.coin;
		data_replace.ticket = _replocalData.ticket;
		data_replace.revenew_coin = _replocalData.revenew_coin;
		data_replace.revenew_exp = _replocalData.revenew_exp;
		data_replace.revenew_interval = _replocalData.revenew_interval;
		data_replace.open_work_id = _replocalData.open_work_id;
		data_replace.description_book = _replocalData.description_book;
		data_replace.size = _replocalData.size;
		data_replace.rare = _replocalData.rare;
		data_replace.status = iInitStatus;

		m_soDataMonsterMaster.list.Add (data_replace);

		/*
		//データの上書きのコマンドを設定する　
		string strQuery = "replace into " + TABLE_NAME + " (monster_id,name,description_cell,cost,fill,dust,coin,ticket,revenew_coin,revenew_exp,revenew_interval,open_work_id,description_book,size,rare,status) values( '" +
		                  _replocalData.monster_id.ToString () + "',\"" +
		                  strName.ToString () + "\",\"" +
		                  strDescriptionCell.ToString () + "\",'" +
		                  _replocalData.cost.ToString () + "','" +
		                  _replocalData.fill.ToString () + "','" +
		                  _replocalData.dust.ToString () + "',\"" +
		                  _replocalData.coin.ToString () + "\",'" +
		                  _replocalData.ticket.ToString () + "','" +
		                  _replocalData.revenew_coin.ToString () + "','" +
		                  _replocalData.revenew_exp.ToString () + "','" +
		                  _replocalData.revenew_interval.ToString () + "','" +
		                  _replocalData.open_work_id.ToString () + "',\"" +
		                  strDescriptionBook+ "\",'" +
		                  _replocalData.size.ToString () + "','" +
		                  _replocalData.rare.ToString () + "','" +
		                  iInitStatus.ToString ()  + "');";

		Debug.Log( "DBMonsterMaster:"+strQuery);

		//m_sqlDBはDBDataBaseのプロテクト変数
		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);

		query.Step ();      //
		query.Release ();   //解放
		*/
	}


	public List<DataMonsterMaster> Select( string _strWhere = null ){

		List<DataMonsterMaster> ret_list = new List<DataMonsterMaster> ();
		foreach (DataMonsterMaster data in m_soDataMonsterMaster.list) {

			if (data.Equals (_strWhere) == true) {
				ret_list.Add (data);
			}
		}

		return ret_list;
		/*
		string strQuery = "select * from " + TABLE_NAME;

		if (_strWhere != null) {
			strQuery += " where " + _strWhere;
		}

		Debug.Log ("DBWork SelectQuery : "+strQuery);

		//m_sqlDBはDBDataBaseのプロテクト変数
		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);

		List<DataMonsterMaster> ret = new List<DataMonsterMaster> ();

		//テーブルからデータを取ってくる
		while (query.Step ()) {
			DataMonsterMaster data = MakeData (query);
			ret.Add (data);
		}
		query.Release ();
		return ret;
		*/
	}

	public List<DataMonsterMaster> Select(List<string> _whereList ){

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

	public DataMonsterMaster Select( int _iMonsterId ){
		foreach (DataMonsterMaster data in m_soDataMonsterMaster.list) {
			if (data.monster_id == _iMonsterId) {
				return data;
			}
		}
		return new DataMonsterMaster ();
		/*
		DataMonsterMaster ret;
		string strQuery = "select * from " + TABLE_NAME + " where monster_id = '" + _iMonsterId + "';";

		SQLiteQuery query = new SQLiteQuery(m_sqlDB , strQuery );
		if( query.Step() ){
			ret = MakeData (query);
		}
		else {
			ret = new DataMonsterMaster();
		}
		return ret;
		*/
	}


	//テーブル以下全て取ってくる
	public List<DataMonsterMaster> SelectAll()
	{
		return m_soDataMonsterMaster.list;
		/*
		//データをクリア
		data_list.Clear ();

		string strQuery = "SELECT * FROM "+TABLE_NAME;

		SQLiteQuery query = new SQLiteQuery(m_sqlDB,strQuery);

		//テーブルからデータを取ってくる
		while (query.Step ()) {
			DataMonsterMaster data = MakeData (query);
			data_list.Add (data);
		}
		query.Release ();

		return data_list;
		*/
	}


	public DataMonsterMaster MakeData( SQLiteQuery _qr ){
		SQLiteQuery qr = _qr;
		DataMonsterMaster data = new DataMonsterMaster();

		if( m_bDebugLog ){
			//Debug.Log( "key  :" + strKey );
			//Debug.Log( "value:" + strValue );
		}
		data.monster_id = qr.GetInteger("monster_id");
		data.name = qr.GetString("name");
		data.description_cell = qr.GetString("description_cell");
		data.cost = qr.GetInteger("cost");
		data.fill = qr.GetInteger("fill");
		data.dust = qr.GetInteger("dust");
		data.coin = qr.GetInteger("coin");
		data.ticket = qr.GetInteger("ticket");
		data.revenew_coin = qr.GetInteger("revenew_coin");
		data.revenew_exp = qr.GetInteger("revenew_exp");
		data.revenew_interval = qr.GetInteger("revenew_interval");
		data.open_work_id = qr.GetInteger("open_work_id");
		data.description_book = qr.GetString("description_book");
		data.size = qr.GetInteger("size");
		data.rare = qr.GetInteger("rare");
		data.status = qr.GetInteger("status");

		data.name = CommonConvert (data.name);
		data.description_book = CommonConvert (data.description_book);
		data.description_cell = CommonConvert (data.description_cell);


		return data;

	}

	protected override void pickup_data( SQLiteQuery _qr ){
		data_list.Add( MakeData(_qr) );
		return;
	}

}



