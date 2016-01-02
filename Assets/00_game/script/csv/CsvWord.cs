using UnityEngine;
using System.Collections;

public class CsvWord : MasterTableBase<CsvWordData> {

	private static readonly string FilePath = "csv/japanese";
	public void Load() { Load(FilePath); }
}

public class CsvWordData : MasterBase
{
	public string key { get; private set; }
	public string word { get; private set; }
}

