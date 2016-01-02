using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UISprite))]
public class WorkNewButton : MonoBehaviour {

	public enum STEP {
		NONE		= 0,
		IDLE		,
		CHECK		,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	private UISprite m_sprNew;
	private float m_fTimer;
	readonly private float CHECK_INTERVAL = 2.0f;

	// Use this for initialization
	void Start () {
		m_sprNew = GetComponent<UISprite> ();
		m_sprNew.enabled = false;

		m_eStep = STEP.CHECK;
		m_eStepPre = STEP.MAX;
		return;
	}

	void OnEnable(){
		//Debug.LogError ("OnEnable:WorkNewButton");
		m_eStep = STEP.CHECK;
		m_eStepPre = STEP.MAX;
	}
	
	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre = m_eStep;
			bInit = true;
		}
		switch (m_eStep) {
		case STEP.IDLE:
			if (bInit) {
				m_fTimer = 0.0f;
			}

			m_fTimer += Time.deltaTime;
			if (CHECK_INTERVAL < m_fTimer) {
				m_eStep = STEP.CHECK;
			}
			break;

		case STEP.CHECK:
			if (bInit) {

			}

			// 表示される仕事一覧
			List<DataWork> data_work_list = GameMain.dbWork.Select (string.Format ("status = {0}", (int)Define.Work.STATUS.APPEAR));

			foreach (DataWork check_work in data_work_list) {
				if (PlayerPrefs.HasKey (Define.GetWorkNewKey (check_work.work_id))) {
					if (PlayerPrefs.GetInt (Define.GetWorkNewKey (check_work.work_id)) == 0) {
						m_sprNew.enabled = false;
					} else {
						m_sprNew.enabled = true;
					}
				}
			}

			m_eStep = STEP.IDLE;
			break;

		case STEP.MAX:
		default:
			break;

		}



	
	}
}
