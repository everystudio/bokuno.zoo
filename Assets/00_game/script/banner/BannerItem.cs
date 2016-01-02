using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ButtonBase))]
public class BannerItem : BannerBase {
	public enum STEP
	{
		NONE			= 0,
		IDLE			,
		DETAIL			,

		EXPAND_CHECK	,
		EXPAND_BUY		,

		TICKET_CHECK	,
		TICKET_BUY		,

		GOLD_CHECK		,
		GOLD_BUY		,

		MAX				,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	#region SerializeField設定
	[SerializeField]
	private UISprite m_sprBuyBase;
	[SerializeField]
	private UILabel m_lbBuyPrice;
	#endregion

	public int m_iItemId;
	public int m_iItemSerial;
	private ButtonBase m_buttonBase;
	public int m_iTicketNum;

	public DataItemMaster m_ItemMaster;
	public CtrlOjisanCheck m_ojisanCheck;


	public bool Initialize( DataItemMaster _data , int _iCostNokori ){

		bool bRet = true;

		m_iTicketNum = 0;
		m_ItemMaster = _data;
		m_bIsUserData = false;

		m_iItemId = _data.item_id;
		m_iItemSerial = 0;
		m_buttonBase = GetComponent<ButtonBase> ();

		m_lbTitle.text = _data.name;
		m_lbTitle2.text = m_lbTitle.text;
		m_lbDescription.text = _data.description;

		m_lbPrize.text = _data.size.ToString();
		m_lbPrizeExp.text = _data.cost.ToString ();

		m_lbDifficulty.text = "";

		string strIcon = GetSpriteName (_data);
		UIAtlas atlas = AtlasManager.Instance.GetAtlas (strIcon);
		m_sprIcon.atlas = atlas;
		m_sprIcon.spriteName = strIcon;

		SetPrice (_data);

		// 上限確認の為にここで所持数チェック
		int iHave = GameMain.dbItem.Select (string.Format (" item_id = {0} ", _data.item_id)).Count;

		m_bAbleUse = DataManager.user.AbleBuy (_data.need_coin, _data.need_ticket, 0 , _iCostNokori , iHave , _data.setting_limit ,ref m_eReason);
		SetReasonSprite (m_sprReason, m_eReason);
		m_sprIgnoreBlack.gameObject.SetActive (!m_bAbleUse);
		if ((Define.Item.Category)_data.category == Define.Item.Category.SHOP) {
			m_lbPrize.text = _data.size.ToString();
			m_lbPrizeExp.text = "";
			m_sprBackground.spriteName = "list_item_2";
			m_lbDifficulty.text = UtilString.GetSyuunyuu( m_ItemMaster.revenue , m_ItemMaster.revenue_interval );
		} else if ((Define.Item.Category)_data.category == Define.Item.Category.EXPAND || 
			(Define.Item.Category)_data.category == Define.Item.Category.GOLD ||
			(Define.Item.Category)_data.category == Define.Item.Category.TICKET ) {
			m_sprBackground.spriteName = "list_item_4";
			m_lbPrize.text = "";
			m_lbPrizeExp.text = "";
			m_lbDifficulty.text = "";
		} else {
		}
		SetEnableIcon (m_bAbleUse);
		// こっちのInitializeは通る

		//Debug.LogError ( string.Format( "item_id={0} setting_limit={1}" , _data.item_id,_data.setting_limit));
		if (m_eReason == ABLE_BUY_REASON.LIMIT) {
			bRet = false;
		}

		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;
		return bRet;

	}
	public bool Initialize( DataItem _data , int _iCostNokori ){

		DataItemMaster item_master = DataManager.GetItemMaster (_data.item_id);
		Initialize (item_master , _iCostNokori );
		m_iItemSerial = _data.item_serial;

		m_bIsUserData = true;
		m_sprBuyBase.gameObject.SetActive (false);

		//m_bAbleUse = DataManager.user.AbleBuy (0 , 0, 0 , _iCostNokori);
		m_bAbleUse = DataManager.user.AbleBuy (0, 0, 0, 0, 0, 0, ref m_eReason);

		//m_lbReason.gameObject.SetActive (!m_bAbleUse);
		SetReasonSprite (m_sprReason, m_eReason);
		m_sprIgnoreBlack.gameObject.SetActive (!m_bAbleUse);
		SetEnableIcon (m_bAbleUse);

		return true;
	}

	private void SetPrice( DataItemMaster _data ){
		string strText = "";
		string strImageName = "";
		if (0 < _data.need_coin) {
			strImageName = "list_buy1";
			strText = _data.need_coin.ToString () + "G";
		} else if (0 < _data.need_ticket) {
			strImageName = "list_buy2";
			strText = _data.need_ticket.ToString () + "枚";
		} else if (0 < _data.need_money) {
			strImageName = "list_buy3";
			strText = _data.need_money.ToString () + "円";
		} else {
			Debug.LogError ("no need");
		}
		m_sprBuyBase.spriteName = strImageName;
		m_lbBuyPrice.text = strText;
		return;
	}

	static public string GetItemSpriteName( int _iItemId ){
		string strRet = "";

		switch (_iItemId) {

		case 30:
			strRet = "ticket10";
			break;
		case 31:
			strRet = "ticket55";
			break;
		case 32:
			strRet = "ticket125";
			break;
		case 33:
			strRet = "ticket350";
			break;
		case 34:
			strRet = "ticket800";
			break;
		case 35:
			strRet = "coin1000";
			break;
		case 36:
			strRet = "coin5500";
			break;
		case 37:
			strRet = "coin125000";
			break;
		case 38:
			strRet = "coin500000";
			break;

		default:
			strRet = "item" + _iItemId.ToString ();
			break;
		}
		return strRet;
	}

	private string GetSpriteName( DataItemMaster _data ){

		return GetItemSpriteName(_data.item_id);
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
				m_buttonBase.TriggerClear ();
			}
			if (m_buttonBase.ButtonPushed) {
				m_buttonBase.TriggerClear ();
				//Debug.Log (m_ItemMaster.category);
				if (m_bAbleUse) {

					switch ((Define.Item.Category) m_ItemMaster.category) {

					case Define.Item.Category.EXPAND:
						m_eStep = STEP.EXPAND_CHECK;
						break;

					case Define.Item.Category.TICKET:
						m_eStep = STEP.TICKET_CHECK;
						break;
					case Define.Item.Category.GOLD:
						m_eStep = STEP.GOLD_CHECK;
						break;
					default:
						GameMain.Instance.PreSettingItemId = 0;
						SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);
						GameMain.Instance.SettingItem (m_iItemId, m_iItemSerial);
						GameMain.Instance.SetStatus (GameMain.STATUS.PARK);
						break;
					}
				}
			}
			break;

		case STEP.EXPAND_CHECK:
			if (bInit) {
				GameObject objOjisan = PrefabManager.Instance.MakeObject ("prefab/PrefOjisanCheck", gameObject.transform.parent.parent.parent.parent.gameObject );
				m_ojisanCheck = objOjisan.GetComponent<CtrlOjisanCheck> ();
				m_ojisanCheck.Initialize ("動物園を\n拡張します\n\nよろしいですか");
			}
			if (m_ojisanCheck.IsYes ()) {
				SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);
				Destroy (m_ojisanCheck.gameObject);
				m_eStep = STEP.EXPAND_BUY;
			} else if (m_ojisanCheck.IsNo ()) {
				SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);
				Destroy (m_ojisanCheck.gameObject);
				m_eStep = STEP.IDLE;
			} else {
			}
			break;

		case STEP.EXPAND_BUY:
			if (bInit) {
				GameObject objOjisan = PrefabManager.Instance.MakeObject ("prefab/PrefOjisanCheck", gameObject.transform.parent.parent.parent.parent.gameObject);
				m_ojisanCheck = objOjisan.GetComponent<CtrlOjisanCheck> ();
				m_ojisanCheck.Initialize ("拡張いたしました！", true);

				GameMain.dbItem.Insert (m_ItemMaster, 0, 0, 0);
				DataItem.OpenNewItem (m_ItemMaster.item_id);

				GameObject prefab = PrefabManager.Instance.PrefabLoadInstance ("prefab/PrefFieldItem");
				DataManager.user.AddGold (-1 * m_ItemMaster.need_coin);
				for (int x = 0; x < DataManager.user.m_iWidth + Define.EXPAND_FIELD + 1; x++) {
					for (int y = 0; y < DataManager.user.m_iHeight + Define.EXPAND_FIELD + 1; y++) {
						if ( DataManager.user.m_iWidth <= x || DataManager.user.m_iHeight <= y ) {

							CtrlFieldItem script = null;
							script = GameMain.ParkRoot.GetFieldItem (x, y);

							if (script == null) {
								GameObject obj = PrefabManager.Instance.MakeObject (prefab, GameMain.ParkRoot.gameObject);
								obj.name = "fielditem_" + x.ToString () + "_" + y.ToString ();
								script = obj.GetComponent<CtrlFieldItem> ();
								GameMain.ParkRoot.AddFieldItem (script);
							}

							int iDummyItemId = 0;
							if (x == DataManager.user.m_iWidth + Define.EXPAND_FIELD|| y == DataManager.user.m_iHeight+ Define.EXPAND_FIELD) {
								iDummyItemId = -1;
							}
							script.Init (x, y, iDummyItemId);

						}
					}
				}
				DataManager.user.m_iWidth += Define.EXPAND_FIELD;
				DataManager.user.m_iHeight += Define.EXPAND_FIELD;
				PlayerPrefs.SetInt (Define.USER_WIDTH, DataManager.user.m_iWidth);
				PlayerPrefs.SetInt (Define.USER_HEIGHT, DataManager.user.m_iHeight);
			}
			if (m_ojisanCheck.IsYes ()) {
				GameMain.ListRefresh = true;
				Destroy (m_ojisanCheck.gameObject);
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.TICKET_CHECK:
			if (bInit) {
				string strBuyProductId = Define.GetProductId (m_ItemMaster.item_id , ref m_iTicketNum );
				PurchasesManager.buyItem (strBuyProductId);
			}
			if (PurchasesManager.Instance.IsPurchased ()) {
				m_eStep = STEP.IDLE;
				if (PurchasesManager.Instance.Status == PurchasesManager.STATUS.SUCCESS) {
					m_eStep = STEP.TICKET_BUY;
				}
			}
			break;

		case STEP.TICKET_BUY:
			Debug.Log (string.Format ("add ticket num:{0}" , m_iTicketNum ));
			DataManager.user.AddTicket (m_iTicketNum);
			GameMain.Instance.HeaderRefresh ();
			m_eStep = STEP.IDLE;
			break;

		case STEP.GOLD_CHECK:
			if (bInit) {
				GameObject objOjisan = PrefabManager.Instance.MakeObject ("prefab/PrefOjisanCheck", gameObject.transform.parent.parent.parent.parent.gameObject );
				m_ojisanCheck = objOjisan.GetComponent<CtrlOjisanCheck> ();
				m_ojisanCheck.Initialize ( string.Format("チケットをゴールドに\n変換します\n\n{0}G→ {1}G\nよろしいですか" , DataManager.user.m_iGold ,DataManager.user.m_iGold+m_ItemMaster.add_coin ));
			}
			if (m_ojisanCheck.IsYes ()) {
				SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);
				Destroy (m_ojisanCheck.gameObject);
				m_eStep = STEP.GOLD_BUY;
			} else if (m_ojisanCheck.IsNo ()) {
				SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);
				Destroy (m_ojisanCheck.gameObject);
				m_eStep = STEP.IDLE;
			} else {
			}
			break;

		case STEP.GOLD_BUY:
			DataManager.user.AddTicket (-1 * m_ItemMaster.need_ticket);
			DataManager.user.AddGold (m_ItemMaster.add_coin);
			GameMain.Instance.HeaderRefresh ();
			m_eStep = STEP.IDLE;
			break;

		default:
			break;
		}
		return;
	}

}















