using UnityEngine;
using System.Collections;

public class ReviewManager : MonoBehaviourEx {

	protected static ReviewManager instance = null;
	public static ReviewManager Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("ReviewManager");
				if (obj == null) {
					obj = new GameObject("ReviewManager");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<ReviewManager> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<ReviewManager>() as ReviewManager;
				}
				instance.Initialize ();
			}
			return instance;
		}
	}

	public enum STATUS
	{
		NONE			= 0,
		WAIT			,
		READY_REVIEW	,
		REVIEWED		,
		NEVER			,
		MAX				,
	}

	public enum REPLY
	{
		NONE		= 0,
		REVIEWED	,
		RETRY		,
		NEVER		,
		MAX			,
	}

	public bool IsReadyReview(){
		return (m_eStatus == STATUS.READY_REVIEW);
	}

	public void Reviewed( REPLY _eReply ){
		Debug.Log (_eReply);
		switch (_eReply) {
		case REPLY.REVIEWED:
			m_eStatus = STATUS.REVIEWED;
			Application.OpenURL ("https://play.google.com/store/apps/details?id=jp.app.bokunosuizokukan");
			//Application.OpenURL ("itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=1052503169");
			break;
		case REPLY.RETRY:
			m_iReviewIntervalCount = 0;
			m_eStatus = STATUS.WAIT;
			break;
		case REPLY.NEVER:
			m_eStatus = STATUS.NEVER;
			break;
		default:
			break;
		}
		PlayerPrefs.SetInt (KEY_STATUS, (int)m_eStatus);
		PlayerPrefs.SetInt (KEY_COUNT, m_iReviewIntervalCount);
		return;
	}

	public const string KEY_COUNT 	=  "key_review_count";
	public const string KEY_STATUS	=  "key_review_status";

	public int ReviewInterval = 3;
	public int m_iReviewIntervalCount;
	public STATUS m_eStatus;
	public bool m_bInitialized;

	public void Initialize(){
		DontDestroyOnLoad(gameObject);
		if (m_bInitialized == false) {
			//Debug.LogError ("ReviewManager.Initialize()");
			if (false == PlayerPrefs.HasKey (KEY_STATUS)) {
				PlayerPrefs.SetInt (KEY_STATUS, (int)STATUS.WAIT);
			}
			m_eStatus = (STATUS)PlayerPrefs.GetInt (KEY_STATUS);

			if (false == PlayerPrefs.HasKey (KEY_COUNT )) {
				m_iReviewIntervalCount = 0;
				PlayerPrefs.SetInt (KEY_COUNT , m_iReviewIntervalCount );
			}
			m_iReviewIntervalCount = PlayerPrefs.GetInt (KEY_COUNT);
		}
		m_bInitialized = true;
		Instance.DummyCall ();
		//m_eStatus = STATUS.READY_REVIEW;
	}

	public void DummyCall(){
		//Debug.Log ("DummyCall");
	}

	void Start(){
		Initialize ();
	}

	void OnApplicationPause(bool _isPause ) {

		// 一応初期化
		Initialize ();

		// 待ち状態以外は即終了
		if (m_eStatus != STATUS.WAIT) {
			return;
		}

		//Debug.LogError ("OnApplicationPause");
		if (_isPause == false ) {
			m_iReviewIntervalCount += 1;
			PlayerPrefs.SetInt (KEY_COUNT, m_iReviewIntervalCount);
			if (ReviewInterval <= m_iReviewIntervalCount) {
				Debug.Log ("set status READY_REVIEW");
				m_eStatus = STATUS.READY_REVIEW;
			}
		} else {
		}
	}
}














