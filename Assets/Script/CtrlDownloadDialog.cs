using UnityEngine;
using System.Collections;

public class CtrlDownloadDialog : MonoBehaviour {

	[SerializeField] public GameObject m_goDialog;
	[SerializeField] public GameObject[] m_goObjectArr;

	[SerializeField] public UILabel m_lbPercentage;
	[SerializeField] public UILabel m_lbRestTime;
	[SerializeField] public UISlider m_slDownloadBar;

	public TweenAlpha tAlpha;

	public void Init(){
		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;

		m_goDialog.transform.localScale = Vector3.zero;
		m_goDialog.GetComponent<TweenScale>().from = m_goDialog.transform.localScale;
		m_goDialog.GetComponent<TweenScale>().to = m_goDialog.transform.localScale;

		#if UNITY_IPHONE
//		UnityiOSNative.setActivityIndicatorUnity3DNativeIO(150,540,1);
		#endif

		StartCoroutine( StartFadeIn(0.252f) );

		tAlpha.enabled = false;
		tAlpha.ResetToBeginning();

	}

	public void ForceFadeOut(){
		tAlpha.enabled = true;
		tAlpha.ResetToBeginning();
	}

	public IEnumerator StartFadeIn(float _fMoveTime){
		for(int i = 0; i < m_goObjectArr.Length; i++){
			TweenAlpha.Begin(m_goObjectArr[i], 0.0f, 0.0f);
			TweenAlpha.Begin(m_goObjectArr[i], _fMoveTime, 1.0f);
		}
		TweenScale.Begin(m_goDialog, _fMoveTime, Vector3.one);
		yield return 0;
	}

	public IEnumerator StartFadeOut(float _fMoveTime){

//		UnityiOSNative.stopActivityIndicatorUnity3DNativeIO ();

		for(int i = 0; i < m_goObjectArr.Length; i++){
			TweenAlpha.Begin(m_goObjectArr[i], _fMoveTime, 0.0f);
		}
		TweenScale.Begin(m_goDialog, _fMoveTime, Vector3.zero);
		yield return 0;
	}

	public void SetSliderValue( float _fNow , float _fMax ){
		float temp = _fNow / _fMax;
		SetSliderValue( _fNow / _fMax );
		return;
	}
	public void SetSliderValue( float _fValue ){
		if( m_slDownloadBar ){
			m_slDownloadBar.sliderValue = _fValue;
		}
		return;
	}

	public void SetPercentage(int _intNumerator, int _intDenominator){
		float intPercentage = (100f / (float)_intDenominator) * (float)_intNumerator;
		m_lbPercentage.text = intPercentage.ToString("n1") + "%";
	}

	public void SetRestTime(string endTime){
		m_lbRestTime.text = endTime;
		//m_lbRestTime.text = "00" + "分" + "00" + "秒";
	}

	public enum STEP{
		IDLE = 0,
		DOWNLOAD_START,
		DOWNLOAD_CONTINUE,
		DOWNLOAD_END,
		MAX
	}

	public STEP m_eStep;
	public STEP m_eStepPre;

	void Update(){
		bool bInit = false;
		if(m_eStepPre != m_eStep){
			m_eStepPre = m_eStep;
			bInit = true;
		}
		switch(m_eStep){
		case STEP.IDLE:
			break;
		case STEP.DOWNLOAD_START:
			break;
		case STEP.DOWNLOAD_CONTINUE:
			break;
		case STEP.DOWNLOAD_END:
			StartCoroutine( StartFadeOut(0.252f) );
			tAlpha.enabled = false;
			tAlpha.ResetToBeginning();
			break;
		case STEP.MAX:
			break;
		}
	}
}
