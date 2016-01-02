#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MasterSlotProbTableObjectMaker : ScriptableWizard
{
	public static void MakeScriptableWizard ()
	{
		ObjectCreate ();
	}

	public static void ObjectCreate ()
	{
		MasterSlotProbTable loadSettingData = CreateInstance<MasterSlotProbTable> ();
		string path = SystemSetting.GetScriptableobjectPath() + "MasterSlotProbTableAsset" + ".asset";

		AssetDatabase.CreateAsset (loadSettingData, path);
		AssetDatabase.ImportAsset (path);
		EditorUtility.UnloadUnusedAssets ();
	}
}
#endif
