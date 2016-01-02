using UnityEngine;
using System.Collections;

public class CtrlLoading : MonoBehaviour {

	[SerializeField]
	private GameObject m_goLoadingRoot;

	[SerializeField]
	private UILabel m_lbLoading;
	[SerializeField]
	private UILabel m_lbPercent;

	private string m_strLoading;
	private float m_fTimer;

	private int m_iLoopCount;

	void Start(){
		m_fTimer = 0.0f;
		m_goLoadingRoot.SetActive (false);
	}

	public void ViewPercent( float _fProgress ){

		m_goLoadingRoot.SetActive (true);

		int iProgress = (int)(100.0f * _fProgress);
		string strTemp = "Loading";

		if (iProgress == 100) {
			strTemp = "Load Finished";
		} else {
			for (int i = 0; i < m_iLoopCount % 4; i++) {
				strTemp += "・";
			}
		}
		m_lbLoading.text = strTemp;
		m_lbPercent.text = string.Format( "{0} %" , iProgress );
	}

	void Update(){

		float fInterval = 0.5f;

		m_fTimer += Time.deltaTime;
		if (fInterval < m_fTimer) {
			m_fTimer -= fInterval;
			m_iLoopCount += 1;
		}

	}


}
