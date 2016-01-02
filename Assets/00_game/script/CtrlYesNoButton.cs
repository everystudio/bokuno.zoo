using UnityEngine;
using System.Collections;

public class CtrlYesNoButton : ButtonManager {

	#region SerializeField
	[SerializeField]
	private UISprite m_sprYes;
	[SerializeField]
	private BoxCollider m_bcYes;

	[SerializeField]
	private UISprite m_sprNo;
	[SerializeField]
	private BoxCollider m_bcNo;
	#endregion

	private  bool m_bYesEnable = true;
	private  bool m_bNoEnable = true;

	public void YesOnlyMode(){
		m_sprYes.transform.localPosition = Vector3.zero;
		m_sprNo.gameObject.SetActive (false);
		return;
	}

	public void EnableButtonYes( bool _bFlag ){
		if (_bFlag) {
			m_sprYes.color = Color.white;
			//m_bcYes.enabled = true;
			m_bYesEnable = true;
		} else {
			m_sprYes.color = new Color (0.75f, 0.75f, 0.75f);
			//m_bcYes.enabled = false;
			m_bYesEnable = false;
		}
	}
	public void EnableButtonNo( bool _bFlag ){
		if (_bFlag) {
			m_sprNo.color = Color.white;
			m_bNoEnable = true;
			//m_bcNo.enabled = true;
		} else {
			m_sprNo.color = new Color (0.75f, 0.75f, 0.75f);
			//m_bcNo.enabled = false;
			m_bNoEnable = false;
		}
	}

	public bool IsYes(){
		if (ButtonPushed) {
			if (Index == 0) {
				if (m_bYesEnable) {
					return true;
				}
			}
		}
		return false;
	}

	public bool IsNo(){
		if (ButtonPushed) {
			if (Index == 1) {
				if (m_bNoEnable) {
					return true;
				}
			}
		}
		return false;
	}

}
