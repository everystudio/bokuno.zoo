using UnityEngine;
using System.Collections;

public class CsvStaff : MasterTableBase<CsvStaffData> {

	private static readonly string FilePath = "csv/staff";
	public void Load() { Load(FilePath); }
}

public class CsvStaffData : MasterBase
{
	public int staff_id { get; private set; }
	public string name { get; private set; }
	public string description { get; private set; }
	public int cost { get; private set; }
	public int coin { get; private set; }
	public int ticket { get; private set; }
	public int expenditure { get; private set; }
	public int expenditure_interval { get; private set; }
	public int effect_param { get; private set; }
	public int effect_num { get; private set; }
}



//staff_id	name	description	cost	coin	ticket	expenditure	expenditure_interval	effect_param	effect_num




