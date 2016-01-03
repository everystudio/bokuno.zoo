using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class DatabaseAssetEditor : ScriptableObject {
	[Range(0,30)]
	public int number = 10;
	public bool toggle = false;

	public const string FILE_PATH_ITEM = "Assets/Resources/SODataItem.asset";
	public const string FILE_PATH = "Assets/Resources";

	[MenuItem ("Example/StringTest")]
	static void StringTest ()
	{
		string[] test = "item_serial != 0 ".Split (' ');
		foreach (string str in test) {
			Debug.Log (str);
		}

        Debug.Log(Application.persistentDataPath);
	}

	[MenuItem ("Example/WhereCheck")]
	static void WhereTest ()
	{
		for (int i = 0; i < 10; i++) {
			CsvMonsterData monster_csv = DataManager.GetMonster (i);
			Debug.Log ( string.Format( "name:{0} revin:{1}" , monster_csv.name , monster_csv.revenew_interval));
		}
	}


	/*
	[MenuItem ("Example/WhereCheck")]
	static void WhereTest ()
	{

		string strCheck = "item_serial != 0 and category != 3";
		string[] test = strCheck.Split (' ');

		SODataItem m_soDataItem;
		UnityEngine.Object so_asset;
		so_asset = Resources.Load (DBItem.FILE_NAME);
		m_soDataItem = (SODataItem)so_asset;

		List<DataItem> ret = new List<DataItem> ();
		foreach (DataItem data in m_soDataItem.list) {
			if (data.Equals (strCheck) == true) {
				ret.Add (data);
			}
		}

		foreach (DataItem data in ret) {
			Debug.Log (data.item_serial);
		}
	}


	[MenuItem ("Example/Load ExampleAsset")]
	static void LoadExampleAsset ()
	{

		SODataItem m_soDataItem;
		UnityEngine.Object so_asset;
		so_asset = Resources.Load (DBItem.FILE_NAME);
		m_soDataItem = (SODataItem)so_asset;

		int iCount = 0;
		foreach (DataItem data in m_soDataItem.list) {
			Debug.Log (data.item_serial);
			data.item_serial = iCount;
			iCount += 1;
		}
	}

	[MenuItem ("Example/Create ExampleAsset")]
	static void CreateExampleAsset ()
	{

		string file_path = "";

		file_path = string.Format ("{0}/{1}.asset", FILE_PATH, DBItem.FILE_NAME);
		if (System.IO.File.Exists (file_path) == false) {
			SODataItem resources_data = CreateInstance<SODataItem> ();
			AssetDatabase.CreateAsset (resources_data, file_path);
			AssetDatabase.Refresh ();
		}

		file_path = string.Format ("{0}/{1}.asset", FILE_PATH, DBKvs.FILE_NAME);
		if (System.IO.File.Exists (file_path) == false) {
			SODataKvs resources_data = CreateInstance<SODataKvs> ();
			AssetDatabase.CreateAsset (resources_data, file_path);
			AssetDatabase.Refresh ();
		}

		file_path = string.Format ("{0}/{1}.asset", FILE_PATH, DBItemMaster.FILE_NAME);
		if (System.IO.File.Exists (file_path) == false) {
			SODataItemMaster resources_data = CreateInstance<SODataItemMaster> ();
			AssetDatabase.CreateAsset (resources_data, file_path);
			AssetDatabase.Refresh ();
		}

		file_path = string.Format ("{0}/{1}.asset", FILE_PATH, DBMonster.FILE_NAME);
		if (System.IO.File.Exists (file_path) == false) {
			SODataMonster resources_data = CreateInstance<SODataMonster> ();
			AssetDatabase.CreateAsset (resources_data, file_path);
			AssetDatabase.Refresh ();
		}

		file_path = string.Format ("{0}/{1}.asset", FILE_PATH, DBMonsterMaster.FILE_NAME);
		if (System.IO.File.Exists (file_path) == false) {
			SODataMonsterMaster resources_data = CreateInstance<SODataMonsterMaster> ();
			AssetDatabase.CreateAsset (resources_data, file_path);
			AssetDatabase.Refresh ();
		}

		file_path = string.Format ("{0}/{1}.asset", FILE_PATH, DBWork.FILE_NAME);
		if (System.IO.File.Exists (file_path) == false) {
			SODataWork resources_data = CreateInstance<SODataWork> ();
			AssetDatabase.CreateAsset (resources_data, file_path);
			AssetDatabase.Refresh ();
		}

		file_path = string.Format ("{0}/{1}.asset", FILE_PATH, DBStaff.FILE_NAME);
		if (System.IO.File.Exists (file_path) == false) {
			SODataStaff resources_data = CreateInstance<SODataStaff> ();
			AssetDatabase.CreateAsset (resources_data, file_path);
			AssetDatabase.Refresh ();
		}

	}

		*/

	[MenuItem ("Example/Reset AssetData")]
	static void reset_data ()
	{

		string file_path = "";

		GameObject obj;

		obj = PrefabManager.Instance.PrefabLoadInstance (DBItem.FILE_NAME);
		//obj = PrefabManager.Instance.PrefabLoadInstance ("NameChange");
		obj.GetComponent<SODataItem> ().list.Clear ();
		obj = PrefabManager.Instance.PrefabLoadInstance (DBItemMaster.FILE_NAME);
		obj.GetComponent<SODataItemMaster> ().list.Clear ();
		obj = PrefabManager.Instance.PrefabLoadInstance (DBKvs.FILE_NAME);
		obj.GetComponent<SODataKvs> ().list.Clear ();
		obj = PrefabManager.Instance.PrefabLoadInstance (DBMonster.FILE_NAME);
		obj.GetComponent<SODataMonster> ().list.Clear ();
		obj = PrefabManager.Instance.PrefabLoadInstance (DBMonsterMaster.FILE_NAME);
		obj.GetComponent<SODataMonsterMaster> ().list.Clear ();
		obj = PrefabManager.Instance.PrefabLoadInstance (DBWork.FILE_NAME);
		obj.GetComponent<SODataWork> ().list.Clear ();
		obj = PrefabManager.Instance.PrefabLoadInstance (DBStaff.FILE_NAME);
		obj.GetComponent<SODataStaff> ().list.Clear ();
		/*
		file_path = string.Format ("{0}/{1}.asset", FILE_PATH, DBItem.FILE_NAME);
		if (System.IO.File.Exists (file_path) == false) {
			SODataItem resources_data = CreateInstance<SODataItem> ();
			AssetDatabase.CreateAsset (resources_data, file_path);
			AssetDatabase.Refresh ();
		}

		file_path = string.Format ("{0}/{1}.asset", FILE_PATH, DBKvs.FILE_NAME);
		if (System.IO.File.Exists (file_path) == false) {
			SODataKvs resources_data = CreateInstance<SODataKvs> ();
			AssetDatabase.CreateAsset (resources_data, file_path);
			AssetDatabase.Refresh ();
		}

		file_path = string.Format ("{0}/{1}.asset", FILE_PATH, DBItemMaster.FILE_NAME);
		if (System.IO.File.Exists (file_path) == false) {
			SODataItemMaster resources_data = CreateInstance<SODataItemMaster> ();
			AssetDatabase.CreateAsset (resources_data, file_path);
			AssetDatabase.Refresh ();
		}

		file_path = string.Format ("{0}/{1}.asset", FILE_PATH, DBMonster.FILE_NAME);
		if (System.IO.File.Exists (file_path) == false) {
			SODataMonster resources_data = CreateInstance<SODataMonster> ();
			AssetDatabase.CreateAsset (resources_data, file_path);
			AssetDatabase.Refresh ();
		}

		file_path = string.Format ("{0}/{1}.asset", FILE_PATH, DBMonsterMaster.FILE_NAME);
		if (System.IO.File.Exists (file_path) == false) {
			SODataMonsterMaster resources_data = CreateInstance<SODataMonsterMaster> ();
			AssetDatabase.CreateAsset (resources_data, file_path);
			AssetDatabase.Refresh ();
		}

		file_path = string.Format ("{0}/{1}.asset", FILE_PATH, DBWork.FILE_NAME);
		if (System.IO.File.Exists (file_path) == false) {
			SODataWork resources_data = CreateInstance<SODataWork> ();
			AssetDatabase.CreateAsset (resources_data, file_path);
			AssetDatabase.Refresh ();
		}

		file_path = string.Format ("{0}/{1}.asset", FILE_PATH, DBStaff.FILE_NAME);
		if (System.IO.File.Exists (file_path) == false) {
			SODataStaff resources_data = CreateInstance<SODataStaff> ();
			AssetDatabase.CreateAsset (resources_data, file_path);
			AssetDatabase.Refresh ();
		}
		*/
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
