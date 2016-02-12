using UnityEngine;
using System.Collections;

public class CtrlShopDetail : MonoBehaviour {

	#region SerializeField
	[SerializeField]
	private ButtonBase m_ButtonClose;

	[SerializeField]
	private ButtonBase m_ButtonCollect;

	[SerializeField]
	private UILabel m_lbName;
	[SerializeField]
	private UILabel m_lbUriage;
	[SerializeField]
	private UILabel m_lbExp;
	[SerializeField]
	private UILabel m_lbDescription;
	[SerializeField]
	private UISprite m_sprItem;
	#endregion

	public bool m_bIsEnd;
	private int m_iItemSerial;
	private CtrlParkRoot m_csParkRoot;

	public DataItem m_dataItem;

	public void Init( DataItem _dataItem , CtrlParkRoot _parkRoot ){

		m_dataItem = _dataItem;
		m_csParkRoot = _parkRoot;
		m_iItemSerial = _dataItem.item_serial;

		m_bIsEnd = false;

		DataItemMaster master_data = GameMain.dbItemMaster.Select (_dataItem.item_id);

		m_lbName.text = master_data.name;
		m_lbUriage.text = UtilString.GetSyuunyuu( master_data.revenue , master_data.revenue_interval );
		m_lbExp.text = "";
		m_lbDescription.text = master_data.description;

		string strName = "item" + _dataItem.item_id.ToString ();
		m_sprItem.spriteName = strName;

		return;
	}

	public bool IsEnd() {
		return m_bIsEnd;
	}

	// Update is called once per frame
	void Update () {

		if (m_bIsEnd == false) {
			if (m_ButtonCollect.ButtonPushed) {

				// 消す予定のところに新しい土地を設置する
				for (int x = m_dataItem.x; x < m_dataItem.x + m_dataItem.width; x++) {
					for (int y = m_dataItem.y; y < m_dataItem.y + m_dataItem.height; y++) {
						GameObject obj = PrefabManager.Instance.MakeObject ("prefab/PrefFieldItem", GameMain.ParkRoot.gameObject);
						obj.name = "fielditem_" + x.ToString () + "_" + y.ToString ();
						CtrlFieldItem script = obj.GetComponent<CtrlFieldItem> ();
						script.Init (x, y, 0);
						GameMain.ParkRoot.m_fieldItemList.Add (script);
					}
				}
				// 取り下げ
				GameMain.dbItem.Update (m_iItemSerial, 0, 0, 0);


				int iRemoveIndex = 0;
				foreach (CtrlFieldItem item in GameMain.ParkRoot.m_fieldItemList) {
					if (item.m_dataItem.item_serial == GameMain.Instance.m_iSettingItemSerial) {
						item.Remove ();
						GameMain.ParkRoot.m_fieldItemList.RemoveAt (iRemoveIndex);
						break;
					}
					iRemoveIndex += 1;
				}
				/*
				foreach (CtrlFieldItem script in m_csParkRoot.m_fieldItemList) {
					if (script.m_dataItem.item_serial == m_iItemSerial) {

						m_csParkRoot.m_fieldItemList.Remove (script);
						Destroy (script.gameObject);
						break;
					}
				}
				*/
				// 仕事の確認
				DataWork.WorkCheck ();
				GameMain.Instance.HeaderRefresh ();

				m_csParkRoot.ConnectingRoadCheck ();

				m_bIsEnd = true;
				SoundManager.Instance.PlaySE (SoundName.BUTTON_CANCEL);

			} else if (m_ButtonClose.ButtonPushed) {
				m_bIsEnd = true;
				SoundManager.Instance.PlaySE (SoundName.BUTTON_CANCEL);

			} else {
			}
		}




	
	}
}














