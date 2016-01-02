using UnityEngine;
using System.Collections;

public class BuyItemButton : MonoBehaviourEx {

	[SerializeField]
	private UISprite m_sprImage;
	[SerializeField]
	private UILabel m_lbText;

	public void SetText( string _strText ){
		m_lbText.text = _strText;
	}

}
