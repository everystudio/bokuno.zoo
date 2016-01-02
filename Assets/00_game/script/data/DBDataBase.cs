using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class DBDataBase {

	//暗号化DB作成用
	public void SetDBPassword () {
		m_sqlDB.Key ("");
		m_sqlDB.Rekey (Define.DB_PASSWORD);
		m_sqlDB.Key (Define.DB_PASSWORD);
	}

	//暗号化解除
	public void ResetDBPassword () {
		m_sqlDB.Key (Define.DB_PASSWORD);
		m_sqlDB.Rekey ("");
		m_sqlDB.Key ("");
	}
	public DBDataBase( SQLiteAsync _sqlAsync ){
		m_sqlAsync = _sqlAsync;
	}
	public DBDataBase( string _strAsyncName ){
		m_sqlAsync = SQLiteManager.Instance.GetSQLiteAsync(_strAsyncName);
		return;
	}

	private string m_strKey;
	private ThreadQueue.TaskControl m_tLastTaskControl;
	public ThreadQueue.TaskControl Open( string filename, string _strDirectory , object state , string _strKey){
		m_strKey = _strKey;
		//Debug.Log("called Open");
		//ThreadQueue.TaskControl ret = m_sqlAsync.Query( sql , SelectQueryCreated , this );
		ThreadQueue.TaskControl ret = m_sqlAsync.AutoOpen( filename, _strDirectory , AutoOpenCallback , this );
		m_tLastTaskControl = ret;
		return ret;
	}
	public ThreadQueue.TaskControl Open( string filename, string _strDirectory , string _strKey ){
		return this.Open( filename,  _strDirectory , this , _strKey );
	}

	// とりあえず簡単に呼び出したい人向け（時間ができた時にちゃんと理解してね！）
	// このあたりはデフォルト引数禁止してます！勝手な変更はNG!!
	public ThreadQueue.TaskControl Select( string _strTableName , string _strOption ){
		// とりあえずあんまり指定しないで読みたいとき
		string sql = "select * from " + _strTableName + _strOption;
		// この簡単関数を利用する場合はもれなくasyncの設定がされるので安心！
		ThreadQueue.TaskControl ret = m_sqlAsync.Query( sql , SelectQueryCreated , this );
		m_tLastTaskControl = ret;
		return ret;
	}

	public ThreadQueue.TaskControl Update( string _strTableName , string _strParams , string _strWhere ){
		// とりあえずあんまり指定しないで上書きしたいとき
		string sql = "update " + _strTableName + " set " + _strParams + " " + _strWhere;
		// この簡単関数を利用する場合はもれなくasyncの設定がされるので安心！
		ThreadQueue.TaskControl ret = m_sqlAsync.Query( sql , UpdateQueryCreated , this );
		m_tLastTaskControl = ret;
		return ret;
	}

	public ThreadQueue.TaskControl Close(){
		ThreadQueue.TaskControl ret = m_sqlAsync.Close( CloseCallback , this );
		return ret;
	}



	// 利用時に必ず代入してください・コンストラクタで強要したかったけどやり方よくわからん
	// nullチェックとか入れておくとよいかも
	protected SQLiteAsync m_sqlAsync;
	protected SQLiteDB m_sqlDB {
		get{ return m_sqlAsync.GetSQLiteDB();}
	}

	// ユーザー実装処理 ---------------------------------------------------
	// 継承先でデータを取得・格納する関数
	protected abstract void pickup_data( SQLiteQuery _qr );

	// DBオープン処理
	virtual public void AutoOpenCallback( bool _bSucceed , object _state ){
		if( _bSucceed ){
			//Debug.Log( "AutoOpenCallback true m_strKey=" + m_strKey );
			if( m_strKey != ""){
				m_sqlAsync.Key( m_strKey );
			}
		}
		else {
			//Debug.Log( "AutoOpenCallback false");
		}
	}
	// DBクローズ処理
	virtual public void CloseCallback( object _state ){
		return;
	}


	// カスタム処理（お好み・必要に合わせて実装しなおしてください） --------------
	virtual public void SelectStepCallback( SQLiteQuery _qr , bool _bStep , object _state ){
		if( _bStep ){
			pickup_data( _qr );
			m_sqlAsync.Step( _qr , SelectStepCallback , _state );
		}
		else {
			m_sqlAsync.Release( _qr , SelectQueryReleased , null );
		}
	}
	public void SelectQueryCreated( SQLiteQuery _qr , object _state ){
		m_sqlAsync.Step( _qr , SelectStepCallback , _state );
		return;
	}
	virtual public void SelectQueryReleased(object state){
		return;
	}

	// お好みに合わせて(updateとかでも一応コールバック指定してないということ聞いてくれあないので) --------------
	// 以下３つの関数はアップデートするときとかの空読みさせる関数群です。
	// 用途に合わせて新しく作るなりしてください
	public void UpdateQueryCreated( SQLiteQuery _qr , object _state){
		m_sqlAsync.Step( _qr , UpdateStepCallback , _state);
	}
	public void UpdateStepCallback( SQLiteQuery _qr , bool _bStep , object _state ){
		m_sqlAsync.Release( _qr , UpdateQueryReleased , null);
	}
	public void UpdateQueryReleased(object state){
		return;
	}
	// --------------------------------------------------------------------------------------------

	private string [] BaseWord = new string [5]{
		"最",
		"一",
		"言",
		"漢",
		"退",
	};

	protected string CommonConvert( string _str ){
		string retString = _str;

		for (int i = 0; i < BaseWord.Length; i++) {
			retString = retString.Replace ("word" + i.ToString() , BaseWord[i] );
		}
		return retString;

	}

	protected string CommonConvertToSaveDb( string _str ){

		string retString = _str;
		for (int i = 0; i < BaseWord.Length; i++) {
			retString = retString.Replace ( BaseWord[i] , "word" + i.ToString() );
		}
		return retString;
	}

}







