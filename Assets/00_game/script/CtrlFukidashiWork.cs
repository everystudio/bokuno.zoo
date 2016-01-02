using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlFukidashiWork : MonoBehaviourEx{

	[SerializeField]
	private UILabel m_lbMessage;

	public enum STEP {
		NONE		= 0,
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

	public int m_iLoopIndex;

	public bool m_bQuickEnd;

	private  Queue<string> m_MessageQueue = new Queue<string>(){};

	public void AddMessage( string _strMessage ){
		m_MessageQueue.Enqueue ( string.Format( "[000000]{0}[-]" , _strMessage ) );
	}


	public void Initialize(){

		if (m_bInitialize == false) {
			m_bInitialize = true;

			m_bQuickEnd = false;
			m_eStep = STEP.IDLE;
			m_eStepPre = STEP.MAX;

			myTransform.localScale = Vector3.zero;
			m_lbMessage.text = "-------------";

			m_MessageQueue.Clear ();

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
		case STEP.IDLE:
			if (bInit) {
				m_fTimer = 0.0f;
				m_bQuickEnd = false;

				List<DataWork> osusume_list = GameMain.dbWork.Select (" status = 1 " , false );
				if (0 < osusume_list.Count && m_MessageQueue.Count == 0) {

					m_bQuickEnd = true;

					int iUseIndex = m_iLoopIndex % osusume_list.Count;
					AddMessage (osusume_list [iUseIndex].description);
					m_iLoopIndex += 1;
				}
			}
			m_fTimer += Time.deltaTime;

			if (m_fDispWait < m_fTimer && 0 < m_MessageQueue.Count && GameMain.Instance.TutorialInputLock == false) {
				m_eStep = STEP.OPEN;
			}
			break;

		case STEP.OPEN:
			if (bInit) {
				string strMessage = m_MessageQueue.Dequeue ();
				m_lbMessage.text = strMessage;
				m_bIsEndTween = false;
				TweenScale ts = TweenScale.Begin (gameObject, 0.2f, Vector3.one);
				EventDelegate.Set (ts.onFinished, EndTween);
			}
			if (m_bIsEndTween) {
				m_eStep = STEP.WAIT;
			}
			break;
		case STEP.WAIT:
			if (bInit) {
				m_fDispWait = 5.0f;
				m_fTimer = 0.0f;
			}

			// ささっと終わらせたくて、次のが控えてたらすぐに終了させる
			if (m_bQuickEnd && 0 < m_MessageQueue.Count) {
				m_fTimer += m_fDispWait;
			}

			m_fTimer += Time.deltaTime;
			if (m_fDispWait < m_fTimer ) {
				m_eStep = STEP.CLOSE;
			}
			break;

		case STEP.CLOSE:
			if (bInit) {
				m_bIsEndTween = false;
				TweenScale ts = TweenScale.Begin (gameObject, 0.2f, Vector3.zero);
				EventDelegate.Set (ts.onFinished, EndTween);
			}
			if (m_bIsEndTween) {
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.MAX:
		default:
			break;
		}
		return;
	}
}
