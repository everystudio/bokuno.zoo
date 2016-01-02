using UnityEngine;
using System.Collections;

public class CtrlFieldItemBuildTime : ButtonBase {

	#region SerializeField
	[SerializeField]
	private UISprite m_sprBack;

	[SerializeField]
	private UILabel m_lbName;
	[SerializeField]
	private UILabel m_lbNokoriTime;
	#endregion

	public enum STEP {
		NONE		= 0,
		INIT		,
		APPEAR		,
		IDLE		,

		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	// もっと言うとdoubleの方がよい？
	public int m_iNokoriSec;

	public void Init( int _iNokoriSec , int _iDepth , int _iItemId ){

		int iUseDepth = _iDepth + 5;

		m_sprBack.depth = iUseDepth;
		m_lbNokoriTime.depth = iUseDepth + 1;
		m_lbName.depth = iUseDepth + 1;

		CsvItemData item_data = DataManager.GetItem (_iItemId);
		m_lbName.text = item_data.name;

		m_eStep = STEP.INIT;
		m_eStepPre = STEP.MAX;

		// これはあまり意味ないかも
		m_iNokoriSec = _iNokoriSec;
		SetNokoriSec (m_iNokoriSec);

		//m_sprBack.gameObject.transform.localScale = new Vector3 (1.0f, 0.0f, 1.0f);

		return;
	}

	public void SetNokoriSec( int _iNokoriSec ){

		string strNokoriTime = "あと";
		int iHour = _iNokoriSec / 3600;
		int iMinute = (_iNokoriSec % 3600) / 60;
		int iSec = (_iNokoriSec % 60);

		if (3600 <= _iNokoriSec) {
			strNokoriTime += iHour.ToString () + "時間" + iMinute.ToString () + "分";
		} else if (60 <= _iNokoriSec) {
			strNokoriTime += iMinute.ToString () + "分" + iSec.ToString () + "秒";
		} else {
			strNokoriTime += iSec.ToString () + "秒";
		}
		m_lbNokoriTime.text = strNokoriTime;
	}

	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {
		case STEP.INIT:
			m_eStep = STEP.APPEAR;
			break;
		case STEP.APPEAR:
			if (bInit) {
				m_bIsEndTween = false;
				TweenScale ts = TweenScale.Begin (m_sprBack.gameObject, 0.5f, Vector3.one * 0.7f);
				EventDelegate.Set (ts.onFinished, EndTween);
			}
			if (m_bIsEndTween == true) {
				m_eStep = STEP.IDLE;
			}
			break;
		case STEP.MAX:
			break;
		};

	
	}
}
