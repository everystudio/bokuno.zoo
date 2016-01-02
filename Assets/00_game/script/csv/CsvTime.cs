using UnityEngine;
using System.Collections;

public class CsvTime : MasterTableBase<CsvTimeData> {

	private static readonly string FilePath = "csv/time";
	public void Load() { Load(FilePath); }
}

public class CsvTimeData : MasterBase
{
	public int type { get; private set; }
	public int now { get; private set; }
	public int delta_time { get; private set; }
}



