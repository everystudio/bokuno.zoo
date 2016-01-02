using UnityEngine;
using System.Collections;

public class CtrlOjisanCheck : MonoBehaviour {

	#region SerializeField
	[SerializeField]
	private UILabel m_lbMessage;
	[SerializeField]
	private CtrlYesNoButton m_buttonYesNo;

	#endregion


	public CtrlYesNoButton YesOrNo{
		get{ return m_buttonYesNo; }
	}

	public bool IsYes(){

		if (GameMain.Instance.bOjisanCheck) {
			if (GameMain.Instance.OjisanCheckIndex == 0) {
				GameMain.Instance.bOjisanCheck = false;
				return true;
			}
		}

		return m_buttonYesNo.IsYes();
	}

	public bool IsNo(){
		if (GameMain.Instance.bOjisanCheck) {
			if (GameMain.Instance.OjisanCheckIndex == 1) {
				GameMain.Instance.bOjisanCheck = false;
				return true;
			}
		}
		return m_buttonYesNo.IsNo();
	}

	virtual public void Initialize(string _strMessage , bool _bIsYesOnly = false ){
		m_lbMessage.text = _strMessage;

		m_buttonYesNo.ButtonInit ();
		if (_bIsYesOnly) {
			m_buttonYesNo.YesOnlyMode ();
		}
	}

}
