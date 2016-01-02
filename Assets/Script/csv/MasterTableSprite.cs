using UnityEngine;
using System.Collections;

public class MasterTableSprite : MasterTableBase<MasterSpriteCSV>  {

	private static readonly string FilePath = "CSV/master_load_sprite";
	public void Load() { Load(FilePath); }
}

public class MasterSpriteCSV : MasterBase
{
	public string filename { get; private set; }
	public int version { get; private set; }
	public int pre_load { get; private set; }
	public string path { get; private set; }
	public int del_flg { get; private set; }
}





