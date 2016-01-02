using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//public class BundleLoader : SingletonMonoBehaviourFast<BundleLoader>{
public class BundleLoader : MonoBehaviour{

	public LoadSettingData m_LoadSettingData;
	protected EditPlayerSettingsData m_EditPlayerSettingsData;
	private string m_strLoadURL = "";


	void Start ()
	{
		Init ();
	}

	public void Init ()
	{
		LoadSetting ();
	}
	
	public LoadSettingData GetLoadSettingData ()
	{
		return m_LoadSettingData;
	}
	

	public delegate void CallBack( GameObject obj, LoadManager.LoadDataParam loadDataParam);	
	
	public void LoadData ( LoadManager.LoadDataParam loadDataParam, CallBack callNext)	
	{
		StartCoroutine ( LoadAssetsData( loadDataParam, callNext)) ;
	}


    // 指定されたパスからオブジェクトを取得する場合
	public IEnumerator LoadAssetsData (LoadManager.LoadDataParam loadDataParam, CallBack callNext)
	{
//		string resultUrl = "file://" + SystemSetting.GetStreamingAssetspath() + m_strLoadURL + loadDataParam.url;
        string resultUrl = m_strLoadURL + loadDataParam.url;
		GameObject returnObject = null;
        Debug.Log ("Loading :" + resultUrl+ " ver="+loadDataParam.version);

		// キャッシュシステムの準備が完了するのを待ちます
		while (!Caching.ready){
			yield return null;
		}
		// 同じバージョンが存在する場合はアセットバンドルをキャッシュからロードするか、またはダウンロードしてキャッシュに格納します。

		using (WWW www = WWW.LoadFromCacheOrDownload ( resultUrl, loadDataParam.version)) {
			yield return www;
			if (www.error != null) {
				throw new Exception ("WWWダウンロードにエラーがありました:" + www.error);
			}

			AssetBundle bundle = www.assetBundle;
            if (loadDataParam.fileName == "") {
                Instantiate (bundle.mainAsset);
            } else if (loadDataParam.loadType == LoadParam.LoadType.Atlas ||
            loadDataParam.loadType == LoadParam.LoadType.IconAtlas ||
            loadDataParam.loadType == LoadParam.LoadType.EventAtlas ||
			loadDataParam.loadType == LoadParam.LoadType.System ||
            loadDataParam.loadType == LoadParam.LoadType.GameDataAsset ||
            loadDataParam.loadType == LoadParam.LoadType.Audio ||
            loadDataParam.loadType == LoadParam.LoadType.BitmapFont) {
                Debug.Log ("loadDataParam.fileName=" + loadDataParam.fileName);
//				bundle.LoadAsset (loadDataParam.fileName);
                GameObject tmpObj = Instantiate (bundle.LoadAsset (loadDataParam.fileName, typeof(GameObject))) as GameObject;
                returnObject = tmpObj;
			} else{
                Instantiate (bundle.LoadAsset (loadDataParam.fileName));
			}
			// メモリ節約のため圧縮されたアセットバンドルのコンテンツをアンロード
			bundle.Unload (false);
			
		} // memory is freed from the web stream (www.Dispose() gets called implicitly)

		callNext( returnObject, loadDataParam);
		
	}
	

	public void LoadSetting ()
	{
		UnityEngine.Object[] assets;
		assets = Resources.LoadAll("LoadSettingData");
		
		foreach (UnityEngine.Object asset in assets)
		{
			if (asset is LoadSettingData)
			{
			    m_LoadSettingData = (LoadSettingData)asset;
			}
		}

        m_EditPlayerSettingsData = ConfigManager.instance.GetEditPlayerSettingsData();
        m_strLoadURL = m_EditPlayerSettingsData.m_strS3Url;

	}



	//==========================================================================================
	//	以下AssetBundleList読み込み専用
	//==========================================================================================


	public delegate void LoadAssetbundleListCallBack( AssetBundleData data);


	public void LoadAssetbundleList ( LoadAssetbundleListCallBack callback)	
	{
		StartCoroutine ( LoadAssetbundleListGo ( callback)) ;
	}

	//Assetbundleリスト専用のローダー
	public IEnumerator LoadAssetbundleListGo( LoadAssetbundleListCallBack callback){

		// キャッシュシステムの準備が完了するのを待ちます
		while (!Caching.ready){
			yield return null;
		}

		//TODO 後で修正
		int assetVersion = 0;
		//int assetVersion = DataManager.Instance.asset_version;

		//Debug.Log ("####### LoadAssetbundleList Ver :" + assetVersion );

		int version = assetVersion;
		AssetBundleData assetBundleData;

//        string resultUrl = "file://" + SystemSetting.GetStreamingAssetspath() +   SystemSetting.GetAdminAssetListPath () + SystemSetting.GetDataListName () + "."+DEFINE.ASSET_BUNDLE_PREFIX+".unity3d";
		EditPlayerSettingsData data = ConfigManager.instance.GetEditPlayerSettingsData();
		string resultUrl = data.m_strS3Url + Define.ASSET_BUNDLES_ROOT +   SystemSetting.GetAdminAssetListPath () + SystemSetting.GetDataListName () +".unity3d";

		//Debug.Log ("Loading :"+m_strLoadURL);
		Debug.Log ("LoadAssetbundleListGo:" + resultUrl);
		// 同じバージョンが存在する場合はアセットバンドルをキャッシュからロードするか、またはダウンロードしてキャッシュに格納します。
		using (WWW www = WWW.LoadFromCacheOrDownload ( resultUrl, version)) {
			yield return www;

			if (www.error != null) {
				throw new Exception ("WWWダウンロードにエラーがありました:" + www.error);
			}
			AssetBundle bundle = www.assetBundle;
            GameObject tmpObj = Instantiate (bundle.LoadAsset( SystemSetting.GetDataListName (),typeof(GameObject))) as GameObject;
			AssetBundleDataHolder assetBundleDataHolder = tmpObj.GetComponent<AssetBundleDataHolder>();
			assetBundleData = assetBundleDataHolder.assetBundleData;

			if(tmpObj == null){
				Debug.LogError ("アセットバンドルList内容に異常があります");
			}

			tmpObj.transform.parent = DataContainer.Instance.gameObject.transform;

			// メモリ節約のため圧縮されたアセットバンドルのコンテンツをアンロード
			bundle.Unload (false);
		}
		callback ( assetBundleData);
	}


    static BundleLoader instance = null;
    public static BundleLoader Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = GameObject.Find("BundleLoader");
                if( obj == null )
                {
                    obj = new GameObject("BundleLoader");
                }

                instance = obj.GetComponent<BundleLoader>();

                if(instance == null)
                {
                    instance = obj.AddComponent<BundleLoader>() as BundleLoader;
                }

            }
            return instance;
        }
    }
}
