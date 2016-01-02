using UnityEngine;
using System.Collections;

public class MasterTableMapChip : MasterTableBase<MapChipCSV>
{
	private static readonly string FilePath = "csv/start";
	public void Load() { Load(FilePath); }
}

public class MapChipCSV : MasterBase
{
	public int serial { get; private set; }
	public int item_id { get; private set; }
	public int x { get; private set; }
	public int y { get; private set; }
	public int width { get; private set; }
	public int height { get; private set; }
}


