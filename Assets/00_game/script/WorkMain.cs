using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkMain : PageBase {

	#region SerializeFieldでの設定が必要なメンバー変数
	[SerializeField]
	private GameObject m_posBannerGrid;
	#endregion

	public Tab [] WORK_TABS = new Tab[5] 
	{
		new Tab( Tab.TYPE.WORK_OSUSUME	, "aaa" , "workpage_tab1" , "" , new SearchData[1] {
			new SearchData( GameMain.TABLE_TYPE.WORK , " status = 1 " )
		} , "OshigotoMes1" ),
		new Tab( Tab.TYPE.WORK_BUILD	, "aaa" , "workpage_tab2", "workpage_menu" , new SearchData[2] {
			new SearchData( GameMain.TABLE_TYPE.WORK , " status = 1 and type = 1 " ),
			new SearchData( GameMain.TABLE_TYPE.WORK , " status != 0 and type = 1 " )
		} , "OshigotoMes2"),
		new Tab( Tab.TYPE.WORK_MONSTER	, "aaa" , "workpage_tab3", "workpage_menu" , new SearchData[2] {
			new SearchData( GameMain.TABLE_TYPE.WORK , " status = 1 and type = 2 " ),
			new SearchData( GameMain.TABLE_TYPE.WORK , " status != 0 and type = 2 " )
		} , "OshigotoMes3"),
		new Tab( Tab.TYPE.WORK_STAFF	, "aaa" , "workpage_tab4", "workpage_menu" , new SearchData[2] {
			new SearchData( GameMain.TABLE_TYPE.WORK , " status = 1 and type = 3 " ),
			new SearchData( GameMain.TABLE_TYPE.WORK , " status != 0 and type = 3 " )
		} , "OshigotoMes4"),
		new Tab( Tab.TYPE.WORK_OTHER	, "aaa" , "workpage_tab5", "workpage_menu" , new SearchData[2] {
			new SearchData( GameMain.TABLE_TYPE.WORK , " status = 1 and type = 4 " ),
			new SearchData( GameMain.TABLE_TYPE.WORK , " status != 0 and type = 4 " )
		} , "OshigotoMes5"),
	};

	// 表示用
	public List<DataWork > m_dataWorkDispList = new List<DataWork>();

	protected override void initialize(){
		m_pageHeader = makeHeader ("header_work" , WORK_TABS[0].m_strWordKey);

		makeCloseButton ();

		GameObject objTabParent = PrefabManager.Instance.MakeObject ("prefab/PrefTabParent", gameObject);
		m_tabParent = objTabParent.GetComponent<CtrlTabParent> ();
		m_tabParent.Init (WORK_TABS);

		// 自動削除の登録
		m_goDeleteList.Add (objTabParent);

		GameObject bannerParent = PrefabManager.Instance.MakeObject ("prefab/PrefBannerScrollParent", gameObject);
		m_bannerScrollParen = bannerParent.GetComponent<BannerScrollParent> ();

		m_iTabIndex = 0;
		m_iSwitchIndex = 0;

		Display (m_bannerScrollParen, WORK_TABS, m_iTabIndex, m_iSwitchIndex);
		/*
		List<DataWork> list = GameMain.dbWork.Select ( " work_id < 10 ");
		m_bannerScrollParen.Display (list);
		m_goDeleteList.Add (bannerParent);
		*/
		return;
	}

	protected override void close(){
	}

	// Update is called once per frame
	void Update () {

		displayAutoUpdate (WORK_TABS);


	}
}
