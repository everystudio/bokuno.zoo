using UnityEngine;
using System.Collections;

public class CtrlDispMonsterDetailStaffSub : MonoBehaviour {

	public void Initialize( string _strStaff ){
		m_sprChara.spriteName = _strStaff;
		Debug.Log (m_sprChara.spriteName);
		UISpriteData sprite_data = m_sprChara.GetAtlasSprite ();
		m_sprChara.width = sprite_data.width;
		m_sprChara.height = sprite_data.height;

		m_lbNo.enabled = false;
	}

	#region SerializeField
	[SerializeField]
	private UISprite m_sprChara;
	[SerializeField]
	private UILabel m_lbNo;			// NOしかないんだろうけど
	#endregion


}
