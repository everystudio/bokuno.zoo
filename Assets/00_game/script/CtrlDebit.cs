using UnityEngine;
using System.Collections;

public class CtrlDebit : MonoBehaviourEx {

	public enum STEP {
		NONE		= 0,
		INITIALIZE	,
		IDLE		,

		OPEN		,
		WAIT		,
		CLOSE		,

		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;
	public bool m_bInitialize;
	public float m_fTimer;
	public float m_fDispWait;

	public UISprite m_sprWhite;
	public UtilSwitchSprite m_switchSprite;
	public UISprite m_sprWorkIcon;

	public GameObject m_goHit;
	public ButtonBase m_btnClose;

	public int m_iLoopIndex;

	static public bool m_bOpen;

	public int m_iPopupWorkId;

	void Start(){
		Initialize ();
	}

	public void Initialize(){

		if (m_bInitialize == false) {
			m_bInitialize = true;

			m_bOpen = false;

			m_eStep = STEP.INITIALIZE;
			m_eStepPre = STEP.MAX;

			m_goHit.SetActive (false);
			m_sprWhite.gameObject.SetActive (false);
			m_btnClose.gameObject.SetActive (false);

			m_switchSprite.gameObject.transform.localScale = Vector3.zero;

			m_fDispWait = 0.0f;
		}
	}


	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {

		case STEP.INITIALIZE:
			if (bInit) {
				m_switchSprite.SetSprite ("debt_finish");
			}

			//Debug.LogError (SpriteManager.Instance.IsIdle ());
			if (SpriteManager.Instance.IsIdle ()) {
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.IDLE:
			if (bInit) {
				m_goHit.SetActive (false);
				m_sprWhite.gameObject.SetActive (false);
				m_btnClose.gameObject.SetActive (false);

			}
			if (m_bOpen) {
				m_eStep = STEP.OPEN;
			}
			break;

		case STEP.OPEN:
			if (bInit) {
				SoundManager.Instance.PlaySE ("se_work_clear");

				m_goHit.SetActive (true);
				m_sprWhite.gameObject.SetActive (true);
				m_btnClose.gameObject.SetActive (true);

				TweenScale ts = TweenScale.Begin (m_switchSprite.gameObject, 0.2f, Vector3.one);
				EventDelegate.Set (ts.onFinished, EndTween);
			}
			if (m_bIsEndTween) {
				m_eStep = STEP.WAIT;
			}
			break;
		case STEP.WAIT:
			if (bInit) {
				m_btnClose.TriggerClear ();
			}
			if (m_btnClose.ButtonPushed) {
				m_eStep = STEP.CLOSE;
			}
			break;

		case STEP.CLOSE:
			if (bInit) {
				m_bIsEndTween = false;
				TweenScale ts = TweenScale.Begin (m_switchSprite.gameObject, 0.2f, Vector3.zero);
				EventDelegate.Set (ts.onFinished, EndTween);
			}
			if (m_bIsEndTween) {
				m_goHit.SetActive (false);
				m_sprWhite.gameObject.SetActive (false);
				m_btnClose.gameObject.SetActive (false);
				m_eStep = STEP.MAX;
			}
			break;

		case STEP.MAX:
		default:
			break;
		}
		return;
	}
}














