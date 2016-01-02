using UnityEngine;
using System.Collections;

public class CtrlKatazukeCheck : MonoBehaviour {
	#region SerializeField
	[SerializeField]
	private CtrlYesNoButton m_buttonYesNo;

	#endregion

	public CtrlYesNoButton YesOrNo{
		get{ return m_buttonYesNo; }
	}

	public void Initialize(){

		m_buttonYesNo.ButtonInit ();
	}

}









