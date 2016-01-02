using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MasterLoadSpriteCSVLoader : EditorWindow
{

	public static void SetScriptableData (Type _type)
	{
		//プレハブにスクリプタブルオブジェクトを設置
		UnityEngine.Object[] assets;
		string assetName = SystemSetting.GetResourcesLoadPath () + _type.Name + "Asset";
		assets = Resources.LoadAll (assetName);

		MasterLoadSprite tmp = new MasterLoadSprite();
		ScriptableObject sObject = null;

		foreach (UnityEngine.Object asset in assets) {
			if (asset is MasterLoadSprite) {
				tmp = (MasterLoadSprite)asset;
				sObject = (ScriptableObject)asset;
			}
		}
		List<MasterLoadSprite.Data> listData = MasterLoadSpriteCSVLoader.GetListDataFromCSV(_type);
		tmp.DataList = listData;

		//最後に保存して変更を確定
		EditorUtility.SetDirty (sObject);
	}


	public static void LoadCSVAndMakeExcel ()
	{
		CSVLoader(typeof(MasterLoadSprite));
	}

	public static void UpdateCSVFile (List<MasterLoadSprite.Data> _dataObject)
	{
		MakeData (typeof( MasterLoadSprite),_dataObject);
	}

	public static void CSVLoader(Type _dataType)
	{
		List<MasterLoadSprite.Data> listData = GetListDataFromCSV(_dataType);
		MasterLoadSpriteExcelLoader.WriteExcelData( listData, typeof(MasterLoadSprite), typeof(MakeDataNamespace.MasterLoadSprite));
	}


	public static List<MasterLoadSprite.Data> GetListDataFromCSV(Type _dataType){

		string filePath = SystemSetting.GetResourcesCSVFilePath() + StringExtensions.UpperCamelToSnake(_dataType.Name) + "";
		TextAsset csv = (TextAsset)Resources.Load(filePath, typeof(TextAsset)) as TextAsset;

		FieldInfo[] fieldInfoList = CSMaker.GetFieldInfo (typeof(MakeDataNamespace.MasterLoadSprite));
		List<string> memberNameList = CSMaker.GetMemberList ( typeof(MakeDataNamespace.MasterLoadSprite));
		List<MasterLoadSprite.Data> listData = new List<MasterLoadSprite.Data>();

        StringReader reader = new StringReader(csv.text);

		Type curType = typeof(MasterLoadSprite.Data);


		int rowNum = -1;

        while (reader.Peek() > -1) {

			MasterLoadSprite.Data lineData = new MasterLoadSprite.Data();
			int columnCount = 0;

            string line = reader.ReadLine();
            string[] values = line.Split(',');

			rowNum++;
			//0行はタイトルなので飛ばす
			if(rowNum == 0){ continue;}

			foreach (string item in values) {
				string columnType = memberNameList[columnCount];
				FieldInfo memberFieldInfo = fieldInfoList[columnCount];

				if (memberFieldInfo.FieldType.ToString () == "System.Int32") {
					string convertStr = ConvertString(item);
					int setNumber = int.Parse(convertStr);
					curType.InvokeMember(columnType,BindingFlags.SetField,null,lineData,new object[] {setNumber});
				} else if (memberFieldInfo.FieldType.ToString () == "System.Single") {
					string convertStr = ConvertString(item);
					float setNumber = float.Parse(convertStr);
					curType.InvokeMember(columnType,BindingFlags.SetField,null,lineData,new object[] {setNumber});
				} else if (memberFieldInfo.FieldType.ToString () == "System.String") {
					string convertStr = ConvertString(item);
					string setString = convertStr.Substring(1, convertStr.Length-2); 
					// エスケープされたダブルクオーテーションがない場合はそのままの文字列を利用する
					if (convertStr.Contains ("\"") == false) {
						setString = convertStr;
					}
					setString = setString.Replace ("\\n","\n");
					curType.InvokeMember(columnType,BindingFlags.SetField,null,lineData,new object[] {setString});
				} else if (memberFieldInfo.FieldType.ToString () == "System.Boolean") {
					if(item == "1"){
						curType.InvokeMember(columnType,BindingFlags.SetField,null,lineData,new object[] {true});
					}else{
						curType.InvokeMember(columnType,BindingFlags.SetField,null,lineData,new object[] {false});
					}


				} else {
					Debug.LogError ("データ定義に異常があります!!!!!!!" + memberFieldInfo.FieldType);
											//異常なデータは出力してはいけないので強制停止
					break;
				}

				columnCount++;
			}
			listData.Add(lineData);
        }
        return listData;
	}

	private static string ConvertString (string targetStr) {
		string convertStr = targetStr;
		if (convertStr.IndexOf ('=') != -1) {
			convertStr = convertStr.Replace("=","");
			convertStr = convertStr.Replace("\"","");
		}

		return convertStr;

//		System.Text.Encoding dest = System.Text.Encoding.ASCII;
//		System.Text.Encoding src = System.Text.Encoding.GetEncoding("Shift_JIS");
//		byte [] temp = src.GetBytes(convertStr);
//		byte[] utf_temp = System.Text.Encoding.Convert(dest,src,temp);
//		string utf_str = dest.GetString(utf_temp);
//
//		return utf_str;
	}




	public static void MakeData ( Type _type, List<MasterLoadSprite.Data> _dataObject)
	{
		string filePath = SystemSetting.GetCSVFilePath () + _type.Name + ".csv";
		List<MasterLoadSprite.Data> _MasterLoadSprite = _dataObject;

		FieldInfo[] fieldInfoList = CSMaker.GetFieldInfo (typeof(MakeDataNamespace.MasterLoadSprite));
		List<string> memberNameList = CSMaker.GetMemberList ( typeof(MakeDataNamespace.MasterLoadSprite));
		Type curType = typeof(MasterLoadSprite.Data);

		using (Workaholism.IO.CsvWriter writer = new Workaholism.IO.CsvWriter (filePath, Encoding.GetEncoding ("utf-8"))) {

			List<string> headData = new List<string>();

			foreach (string fieldName in memberNameList) {
				headData.Add("\""+fieldName+"\"");
			}
			writer.WriteLine (headData);

			foreach (MasterLoadSprite.Data curData in _MasterLoadSprite) {
				
				List<string> lineData = new List<string>();

				int columnCount = 0;

				foreach (string fieldName in memberNameList) {

					string getString = curType.InvokeMember(fieldName,BindingFlags.GetField,null,curData,null).ToString();

					FieldInfo memberFieldInfo = fieldInfoList[columnCount];

					string addString = "";

					if (memberFieldInfo.FieldType.ToString () == "System.Int32") {
						addString = getString;
					} else if (memberFieldInfo.FieldType.ToString () == "System.Single") {
						addString = getString; 
					} else if (memberFieldInfo.FieldType.ToString () == "System.String") {
						addString = "\"" + getString + "\"";
					} else if (memberFieldInfo.FieldType.ToString () == "System.Boolean") {
						if(getString == "true"){
							addString = "1";
						}else{
							addString = "0";
						}
					} else {
						Debug.LogError ("データ定義に異常があります!!!!!!!" + memberFieldInfo.FieldType);
												//異常なデータは出力してはいけないので強制停止
						break;
					}

					lineData.Add(addString);
					columnCount++;

				}

				writer.WriteLine (lineData);

			}
		}
	}

}
