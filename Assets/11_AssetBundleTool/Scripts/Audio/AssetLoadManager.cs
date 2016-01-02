using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AssetLoadManager : Singleton<AssetLoadManager>
{

	//空だけどSingleton使用時は誤作動防止のためコンストラクタはつぶしておく
	protected AssetLoadManager ()
	{
	}


	public class LoadDataParam
	{
		public string url;
		public string fileName;
		public string folderName;
		// フォルダの名前が別のばあいはこちら
		public int version;
		public int arr_index;
		// 配列利用時のindex
		public LoadType loadType;
	}


	public enum LoadType
	{
		IconAtlas,
		Atlas,
		EventAtlas,
		GameDataAsset,
		System,
		// 常駐用(UIAtlas,Common,TrueColor)
		Audio,
		BitmapFont
	}

	public LoadSettingData m_LoadSettingData;


	//m_LoadSettingData = ConfigManager.instance.GetLoadSettingData();

	//==========================================================================================
	//	以下起動前に生成型
	//==========================================================================================
	
	public delegate void CallBack (GameObject obj, LoadDataParam loadDataParam);

	public void LoadData (LoadDataParam loadDataParam, CallBack callNext)
	{
		StartCoroutine (LoadStreamingAssets (loadDataParam, callNext));
	}


	// StreamingAssetsからオブジェクトを取得する場合
	public IEnumerator LoadStreamingAssets (LoadDataParam loadDataParam, CallBack callNext)
	{
		
		string resultUrl = "";
		
		resultUrl = "file://" + Application.streamingAssetsPath + "/" + loadDataParam.url;

		
		//Debug.Log ("AssetBundleUrl : " + resultUrl);

		GameObject returnObject = null;
		
		
		// キャッシュシステムの準備が完了するのを待ちます
		while (!Caching.ready) {
			yield return null;
		}
		// 同じバージョンが存在する場合はアセットバンドルをキャッシュからロードするか、またはダウンロードしてキャッシュに格納します。
		using (WWW www = WWW.LoadFromCacheOrDownload (resultUrl, loadDataParam.version)) {
			yield return www;
			if (www.error != null) {
				throw new Exception ("WWWダウンロードにエラーがありました:" + www.error);
			}
			
			AssetBundle bundle = www.assetBundle;

			if (loadDataParam.fileName == "") {
				Instantiate (bundle.mainAsset);
			} else if (loadDataParam.loadType == LoadType.Audio) {
				//Debug.LogWarning ("Filename :" + loadDataParam.fileName);
                GameObject tmpObj = Instantiate (bundle.LoadAsset (loadDataParam.fileName, typeof(GameObject))) as GameObject;
				returnObject = tmpObj;
			} else {
                Instantiate (bundle.LoadAsset (loadDataParam.fileName));
			}
			// メモリ節約のため圧縮されたアセットバンドルのコンテンツをアンロード
			bundle.Unload (false);
			
		}
		
		////Debug.Log(Caching.IsVersionCached( resultUrl, 1));
		////Debug.Log("DownloadAndCache end");

		callNext (returnObject, loadDataParam);
		
	}
}
