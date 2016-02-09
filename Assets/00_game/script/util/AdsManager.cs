using UnityEngine;
using System.Collections;

public class AdsManager : MonoBehaviour {


	#if UNITY_IPHONE
	public const string ASSET_BUNDLE_PREFIX             = "iphone";
	public const string ASSET_BUNDLES_ROOT              = "AssetBundles/iOS";
	#elif UNITY_ANDROID
	public const string ASSET_BUNDLE_PREFIX             = "android";
	public const string ASSET_BUNDLES_ROOT              = "AssetBundles/Android";
	#endif

	public static readonly int START_GOLD = 300;
	#if UNITY_IPHONE
	//iOS
	public static readonly string IMOBILE_PID = "34367";
	public static readonly string IMOBILE_MID = "215443";
	public static readonly string IMOBILE_SID_ICON = "371577";		// 使ってないけどね
	public static readonly string IMOBILE_SID_BANNER = "622054";
	//public static readonly string IMOBILE_SID_RECT = "391442";
	#elif UNITY_ANDROID
	public static readonly string IMOBILE_PID = "34367";
	public static readonly string IMOBILE_MID = "247749";
	public static readonly string IMOBILE_SID_ICON = "760610";
	public static readonly string IMOBILE_SID_BANNER = "760609";
	public static readonly string IMOBILE_SID_RECT = "412437";
	#endif

	protected static AdsManager instance = null;
	public static AdsManager Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("AdsManager");
				if (obj == null) {
					obj = new GameObject("AdsManager");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<AdsManager> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<AdsManager>() as AdsManager;
				}
				instance.Initialize ();
			}
			return instance;
		}
	}

	public void Initialize(){
	}
	private int m_iIMobileBannerId = 0;
	public void ShowAdBanner( bool _bFlag ){
		if (_bFlag) {
			if (m_iIMobileBannerId == 0) {
				#if (UNITY_IPHONE || UNITY_ANDROID ) && !UNITY_EDITOR
				IMobileSdkAdsUnityPlugin.registerInline(IMOBILE_PID, IMOBILE_MID, IMOBILE_SID_BANNER);

				// 広告の取得を開始します
				IMobileSdkAdsUnityPlugin.start(IMOBILE_SID_BANNER);
				// 広告の表示位置を指定して表示します(画面左下)
				m_iIMobileBannerId = (int)IMobileSdkAdsUnityPlugin.show(IMOBILE_SID_BANNER,
				IMobileSdkAdsUnityPlugin.AdType.BANNER,
				IMobileSdkAdsUnityPlugin.AdAlignPosition.CENTER,
				IMobileSdkAdsUnityPlugin.AdValignPosition.BOTTOM);
				#else
				#endif
			} else {
				#if (UNITY_IPHONE || UNITY_ANDROID ) && !UNITY_EDITOR
				IMobileSdkAdsUnityPlugin.setVisibility(m_iIMobileBannerId, true);
				#else
				#endif
			}
		} else {
			if (m_iIMobileBannerId != 0) {
				#if (UNITY_IPHONE || UNITY_ANDROID ) && !UNITY_EDITOR
				IMobileSdkAdsUnityPlugin.setVisibility (m_iIMobileBannerId, false);
				#else
				#endif
			}
		}
	}

	static private int m_iIMobileIconId = 0;
	public void ShowIcon( GameObject _goIcon , bool _bFlag ){

		string strSid = IMOBILE_SID_ICON;
		if (_bFlag) {

			if (m_iIMobileIconId == 0) {
				#if UNITY_ANDROID  // && !UNITY_EDITOR
				IMobileSdkAdsUnityPlugin.registerInline(IMOBILE_PID, IMOBILE_MID, strSid);
				var iconParams = new IMobileIconParams ();
				iconParams.iconNumber = 2; // アイコンの数を6個に
				iconParams.iconTitleFontColor = "#000000"; // タイトルの色を黒に
				iconParams.iconTitleShadowEnable = false; // タイトルの影を非表示に
				iconParams.iconTitleEnable = false;

				// 広告の取得を開始します
				IMobileSdkAdsUnityPlugin.start(strSid);
				// 広告の表示位置を指定して表示します(画面左下)
				m_iIMobileIconId = (int)IMobileSdkAdsUnityPlugin.show(strSid,
				IMobileSdkAdsUnityPlugin.AdType.ICON,
				0 , 90 ,iconParams );
				/*
				IMobileSdkAdsUnityPlugin.AdAlignPosition.LEFT,
				IMobileSdkAdsUnityPlugin.AdValignPosition.MIDDLE);
				*/
				#else
				#endif
			} else {
				#if UNITY_ANDROID //&& !UNITY_EDITOR
				IMobileSdkAdsUnityPlugin.setVisibility(m_iIMobileIconId, true);
				#else
				#endif
			}

		} else {
			#if UNITY_ANDROID //&& !UNITY_EDITOR
			IMobileSdkAdsUnityPlugin.setVisibility(m_iIMobileIconId, false);
			#else
			#endif
		}

		/*		
		#if UNITY_ANDROID
		NendAdIcon script = _goIcon.GetComponent<NendAdIcon> ();
		if (script == null) {
			Debug.Log ("ShowIcon script=null! ");

		} else if (_bFlag) {
			Debug.Log ("Show Icon! ");
			script.Show ();
			script.Resume ();
		} else {
			Debug.Log ("Hide Icon! ");
			script.Hide();
			script.Pause();
		}
		#endif
		*/
		return;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
