using UnityEngine;
using System.Collections;

public class CtrlPageHeader : ButtonBase {

	public UILabel m_lbMessage;
	public UISprite m_sprBackGround;
	public UILabel m_lbCompRate;
	public UISprite m_sprButton;

	public void SetKeyWord (string _strKey){
		SetString (DataManager.Instance.GetWord (_strKey));
		return;
	}
	public void SetString( string _strMessage ){
		m_lbMessage.text = _strMessage;
	}

	public void SetCompRate( int _iRate ){
		m_lbCompRate.gameObject.SetActive (true);
		m_lbCompRate.enabled = true;
		m_lbCompRate.text = string.Format ("{0} %", _iRate);
	}

	public void Init( string _strImage , string _strWordKey , string _strButton = "" ){

		// とりあえずプリセット
		myTransform.localPosition = new Vector3 (0.0f, 360.0f, 0.0f);

		m_sprBackGround.spriteName = _strImage;

		//SetString ("-------");			// 多分表示されない
		SetKeyWord (_strWordKey);

		if (_strButton.Equals ("") == true) {
			m_sprButton.gameObject.SetActive (false);
		} else {
			m_sprButton.spriteName = _strButton;
			m_sprButton.gameObject.SetActive (true);
		}
		return;
	}

	// ボタンの機能をあとからつける

}
