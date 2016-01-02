using UnityEngine;
using System.Collections;

public class LoadParam : MonoBehaviour {

	public enum LoadMode
	{
		LocalResources,
		LocalSteamingAsset,
		Web,
	}
	
	public enum LoadSystemIndex{
		UI,
        Symbol,
 		MAX
	}

	public enum LoadType
	{
		GameDataAsset,
		IconAtlas,
		Atlas,
		EventAtlas,
		System,				// 常駐用(UIAtlas,Common,TrueColor)
		Audio,
		BitmapFont
	}

	public enum	WWWServerType
	{	
		Local,
		AWS,
	}	
}
