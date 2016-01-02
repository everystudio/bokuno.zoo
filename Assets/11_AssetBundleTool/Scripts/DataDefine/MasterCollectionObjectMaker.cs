#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MasterCollectionObjectMaker : ScriptableWizard
{
	public static void MakeScriptableWizard ()
	{
		ObjectCreate ();
	}

	public static void ObjectCreate ()
	{
		MasterCollection loadSettingData = CreateInstance<MasterCollection> ();
		string path = SystemSetting.GetScriptableobjectPath() + "MasterCollectionAsset" + ".asset";

		AssetDatabase.CreateAsset (loadSettingData, path);
		AssetDatabase.ImportAsset (path);
		EditorUtility.UnloadUnusedAssets ();
	}
}
#endif
