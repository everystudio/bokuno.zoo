using UnityEngine;
using System.Collections;

public class CsvLevel : MasterTableBase<CsvLevelData> {

	private static readonly string FilePath = "csv/level";
	public void Load() { Load(FilePath); }
}

public class CsvLevelData : MasterBase
{
	public int level { get; private set; }
	public int need_exp { get; private set; }
}






