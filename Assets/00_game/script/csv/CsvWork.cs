using UnityEngine;
using System.Collections;

public class CsvWork : MasterTableBase<CsvWorkData> {

	private static readonly string FilePath = "csv/work";
	public void Load() { Load(FilePath); }
}

public class CsvWorkData : MasterBase
{
	public int work_id { get; private set; }
	public string title { get; private set; }
	public string description { get; private set; }
	public int type { get; private set; }
	public int level { get; private set; }
	public int appear_work_id { get; private set; }
	public int exp { get; private set; }
	public string difficulty { get; private set; }
	public int prize_ticket { get; private set; }
	public int prize_coin{ get; private set; }
	public int prize_monster { get; private set; }
	public int mission_level{ get; private set; }
	public int mission_monster{ get; private set; }
	public int mission_staff{ get; private set; }
	public int mission_item{ get; private set; }
	public int mission_collect{ get; private set; }
	public int mission_tweet{ get; private set; }
	public int mission_login{ get; private set; }
	public int mission_num{ get; private set; }

}



