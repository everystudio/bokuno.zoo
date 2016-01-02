#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MasterJackpotLotSecondAnimationObjectMaker : ScriptableWizard
{
	public static void MakeScriptableWizard ()
	{
		ObjectCreate ();
	}

	public static void ObjectCreate ()
	{
		MasterJackpotLotSecondAnimation loadSettingData = CreateInstance<MasterJackpotLotSecondAnimation> ();
		string path = SystemSetting.GetScriptableobjectPath() + "MasterJackpotLotSecondAnimationAsset" + ".asset";

		AssetDatabase.CreateAsset (loadSettingData, path);
		AssetDatabase.ImportAsset (path);
		EditorUtility.UnloadUnusedAssets ();
	}
}
#endif
