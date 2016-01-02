using UnityEngine;
using System.Collections;

public class CtrlItemCheck : CtrlOjisanCheck {

	[SerializeField]
	UISprite m_sprItem;

	public void Initialize ( int _iItemId , string _strMessage, bool _bIsYesOnly = false )
	{
		base.Initialize (_strMessage, _bIsYesOnly);

		string strSpriteName = BannerItem.GetItemSpriteName (_iItemId);

		UIAtlas useatlas = AtlasManager.Instance.GetAtlas (strSpriteName);
		m_sprItem.atlas = useatlas;
		m_sprItem.spriteName = strSpriteName;
		return;
	}

}
