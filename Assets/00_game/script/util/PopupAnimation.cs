using UnityEngine;
using System.Collections;

public class PopupAnimation : MonoBehaviourEx {

	public enum STEP
	{
		NONE		= 0,
		IDLE		,
		APPEAR		,
		WAIT		,
		DISAPPEAR	,
		END			,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	private float m_fTimer;

	// Use this for initialization
	void Start () {
		m_eStep = STEP.IDLE;
		m_eStepPre	= STEP.MAX;

		myTransform.localScale = Vector3.zero;
		return;
	}

	public void Popup(){
		m_eStep = STEP.APPEAR;
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

		case STEP.APPEAR:
			if (bInit) {
				myTransform.localScale = Vector3.zero;
				m_bIsEndTween = false;
				TweenScale ts = TweenScale.Begin (gameObject, 1.5f, Vector3.one);
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
				TweenScale ts = TweenScale.Begin (gameObject, 1.0f, Vector3.zero);
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
