using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;


//public class LoadManager : SingletonMonoBehaviourFast<LoadManager>{
public class LoadManager : MonoBehaviour {

	[System.Serializable]
	public class LoadDataParam {
		public string url;
		public string fileName;
		public string folderName;				// フォルダの名前が別のばあいはこちら
		public int version;
		public int arr_index;						// 配列利用時のindex
		public LoadParam.LoadType loadType;
	}

	public List<LoadDataParam> loadDataParamList;

	public int loadcount = 0;
    private int loadMax = 0;
    private int loadNow = 0;
    private float nowTime = 0f;
    private float loadStartTime = 0.0f;

    public CtrlDownloadDialog ctrlDownloadDialog;
    private CtrlDownloadDialog m_ctrlDownloadDialog;

	private bool m_bInitialaize;

	public LoadSettingData m_LoadSettingData;
	private bool m_IsLoadEnd = false;

	private AssetBundleData assetBundleDataList;

    [SerializeField]
    private UIAtlas[] m_atlSystem = new UIAtlas[(int)LoadParam.LoadSystemIndex.MAX];
    [SerializeField]
    private UIAtlas[] m_atlSystemTemp = new UIAtlas[(int)LoadParam.LoadSystemIndex.MAX];
    private int m_atlKey = 0;

    [SerializeField]
    private List<UIFont> m_atlBitmapFont = new List<UIFont>();
    private List<UIFont> m_atlBitmapFontTemp = new List<UIFont>();

    [SerializeField]
    public AudioSettingData audioSettingData = null;

    public UIAtlas GetUIAtlas(){
        return m_atlSystem[(int)LoadParam.LoadSystemIndex.UI];
    }

    public UIAtlas GetSymbolAtlas(){
        return m_atlSystem[(int)LoadParam.LoadSystemIndex.Symbol];
    }

    public void ReplaceAtlas(){
		// ここでのリプレイスはしない
		/*
		Debug.Log ("temp num = "+ m_atlSystemTemp.Length.ToString());


        for( int i = 0 ; i < (int)LoadParam.LoadSystemIndex.MAX ; i++ ){
			Debug.Log (m_atlSystemTemp [i]);
            if( m_atlSystemTemp.Length != 0 && m_atlSystemTemp[i] != null ){
                m_atlSystem[i].replacement = m_atlSystemTemp[i];
				//m_atlSystemTemp[i]  = null;
            }
        }

        for( int i = 0 ; i < m_atlBitmapFont.Count ; i++ ){
            if( m_atlBitmapFontTemp.Count != 0 && m_atlBitmapFontTemp[i] != null ){
                m_atlBitmapFont[i].replacement = m_atlBitmapFontTemp[i];
                m_atlBitmapFontTemp[i]  = null;
            }
        }
        */
    }

	public void LoadGameDataCallBack ( GameObject obj, LoadDataParam loadDataParam)
	{
		if(obj !=null){
			Debug.LogWarning ("データ読み込み :" + obj);
			DataContainer.DataSet( obj, loadDataParam);

			obj.transform.parent = DataContainer.Instance.gameObject.transform;

		}else{
			Debug.LogError ("オブジェクトが読めていません :" + loadDataParam.fileName);
		}

//		loadcount++;
		if (loadcount < loadDataParamList.Count) {
			//Debug.Log("次の処理" + "===========================================");
			LoadingAssetBundle ();
		} else {
            //ゲージをMAXに
            m_ctrlDownloadDialog.SetSliderValue( 1.0f);
            m_ctrlDownloadDialog.SetPercentage ( loadMax, loadMax);
            m_ctrlDownloadDialog.ForceFadeOut ();
            m_IsLoadEnd = true;
		}
	}

    public void LoadCallBack ( GameObject obj, LoadDataParam loadDataParam)
    {
        if(obj !=null){
            if( loadDataParam.loadType == LoadParam.LoadType.Atlas){
//                UIAtlas atlas = obj.GetComponent<UIAtlas>();
//                m_atlMonsterTexture.Add(atlas);
            } else if( loadDataParam.loadType == LoadParam.LoadType.IconAtlas){
//                UIAtlas atlas = obj.GetComponent<UIAtlas>();
//                m_atlMonsterIcon.Add(atlas);
            } else if( loadDataParam.loadType == LoadParam.LoadType.System ){
                UIAtlas atlas = obj.GetComponent<UIAtlas>();
                m_atlSystemTemp[m_atlKey] = atlas;
                m_atlKey++;
            } else if (loadDataParam.loadType == LoadParam.LoadType.EventAtlas) {
//                UIAtlas atlas = obj.GetComponent<UIAtlas>();
//                m_atlEventAtlasTemp.Add(atlas);
            } else if(loadDataParam.loadType == LoadParam.LoadType.BitmapFont){
                UIFont bitmapFont = obj.GetComponent<UIFont>();
                m_atlBitmapFontTemp.Add(bitmapFont);
            }
            obj.transform.parent = DataContainer.Instance.gameObject.transform;
        }else{
            Debug.LogError ("オブジェクトが読めていません :" + loadDataParam.fileName);
        }

//        loadcount++;
        if (loadcount < loadDataParamList.Count) {
            LoadingAssetBundle ();
        } else {
            //ゲージをMAXに
            m_ctrlDownloadDialog.SetSliderValue( 1.0f);
            m_ctrlDownloadDialog.SetPercentage ( loadMax, loadMax);
            m_ctrlDownloadDialog.ForceFadeOut ();
            m_IsLoadEnd = true;
        }
    }

    public void AudioDataCallBack ( GameObject obj, LoadDataParam loadDataParam)
    {
//        Debug.LogWarning("File Name :" + loadDataParam.fileName);

        if(obj !=null){
            if (loadDataParam.fileName == "AudioDataHolder_MedalGamePrefab") {

                AudioDataHolder audioDataHolder = obj.gameObject.GetComponent<AudioDataHolder> ();

//                Debug.LogWarning ("audioDataHolder :" + audioDataHolder.name);

                audioSettingData = audioDataHolder.audioSettingData;

//                Debug.LogWarning("SetEnd :" + audioSettingData);

				Debug.LogError ("dont use");
				//SoundManager.Instance.Init (audioSettingData);
            } else {
                Debug.LogError ("オーディオオブジェクトの設定に異常があります :" + loadDataParam.fileName);
            }
            obj.transform.parent = DataContainer.Instance.gameObject.transform;
        }else{
            Debug.LogError ("オブジェクトが読めていません :" + loadDataParam.fileName);
        }

//        loadcount++;
        if (loadcount < loadDataParamList.Count) {
            //Debug.Log("次の処理" + "===========================================");
            LoadingAssetBundle ();
        } else {
            //ゲージをMAXに
            m_ctrlDownloadDialog.SetSliderValue( 1.0f);
            m_ctrlDownloadDialog.SetPercentage ( loadMax, loadMax);
            m_ctrlDownloadDialog.ForceFadeOut ();
            m_IsLoadEnd = true;
        }
    }

	private LoadParam.LoadMode loadMode;

	public bool GetIsLoadEnd(){
		return m_IsLoadEnd;
	}

	public void Start ()
	{
		//TODO バージョン管理システムが入るまではキャッシュをクリア。後で削除
		// タイトルのButtonCacheClearに移動
		//Caching.CleanCache();
//		Initialize();
	}

	public void Initialize ( bool bInit = false )
	{
		if( bInit  ) {
			m_bInitialaize = false;
			m_IsLoadEnd = false;
            m_atlKey = 0;

            Transform[] objList = DataContainer.Instance.gameObject.GetComponentsInChildren<Transform>();
            if (objList.Length > 0) {
                foreach( Transform child in objList ) {
                    if( child.gameObject ){
                        Destroy( child.gameObject );
                    }
                }
            }

            m_atlBitmapFontTemp.Clear ();
		}
		// 初期化済みの場合はここで帰る
		if (m_bInitialaize) {
			return;
		}
		m_bInitialaize = true;

		//Loaderの初期化
		if(BundleLoader.Instance == null){
			BundleLoader.Instance.Init();

		}
		BundleLoader.Instance.Init();

        nowTime = 0f;
        loadStartTime = Time.time;

        OpenLoadDialog ();

		m_LoadSettingData = BundleLoader.Instance.GetLoadSettingData();

		//iOSとUnityエディタ上の切り替え
		#if UNITY_IPHONE  && !UNITY_EDITOR
			loadMode = m_LoadSettingData.iOSLoadMode;
		#else
			loadMode = m_LoadSettingData.editorLoadMode;
		#endif

		LoadAssetbundleList();

	}

	private void LoadBranch ()
	{

		LoadInit ();//ロードリストのクリア

		foreach (AssetBundleData.Param param in assetBundleDataList.list) {

			//Debug.LogWarning("param.type :" + param.type);

            if (param.type == 0) {
                loadMax++;
                //loadGameDataAssetMax++;
                SetLoadPath (SystemSetting.GetassetBundlePathPlane (), param.assetbundleName, LoadParam.LoadType.GameDataAsset, param.version);
            } else if (param.type == 4) {
                loadMax++;
                SetLoadPath (param.folderName, param.assetbundleName, LoadParam.LoadType.System, param.version);
            } else if (param.type == 6) {
                loadMax++;
                string strAudio = "AudioDataAsset";
                if (m_LoadSettingData.editorLoadMode == LoadParam.LoadMode.Web) {
                    strAudio = "GameDataAsset";
                }
                SetLoadPath (strAudio, param.assetbundleName, LoadParam.LoadType.Audio, param.version);
				//後でタイプ読みが必要かも
            } else if (param.type == 7) {
                loadMax++;
                SetLoadPath (param, LoadParam.LoadType.BitmapFont);
//			} else if (param.type == 6) {
//				loadAudioDataAssetMax++;
//				string strAudio = "AudioDataAsset";
//				if (m_LoadSettingData.editorLoadMode == LoadParam.LoadMode.Web) {
//					strAudio = "GameDataAsset";
//				}
//				SetLoadPath (strAudio, param.assetbundleName, LoadParam.LoadType.Audio, param.version);
			}else {
				//Debug.Log( "unknown type=" + param.type );
			}

		}

	//	Debug.LogWarning (" loadGameDataAsset数 :" + loadGameDataAssetMax);

		//loadMax = loadIconMax + loadMonsterAtlasMax + loadEventAtlasMax + loadGameDataAssetMax + m_iLoadSystemAtlasMax +loadAudioDataAssetMax;
		LoadingAssetBundle ();

		//データ読み込みソースの分岐
		if (loadMode == LoadParam.LoadMode.LocalResources) {
			m_IsLoadEnd = true;
		}
	}

	private void LoadAssetbundleList(){
		BundleLoader.Instance.LoadAssetbundleList (CallBackLoadAssetbundleList);
	}
	public void CallBackLoadAssetbundleList (AssetBundleData data){
		//Debug.LogWarning ("CallBack成功"+data);
		assetBundleDataList = data;
		//m_bLoadedAssetBundelDataList = true;

		LoadBranch();
		return;
	}

	private void LoadInit(){
		loadDataParamList = new List<LoadDataParam>();
		loadcount = 0;

	}

	private LoadDataParam MakeLoadDataParam( string folderName, string filename,LoadParam.LoadType loadType, int loadVersion ){
		LoadDataParam tmpLoadData = new LoadDataParam();

		tmpLoadData.url = Define.ASSET_BUNDLES_ROOT + folderName + "/" + filename + "Prefab.unity3d";
		tmpLoadData.fileName = filename + "Prefab";
		tmpLoadData.loadType = loadType;
		tmpLoadData.version = loadVersion;

		//Debug.Log ("url="+tmpLoadData.url+" fileName="+tmpLoadData.fileName+" loadType="+tmpLoadData.loadType+" loadVersion="+tmpLoadData.version);

		return tmpLoadData;
	}

	private void SetLoadPath( AssetBundleData.Param _tParam , LoadParam.LoadType _eLoadType){
		LoadDataParam tmpLoadData = MakeLoadDataParam(_tParam.folderName, _tParam.assetbundleName, _eLoadType , _tParam.version );
		tmpLoadData.arr_index = _tParam.arr_index;
		loadDataParamList.Add(tmpLoadData);
		return;
	}
	private void SetLoadPath( string folderName, string filename,LoadParam.LoadType loadType, int loadVersion ){
		LoadDataParam tmpLoadData = MakeLoadDataParam(folderName, filename,loadType, loadVersion );
		loadDataParamList.Add(tmpLoadData);
		return;
	}

	/*
	private LoadDataParam MakeLoadData( string folderName, string filename , LoadParam.LoadType loadType, int loadVersion ){
		LoadDataParam tmpLoadData = new LoadDataParam();

		tmpLoadData.url = folderName + "/" + filename + "."+DEFINE.ASSET_BUNDLE_PREFIX+".unity3d";
		tmpLoadData.fileName = filename;
		tmpLoadData.loadType = loadType;
		tmpLoadData.version = loadVersion;

		return tmpLoadData;
	}
	*/

	private void LoadingAssetBundle ()
	{
		LoadDataParam tmpLoadParam = loadDataParamList[loadcount];

        if (tmpLoadParam.loadType == LoadParam.LoadType.GameDataAsset) {
            BundleLoader.Instance.LoadData (tmpLoadParam, LoadGameDataCallBack);
        } else if( tmpLoadParam.loadType == LoadParam.LoadType.System){
            BundleLoader.Instance.LoadData( tmpLoadParam, LoadCallBack);
        } else if( tmpLoadParam.loadType == LoadParam.LoadType.Audio){
            BundleLoader.Instance.LoadData( tmpLoadParam, AudioDataCallBack);
        }else if( tmpLoadParam.loadType == LoadParam.LoadType.BitmapFont){
            BundleLoader.Instance.LoadData( tmpLoadParam, LoadCallBack);
		}else{
			Debug.LogError("読み込みデータの指定に異常があります");
		}

        LoadDialogAdd ();
	}

    private void OpenLoadDialog(){
        GameObject _ctrlDialogObj = (GameObject)Instantiate (ctrlDownloadDialog.gameObject) as GameObject;
        m_ctrlDownloadDialog = _ctrlDialogObj.GetComponent<CtrlDownloadDialog>();
    }

    /// <summary>
    /// Loadダイヤログの表示
    /// </summary>
    private void LoadDialogAdd(){
        //LoadDialog表示
        float loadValue = (float)loadcount / (float)loadMax;

        m_ctrlDownloadDialog.SetSliderValue( loadValue);
        m_ctrlDownloadDialog.SetPercentage ( loadcount, loadMax);

        nowTime = Time.time - loadStartTime;//Load開始からの現在時間

//        Debug.LogWarning ("経過時間 :" + nowTime);
//        Debug.LogWarning ("経過時間内に読めた数 :" + loadcount);
//        Debug.LogWarning ("残り数 :" + (loadMax-loadcount).ToString());

        float _expectedEndTime = (nowTime/(float)loadcount)*(loadMax-loadcount);

        string endTimeString = "";

        if (_expectedEndTime > 0f && _expectedEndTime < 7200f) {

            int endMinute = Mathf.FloorToInt ((_expectedEndTime) / 60f);
            int endSecond = Mathf.CeilToInt (_expectedEndTime - 60f * endMinute);

            if (endMinute == 0) {
                endTimeString = endSecond + "秒";
            } else {
                endTimeString = endMinute.ToString () + "分" +"  " + endSecond.ToString () + "秒";
            }
        } else {
            endTimeString = "計測中‥‥";
        }

        m_ctrlDownloadDialog.SetRestTime ( endTimeString);

        ////Debug.LogWarning ("EndTime:"+_expectedEndTime);
        ////Debug.LogWarning ("Max :"+loadMax + " count :"+loadcount);
        ////Debug.LogWarning ("Count :"+loadValue);

        loadcount++;
    }

    /// <summary>
    /// Loadダイヤログのフェードアウト
    /// </summary>
    private void ForceFadeOut(){
        m_ctrlDownloadDialog.ForceFadeOut ();
    }

	// キャッシュから読み込んだりなんだり ---------------------------------------------
	private bool m_bMonsterTextureLoad;
	//private List<LoadDataParam> m_LoadDataFromCacheList = new List<LoadDataParam>();
	//private List<GameObject> m_goMonsterAtlasList = new List<GameObject>();
	//private List<int> m_intLoadMonsterAtlasIndexList = new List<int>();

    private static LoadManager instance = null;
    public static LoadManager Instance {
        get{
            if( instance == null ){
                instance = (LoadManager) FindObjectOfType(typeof(LoadManager));
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "LoadManager";
                    obj.AddComponent<LoadManager>();
                    instance = obj.GetComponent<LoadManager>();
                    instance.Init();
                }
            }
            return instance;
        }
    }

    protected void Init(){
        LoadManager[] obj = FindObjectsOfType(typeof(LoadManager)) as LoadManager[];
        if( obj.Length > 1 ){
            // 既に存在しているなら削除
            Destroy(gameObject);
        }else{
            // 音管理はシーン遷移では破棄させない
            DontDestroyOnLoad(gameObject);
        }
        return;
    }
}
