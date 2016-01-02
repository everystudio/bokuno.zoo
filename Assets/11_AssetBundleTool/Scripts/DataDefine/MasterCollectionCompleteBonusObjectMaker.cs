#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MasterCollectionCompleteBonusObjectMaker : ScriptableWizard
{
	public static void MakeScriptableWizard ()
	{
		ObjectCreate ();
	}

	public static void ObjectCreate ()
	{
		MasterCollectionCompleteBonus loadSettingData = CreateInstance<MasterCollectionCompleteBonus> ();
		string path = SystemSetting.GetScriptableobjectPath() + "MasterCollectionCompleteBonusAsset" + ".asset";

		AssetDatabase.CreateAsset (loadSettingData, path);
		AssetDatabase.ImportAsset (path);
		EditorUtility.UnloadUnusedAssets ();
	}
}
#endif
