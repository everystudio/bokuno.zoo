using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ButtonBase))]
public class CtrlCloseButton : MonoBehaviourEx {

	private ButtonBase m_btnClose;

	private bool m_bForceClose;
	public void Close(){
		m_bForceClose = true;
		return;
	}

	// Use this for initialization
	void Start () {
		m_btnClose = GetComponent<ButtonBase > ();
	}
	
	// Update is called once per frame
	void Update () {

		if (GameMain.IsInstance ()) {
			if (0 < GameMain.Instance.SwitchClose) {
				GameMain.Instance.SwitchClose = 0;
				m_bForceClose = true;
			}
		}

		// 戻り先ったん固定（変えれるようにしておいたほうがよいかも）
		if (m_btnClose.ButtonPushed || m_bForceClose ) {
			SoundManager.Instance.PlaySE (SoundName.BUTTON_CANCEL);
			if (GameMain.IsInstance ()) {
				GameMain.Instance.SetStatus (GameMain.STATUS.PARK);
			}
		}
	}
}
