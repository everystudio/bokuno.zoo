using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class KVSData : SODataParam {
	public string m_key;
	public string m_value;

	public string key{ get{ return m_key;} set{m_key = value; } }
	public string value{ get{ return m_value;} set{m_value = value; } }
}

public class DBKvs : DBDataBase {

	private bool m_bDebugLog = false;
	public const string FILE_NAME = "SODataKvs";

	public List<KVSData> data_list = new List<KVSData>();
	public SODataKvs m_soDataKvs;

	public DBKvs( SQLiteAsync _Async ):base(_Async){
	}
	public DBKvs( string _strAsyncName ):base(_strAsyncName){
		m_soDataKvs = PrefabManager.Instance.PrefabLoadInstance (FILE_NAME).GetComponent<SODataKvs> ();
	}
	public const string READ_ERROR_STRING = "sql_datamanager_read_error";

	/*
		ここのWriteは即書き込み
	*/
	public void Write( string _strKey , string _strValue ){

		//Debug.LogError (string.Format ("key:{0} value:{1}", _strKey, _strValue));

		foreach (KVSData data in m_soDataKvs.list) {
			if (data.key.Equals (_strKey) == true) {
				data.value = _strValue;
				return;
			}
		}

		KVSData insert_data = new KVSData ();
		insert_data.key = _strKey;
		insert_data.value = _strValue;
		m_soDataKvs.list.Add (insert_data);
		return;
		/*
		// オープンしてる前提
		string strQuery = "replace into kvs (key,value) values ('" + _strKey + "','" + _strValue + "');";
		if( m_bDebugLog ){
			Debug.Log( "DBKvs.Write:"+strQuery);
		}
		SQLiteQuery query = new SQLiteQuery(m_sqlDB , strQuery );
		query.Step();
		query.Release();
		*/
		return;
	}
	public void WriteString(string _strKey , string _strValue){
		this.Write(_strKey , _strValue);
	}

	public void WriteInt( string _strKey , int _intValue ){
		this.WriteString( _strKey , _intValue.ToString());
	}

	public string Read( string _strKey ){
		foreach (KVSData data in m_soDataKvs.list) {
			if (data.key.Equals (_strKey) == true) {
				return data.value;
			}
		}
		//Debug.LogError (string.Format ("unknown key:{0}", _strKey));
		return READ_ERROR_STRING;
		/*
		string ret = "dummy";
		string strQuery = "select * from kvs where key = '" + _strKey + "';";
		if( m_bDebugLog ){
			//Debug.Log( "DBKvs.Read:" + strQuery );
		}
		SQLiteQuery query = new SQLiteQuery(m_sqlDB , strQuery );
		if( query.Step() ){
			ret = query.GetString("value");
		}
		else {
			ret = READ_ERROR_STRING;
		}
		query.Release();
		return ret;
		*/
	}

	public int ReadInt( string _strKey ){
		string strValue = this.Read(_strKey);
		if( Define.READ_ERROR_STRING == strValue ){
			strValue = "0";
			WriteInt (_strKey, 0);
		}
		return int.Parse(strValue);
	}

	protected override void pickup_data( SQLiteQuery _qr ){
		KVSData data = new KVSData();
		string strKey = _qr.GetString("key");
		string strValue = _qr.GetString("value");
		if( m_bDebugLog ){
			//Debug.Log( "key  :" + strKey );
			//Debug.Log( "value:" + strValue );
		}
		data.key  = strKey;
		data.value= strValue;
		data_list.Add( data );
		return;
	}



}



