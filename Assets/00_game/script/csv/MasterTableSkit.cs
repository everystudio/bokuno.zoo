using UnityEngine;
using System.Collections;

public class MasterTableSkit : MasterTableBase<SkitDataCSV>
{
	private static readonly string FilePath = "CSV/master_skit";
	public void Load() { Load(FilePath); }
}

public class SkitDataCSV : MasterBase
{
	public int skit_id { get; private set; }
	public int skit_type { get; private set; }
	public int page { get; private set; }
	public int page_next { get; private set; }
	public string memo { get; private set; }
	public string message { get; private set; }
	public string image { get; private set; }
	public int del_flg { get; private set; }
}




