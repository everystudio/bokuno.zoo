using UnityEngine;
using System.Collections;

public class CtrlReviewWindow : MonoBehaviour {

	public enum STEP
	{
		NONE		= 0,
		LOAD		,
		IDLE		,
		PUSHED		,
		END			,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	public ButtonBase m_btnYes;
	public ButtonBase m_btnNo;
	public ButtonBase m_btnClose;

	public UtilSwitchSprite m_switchSprite;
	public GameObject m_goDispRoot;

	public ReviewManager.REPLY m_eReply;
	public ReviewManager.REPLY GetReply(){
		return m_eReply;
	}

	public bool IsEnd(){
		return (m_eStep == STEP.END);
	}

	public void Initialize(){
		m_goDispRoot.transform.localScale = Vector3.zero;

		m_eStep = STEP.LOAD;
		m_eStepPre = STEP.MAX;
		m_eReply = ReviewManager.REPLY.NONE;
		return;
	}

	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}
		switch (m_eStep) {
		case STEP.LOAD:
			if (bInit) {
				m_switchSprite.SetSprite ("popup_review");
			}
			if (SpriteManager.Instance.IsIdle ()) {
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.IDLE:
			if (bInit) {
				m_goDispRoot.transform.localScale = Vector3.one;
				m_btnYes.TriggerClear ();
				m_btnNo.TriggerClear ();
				m_btnClose.TriggerClear ();
			}
			if (m_btnYes.ButtonPushed) {
				ReviewManager.Instance.Reviewed (ReviewManager.REPLY.REVIEWED);
				m_btnYes.TriggerClear ();
				m_eStep = STEP.END;
			} else if (m_btnNo.ButtonPushed) {
				ReviewManager.Instance.Reviewed (ReviewManager.REPLY.NEVER);
				m_btnNo.TriggerClear ();
				m_eStep = STEP.END;
			} else if (m_btnClose.ButtonPushed) {
				m_btnClose.TriggerClear ();
				ReviewManager.Instance.Reviewed (ReviewManager.REPLY.RETRY);
				m_eStep = STEP.END;
			} else {
			}
			break;

		case STEP.PUSHED:
			break;

		case STEP.END:
			break;

		case STEP.MAX:
		default:
			break;



		}
	
	}
}
