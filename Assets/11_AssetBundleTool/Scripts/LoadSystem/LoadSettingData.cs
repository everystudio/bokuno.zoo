using UnityEngine;
 
public class LoadSettingData : ScriptableObject
{
	public string loadURL;
	public LoadParam.LoadMode editorLoadMode;
	public string iOSLoadURL;	
	public LoadParam.LoadMode iOSLoadMode;
	
	public string wwwLocalServerURL;
	public string wwwAwsServerURL;
	public LoadParam.WWWServerType wwwServerType;

	public LoadParam.LoadMode gameDataLoadMode;

}