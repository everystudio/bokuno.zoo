using UnityEngine;
using System;
using System.Threading;

public class SQLiteAsync
{
	
	#region Public

	public SQLiteQuery lastSqliteQuery;
	public SQLiteQuery GetSQLiteQuery(){
		return lastSqliteQuery;
	}
	public SQLiteAsync()
	{
	}

	public void Key( string _strKey ){
		if( db != null ){
			//Debug.Log( "key_lock or unlock;"+_strKey);

			//暗号化確認
			if (PlayerPrefs.HasKey (Define.PLAYREPREFS_KEY_IS_ENC) && PlayerPrefs.GetInt (Define.PLAYREPREFS_KEY_IS_ENC) == 1) {
				db.Key (_strKey);
			} else {
				//パス設定
				db.Key ("");
				db.Rekey (_strKey);
				db.Key (_strKey);
				PlayerPrefs.SetInt (Define.PLAYREPREFS_KEY_IS_ENC, 1);
			}
			//DBバージョンの保存
			PlayerPrefs.SetString (Define.PLAYREPREFS_KEY_DB_VER,Define.APP_VERSION);
		}
		return;
	}
	public SQLiteDB GetSQLiteDB(){
		if( db != null ){
			////Debug.Log( "not null db");
		}
		return db;
	}
	
	
	public delegate void OpenCallback(bool succeed, object state);
	public ThreadQueue.TaskControl Open(string filename, OpenCallback callback, object state)
	{
		return ThreadQueue.QueueUserWorkItem(new ThreadQueue.WorkCallback(OpenDatabase), new WaitCallback(OpenDatabaseComplete), new OpenState(filename,callback,state));
	}
	// 基本的にはオープンとやることは同じなんですけど、
	// StreamingAssetにファイルがあるかどうかの確認を行う
	public ThreadQueue.TaskControl AutoOpen(string filename, string _strDirectory , OpenCallback callback, object state ){
		string pathDB = System.IO.Path.Combine (Application.persistentDataPath, filename );
		//Debug.Log( pathDB );
		bool bFileCtrlError = false;
		if (!System.IO.File.Exists (pathDB)) {
			//Debug.Log ("DB not Exists in documents folder");
			//original path
			string sourcePath = System.IO.Path.Combine (Application.streamingAssetsPath, _strDirectory + filename );
			//Debug.Log( sourcePath );
			if (sourcePath.Contains ("://")) {
				// Android	
				WWW www = new WWW (sourcePath);
				// Wait for download to complete - not pretty at all but easy hack for now 
				// and it would not take long since the data is on the local device.
				while (!www.isDone) {
						;
				}
				if (String.IsNullOrEmpty (www.error)) {
						System.IO.File.WriteAllBytes (pathDB, www.bytes);
				} else {
					bFileCtrlError = true;
					//CanExQuery = false;
					// エラーで返す
				}
		
			} else {
				// Mac, Windows, Iphone
				//validate the existens of the DB in the original folder (folder "streamingAssets")
				if (System.IO.File.Exists (sourcePath)) {
					//copy file - alle systems except Android
					System.IO.File.Copy (sourcePath, pathDB, true);
				} else {
					bFileCtrlError = true;
					//CanExQuery = false;
					// エラーで返す
					Debug.Log ("ERROR: the file DB named " + filename + " doesn't exist in the StreamingAssets Folder, please copy it there.");
				}
			}
		}
		if( bFileCtrlError ){
			//Debug.Log( "open error");
			ThreadQueue.TaskControl retError = new ThreadQueue.TaskControl();
			retError.Cancel();
			return retError;
		}
		
		//Debug.Log( "open:"+pathDB );
		return Open(pathDB, callback, state);
	}
	
	public delegate void CloseCallback(object state);
	public ThreadQueue.TaskControl Close(CloseCallback callback, object state)
	{
		return ThreadQueue.QueueUserWorkItem(new ThreadQueue.WorkCallback(CloseDatabase), new WaitCallback(CloseDatabaseComplete), new CloseState(callback,state));
	}
	
	public delegate void QueryCallback(SQLiteQuery qr, object state);
	public ThreadQueue.TaskControl Query(string query, QueryCallback callback, object state)
	{
		//Debug.Log( "ThreadQueue.TaskControl Query :"+ state);
		return ThreadQueue.QueueUserWorkItem(new ThreadQueue.WorkCallback(CreateQuery), new WaitCallback(CreateQueryComplete), new QueryState(query,callback,state));
	}
	
	public delegate void StepCallback(SQLiteQuery qr, bool rv, object state);
	public ThreadQueue.TaskControl Step(SQLiteQuery qr, StepCallback callback, object state)
	{
		return ThreadQueue.QueueUserWorkItem(new ThreadQueue.WorkCallback(StepQuery), new WaitCallback(StepQueryComplete), new StepState(qr,callback,state));
	}
	
	public delegate void ReleaseCallback(object state);
	public ThreadQueue.TaskControl Release(SQLiteQuery qr, ReleaseCallback callback, object state)
	{
		return ThreadQueue.QueueUserWorkItem(new ThreadQueue.WorkCallback(ReleaseQuery), new WaitCallback(ReleaseQueryComplete), new ReleaseState(qr,callback,state));
	}

	#endregion
	
	
	#region Implementation
	
	//
	// members
	//
	SQLiteDB db = null;
	//
	// internal classes
	//
	class OpenState
	{
		string 			filename;
		OpenCallback	callback;
		object 			state;
		bool			succeed;
		
		public string 			Filename	{ get { return filename; } }
		public OpenCallback		Callback	{ get { return callback; } }
		public object 			State		{ get { return state; } }
		public bool				Succeed		{ get { return succeed; } set { succeed = value; } }
		
		public OpenState(string filename, OpenCallback callback, object state){
			this.filename = filename; 
			this.callback = callback;
			this.state = state;
		}
	}
	
	class CloseState
	{
		CloseCallback	callback;
		object 			state;
		
		public CloseCallback	Callback	{ get { return callback; } }
		public object 			State		{ get { return state; } }
		
		public CloseState(CloseCallback callback, object state){
			this.callback = callback;
			this.state = state;
		}
	}
	
	class QueryState 
	{
		string 			sql;
		QueryCallback	callback;
		object 			state;
		SQLiteQuery		query;
		
		public string 			Sql 		{ get { return sql; } }
		public SQLiteQuery 		Query 		{ get { return query; } set { query = value; } }
		public QueryCallback	Callback	{ get { return callback; } }
		public object 			State		{ get { return state; } }
		
		public QueryState(string sql, QueryCallback callback, object state){
			//Debug.Log("construct QueryState sql= "+sql+" state=" +state);
			this.sql = sql; 
			this.callback = callback;
			this.state = state;
		}
	}

	class StepState 
	{
		SQLiteQuery		query;
		StepCallback	callback;
		object 			state;
		bool			step;
		
		public SQLiteQuery		Query 		{ get { return query; } }
		public StepCallback		Callback	{ get { return callback; } }
		public object 			State		{ get { return state; } }
		public bool 			Step		{ get { return step; } set { step = value; } }
		
		public StepState(SQLiteQuery query, StepCallback callback, object state){
			this.query = query; 
			this.callback = callback;
			this.state = state;
		}
	}

	class ReleaseState
	{
		SQLiteQuery		query;
		ReleaseCallback	callback;
		object 			state;
		
		public SQLiteQuery		Query 		{ get { return query; } }
		public ReleaseCallback	Callback	{ get { return callback; } }
		public object 			State		{ get { return state; } }
		
		public ReleaseState(SQLiteQuery query, ReleaseCallback callback, object state){
			this.query = query; 
			this.callback = callback;
			this.state = state;
		}
	}
	
	//
	// functions
	//
	private void OpenDatabase(ThreadQueue.TaskControl control, object state)
    {
		//Debug.Log( "OpenDatabase");
		OpenState opState = state as OpenState;
        try
        {
			db = new SQLiteDB();
            db.Open(opState.Filename);
			opState.Succeed = true;
        }
        catch (Exception ex)
        {
			opState.Succeed = false;
            Debug.LogError("SQLiteAsync : OpenDatabase : Exception : " + ex.Message);
        }

        if( db != null ){
        	//Debug.Log( "db dekitayo");
        }
        else {

        	//Debug.Log( "db dekinakattayo");
        }
    }
	
	private void OpenDatabaseComplete(object state)
	{
		//Debug.Log( "OpenDatabaseComplete");
		OpenState opState = state as OpenState;
		if( opState.Callback!=null ){
			opState.Callback(opState.Succeed, opState.State);
			//Debug.Log( "callback call");
		}
	}
	
    private void CloseDatabase(ThreadQueue.TaskControl control, object state)
    {
        try
        {
			if( db != null )
			{
            	db.Close();
				db = null;
			}
			else
			{
				throw new Exception( "Database not ready!" );
			}
        }
        catch (Exception ex)
        {
			Debug.LogError("SQLiteAsync : Exception : " + ex.Message);
        }
    }
	
	private void CloseDatabaseComplete(object state)
	{
		CloseState clState = state as CloseState;
		if( clState.Callback!=null ){
			clState.Callback(clState.State);
		}
	}
	
	private void CreateQuery(ThreadQueue.TaskControl control, object state)
	{
		//Debug.Log( "CreateQuery :" + state );
        try
        {
			if( db != null )
			{
				QueryState qrState = state as QueryState;
				qrState.Query = new SQLiteQuery(db,qrState.Sql);
				lastSqliteQuery = qrState.Query;
			}
			else
			{
				throw new Exception( "Database not ready!" );
			}/**/
        }
        catch (Exception ex)
        {
			Debug.LogError("SQLiteAsync : CreateQuery : Exception : " + ex.Message);
        }
	}
	
	private void CreateQueryComplete(object state)
	{
		//Debug.Log( "CreateQueryComplete state=" + state );
		QueryState qrState = state as QueryState;
		// ここで上書きすればいいのか？
		lastSqliteQuery = qrState.Query;

		if( qrState.Callback != null){
			qrState.Callback(qrState.Query, qrState.State);
		}
	}
	
	private void StepQuery(ThreadQueue.TaskControl control, object state)
	{
        try
        {
			if( db != null )
			{
				StepState stState = state as StepState;
				stState.Step = stState.Query.Step();
			}
			else
			{
				throw new Exception( "Database not ready!" );
			}
        }
        catch (Exception ex)
        {
			Debug.LogError("SQLiteAsync : Exception : " + ex.Message);
        }
	}
	
	private void StepQueryComplete(object state)
	{
		StepState stState = state as StepState;
		stState.Callback(stState.Query,stState.Step,stState.State);
	}

	private void ReleaseQuery(ThreadQueue.TaskControl control, object state)
	{
        try
        {
			if( db != null )
			{
				ReleaseState rlState = state as ReleaseState;
				rlState.Query.Release();
			}
			else
			{
				throw new Exception( "Database not ready!" );
			}
        }
        catch (Exception ex)
        {
			Debug.LogError("SQLiteAsync : Exception : " + ex.Message);
        }
	}
	
	private void ReleaseQueryComplete(object state)
	{
		ReleaseState rlState = state as ReleaseState;
		rlState.Callback(rlState.State);
	}
	
	private void EmptyCallback(object obj)
	{
		// nothing to do here
	}

	
	#endregion
}
