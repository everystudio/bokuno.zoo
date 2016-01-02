using UnityEngine;
using System.Collections;

public class CsvTimeTicket : MasterTableBase<CsvTimeTiecketData> {

	private static readonly string FilePath = "csv/time_ticket";
	public void Load() { Load(FilePath); }
}

public class CsvTimeTiecketData : MasterBase
{
	public int num { get; private set; }
	public string reduce { get; private set; }
}



