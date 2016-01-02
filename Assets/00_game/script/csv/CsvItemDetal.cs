using UnityEngine;
using System.Collections;

public class CsvItemDetal : MasterTableBase<CsvItemDetailData> {

	private static readonly string FilePath = "csv/item_detail";
	public void Load() { Load(FilePath); }
}

public class CsvItemDetailData : MasterBase
{
	public int item_id { get; private set; }
	public int level { get; private set; }
	public int need_level { get; private set; }
	public int coin { get; private set; }
	public int get_exp { get; private set; }
	public int cost { get; private set; }
	public int revenue_rate { get; private set; }
	public int area { get; private set; }
}


