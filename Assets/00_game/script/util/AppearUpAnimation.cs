using UnityEngine;
using System.Collections;

public class AppearUpAnimation : MonoBehaviourEx {

	public string m_strSpriteName;
	public UtilSwitchSprite m_switchSprite;

	public enum STEP
	{
		NONE		= 0,
		IDLE		,
		APPEAR_WAIT	,
		APPEAR		,
		WAIT		,
		DISAPPEAR	,
		END			,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	private float m_fTimer;

	private Vector3 START_POS;

	private float MOVE_TIME = 1.5f;

	// Use this for initialization
	void Start () {
		m_eStep = STEP.IDLE;
		m_eStepPre	= STEP.MAX;

		myTransform.localScale = Vector3.zero;
		START_POS = myTransform.localPosition;

		m_switchSprite.SetSprite (m_strSpriteName);
		return;
	}

	public void Popup(){
		m_eStep = STEP.APPEAR_WAIT;
	}

	// Update is called once per frame
	void Update () {
		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {
		case STEP.IDLE:
			if (bInit) {
				myTransform.localScale = Vector3.zero;
			}
			break;

		case STEP.APPEAR_WAIT:
			if (SpriteManager.Instance.IsIdle ()) {
				m_eStep = STEP.APPEAR;
			}
			break;

		case STEP.APPEAR:
			if (bInit) {
				myTransform.localScale = Vector3.one;
				m_bIsEndTween = false;

				TweenAlpha.Begin (gameObject, 0.0f, 0.0f);
				TweenAlpha ts = TweenAlpha.Begin (gameObject, MOVE_TIME, 1.0f);

				TweenPosition.Begin (gameObject, 0.0f, new Vector3 (START_POS.x, START_POS.y , START_POS.z));
				TweenPosition.Begin (gameObject, MOVE_TIME, new Vector3 (START_POS.x, START_POS.y + 10.0f, START_POS.z));
				EventDelegate.Set (ts.onFinished, EndTween);
			}
			if (m_bIsEndTween) {
				m_eStep = STEP.WAIT;
			}
			break;

		case STEP.WAIT:
			if (bInit) {
				m_fTimer = 0.0f;
			}

			m_fTimer += Time.deltaTime;
			if (1.0f < m_fTimer) {
				m_eStep = STEP.DISAPPEAR;
			}
			break;
		case STEP.DISAPPEAR:
			if (bInit) {
				m_bIsEndTween = false;
				TweenAlpha ts = TweenAlpha.Begin (gameObject, MOVE_TIME, 0.0f);
				EventDelegate.Set (ts.onFinished, EndTween);
			}
			if (m_bIsEndTween) {
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.END:
			break;

		case STEP.MAX:
		default:
			break;
		}

	}
}

















