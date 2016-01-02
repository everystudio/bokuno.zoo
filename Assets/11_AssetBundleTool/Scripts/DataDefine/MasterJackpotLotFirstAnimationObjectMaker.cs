#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MasterJackpotLotFirstAnimationObjectMaker : ScriptableWizard
{
	public static void MakeScriptableWizard ()
	{
		ObjectCreate ();
	}

	public static void ObjectCreate ()
	{
		MasterJackpotLotFirstAnimation loadSettingData = CreateInstance<MasterJackpotLotFirstAnimation> ();
		string path = SystemSetting.GetScriptableobjectPath() + "MasterJackpotLotFirstAnimationAsset" + ".asset";

		AssetDatabase.CreateAsset (loadSettingData, path);
		AssetDatabase.ImportAsset (path);
		EditorUtility.UnloadUnusedAssets ();
	}
}
#endif
