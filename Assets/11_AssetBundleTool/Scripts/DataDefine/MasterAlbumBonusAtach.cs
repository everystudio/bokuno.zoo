﻿#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


public class MasterAlbumBonusAtach : MonoBehaviour
{


	public static void Makeholder ()
	{
		AtachFolder ();
	}

	public static void AtachFolder ()
	{

		CreateComponents (typeof(MasterAlbumBonus));
	}

	public static void CreateComponents (Type _type)
	{
		string name = "target";
		string outputPath = SystemSetting.GetAssetSrcPath() + _type.Name + "Prefab.prefab";


		GameObject gameObject = EditorUtility.CreateGameObjectWithHideFlags (
			                          name,
			                          HideFlags.HideInHierarchy,
			typeof(MasterAlbumBonusDataHolder)
		);

		//プレハブにスクリプタブルオブジェクトを設置
		UnityEngine.Object[] assets;

		string assetName = SystemSetting.GetResourcesLoadPath () + _type.Name + "Asset";

		assets = Resources.LoadAll (assetName);
		Debug.LogWarning ("GetObj :" + assetName.ToString ());


		MasterAlbumBonus tmp = new MasterAlbumBonus();

		foreach (UnityEngine.Object asset in assets) {
			if (asset is MasterAlbumBonus) {
				tmp = (MasterAlbumBonus)asset;
			}
		}

		MasterAlbumBonusDataHolder holder = gameObject.GetComponent<MasterAlbumBonusDataHolder> ();

		holder.assetBundleData = tmp;
		SetAssetBundleInfo (gameObject);

		PrefabUtility.CreatePrefab (outputPath, gameObject, ReplacePrefabOptions.ReplaceNameBased);
		Editor.DestroyImmediate (gameObject);
	}

	public static void SetAssetBundleInfo (GameObject _gameObject)
	{

		string user    = Environment.UserName;
		DateTime thisDay = DateTime.Now;
		string makeTime =thisDay.ToString("G");

		AssetBundleInfo info = _gameObject.GetComponent<AssetBundleInfo> ();
		if (info == null) {
			info = _gameObject.AddComponent<AssetBundleInfo> ();
		}

		info.lastAuthor =  user;
		info.makeTime = makeTime;

		info.verID = 100;


	}

}
#endif
