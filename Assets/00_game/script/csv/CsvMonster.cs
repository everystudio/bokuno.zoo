using UnityEngine;
using System.Collections;

public class CsvMonster : MasterTableBase<CsvMonsterData> {

	private static readonly string FilePath = "csv/monster";
	public void Load() { Load(FilePath); 	
		foreach (CsvMonsterData data in All) {
			Debug.LogError(data.name);
		}
	}
}

public class CsvMonsterData : MasterBase
{
	public int monster_id { get; private set; }
	public string name { get; private set; }
	public string description_cell { get; private set; }
	public int cost { get; private set; }
	public int fill { get; private set; }
	public int dust { get; private set; }
	public int coin { get; private set; }
	public int ticket { get; private set; }
	public int revenew_coin { get; private set; }
	public int revenew_exp { get; private set; }
	public int revenew_interval { get; private set; }
	public int open_work_id { get; private set; }
	public string description_book { get; private set; }
	public int size { get; private set; }
	public int rare { get; private set; }
}




