using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageBase : MonoBehaviourEx {

	public CtrlPageHeader m_pageHeader;
	private ButtonBase m_btnClose;				// 面倒なので直接押させない
	protected CtrlCloseButton m_closeButton;	// 外から終了させたい事情が出来たのでスクリプト追加
	protected CtrlItemDetailBase m_itemDetailBase;

	public Tab.TYPE m_eTab;
	public Tab.TYPE m_eTabPre;

	public int m_iTabIndex;
	public int m_iTabIndexPre;
	public int m_iSwitchIndex;
	public int m_iSwitchIndexPre;

	public Dictionary<string , string> m_WhereHash = new Dictionary<string , string>();

	// 作りがよくない
	// 最初い外部から作られてるのがなんともわかりにくくなってる
	public BannerScrollParent m_bannerScrollParen;
	protected CtrlTabParent m_tabParent;
	#region 自動削除対応
	protected List<GameObject> m_goDeleteList = new List<GameObject> ();

	#endregion

	#region SerializeFieldでの設定が必要なメンバー変数
	#endregion

	public virtual void SetTab( Tab.TYPE _eTab ){
		m_eTab = _eTab;
		return;
	}

	protected void Display( BannerScrollParent _parent , Tab [] _tabArr , int _iTabIndex , int _iSwitchIndex ){

		//Debug.Log (_iSwitchIndex);
		Tab tabData = _tabArr [_iTabIndex];
		m_iTabIndex = _iTabIndex;
		SearchData searchData = tabData.m_SearchDataList [_iSwitchIndex];

		string strWhere = searchData.m_strWhere;
		if (strWhere.Contains ("@") == true ) {
			if (m_WhereHash.TryGetValue (strWhere, out strWhere)) {
				//Debug.Log ("success:" + strWhere );
			} else {
				//Debug.Log ("fail");
			}
		}
		// BannerBase.MODEの設定
		BannerBase.Mode = searchData.m_eBannerMode;

		//Debug.Log (searchData.m_eTableType);
		switch (searchData.m_eTableType) {
		case GameMain.TABLE_TYPE.WORK:
			_parent.Display (GameMain.dbWork.Select (strWhere) , _iSwitchIndex , tabData.m_strSwitchHeader );
			break;
		case GameMain.TABLE_TYPE.ITEM_MASTER:
			_parent.Display (GameMain.dbItemMaster.Select (strWhere) , _iSwitchIndex , tabData.m_strSwitchHeader );
			break;
		case GameMain.TABLE_TYPE.ITEM:
			_parent.Display (GameMain.dbItem.Select (strWhere) , _iSwitchIndex , tabData.m_strSwitchHeader );
			break;
		case GameMain.TABLE_TYPE.MONSTER:
			_parent.Display (GameMain.dbMonster.Select (strWhere) , _iSwitchIndex , tabData.m_strSwitchHeader );
			break;
		case GameMain.TABLE_TYPE.MONSTER_MASTER:
			_parent.Display (GameMain.dbMonsterMaster.Select (strWhere) , _iSwitchIndex , tabData.m_strSwitchHeader );
			break;
		case GameMain.TABLE_TYPE.STAFF:
			_parent.Display (GameMain.dbStaff.Select (strWhere) , _iSwitchIndex , tabData.m_strSwitchHeader );
			break;
		case GameMain.TABLE_TYPE.STAFF_MASTER:
			_parent.Display (DataManager.csv_staff , _iSwitchIndex , tabData.m_strSwitchHeader );
			break;
		default:
			_parent.CloseSwitchButton ();
			break;
		}


		if (false == tabData.m_strSubPrefabName.Equals ("")) {
			GameObject objItemDetail = PrefabManager.Instance.MakeObject (tabData.m_strSubPrefabName, _parent.gameObject);
			m_itemDetailBase = objItemDetail.GetComponent<CtrlItemDetailBase> ();
			m_itemDetailBase.Initialize (GameMain.Instance.m_iSettingItemSerial);
		}

		return;
	}

	protected virtual void initialize(){
		Debug.LogError ("dont override initialize");
	}

	public virtual void Initialize(){
		gameObject.SetActive (true);
		m_goDeleteList.Clear ();
		m_pageHeader = null;
		m_btnClose = null;

		m_iTabIndex = 0;
		m_iSwitchIndex = 0;

		initialize ();
		return;
	}

	protected virtual void close(){
		Debug.LogWarning ("dont override close");
	}
	public virtual void Close (){

		if (m_itemDetailBase != null) {
			// 特に削除処理はしてないけど、上の削除に巻き込まれるでしょ（他人事）
			m_itemDetailBase.Close ();
		}
		gameObject.SetActive (false);
		if (m_pageHeader != null) {
			Release (m_pageHeader.gameObject);
		}
		if (m_btnClose != null) {
			Release (m_btnClose.gameObject);
		}
		foreach (GameObject obj in m_goDeleteList) {
			if (obj != null) {
				Release (obj);
			}
		}

		if (m_bannerScrollParen != null) {
			Release (m_bannerScrollParen.gameObject);
		}
		m_goDeleteList.Clear ();

		if (m_tabParent != null) {
			Destroy (m_tabParent.gameObject);
			m_tabParent = null;
		}


		close ();
	}

	public CtrlPageHeader makeHeader( string _strImage , string _strWordKey , string _strButton = "" ){
		GameObject objHeader = PrefabManager.Instance.MakeObject ("prefab/PrefPageHeader", gameObject);
		m_pageHeader = objHeader.GetComponent<CtrlPageHeader> ();
		m_pageHeader.Init ( _strImage , _strWordKey , _strButton );
		return m_pageHeader;
	}

	protected CtrlCloseButton makeCloseButton(){
		GameObject objCloseButton = PrefabManager.Instance.MakeObject ("prefab/PrefCloseButton", gameObject);
		objCloseButton.transform.localPosition = new Vector3 (0.0f, -427.0f, 0.0f);
		m_btnClose = objCloseButton.GetComponent<ButtonBase> ();
		m_btnClose.TriggerClear ();

		m_closeButton = objCloseButton.GetComponent<CtrlCloseButton> ();
		return m_closeButton;
	}
	public bool IsClose(){
		if (m_btnClose != null) {
			return m_btnClose.ButtonPushed;
		}
		return false;
	}

	protected int displayAutoUpdate( Tab[] _tabArr ){

		bool bChange = false;
		if (m_tabParent.ButtonPushed || GameMain.Instance.bSwitchTab ) {

			if (GameMain.Instance.bSwitchTab) {
				GameMain.Instance.bSwitchTab = false;
				m_tabParent.Index = GameMain.Instance.SwitchTabIndex;
				/*
				if (GameMain.Instance.bSwitchTab) {
					GameMain.Instance.bSwitchTab = false;
					m_bannerScrollParen.SetSwitchIndex (GameMain.Instance.SwitchTabIndex);
				}
				*/
			}
			if (m_iTabIndex != m_tabParent.Index ) {
				m_iTabIndex = m_tabParent.Index;
				// 少ない場合は補正
				if (_tabArr [m_iTabIndex].m_SearchDataList.Count <= m_iSwitchIndex) {
					m_iSwitchIndex = _tabArr [m_iTabIndex].m_SearchDataList.Count - 1;
				}

				m_tabParent.TriggerClearAll ();
				bChange = true;
				Debug.Log ("tab:" + m_iTabIndex.ToString ());
			}
		} else if (m_iSwitchIndex != m_bannerScrollParen.GetSwitchIndex () ) {
			m_iSwitchIndex = m_bannerScrollParen.GetSwitchIndex ();
			Debug.Log ("switch_index:" + m_iSwitchIndex.ToString ());
			bChange = true;
		} else {
			;//特に何もなし
		}

		if (bChange || GameMain.ListRefresh ) {
			if (m_itemDetailBase != null) {
				m_itemDetailBase.Close ();
				m_itemDetailBase = null;
			}
			SoundManager.Instance.PlaySE (SoundName.TAB_CHANGE);

			Release (m_bannerScrollParen.gameObject);
			GameObject bannerParent = PrefabManager.Instance.MakeObject ("prefab/PrefBannerScrollParent", gameObject);
			m_bannerScrollParen = bannerParent.GetComponent<BannerScrollParent> ();
			m_bannerScrollParen.Initialize (m_tabParent);
			Display (m_bannerScrollParen, _tabArr, m_iTabIndex , m_iSwitchIndex );
			m_pageHeader.SetKeyWord (_tabArr [m_iTabIndex].m_strWordKey);
		}

		return m_iTabIndex;

	}


}
