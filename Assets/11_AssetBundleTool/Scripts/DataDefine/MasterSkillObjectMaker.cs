#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MasterSkillObjectMaker : ScriptableWizard
{
	public static void MakeScriptableWizard ()
	{
		ObjectCreate ();
	}

	public static void ObjectCreate ()
	{
		MasterSkill loadSettingData = CreateInstance<MasterSkill> ();
		string path = SystemSetting.GetScriptableobjectPath() + "MasterSkillAsset" + ".asset";

		AssetDatabase.CreateAsset (loadSettingData, path);
		AssetDatabase.ImportAsset (path);
		EditorUtility.UnloadUnusedAssets ();
	}
}
#endif
