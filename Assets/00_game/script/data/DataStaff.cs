using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class DataStaff : SearchBase {

	public int m_staff_serial;
	public int m_office_serial;
	public int m_staff_id;
	public int m_item_serial;
	public string m_setting_time;
	public string m_create_time;

	public int staff_serial { get{ return m_staff_serial;} set{m_staff_serial = value; } }
	public int office_serial { get{ return m_office_serial;} set{m_office_serial = value; } }
	public int staff_id { get{ return m_staff_id;} set{m_staff_id = value; } }
	public int item_serial { get{ return m_item_serial;} set{m_item_serial = value; } }
	public string setting_time { get{ return m_setting_time;} set{m_setting_time = value; } }
	public string create_time { get{ return m_create_time;} set{m_create_time = value; } }


	public int GetShisyutsuParHour(){
		int iRet = 0;
		CsvStaffData staff_csv = DataManager.GetStaff (staff_id);

		int iCount = 3600 / staff_csv.expenditure_interval;
		iRet += staff_csv.expenditure * iCount;

		return iRet;
	}


	public int GetPayGold( bool _bCollect ){

		int iPay = 0;
		CsvStaffData csv_staff = DataManager.GetStaff (staff_id);

		double diffSec = TimeManager.Instance.GetDiffNow (setting_time).TotalSeconds * -1.0d;

		double dCount = diffSec / csv_staff.expenditure_interval;

		double dRet = dCount * csv_staff.expenditure;
		if (_bCollect && 0 < dRet ) {

			int iAmari = (int)diffSec % csv_staff.expenditure_interval;
			string strResetTime = TimeManager.StrGetTime (iAmari * -1);

			Dictionary< string , string > dict = new Dictionary< string , string > ();
			dict.Add ("setting_time", "\"" + strResetTime + "\"");
			GameMain.dbStaff.Update (staff_serial, dict);
		}
		//int iCount = 

		return (int)dRet;
	}

}

