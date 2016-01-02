using UnityEngine;
using System.Collections;

public class CsvItem : MasterTableBase<CsvItemData> {

	private static readonly string FilePath = "csv/item";
	public void Load() { Load(FilePath); }
}

public class CsvItemData : MasterBase
{
	public int item_id { get; private set; }
	public string name { get; private set; }
	public int category { get; private set; }
	public int type { get; private set; }
	public int cell_type { get; private set; }
	public string description { get; private set; }
	public int need_coin { get; private set; }
	public int need_ticket { get; private set; }
	public int need_money { get; private set; }
	public int size { get; private set; }
	public int cost { get; private set; }
	public int area { get; private set; }
	public int revenue { get; private set; }
	public int revenue_interval { get; private set; }
	public int revenue_up { get; private set; }
	public int production_time { get; private set; }
	public int setting_limit { get; private set; }
	public int sub_parts_id { get; private set; }
	public int open_item_id { get; private set; }
	public int revenue_up2 { get; private set; }
	public int add_coin { get; private set; }
}





