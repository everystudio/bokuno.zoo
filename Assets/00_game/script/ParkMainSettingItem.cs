using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParkMainSettingItem : ParkMainController {

	public enum STEP
	{
		NONE		= 0,
		IDLE		,

		EDIT_TOUCH	,
		EDIT_SWIPE	,

		SWIPE		,

		HOLD		,

		SETTING		,
		CANCEL		,
		END			,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	public int m_iEditItemX;
	public int m_iEditItemY;
	public int m_iEditOffsetX;
	public int m_iEditOffsetY;
	public CtrlFieldItem m_editItem;
	public DataItemMaster m_editItemMaster;

	public List<Grid> m_DontSetGridList = new List<Grid>();
	public ButtonManager m_YesNoButtonManager;

	public GameObject m_goBuyType;

	override public void Clear() {
		if (m_YesNoButtonManager != null) {
			Destroy (m_YesNoButtonManager.gameObject);
			m_YesNoButtonManager = null;
		}

		if (m_parkMain.m_pageHeader != null) {
			Destroy (m_parkMain.m_pageHeader.gameObject);
		}

		Release (m_goBuyType);
	}

	override protected void initialize(){

		bool bBuyAble = true;
		CtrlFieldItem moveFieldItem = null;
		if (m_parkMain.m_eEditMode == ParkMain.EDIT_MODE.NORMAL) {

			// 道路のみ特殊処理
			// 道のみシリアルがあるものから利用
			// あとあとを見越して、同じitem_idがあれば反応できるようにしておく
			if (GameMain.Instance.m_iSettingItemId == Define.ITEM_ID_ROAD) {
				List<DataItem> same_list = GameMain.dbItem.Select (string.Format (" item_id = {0} and status = {1} ", GameMain.Instance.m_iSettingItemId, (int)Define.Item.Status.NONE));
				if (0 < same_list.Count) {
					GameMain.Instance.m_iSettingItemSerial = same_list [0].item_serial;
				} else {
					GameMain.Instance.m_iSettingItemSerial = 0;
				}
			}
		} else if (m_parkMain.m_eEditMode == ParkMain.EDIT_MODE.MOVE) {
			// そのまま使う
			// 自分を一度消す
			int iRemoveIndex = 0;
			foreach (CtrlFieldItem item in GameMain.ParkRoot.m_fieldItemList) {
				if (item.m_dataItem.item_serial == GameMain.Instance.m_iSettingItemSerial) {
					//item.Remove ();
					moveFieldItem = item;
					GameMain.Instance.m_iSettingItemId = moveFieldItem.m_dataItem.item_id;		// 別にここでやる必要はない
					GameMain.ParkRoot.m_fieldItemList.RemoveAt (iRemoveIndex);
					break;
				}
				iRemoveIndex += 1;
			}

			DataItem dataItem = GameMain.dbItem.Select (GameMain.Instance.m_iSettingItemSerial);
			// 消す予定のところに新しい土地を設置する
			for (int x = dataItem.x; x < dataItem.x + dataItem.width; x++) {
				for (int y = dataItem.y; y < dataItem.y + dataItem.height; y++) {
					GameObject obj = PrefabManager.Instance.MakeObject ("prefab/PrefFieldItem", GameMain.ParkRoot.gameObject);
					obj.name = "fielditem_" + x.ToString () + "_" + y.ToString ();
					CtrlFieldItem script = obj.GetComponent<CtrlFieldItem> ();
					script.Init (x, y, 0);
					GameMain.ParkRoot.m_fieldItemList.Add (script);
				}
			}
		} else {
		}

		//Debug.LogError (GameMain.Instance.m_iSettingItemSerial);
		if (m_parkMain.m_pageHeader == null) {
			m_parkMain.m_pageHeader = m_parkMain.makeHeader ("header_item" , "Setting1");
		}

		m_editItemMaster = GameMain.dbItemMaster.Select (GameMain.Instance.m_iSettingItemId);
		m_DontSetGridList.Clear ();

		Release (m_goBuyType);
		if (GameMain.Instance.m_iSettingItemId == Define.ITEM_ID_ROAD && GameMain.Instance.m_iSettingItemSerial == 0 && GameMain.Instance.PreSettingItemId == Define.ITEM_ID_ROAD ) {
			m_goBuyType = PrefabManager.Instance.MakeObject ("prefab/PrefBuyType", gameObject);
			m_goBuyType.transform.localPosition = new Vector3 (190.0f, 268.0f, 0.0f);
			BuyItemButton buy_type = m_goBuyType.GetComponent<BuyItemButton> ();
			string strBuyType = "[FF0000]購入[-]";
			if (0 < GameMain.Instance.m_iSettingItemSerial) {
				strBuyType = "保管庫";
			}
			buy_type.SetText (strBuyType);
			buy_type.SetText (string.Format("{0}G" , m_editItemMaster.need_coin));
		}

		if (m_YesNoButtonManager == null) {
			GameObject goButtonManager = PrefabManager.Instance.MakeObject ("prefab/PrefYesNoButton", gameObject);
			goButtonManager.transform.localPosition = new Vector3 (0.0f, -400.0f, 0.0f);
			m_YesNoButtonManager = goButtonManager.GetComponent<ButtonManager> ();
			m_YesNoButtonManager.ButtonInit ();
			m_YesNoButtonManager.TriggerClearAll ();
		}
		CtrlYesNoButton yes_no_button = m_YesNoButtonManager.gameObject.GetComponent<CtrlYesNoButton> ();

		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;

		GameMain.Instance.m_bStartSetting = false;
		// ゾンビアイコンを００の位置に表示する
		m_iEditItemX = 3;
		m_iEditItemY = 3;

		GameMain.GetGrid (new Vector2 (Screen.width / 2, Screen.height / 2), out m_iEditItemX, out m_iEditItemY);

		ParkMain.EDIT_MODE eEditMode = ParkMain.EDIT_MODE.NORMAL;
		Debug.Log (m_parkMain.m_eEditMode);
		if (m_parkMain.m_eEditMode == ParkMain.EDIT_MODE.NORMAL) {
			GameObject prefab = PrefabManager.Instance.PrefabLoadInstance ("prefab/PrefFieldItem");
			GameObject obj = PrefabManager.Instance.MakeObject (prefab, m_parkMain.goParkRoot);
			m_editItem = obj.GetComponent<CtrlFieldItem> ();
		} else if (m_parkMain.m_eEditMode == ParkMain.EDIT_MODE.MOVE) {
			eEditMode = ParkMain.EDIT_MODE.MOVE;
			m_editItem = moveFieldItem;//GameMain.ParkRoot.GetFieldItem (GameMain.Instance.m_iSettingItemSerial);

			m_iEditItemX = m_editItem.m_dataItem.x;
			m_iEditItemY = m_editItem.m_dataItem.y;
			Debug.Log (m_editItem.m_dataItem.item_id);
		}
		m_editItem.InitEdit (m_iEditItemX, m_iEditItemY, GameMain.Instance.m_iSettingItemId , eEditMode );
		//Debug.LogError( string.Format( "x:{0} y:{1}" , m_iEditItemX , m_iEditItemY ));


		// お金が足りる
		// 実際には道路しかひっかからないはず
		if (m_editItemMaster.need_coin <= DataManager.user.m_iGold) {
			bBuyAble = true;
			;//OK
		} else {
			bBuyAble = false;
		}

		if (bBuyAble == false && 0 == GameMain.Instance.m_iSettingItemSerial) {
			yes_no_button.EnableButtonYes (false);
		}

		string strWhere = " status != 0 ";
		if (m_parkMain.m_eEditMode == ParkMain.EDIT_MODE.MOVE) {
			strWhere = string.Format (" status != {0} and item_serial != {1} " , (int)Define.Item.Status.NONE , GameMain.Instance.m_iSettingItemSerial );
		}
		DataManager.Instance.m_ItemDataList = GameMain.dbItem.Select (strWhere);
		Grid.SetUsingGrid (ref m_DontSetGridList, DataManager.Instance.m_ItemDataList);
		foreach (Grid dont in m_DontSetGridList) {
			//Debug.Log (string.Format ("x={0} y={1} ", dont.x, dont.y)); 
		}
		bool bAbleSet = Grid.AbleSettingItem (m_editItemMaster, m_iEditItemX, m_iEditItemY, m_DontSetGridList);

		m_editItem.SetEditAble (bAbleSet);

		return;
	}

	public bool m_bButtonLock;

	public bool m_bTapRelease;
	public int m_iTempX;
	public int m_iTempY;

	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
			//Debug.LogError (m_eStep);
		}

		switch (m_eStep) {


		case STEP.IDLE:
			if (bInit) {
				InputManager.Instance.m_TouchInfo.TouchON = false;
				m_YesNoButtonManager.TriggerClearAll ();
				m_bTapRelease = false;
				m_bButtonLock = false;
				Debug.Log (string.Format ("x={0} y={1} size={2} ", m_editItem.m_dataItem.x, m_editItem.m_dataItem.y, m_editItem.m_dataItem.width)); 

			}

			if (InputManager.Instance.m_TouchInfo.Swipe) {
				m_eStep = STEP.SWIPE;


			} else if (m_YesNoButtonManager.ButtonPushed) {
				if (m_YesNoButtonManager.Index == 0) {

					bool bAbleSet = Grid.AbleSettingItem (m_editItemMaster, m_iEditItemX, m_iEditItemY, m_DontSetGridList);
					if (bAbleSet) {
						m_eStep = STEP.SETTING;

						// わけあってこっちから鳴らします
						SoundManager.Instance.PlaySE (SoundName.SET_ITEM);

					} else {
						;// エラー音
					}

				} else {
					m_eStep = STEP.CANCEL;
				}
				m_YesNoButtonManager.TriggerClearAll ();
				m_bButtonLock = true;
			}  else if (InputManager.Instance.m_TouchInfo.TouchON && m_bButtonLock == false ) {
				int iGridX = 0;
				int iGridY = 0;

				if ( Screen.height * 0.2f < InputManager.Instance.m_TouchInfo.TouchPoint.y ) {
					if (GameMain.GetGrid (InputManager.Instance.m_TouchInfo.TouchPoint, out iGridX, out iGridY)) {
						if (GameMain.GridHit (iGridX, iGridY, m_editItem.m_dataItem, out m_iEditOffsetX, out m_iEditOffsetY)) {
							//iSelectSerial = data_item.item_serial;
							m_eStep = STEP.EDIT_TOUCH;
							//Debug.Log ("hit");
						} else {
							//Debug.Log ("miss");
							m_iTempX = iGridX;
							m_iTempY = iGridY;
							m_bTapRelease = true;
						}
					}
				}
				/*
				if (GameMain.GetGrid (InputManager.Instance.m_TouchInfo.TouchPoint, out iGridX, out iGridY)) {
					if (m_iEditItemX == iGridX && m_iEditItemY == iGridY) {
						m_eStep = STEP.EDIT_TOUCH;
					} else {
					}
				}
				*/
			} else if( InputManager.Instance.m_TouchInfo.TouchON == false ){
				if (m_bTapRelease == true) {
					m_bTapRelease = false;
					m_editItem.SetPos (m_iTempX, m_iTempY);

					m_iEditItemX = m_iTempX;
					m_iEditItemY = m_iTempY;
					m_editItem.m_dataItem.x = m_iEditItemX;
					m_editItem.m_dataItem.y = m_iEditItemY;
					bool bAbleSet = Grid.AbleSettingItem (m_editItemMaster, m_iEditItemX, m_iEditItemY, m_DontSetGridList);
					m_editItem.SetEditAble (bAbleSet);
				}
				m_bButtonLock = false;

			} else {
			}
			break;

		case STEP.HOLD:

			break;

		case STEP.SWIPE:
			if (bInit) {
				m_bTapRelease = false;
			}
			m_parkMain.goParkRoot.transform.localPosition += new Vector3( InputManager.Instance.m_TouchInfo.SwipeAdd.x ,InputManager.Instance.m_TouchInfo.SwipeAdd.y , 0.0f );

			if (InputManager.Instance.m_TouchInfo.Swipe == false) {
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.EDIT_TOUCH:
			if (bInit) {
				InputManager.Instance.m_TouchInfo.TouchUp = false;
			}

			if (InputManager.Instance.m_TouchInfo.Swipe) {
				m_eStep = STEP.EDIT_SWIPE;
			} else if (InputManager.Instance.m_TouchInfo.TouchUp) {
				m_eStep = STEP.IDLE;
			} else {
			}


			break;
		case STEP.EDIT_SWIPE:
			if (InputManager.Instance.m_TouchInfo.TouchON) {
				int iGridX = 0;
				int iGridY = 0;

				if (GameMain.GetGrid (InputManager.Instance.m_TouchInfo.TouchPoint, out iGridX, out iGridY)) {

					if (iGridX != m_iEditItemX || iGridY != m_iEditItemY) {
						m_editItem.SetPos (iGridX - m_iEditOffsetX, iGridY - m_iEditOffsetY);
						m_iEditItemX = iGridX - m_iEditOffsetX;
						m_iEditItemY = iGridY - m_iEditOffsetY;
						m_editItem.m_dataItem.x = m_iEditItemX;
						m_editItem.m_dataItem.y = m_iEditItemY;
						bool bAbleSet = Grid.AbleSettingItem (m_editItemMaster, m_iEditItemX, m_iEditItemY, m_DontSetGridList);
						m_editItem.SetEditAble (bAbleSet);

					}
				}

				int iWidth = (int)(Screen.width * 0.1f);
				int iHeight = (int)(Screen.height * 0.3f);
				float fDelta = 20.0f;

				if (InputManager.Instance.m_TouchInfo.TouchPoint.x < iWidth) {

					//Debug.Log ("x short");
					GameMain.ParkRoot.MoveAdd (fDelta, 0.0f);
				} else if ( (Screen.width-iWidth) < InputManager.Instance.m_TouchInfo.TouchPoint.x) {
					//Debug.Log ("x high");
					GameMain.ParkRoot.MoveAdd (fDelta*-1.0f, 0.0f);
				} else {
				}
				if (InputManager.Instance.m_TouchInfo.TouchPoint.y < (int)(Screen.height * 0.3f)) {
					//Debug.Log ("y short");
					GameMain.ParkRoot.MoveAdd (0.0f, fDelta);
				} else if ( ((int)(Screen.height * 0.6f)) < InputManager.Instance.m_TouchInfo.TouchPoint.y) {
					//Debug.Log ("y high");
					GameMain.ParkRoot.MoveAdd (0.0f, fDelta*-1.0f);
				} else {
				}

				//Debug.Log (InputManager.Instance.m_TouchInfo.TouchPoint);


			} else if (InputManager.Instance.m_TouchInfo.TouchUp) {
				m_eStep = STEP.IDLE;
			}

			break;


		case STEP.SETTING:
			if (bInit) {
			}

			if (0 < GameMain.Instance.m_iSettingItemSerial) {

				Debug.LogError (string.Format ("setting serial:{0}", GameMain.Instance.m_iSettingItemSerial));
 				GameMain.dbItem.Update (GameMain.Instance.m_iSettingItemSerial, (int)Define.Item.Status.SETTING, m_iEditItemX, m_iEditItemY);
			} else {
				GameMain.Instance.m_iSettingItemSerial = GameMain.dbItem.Insert (m_editItemMaster, (int)Define.Item.Status.SETTING, m_iEditItemX, m_iEditItemY);

				CsvItemData item_data = DataManager.GetItem (m_editItemMaster.item_id);
				if (0 < item_data.need_coin) {
					DataManager.user.AddGold (-1 * item_data.need_coin);
				} else if (0 < item_data.need_ticket) {
					DataManager.user.AddTicket (-1 * item_data.need_ticket); 
				} else {
					;// エラーちゃう？
					// 課金アイテム
				}

				DataItem.OpenNewItem (m_editItemMaster.item_id);
				/*
				List<DataItemMaster> open_item_list = GameMain.dbItemMaster.Select (string.Format (" status = {0} and open_item_id = {1} ", (int)Define.Item.Status.NONE, m_editItemMaster.item_id));
				foreach (DataItemMaster open_item in open_item_list) {
					Dictionary<string , string > update_value = new Dictionary<string , string > ();
					update_value.Add ("status", string.Format ( "{0}" , (int)Define.Item.Status.SETTING ));
					GameMain.dbItemMaster.Update ( open_item.item_id , update_value);
				}
				*/

			}
			TweenColorAll (m_editItem.gameObject, 0.025f, Color.white);
			TweenAlphaAll (m_editItem.gameObject, 0.025f, 1.0f);

			m_editItem.EditEnd (GameMain.Instance.m_iSettingItemSerial);

			Debug.Log (m_editItem.m_dataItem.width);
			GameMain.ParkRoot.AddFieldItem (m_editItem);
			GameMain.Instance.HeaderRefresh ();
			//m_fieldItemList.Add (m_editItem);
			GameMain.Instance.PreSettingItemId = m_editItem.m_dataItem.item_id;

			if (m_editItem.m_dataItem.item_id == Define.ITEM_ID_ROAD && m_parkMain.m_eEditMode == ParkMain.EDIT_MODE.NORMAL) {
				Debug.Log (m_editItem.m_dataItem.item_id);

				// ここでやり直し
				initialize ();

			} else {
				m_eStep = STEP.END;
			}
			break;

		case STEP.CANCEL:
			if (m_parkMain.m_eEditMode == ParkMain.EDIT_MODE.MOVE) {

				//DataItem return_data_item = GameMain.dbItem.Select (string.Format (" item_serial = {0} " , GameMain.Instance.m_iSettingItemSerial ));

				Debug.LogError (GameMain.Instance.m_iSettingItemSerial);

				DataItem return_data_item = GameMain.dbItem.Select (GameMain.Instance.m_iSettingItemSerial );
				m_iEditItemX = return_data_item.x;
				m_iEditItemY = return_data_item.y;
				m_eStep = STEP.SETTING;

				SoundManager.Instance.PlaySE (SoundName.BUTTON_CANCEL);


			} else {
				Destroy (m_editItem.gameObject);
				m_eStep = STEP.END;
			}


			break;
		case STEP.END:
			break;

		case STEP.MAX:
		default:
			break;
		}
	
	}

	override public bool IsEnd(){
		return (m_eStep == STEP.END);
	}

}














