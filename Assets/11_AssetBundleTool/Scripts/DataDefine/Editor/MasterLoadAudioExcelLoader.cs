using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using System.Xml.Serialization;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Reflection;


public class MasterLoadAudioExcelLoader : EditorWindow
{

	public static string dataTitle;
	public static List<string> memberList;
	public static List<string> typeList;
	static int makeCellLow;

	static HSSFCellStyle headStyle;
	static ICellStyle blackBorder;
	static HSSFCellStyle headStylePrefab;
	static HSSFCellStyle headStyleBundle;

	static FieldInfo[] fieldInfoList;
	static List<string> memberNameList;


	public static void LoadExcelAndMakeCSV ()
	{
		ReadExcel (typeof( MasterLoadAudio));
	}

	private static List<MasterLoadAudio.Data> ReadExcel (Type _type)
	{
		string loadPath = SystemSetting.GetExcelSeetPath () + _type.Name + "Sheet.xls";
		List<MasterLoadAudio.Data> dataList = new List<MasterLoadAudio.Data> ();

		using (FileStream stream = File.Open (loadPath, FileMode.Open, FileAccess.Read)) {
			IWorkbook book = new HSSFWorkbook (stream);
			ISheet sheet = book.GetSheet ("MainSeet");
		
			for (int i = 1; i < sheet.LastRowNum; i++) {
					
				IRow row = sheet.GetRow (i);
				int iColumn = 0;

				MasterLoadAudio.Data tmpData = new MasterLoadAudio.Data ();

				//DataSwitch :
				tmpData.filename = row.GetCell (iColumn++).StringCellValue;
				tmpData.version = (int)row.GetCell (iColumn++).NumericCellValue;
				tmpData.path = row.GetCell (iColumn++).StringCellValue;
				tmpData.audio_type = (int)row.GetCell (iColumn++).NumericCellValue;
				tmpData.del_flg = (int)row.GetCell (iColumn++).NumericCellValue;

				iColumn++;

				dataList.Add(tmpData);
			}
		}

		MasterLoadAudioCSVLoader.UpdateCSVFile(dataList);

		return dataList;

	}


	/// <summary>
	/// データの書き込み.
	/// </summary>
	public static void WriteExcelData ( List<MasterLoadAudio.Data> listData, Type _dataType, Type _dataTypeSrc)
	{
		dataTitle = _dataType.Name;

		fieldInfoList = CSMaker.GetFieldInfo (_dataTypeSrc);
		memberNameList = CSMaker.GetMemberList ( _dataTypeSrc);

		Int32 iRow = 0;
		IRow row;
		ICell cell;
  		
		int generateCellCount = SystemSetting.GetInitGenerateCell();

		// ワークブックオブジェクト生成
		HSSFWorkbook workbook = new HSSFWorkbook ();
 
		// シートオブジェクト生成
		ISheet sheet1 = workbook.CreateSheet ("MainSeet");
		MakeSeetStyle(workbook);
		makeCellLow = 0;
		DataCellMake (sheet1,memberNameList);

		// セルを作成する（垂直方向）
		for (iRow = (makeCellLow); iRow <( generateCellCount + makeCellLow); iRow++) {
			MakeCell( sheet1, _dataType, iRow, listData);
		}

		row = sheet1.CreateRow (generateCellCount + makeCellLow);

		//アセットバンドル名
		cell = row.CreateCell (0);
		cell.SetCellValue ("end");
		cell.CellStyle = blackBorder;

		string dataURL = SystemSetting.GetExcelSeetPath() + dataTitle + "Sheet.xls";
		// Excelファイル出力
		OutputExcelFile (dataURL, workbook);
		AssetDatabase.Refresh (ImportAssetOptions.ImportRecursive);
	}

	static bool BoolSetting (string _val)
	{
		if (_val == "false") {
			return false;
		}else if(_val == "true"){
			return true;

		}else{
				Debug.LogError("Excelシートのboolパラメータにエラーがあります");

		}
		return  false;
	}

	static void OutputExcelFile (String strFileName, HSSFWorkbook workbook)
	{
		FileStream file = new FileStream (strFileName, FileMode.Create);
		workbook.Write (file);
		file.Close ();
	}


	//Excelスタイル定義
	private static void MakeSeetStyle (HSSFWorkbook workbook)
	{

		// 本体のスタイル（黒線）
		blackBorder = workbook.CreateCellStyle ();
		blackBorder.BorderBottom = BorderStyle.THIN;
		blackBorder.BorderLeft = BorderStyle.THIN;
		blackBorder.BorderRight = BorderStyle.THIN;
		blackBorder.BorderTop = BorderStyle.THIN;
		blackBorder.BottomBorderColor = HSSFColor.BLACK.index;
		blackBorder.LeftBorderColor = HSSFColor.BLACK.index;
		blackBorder.RightBorderColor = HSSFColor.BLACK.index;
		blackBorder.TopBorderColor = HSSFColor.BLACK.index;


		// ヘッダのスタイル
		headStyle = (HSSFCellStyle)workbook.CreateCellStyle ();

		headStyle.FillForegroundColor = IndexedColors.BRIGHT_GREEN.Index;
		headStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
 

		headStylePrefab = (HSSFCellStyle)workbook.CreateCellStyle ();
		headStylePrefab.FillForegroundColor = IndexedColors.LEMON_CHIFFON.Index;
		headStylePrefab.FillPattern = FillPatternType.SOLID_FOREGROUND;

		headStyleBundle = (HSSFCellStyle)workbook.CreateCellStyle ();
		headStyleBundle.FillForegroundColor = IndexedColors.GOLD.Index;
		headStyleBundle.FillPattern = FillPatternType.SOLID_FOREGROUND;

		// フォントスタイル
		IFont font = workbook.CreateFont ();
		font.FontHeightInPoints = 12;
		font.FontName = "Arial";
		font.Boldweight = (short)FontBoldWeight.BOLD;


		headStyle.SetFont (font);


	}

	static void DataCellMake ( ISheet _sheet1, List<string> _dataList)
	{
		IRow row = _sheet1.CreateRow (makeCellLow);
		makeCellLow++;

		row.HeightInPoints = 24;

		for (int i = 0; i < _dataList.Count; i++) {
			ICell cell = row.CreateCell (i);
			cell.SetCellValue (_dataList [i]);

			cell.CellStyle = headStyle;
			_sheet1.SetColumnWidth (i, 255 * 20);
		}
	}


	public static void MakeCell (ISheet _sheet1, Type _dataType, int _iRow, List<MasterLoadAudio.Data> listData)
	{
		//Type makeDataType = typeof(List<MasterLoadAudio.Data>);

		IRow row;
		ICell cell;

		int cellIndex = 0;

		row = _sheet1.CreateRow (_iRow);

		memberList = memberNameList;

		string objName = "None :";

		typeList = new List<string> ();

		int countNum = 0;
		int curDataRow = _iRow-1;

		Type tmpType = typeof(MasterLoadAudio.Data);
		MasterLoadAudio.Data tmpData = listData[curDataRow];

		foreach (string item in memberList) {
			cell = row.CreateCell (cellIndex++);
			FieldInfo memberFieldInfo = fieldInfoList [countNum];

			if (memberFieldInfo.FieldType.ToString () == "System.Int32") {
				int setInt = (int)tmpType.InvokeMember (item, BindingFlags.GetField, null, tmpData, null);
				cell.SetCellValue (setInt);
			} else if (memberFieldInfo.FieldType.ToString () == "System.Single") {
				float setFloat = (float)tmpType.InvokeMember (item, BindingFlags.GetField, null, tmpData, null);
				cell.SetCellValue (setFloat);
			} else if (memberFieldInfo.FieldType.ToString () == "System.String") {
				objName = (string)tmpType.InvokeMember (item, BindingFlags.GetField, null, tmpData, null);
				cell.SetCellValue (objName);
			} else if (memberFieldInfo.FieldType.ToString () == "System.Boolean") {
				bool setbool = (bool)tmpType.InvokeMember (item, BindingFlags.GetField, null, tmpData, null);
				string setString = "";

				if(!setbool){
					setString = "false";
				}else{
					setString = "true";

				}
				cell.SetCellValue (setString);

			} else {
				Debug.LogError ("データ定義に異常があります!!!!!!!   :" + memberFieldInfo.FieldType.ToString ());

				//異常なデータは出力してはいけないので強制停止
				break;
			}

			cell.CellStyle = blackBorder;

			countNum++;
		}
	}
}

