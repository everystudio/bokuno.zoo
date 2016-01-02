#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MasterLoadPrefabObjectMaker : ScriptableWizard
{
	public static void MakeScriptableWizard ()
	{
		ObjectCreate ();
	}

	public static void ObjectCreate ()
	{
		MasterLoadPrefab loadSettingData = CreateInstance<MasterLoadPrefab> ();
		string path = SystemSetting.GetScriptableobjectPath() + "MasterLoadPrefabAsset" + ".asset";

		AssetDatabase.CreateAsset (loadSettingData, path);
		AssetDatabase.ImportAsset (path);
		EditorUtility.UnloadUnusedAssets ();
	}
}
#endif
