using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ButtonBase))]
public class BannerStaff : BannerBase {

	public enum STEP
	{
		NONE			= 0,
		IDLE			,

		DETAIL			,

		SETTING			,

		BACKYARD_CHECK	,
		BACKYARD		,

		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	#region SerializeField設定
	[SerializeField]
	private UISprite m_sprBuyBase;
	[SerializeField]
	private UILabel m_lbBuyPrice;
	#endregion

	private ButtonBase m_buttonBase;

	private DataStaff m_dataStaff;

	// スタッフを配置する処理
	public CtrlStaffSetting m_staffSetting;
	public CtrlOjisanCheck m_ojisanCheck;

	public void Initialize( CsvStaffData _csvStaffData ,int _iCostNokori){
		m_bIsUserData = false;
		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;

		m_lbTitle.text = _csvStaffData.name;
		m_lbTitle2.text = _csvStaffData.name;
		m_lbDescription.text = _csvStaffData.description;

		// コスト
		m_lbPrize.text = _csvStaffData.cost.ToString();

		string strIcon = GetSpriteName (_csvStaffData);
		UIAtlas atlas = AtlasManager.Instance.GetAtlas (strIcon);
		m_sprIcon.atlas = atlas;
		m_sprIcon.spriteName = strIcon;

		SetPrice (_csvStaffData);

		m_lbDifficulty.text = _csvStaffData.expenditure.ToString() + "/1時間";

		m_buttonBase = GetComponent<ButtonBase> ();

		m_dataStaff = new DataStaff ();
		m_dataStaff.staff_id = _csvStaffData.staff_id;
		m_dataStaff.staff_serial = 0;

		m_bAbleUse = DataManager.user.AbleBuy (_csvStaffData.coin, _csvStaffData.ticket, _csvStaffData.cost, _iCostNokori, 0, 0, ref m_eReason);
		SetReasonSprite (m_sprReason, m_eReason);
		m_sprIgnoreBlack.gameObject.SetActive (!m_bAbleUse);
		SetEnableIcon (m_bAbleUse);

		return;
	}

	public void Initialize( DataStaff _data , int _iCostNokori ){
		//Debug.LogError (_iCostNokori);
		CsvStaffData staff_data = DataManager.GetStaff (_data.staff_id);
		Initialize (staff_data,_iCostNokori);
		m_dataStaff = _data;
		m_bIsUserData = true;
		m_sprBuyBase.gameObject.SetActive (false);

		m_bAbleUse = DataManager.user.AbleBuy (0, 0, staff_data.cost, _iCostNokori, 0, 0, ref m_eReason);

		if (BannerBase.Mode == BannerBase.MODE.STAFF_BACKYARD_CHECK) {
			m_bAbleUse = true;
			m_eReason = ABLE_BUY_REASON.NONE;
		}
		SetReasonSprite (m_sprReason, m_eReason);
		SetEnableIcon (m_bAbleUse);

		m_sprIgnoreBlack.gameObject.SetActive (!m_bAbleUse);
		return;
	}

	private string GetSpriteName( CsvStaffData _csvStaff ){
		string strRet = "";
		strRet = "staff_icon" + _csvStaff.staff_id.ToString ();
		return strRet;
	}

	private void SetPrice( CsvStaffData _csvStaff ){
		string strText = "";
		string strImageName = "";

		strImageName = "list_buy1";
		strText = _csvStaff.coin.ToString () + "G";

		m_sprBuyBase.spriteName = strImageName;
		m_lbBuyPrice.text = strText;
		return;
	}

	// Update is called once per frame
	void Update () {

		string strPrefab = "prefab/PrefItemDetailDisp";

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
				Debug.Log ("clicked:BannerBase.Mode=" + BannerBase.Mode.ToString ());

				if (m_bAbleUse) {
					switch (BannerBase.Mode) {
					case BannerBase.MODE.STAFF_SET_BUY:
						m_eStep = STEP.SETTING;
						break;
					case BannerBase.MODE.STAFF_SET_MINE:
						m_eStep = STEP.SETTING;
						break;

					case BannerBase.MODE.STAFF_BACKYARD_CHECK:
						m_eStep = STEP.BACKYARD_CHECK;
						break;
					default:
						break;
					}
				}
			}
			break;

		case STEP.SETTING:
			if (bInit) {
				// この作り方はいただけませんねぇ・・・
				GameObject obj = PrefabManager.Instance.MakeObject ("prefab/PrefStaffSetting", gameObject);
				m_staffSetting = obj.GetComponent<CtrlStaffSetting> ();

				Debug.Log (m_staffSetting);

				// ここで取得するっていうのもなんか良くないっすね
				DataItem officeData = GameMain.dbItem.Select (GameMain.Instance.m_iSettingItemSerial);

				Debug.Log (officeData);
				Debug.Log (m_dataStaff.staff_id );
				Debug.Log (m_dataStaff.staff_serial);

				Debug.Log (m_staffSetting);
				m_staffSetting.Initialize (officeData , m_dataStaff.staff_id , m_dataStaff.staff_serial );
			}
			if (m_staffSetting.IsEnd ()) {
				m_staffSetting.Close ();
				Destroy (m_staffSetting.gameObject);
				m_staffSetting = null;
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.BACKYARD_CHECK:
			if (bInit) {
				GameObject objOjisan = PrefabManager.Instance.MakeObject ("prefab/PrefOjisanCheck", gameObject.transform.parent.parent.parent.parent.gameObject );
				m_ojisanCheck = objOjisan.GetComponent<CtrlOjisanCheck> ();
				m_ojisanCheck.Initialize ("こちらのスタッフを\n待機室へ\n移動させます。");
			}
			if (m_ojisanCheck.IsYes ()) {
				GameMain.ListRefresh = true;
				m_eStep = STEP.BACKYARD;
			} else if (m_ojisanCheck.IsNo ()) {
				Destroy (m_ojisanCheck.gameObject);
				m_eStep = STEP.IDLE;
			} else {
			}
			break;

		case STEP.BACKYARD:

			CtrlFieldItem field_item = GameMain.ParkRoot.GetFieldItem (m_dataStaff.item_serial);
			field_item.RemoveStaff (m_dataStaff.staff_serial);

			Dictionary< string , string > dict = new Dictionary< string , string > ();
			dict.Add ("office_serial", 0.ToString ()); 
			dict.Add ("item_serial", 0.ToString ()); 
			//dict.Add ("setting_time", "\"" + strNow + "\"");
			GameMain.dbStaff.Update (m_dataStaff.staff_serial, dict);

			CsvStaffData staff_data = DataManager.GetStaff (m_dataStaff.staff_id);
			GameMain.Instance.m_iCostNokori += staff_data.cost;

			GameMain.ListRefresh = true;

			m_eStep = STEP.IDLE;
			// 仕事の確認
			DataWork.WorkCheck ();
			GameMain.Instance.HeaderRefresh ();
			// バックヤードに戻す場合はこっちでおじさんをけすd
			Destroy (m_ojisanCheck.gameObject);

			break;

		default:
			break;
		}
	
	}
}






























