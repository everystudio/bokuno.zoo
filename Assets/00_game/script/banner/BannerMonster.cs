using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ButtonBase))]
public class BannerMonster : BannerBase {

	public enum STEP
	{
		NONE		= 0,
		IDLE		,

		DETAIL		,
		SET_CAGE	,

		SET_BUY		,
		SET_MINE	,

		SICK		,

		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	#region SerializeField設定
	[SerializeField]
	private UISprite m_sprBuyBase;
	[SerializeField]
	private UILabel m_lbBuyPrice;

	[SerializeField]
	private UISprite m_sprIllness;

	#endregion

	public GameObject m_goTutorialRoot;

	public int m_iMonsterId;
	public int m_iMonsterSerial;
	private ButtonBase m_buttonBase;

	private CtrlMonsterDetail m_monsterDetail;
	private DataMonster m_dataMonster;
	private DataMonsterMaster m_dataMonsterMaster;
	public CtrlOjisanCheck m_ojisanCheck;

	public void Initialize( DataMonsterMaster _dataMaster , int _iCostNokori ){
		m_bIsUserData = false;
		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;

		m_lbTitle.text = _dataMaster.name;
		m_lbTitle2.text = _dataMaster.name;
		m_lbDescription.text = _dataMaster.description_cell;

		//m_lbPrize.text = _dataMaster.size.ToString();
		m_lbPrize.text = _dataMaster.cost.ToString();
		m_lbPrizeExp.text = _dataMaster.cost.ToString ();

		string strIcon = GetSpriteName (_dataMaster);
		UIAtlas atlas = AtlasManager.Instance.GetAtlas (strIcon);
		m_sprIcon.atlas = atlas;
		m_sprIcon.spriteName = strIcon;

		SetPrice (_dataMaster);

		m_buttonBase = GetComponent<ButtonBase> ();
		m_monsterDetail = null;

		m_dataMonsterMaster = _dataMaster;

		m_lbDifficulty.text = UtilString.GetSyuunyuu (m_dataMonsterMaster.revenew_coin, m_dataMonsterMaster.revenew_interval);

		m_bAbleUse = DataManager.user.AbleBuy (_dataMaster.coin, _dataMaster.ticket,  m_dataMonsterMaster.cost , _iCostNokori , 0 , 0 , ref m_eReason );

		if (0 < GameMain.Instance.TutorialMonster) {
			if (GameMain.Instance.TutorialMonster == _dataMaster.monster_id) {
				m_goTutorialRoot.SetActive (true);
			} else {
				// 実際は違うけどとりあえず何か理由が欲しかった
				m_eReason = ABLE_BUY_REASON.LIMIT;
				m_bAbleUse = false;
			}
		}

		SetReasonSprite (m_sprReason, m_eReason);
		m_sprIgnoreBlack.gameObject.SetActive (!m_bAbleUse);
		SetEnableIcon (m_bAbleUse);

		m_sprIllness.enabled = false;

	}

	public void Initialize( DataMonster _data , int _iCostNokori ){
		DataMonsterMaster master_data = GameMain.dbMonsterMaster.Select (_data.monster_id);
		Initialize (master_data , _iCostNokori);
		m_dataMonster = _data;

		m_bIsUserData = true;

		Debug.Log (m_dataMonsterMaster.cost);
		Debug.Log (_iCostNokori);
		m_bAbleUse = DataManager.user.AbleBuy (0, 0, m_dataMonsterMaster.cost, _iCostNokori, 0, 0, ref m_eReason);
		SetReasonSprite (m_sprReason, m_eReason);
		if (BannerBase.Mode == BannerBase.MODE.MONSTER_DETAIL) {
			m_bAbleUse = true;
		} else if (BannerBase.Mode == BannerBase.MODE.MONSTER_SICK) {
			m_bAbleUse = true;
		} else {
		}

		m_sprIllness.enabled = false;
		if (m_dataMonster.condition == (int)Define.Monster.CONDITION.SICK) {
			m_sprIllness.enabled = true;
		}
		m_sprIgnoreBlack.gameObject.SetActive (!m_bAbleUse);
		SetEnableIcon (m_bAbleUse);

		m_sprBuyBase.gameObject.SetActive (false);
	}

	private string GetSpriteName( DataMonsterMaster _data ){
		string strRet = "";
		strRet = "chara" + _data.monster_id.ToString ();
		return strRet;
	}

	private void SetPrice( DataMonsterMaster _data ){
		string strText = "";
		string strImageName = "";


		if (0 < _data.coin) {
			strImageName = "list_buy1";
			strText = _data.coin.ToString () + "G";
		} else if (0 < _data.ticket) {
			strImageName = "list_buy2";
			strText = _data.ticket.ToString () + "枚";
		} else {
		}
		/*
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
		*/
		m_sprBuyBase.spriteName = strImageName;
		m_lbBuyPrice.text = strText;
		return;
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

				if (m_bAbleUse) {
					Debug.Log ("clicked:BannerBase.Mode=" + BannerBase.Mode.ToString ());
					//SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);

					switch (BannerBase.Mode) {
					case BannerBase.MODE.MONSTER_DETAIL:
						m_eStep = STEP.DETAIL;
						break;
					case BannerBase.MODE.MONSTER_SET_BUY:
						m_eStep = STEP.SET_BUY;
						break;
					case BannerBase.MODE.MONSTER_SET_MINE:
						m_eStep = STEP.SET_MINE;
						break;
					case BannerBase.MODE.MONSTER_SICK:
						m_eStep = STEP.SICK;
						break;
					default:
						break;
					}
				}
			}
			break;

		case STEP.DETAIL:
			if (bInit) {
				SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);
				// この作り方はいただけませんねぇ・・・
				GameObject obj = PrefabManager.Instance.MakeObject ("prefab/PrefMonsterDetail", gameObject.transform.parent.parent.parent.parent.gameObject);
				m_monsterDetail = obj.GetComponent<CtrlMonsterDetail> ();
				m_monsterDetail.Initialize (m_dataMonster.monster_serial);
			}
			if (m_monsterDetail.IsEnd ()) {
				SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);

				Destroy (m_monsterDetail.gameObject);
				m_monsterDetail = null;
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.SET_BUY:
			if (bInit) {

				SoundManager.Instance.PlaySE (SoundName.SET_ANIMAL);

				CsvMonsterData monster_data = DataManager.GetMonster (m_dataMonsterMaster.monster_id);
				if (0 < monster_data.coin) {
					DataManager.user.AddGold (-1 * monster_data.coin);
				} else if (0 < monster_data.ticket) {
					DataManager.user.AddTicket (-1 * monster_data.ticket); 
				} else {
					;// エラーちゃう？
				}

				// 0番のページに飛ばす
				DataMonster insertMonster = GameMain.dbMonster.Insert (m_dataMonsterMaster.monster_id, GameMain.Instance.m_iSettingItemSerial);

				m_tabParent.TriggerOn (0);

				CtrlFieldItem fielditem = GameMain.ParkRoot.GetFieldItem (GameMain.Instance.m_iSettingItemSerial);
				GameObject objIcon = PrefabManager.Instance.MakeObject ("prefab/PrefIcon", fielditem.gameObject);
				CtrlIconRoot iconRoot = objIcon.GetComponent<CtrlIconRoot> ();
				iconRoot.Initialize (insertMonster,fielditem);
				fielditem.Add (iconRoot);

				// 仕事の確認
				DataWork.WorkCheck ();
				GameMain.Instance.HeaderRefresh ();

				if (0 < GameMain.Instance.TutorialMonster) {
					TutorialManager.Instance.Next ();
					GameMain.Instance.TutorialMonster = 0;
				}

			}
			break;

		case STEP.SET_MINE:
			if (bInit) {

				SoundManager.Instance.PlaySE (SoundName.SET_ANIMAL);
				Dictionary< string , string > dict = new Dictionary< string , string > ();
				dict.Add( "item_serial" , GameMain.Instance.m_iSettingItemSerial.ToString() ); 
				dict.Add( "collect_time" , "\"" + TimeManager.StrNow() +  "\""); 

				GameMain.dbMonster.Update (m_dataMonster.monster_serial , dict );
				//GameMain.dbMonster.Update (m_dataMonster.monster_serial, GameMain.Instance.m_iSettingItemSerial);

				m_tabParent.TriggerOn (0);

				DataMonster insertMonster = GameMain.dbMonster.Select (m_dataMonster.monster_serial);
				CtrlFieldItem fielditem = GameMain.ParkRoot.GetFieldItem (GameMain.Instance.m_iSettingItemSerial);
				GameObject objIcon = PrefabManager.Instance.MakeObject ("prefab/PrefIcon", fielditem.gameObject);
				CtrlIconRoot iconRoot = objIcon.GetComponent<CtrlIconRoot> ();
				iconRoot.Initialize (insertMonster, fielditem);
				fielditem.Add (iconRoot);
				 
				// 仕事の確認
				DataWork.WorkCheck ();
				GameMain.Instance.HeaderRefresh ();
			}
			break;

		case STEP.SICK:
			if (bInit) {
				GameObject objOjisan = PrefabManager.Instance.MakeObject ("prefab/PrefOjisanCheck", gameObject.transform.parent.parent.parent.parent.gameObject );

				CsvMonsterData monster = DataManager.GetMonster (m_dataMonster.monster_id);

				int iCost = monster.revenew_coin * (int)(600.0f / (float)monster.revenew_interval);

				bool bYesOnly = false;
				string strText = string.Format ("こちらの動物を\n治療しますか\n\n治療費:{0}G\n\n{1}G → [FFD900]{2}[-]G", iCost , DataManager.user.m_iGold , DataManager.user.m_iGold -iCost );
				if (DataManager.user.m_iGold < iCost) {
					bYesOnly = true;
					strText = string.Format ("こちらの動物を\n治療しますか\n治療費:{0}G\n\n[FFD900]GOLDが足りません[-]", iCost);
				}

				m_ojisanCheck = objOjisan.GetComponent<CtrlOjisanCheck> ();
				m_ojisanCheck.Initialize ( strText , bYesOnly );
			}
			if (m_ojisanCheck.IsYes ()) {
				SoundManager.Instance.PlaySE ("se_cure");
				CsvMonsterData monster = DataManager.GetMonster (m_dataMonster.monster_id);
				int iCost = monster.revenew_coin * (int)(600.0f / (float)monster.revenew_interval);
				DataManager.user.AddGold (-1 * iCost);

				GameMain.ListRefresh = true;
				Destroy (m_ojisanCheck.gameObject);

				Dictionary< string , string > dict = new Dictionary< string , string > ();
				int iConditionFine = (int)Define.Monster.CONDITION.FINE;
				dict.Add( "condition" , iConditionFine.ToString() ); 
				dict.Add( "clean_time" , string.Format( "\"{0}\" " , TimeManager.StrGetTime()) ); 
				Debug.Log (TimeManager.StrGetTime ());
				GameMain.dbMonster.Update (m_dataMonster.monster_serial , dict );
				m_eStep = STEP.IDLE;
			} else if (m_ojisanCheck.IsNo ()) {
				SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);

				Destroy (m_ojisanCheck.gameObject);
				m_eStep = STEP.IDLE;
			} else {
			}
			break;

		case STEP.SET_CAGE:
			break;

		case STEP.MAX:
		default:
			break;
		}


	
	}
}















