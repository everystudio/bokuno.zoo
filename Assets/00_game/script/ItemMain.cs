using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemMain : PageBase {

	#region SerializeFieldでの設定が必要なメンバー変数
	[SerializeField]
	private GameObject m_posBannerGrid;
	#endregion

	public Tab[] ITEM_TABS = new Tab[5] {
		new Tab (Tab.TYPE.ITEM_GAUGE, "aaa", "itempage_tab1", "itempage_menu", new SearchData[2]{
			//new SearchData( GameMain.TABLE_TYPE.ITEM_MASTER , "  category = 1 " , BannerBase.MODE.ITEM_BUY ),
			new SearchData( GameMain.TABLE_TYPE.ITEM_MASTER , " status = 1 and category = 1 " , BannerBase.MODE.ITEM_BUY ),
			new SearchData( GameMain.TABLE_TYPE.ITEM , " status = 0 and category = 1 " , BannerBase.MODE.ITEM_BACKYARD )
		} , "SettiMes1" ),
		new Tab (Tab.TYPE.ITEM_OFFICE, "aaa", "itempage_tab2", "itempage_menu", new SearchData[2]{
			new SearchData( GameMain.TABLE_TYPE.ITEM_MASTER , " status = 1 and category = 2 " ,BannerBase.MODE.ITEM_BUY),
			new SearchData( GameMain.TABLE_TYPE.ITEM , " status = 0 and category = 2 ",BannerBase.MODE.ITEM_BACKYARD )
		} , "SettiMes2" ),
		new Tab (Tab.TYPE.ITEM_SHOP, "aaa", "itempage_tab3", "itempage_menu", new SearchData[2]{
			new SearchData( GameMain.TABLE_TYPE.ITEM_MASTER , string.Format( " status = 1 and category = {0} " , (int)Define.Item.Category.SHOP ) ,BannerBase.MODE.ITEM_BUY),
			new SearchData( GameMain.TABLE_TYPE.ITEM , string.Format( " status = 0 and category = {0} " , (int)Define.Item.Category.SHOP ) , BannerBase.MODE.ITEM_BACKYARD)
		} , "SettiMes3" ),
		new Tab (Tab.TYPE.ITEM_EXTEND, "", "itempage_tab4", "", new SearchData[1]{ 
			new SearchData( GameMain.TABLE_TYPE.ITEM_MASTER , string.Format( " status = 1 and category = {0} " , (int)Define.Item.Category.EXPAND) ,BannerBase.MODE.ITEM_BUY ),
		} , "SettiMes4" ),
		new Tab (Tab.TYPE.ITEM_TICKET, "", "itempage_tab5", "", new SearchData[1]{ 
			new SearchData( GameMain.TABLE_TYPE.ITEM_MASTER ,  "ticket_gold" ,BannerBase.MODE.ITEM_BUY),
		} , "SettiMes5" ),
	};

	private List<GameObject> m_goBannerList = new List<GameObject> ();

	protected override void initialize(){
		m_goBannerList.Clear ();
		if (m_pageHeader != null) {
			Destroy (m_pageHeader.gameObject);
		}
		m_pageHeader = makeHeader ("header_item" , ITEM_TABS[0].m_strWordKey);
		makeCloseButton ();

		if (m_tabParent != null) {
			Destroy (m_tabParent.gameObject);
		}
		GameObject objTabParent = PrefabManager.Instance.MakeObject ("prefab/PrefTabParent", gameObject);
		m_tabParent = objTabParent.GetComponent<CtrlTabParent> ();

		Debug.Log (GameMain.Instance.m_iMoveTab);
		m_tabParent.Init (ITEM_TABS , GameMain.Instance.m_iMoveTab );

		GameObject bannerParent = PrefabManager.Instance.MakeObject ("prefab/PrefBannerScrollParent", gameObject);
		m_bannerScrollParen = bannerParent.GetComponent<BannerScrollParent> ();

		m_iTabIndex = GameMain.Instance.m_iMoveTab;
		GameMain.Instance.m_iMoveTab = 0;
		m_iSwitchIndex = 0;
		Display (m_bannerScrollParen, ITEM_TABS, m_iTabIndex, m_iSwitchIndex);

		/*
		// 自動削除の登録
		m_goDeleteList.Add (objTabParent);
		List<DataItemMaster> list = GameMain.dbItemMaster.Select ( " 0 < status ");
		m_bannerScrollParen.Display (list,0);
		*/
	}

	// Update is called once per frame
	void Update () {
		displayAutoUpdate (ITEM_TABS);

	}
}
