using UnityEngine;
using System.Collections;

public class CtrlHeaderExp : MonoBehaviour {

	public enum STEP {
		NONE		= 0,
		IDLE		,

		COUNT_UP	,
		LEVEL_UP	,

		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	private UISlider m_slExp;
	private UILabel m_lbLevel;
	private UILabel m_lbAto;
	private UILabel m_lbMade;

	private float m_fTime;
	private float m_fCheckTime;
	private float m_fCheckInterval;

	public int m_iLevel;
	public int m_iTotalExp;
	public int m_iTotalExpTarget;
	public int m_iDiff;


	public int m_iLevelExpNow;
	public int m_iLevelExpNext;

	public float m_fValue;

	public PopupAnimation m_PopupAnimation;


	public void SetExp( int _iLevel , out int _iExpNow , out int _iExpNext ){
		_iExpNow = 0;
		_iExpNext = 0;

		if (_iLevel == Define.USER_LEVEL_MAX) {
			return;
		}

		foreach (CsvLevelData level_data in DataManager.csv_level) {

			if (_iLevel == level_data.level) {
				_iExpNow = level_data.need_exp;
			}

			if ((_iLevel + 1) == level_data.level) {
				_iExpNext = level_data.need_exp;
				return;
			}
		}

		Debug.LogError ("ここに来るとよくない");
		return;

	}

	public void Initialize(UISlider _slExp ,UILabel _lbLevel , UILabel _lbAto , UILabel _lbMade , PopupAnimation _popupAnimation){
		m_slExp = _slExp;
		m_lbLevel = _lbLevel;
		m_lbAto = _lbAto;
		m_lbMade = _lbMade;
		m_PopupAnimation = _popupAnimation;

		m_iTotalExp = GameMain.dbKvs.ReadInt (Define.USER_TOTAL_EXP);
		foreach (CsvLevelData level_data in DataManager.csv_level) {
			if (m_iTotalExp < level_data.need_exp) {
				break;
			}
			m_iLevel = level_data.level;
		}
		GameMain.dbKvs.WriteInt (Define.USER_LEVEL, m_iLevel);
		//m_iLevel = GameMain.dbKvs.ReadInt (Define.USER_LEVEL);
		m_iTotalExpTarget = GameMain.dbKvs.ReadInt (Define.USER_TOTAL_EXP);

		SetExp (m_iLevel, out m_iLevelExpNow, out m_iLevelExpNext);

		m_lbLevel.text = "Lv."+m_iLevel.ToString ();
		m_fValue = Define.GetValue (m_iTotalExp, m_iLevelExpNow, m_iLevelExpNext);
		m_slExp.value = m_fValue;
		m_lbAto.text = (m_iLevelExpNext - m_iTotalExp).ToString ();
		if (m_iLevel == Define.USER_LEVEL_MAX) {
			m_lbAto.text = "[FF0000]MAX[-]";
			m_lbAto.transform.localPosition = new Vector3 (m_lbAto.transform.localPosition.x , 37.0f , m_lbAto.transform.localPosition.z );
			m_lbMade.text = "";
		}

		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;

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
		case STEP.IDLE:
			if (bInit) {
				m_fCheckTime = 0.0f;
				m_fCheckInterval = 3.0f;
				m_lbLevel.text = "Lv."+m_iLevel.ToString ();

				SetExp (m_iLevel, out m_iLevelExpNow, out m_iLevelExpNext);
				m_fValue = Define.GetValue (m_iTotalExp, m_iLevelExpNow, m_iLevelExpNext);
				m_slExp.value = m_fValue;
				m_lbAto.text = (m_iLevelExpNext - m_iTotalExp).ToString ();

				if (m_iLevel == Define.USER_LEVEL_MAX) {
					m_lbAto.text = "[FF0000]MAX[-]";
					m_lbAto.transform.localPosition = new Vector3 (m_lbAto.transform.localPosition.x , 37.0f , m_lbAto.transform.localPosition.z );
					m_lbMade.text = "";
				}

				m_iTotalExpTarget = GameMain.dbKvs.ReadInt (Define.USER_TOTAL_EXP);
				if (m_iLevelExpNext < m_iTotalExpTarget) {
					m_iTotalExpTarget = m_iLevelExpNext;
				}
			}
			m_fCheckTime += Time.deltaTime;
			if (m_fCheckInterval < m_fCheckTime) {
				m_fCheckTime -= m_fCheckInterval;

				m_iTotalExpTarget = GameMain.dbKvs.ReadInt (Define.USER_TOTAL_EXP);
				if (m_iLevelExpNext < m_iTotalExpTarget) {
					m_iTotalExpTarget = m_iLevelExpNext;
				}
				m_iDiff = m_iTotalExpTarget - m_iTotalExp;
			}

			if (m_iTotalExp < m_iTotalExpTarget) {
				int iAdd = Mathf.CeilToInt(m_iDiff * Time.deltaTime);
				m_iTotalExp += iAdd;
				if (m_iTotalExpTarget <= m_iTotalExp) {
					m_iTotalExp = m_iTotalExpTarget;
					if (m_iLevelExpNext <= m_iTotalExp) {
						m_eStep = STEP.LEVEL_UP;
					}
				}
				m_fValue = Define.GetValue (m_iTotalExp, m_iLevelExpNow, m_iLevelExpNext);
				m_slExp.value = m_fValue;
				m_lbAto.text = (m_iLevelExpNext - m_iTotalExp).ToString ();
			}
			break;

		case STEP.COUNT_UP:
			break;

		case STEP.LEVEL_UP:
			if (bInit) {
				m_fTime = 0.0f;
				m_PopupAnimation.Popup ();
				SoundManager.Instance.PlaySE ("se_levelup");
			}
			m_fTime += Time.deltaTime;

			if (0.5f < m_fTime) {
				m_eStep = STEP.IDLE;
				m_iLevel += 1;
				GameMain.dbKvs.WriteInt (Define.USER_LEVEL, m_iLevel);
				DataWork.WorkCheck ();
			}
			break;
		case STEP.MAX:
		default:
			break;
		}
		return;
	}
}









