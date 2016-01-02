using UnityEngine;
using System.Collections;

public class DebugRoot : MonoBehaviour {

	private static  DebugRoot instance;
	public static DebugRoot Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("DebugRoot");
				//Debug.LogError ("here");
				if (obj == null) {
					obj = new GameObject("DebugRoot");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<DebugRoot> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<DebugRoot>() as DebugRoot;
				}
				instance.initialize ();
			}
			return instance;
		}
	}
	private void initialize(){
		m_bMenuDisp = false;
		m_goOnOff.SetActive (true);
		dispMenu (m_bMenuDisp);
		return;
	}

	public bool IsDebugMode(){
		return m_bMenuDisp;
	}


	[SerializeField]
	private ButtonBase m_btnOnOff;

	[SerializeField]
	private UILabel m_lbOnOff;


	[SerializeField]
	private GameObject m_goOnOff;
	[SerializeField]
	private GameObject m_goMenu;

	public bool m_bMenuDisp;

	protected void dispMenu( bool _bFlag ){
		if (_bFlag) {
			m_goMenu.SetActive (true);
			m_lbOnOff.text = "デバッグ:On";
		} else {
			m_goMenu.SetActive (false);
			m_lbOnOff.text = "デバッグ:Off";
		}
	}


	// Update is called once per frame
	void Update () {

		if (m_btnOnOff.ButtonPushed) {
			SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);
			m_btnOnOff.TriggerClear ();
			m_bMenuDisp = !m_bMenuDisp;
			dispMenu (m_bMenuDisp);
		}
	
	}
}
