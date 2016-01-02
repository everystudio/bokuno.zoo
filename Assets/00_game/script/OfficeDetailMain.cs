using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OfficeDetailMain : PageBase {

	public enum STEP
	{
		NONE			= 0,
		IDLE			,
		KATAZUKE_CHECK	,
		KATAZUKE		,
		MAX				,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	private CtrlKatazukeCheck m_csKatazukeCheck;

	#region SerializeFieldでの設定が必要なメンバー変数

	#endregion

	public Tab[] ITEM_DETAIL_TABS = new Tab[5] {
		new Tab (Tab.TYPE.ITEM_GAUGE, "aaa", "shisetsu2page_tab1", "", new SearchData[1]{
			new SearchData( GameMain.TABLE_TYPE.NONE , "" ),
		} , "OfiiceMes1" , "prefab/PrefItemDetailOffice" ),
		//} , "prefab/PrefItemDetailDisp" ),
		new Tab (Tab.TYPE.ITEM_OFFICE, "aaa", "shisetsu2page_tab2", "", new SearchData[1]{
			new SearchData( GameMain.TABLE_TYPE.STAFF , "@where_key01" , BannerBase.MODE.STAFF_BACKYARD_CHECK ),
		} , "OfiiceMes2" ),
		new Tab (Tab.TYPE.ITEM_SHOP, "aaa", "shisetsu2page_tab3", "itempage_menu", new SearchData[2]{
			new SearchData( GameMain.TABLE_TYPE.STAFF_MASTER , " status = 1 " , BannerBase.MODE.STAFF_SET_BUY ),
			new SearchData( GameMain.TABLE_TYPE.STAFF , " item_serial = 0 " , BannerBase.MODE.STAFF_SET_MINE )
		} , "OfiiceMes3" ),
		new Tab (Tab.TYPE.ITEM_EXTEND, "aaa", "shisetsu2page_tab4", "", new SearchData[1]{ 
			new SearchData( GameMain.TABLE_TYPE.NONE , "" ),
		}  , "OfiiceMes4" , "prefab/PrefItemDetailBuildupOffice" ),
		new Tab (Tab.TYPE.ITEM_TICKET, "aaa", "shisetsu2page_tab5", "", new SearchData[1]{ 
			new SearchData( GameMain.TABLE_TYPE.MONSTER , "@where_key02" ),
		} , "OfiiceMes5" ),
	};

	public int m_iItemSerial;

	public DataItem m_dataItem;

	protected override void initialize(){
		m_WhereHash.Clear ();
		m_WhereHash.Add ("@where_key01", "office_serial = " + GameMain.Instance.m_iSettingItemSerial.ToString ());
		m_WhereHash.Add ("@where_key02", "item_serial = " + GameMain.Instance.m_iSettingItemSerial.ToString () + " and condition = 2 " );

		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;

		m_iItemSerial = GameMain.Instance.m_iSettingItemSerial;

		Debug.Log ("serial:" + m_iItemSerial.ToString ());

		GameObject objTabParent = PrefabManager.Instance.MakeObject ("prefab/PrefTabParent", gameObject);
		m_tabParent = objTabParent.GetComponent<CtrlTabParent> ();
		m_tabParent.Init (ITEM_DETAIL_TABS);

		// ここはserialから対応した文字列を選択する必要があります
		m_pageHeader = makeHeader ("header_shisetsu2" , ITEM_DETAIL_TABS[0].m_strWordKey , "btn_katazuke");
		makeCloseButton ();

		m_dataItem = GameMain.dbItem.Select (m_iItemSerial);

		List<DataStaff> staff_list = GameMain.dbStaff.Select (" office_serial = " + m_iItemSerial.ToString ());
		int iUseCost = 0;
		foreach (DataStaff staff in staff_list) {
			CsvStaffData data_master = DataManager.GetStaff (staff.staff_id);
			iUseCost += data_master.cost;
		}
		CsvItemDetailData detail_data = DataManager.GetItemDetail (m_dataItem.item_id, m_dataItem.level);

		Debug.Log (detail_data.cost);
		Debug.Log (iUseCost);
		GameMain.Instance.m_iCostMax = detail_data.cost;
		GameMain.Instance.m_iCostNow = iUseCost;
		GameMain.Instance.m_iCostNokori = detail_data.cost - iUseCost;

		GameObject bannerParent = PrefabManager.Instance.MakeObject ("prefab/PrefBannerScrollParent", gameObject);
		m_bannerScrollParen = bannerParent.GetComponent<BannerScrollParent> ();
		m_bannerScrollParen.Initialize (m_tabParent);

		m_iTabIndex = 0;
		m_iSwitchIndex = 0;
		Display (m_bannerScrollParen, ITEM_DETAIL_TABS, m_iTabIndex, m_iSwitchIndex);

		m_csKatazukeCheck = null;
		//m_dataItem.category 

	}

	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {
		case STEP.IDLE:
			if (bInit) {
				if (m_csKatazukeCheck != null) {
					Destroy (m_csKatazukeCheck.gameObject);
					m_csKatazukeCheck = null;
				}

				m_pageHeader.TriggerClear ();
			}
			displayAutoUpdate (ITEM_DETAIL_TABS);

			if (m_pageHeader.ButtonPushed) {
				m_eStep = STEP.KATAZUKE_CHECK;
			}
			break;
		case STEP.KATAZUKE_CHECK:
			if (bInit) {
				GameObject obj = PrefabManager.Instance.MakeObject ("prefab/PrefKatazukeCheck", gameObject);
				m_csKatazukeCheck = obj.GetComponent<CtrlKatazukeCheck> ();
				m_csKatazukeCheck.Initialize ();
			}
			if (m_csKatazukeCheck.YesOrNo.IsYes ()) {
				m_eStep = STEP.KATAZUKE;
			} else if (m_csKatazukeCheck.YesOrNo.IsNo ()) {
				m_eStep = STEP.IDLE;
			} else {
				;// なにもしない
			}
			break;

		case STEP.KATAZUKE:
			if (bInit) {
			}

			List<DataStaff> staff_list = GameMain.dbStaff.Select (" office_serial = " + GameMain.Instance.m_iSettingItemSerial.ToString ());

			Dictionary< string , string > dict = new Dictionary< string , string > ();
			dict.Add ("office_serial", "0"); 
			dict.Add ("item_serial", "0"); 
			foreach (DataStaff staff in staff_list) {
				CtrlFieldItem fielditem = GameMain.ParkRoot.GetFieldItem (staff.item_serial);
				fielditem.RemoveStaff (staff.staff_serial);
				GameMain.dbStaff.Update (staff.staff_serial, dict);
			}

			// 上のスタッフを削除したとにしてください
			Debug.Log (GameMain.Instance.m_iSettingItemSerial);
			CtrlFieldItem ctrlFieldItem = GameMain.ParkRoot.GetFieldItem (GameMain.Instance.m_iSettingItemSerial);
			for (int x = m_dataItem.x; x < m_dataItem.x + m_dataItem.width; x++) {
				for (int y = m_dataItem.y; y < m_dataItem.y + m_dataItem.height; y++) {
					GameObject obj = PrefabManager.Instance.MakeObject ("prefab/PrefFieldItem", GameMain.ParkRoot.gameObject);
					obj.name = "fielditem_" + x.ToString () + "_" + y.ToString ();
					CtrlFieldItem script = obj.GetComponent<CtrlFieldItem> ();
					script.Init (x, y, 0);
					GameMain.ParkRoot.m_fieldItemList.Add (script);
				}
			}
			ctrlFieldItem.Remove ();

			GameMain.dbItem.Update (GameMain.Instance.m_iSettingItemSerial, 0, 0, 0);
			int iRemoveIndex = 0;
			foreach (CtrlFieldItem item in GameMain.ParkRoot.m_fieldItemList) {
				if (item.m_dataItem.item_serial == GameMain.Instance.m_iSettingItemSerial) {
					GameMain.ParkRoot.m_fieldItemList.RemoveAt (iRemoveIndex);
					break;
				}
				iRemoveIndex += 1;
			}

			// 取り除く
			if (m_itemDetailBase != null) {
				m_itemDetailBase.Remove ();
			}
			// 片付けして戻る
			m_closeButton.Close ();
			Destroy (m_csKatazukeCheck.gameObject);
			m_csKatazukeCheck = null;

			// 仕事の確認
			DataWork.WorkCheck ();
			GameMain.Instance.HeaderRefresh ();


			break;

		default:
			break;
		}
	}
}
