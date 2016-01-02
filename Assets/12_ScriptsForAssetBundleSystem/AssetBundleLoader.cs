using UnityEngine;
using System.IO;
using System.Collections;

public class AssetBundleLoader : BaseLoader {

    public void Init () {

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartLoadRequest (string _assetBundleName,BaseLoader.onComplete _callBack) {
        StartCoroutine (StartLoad(_assetBundleName,_callBack));
    }

    public void StartLoadRequest (string _assetBundleName,BaseLoader.onComplete _callBack,BaseLoader.onComplete _callBack2) {
        StartCoroutine (StartLoadFromPrefabManager(_assetBundleName,_callBack,_callBack2));
    }

    public void StartLoadRequest (string _assetBundleName,UISprite _setSprite,string _spriteName) {
        StartCoroutine (StartLoadUIAtlas(_assetBundleName,_setSprite,_spriteName));
    }

    public void StartLoadRequest (string _assetBundleName,SpriteRenderer _setSprite) {
        StartCoroutine (LoadSpriteRenderer(_assetBundleName,_setSprite));
    }

    //アセットバンドルのロード
    public IEnumerator StartLoad (string _assetBundleName,BaseLoader.onComplete _callBack) {

        string assetName = Path.GetFileName (_assetBundleName);
        assetName = assetName.Replace (".unity3d","");
        yield return StartCoroutine(Initialize() );
        // Load asset.
        yield return StartCoroutine(Load (_assetBundleName, assetName, _callBack) );
        // Unload assetBundles.
        AssetBundleManager.UnloadAssetBundle(_assetBundleName);
    }

    //アセットバンドルのロード
    public IEnumerator StartLoadFromPrefabManager (string _assetBundleName,BaseLoader.onComplete _callBack,BaseLoader.onComplete _callBack2) {

        string assetName = Path.GetFileName (_assetBundleName);
        assetName = assetName.Replace (".unity3d","");
        yield return StartCoroutine(Initialize() );
        // Load asset.
        yield return StartCoroutine(LoadFromPrefabManager (_assetBundleName, assetName, _callBack,_callBack2) );
        // Unload assetBundles.
        AssetBundleManager.UnloadAssetBundle(_assetBundleName);
    }

    //NGUIアトラスダウンロード
    public IEnumerator StartLoadUIAtlas (string _assetBundleName,UISprite _setSprite,string _spriteName) {

        string assetName = Path.GetFileName (_assetBundleName);
        assetName = assetName.Replace (".unity3d","");
        yield return StartCoroutine(Initialize() );
        // Load asset.
        yield return StartCoroutine(LoadUIAtlas (_assetBundleName, assetName, _setSprite,_spriteName) );
        // Unload assetBundles.
        AssetBundleManager.UnloadAssetBundle(_assetBundleName);
    }

    //SpriteRendererダウンロード
    public IEnumerator LoadSpriteRenderer (string _assetBundleName,SpriteRenderer _setSprite)  {

        string assetName = Path.GetFileName (_assetBundleName);
        assetName = assetName.Replace (".unity3d","");
        yield return StartCoroutine(Initialize() );
        // Load asset.
        yield return StartCoroutine(LoadSpriteRenderer (_assetBundleName, assetName, _setSprite));
        // Unload assetBundles.
        AssetBundleManager.UnloadAssetBundle(_assetBundleName);
    }

    private static AssetBundleLoader instance = null;
    public static AssetBundleLoader Instance {
        get{
            if( instance == null ){
                instance = (AssetBundleLoader) FindObjectOfType(typeof(AssetBundleLoader));
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "AssetBundleLoader";
                    obj.AddComponent<AssetBundleLoader>();
                    instance = obj.GetComponent<AssetBundleLoader>();
                    instance.Init();
                }
            }
            return instance;
        }
    }

}
