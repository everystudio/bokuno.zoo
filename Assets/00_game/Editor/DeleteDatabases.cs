using UnityEditor;
using System.Runtime.InteropServices;

using UnityEngine;
using System.Collections;

public class DeleteDatabases : EditorWindow {

	static DeleteDatabases w;
	static string[] m_strDBFilenameList;

	[MenuItem("Tools/Check/Delete Databases")]
	static void CreateWindow(){

		m_strDBFilenameList = DeleteDatabases.filename_search();
		filename_search();
		w = (DeleteDatabases)EditorWindow.GetWindow (typeof(DeleteDatabases));

		float fHeight = 50.0f ;//+ 15.0f * m_strDBFilenameList.Length;

		w.minSize = new Vector2( 200.0f , fHeight );
	}

	// ディレクトリ以下に存在するファイルを取得
	static string[] filename_search(){

		string strPersistentDataPath = System.IO.Path.Combine (Application.persistentDataPath , "");

		string[] filenames = System.IO.Directory.GetFiles(@strPersistentDataPath , "*.db" , System.IO.SearchOption.TopDirectoryOnly );
		// System.IO.SearchOption.TopDirectoryOnly : 指定したディレクトリのみ
		// System.IO.SearchOption.AllDirectories   : 指定したディレクトリのサブディレクトリも含む

		/*
		foreach( string strFilename in filenames ){
			Debug.Log( strFilename );
		}
		*/
		return filenames;
	}

	/// <summary]] > 
	/// UI表示.
	/// </summary]] > 
	void OnGUI ()
	{
		GUILayout.Label ("データベースファイルの削除を行います", EditorStyles.boldLabel);
		GUILayout.Label ("(確認なしなのでご注意ください)", EditorStyles.boldLabel);
		GUILayout.Space(20f);

		GUILayout.Label ("消せるデータベース一覧", EditorStyles.boldLabel);

		m_strDBFilenameList = filename_search();

		if( 0 < m_strDBFilenameList.Length){
			foreach (string file_url in m_strDBFilenameList) {
				string filemane = System.IO.Path.GetFileName(file_url);
				if (GUILayout.Button ( filemane )) {
					DeleteStart (file_url);
				}
			}
		}
	}

	void DeleteStart( string _strDBFilename ){

		string full_path = System.IO.Path.Combine (Application.persistentDataPath , _strDBFilename );

		System.IO.File.Delete( full_path );

		CreateWindow();


	}



	// .dbが付いているファイルのみを削除する





}
