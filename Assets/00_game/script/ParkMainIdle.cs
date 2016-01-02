using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParkMainIdle : ParkMainController
{
	public enum STEP
	{
		NONE	= 0,
		IDLE,
		SWIPE,
		PINCH,

		SHOP,

		SPEED_BUILD_CHECK,
		SPEED_BUILD,

		END,
		MAX,
	}

	public STEP m_eStep;
	protected STEP m_eStepPre;
	public DataItem m_selectItem;

	public CtrlShopDetail m_ctrlShopDetail;
	public CtrlOjisanCheck m_ojisanCheck;
	public CtrlItemCheck m_itemCheck;

	public float m_fPinchValueBase;
	public float m_fPinchValue;
	private int m_iNokoriTime;
	public bool m_bLongTapCheck;
	public float m_fLongTapTime;

	public override bool IsEnd ()
	{
		return (m_eStep == STEP.END);
	}
	override protected void initialize ()
	{
		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;

	}
	// Update is called once per frame
	void Update ()
	{

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre = m_eStep;
			bInit = true;
			Debug.Log (m_eStep);
		}

		switch (m_eStep) {
		case STEP.IDLE:
			if (bInit) {
				m_parkMain.m_bInputTrigger = false;
				InputManager.Instance.m_TouchInfo.TouchUp = false;

				// 更新
				DataManager.Instance.m_ItemDataList = GameMain.dbItem.Select (" status != 0 ");

				m_fLongTapTime = 0.0f;
				m_bLongTapCheck = false;
				GameMain.Instance.BuildingSerial = 0;
			}

			if (0 < GameMain.Instance.SwitchItemSerial) {
				int iSelectSerial = GameMain.Instance.SwitchItemSerial;
				GameMain.Instance.m_iSettingItemSerial = iSelectSerial;
				GameMain.Instance.SwitchItemSerial = 0;
				m_selectItem = GameMain.dbItem.Select (iSelectSerial);
				SoundManager.Instance.PlaySE (SoundName.BUTTON_SELECT);
				CtrlFieldItem field_item = GameMain.ParkRoot.GetFieldItem (iSelectSerial);
				GameMain.Instance.SetStatus (GameMain.STATUS.CAGE_DETAIL);
			}
			else if (GameMain.Instance.TutorialInputLock == true ) {
				// チュートリアルの入力制限中はすぐ終わる
			}
			else if (InputManager.Instance.IsPinch ()) {
				m_eStep = STEP.PINCH;
			} else if (InputManager.Instance.m_TouchInfo.Swipe) {
				m_eStep = STEP.SWIPE;
			} else if (InputManager.Instance.m_TouchInfo.TouchUp) {
				InputManager.Instance.m_TouchInfo.TouchUp = false;
				int iGridX = 0;
				int iGridY = 0;

				if (GameMain.GetGrid (InputManager.Instance.m_TouchInfo.TouchPoint, out iGridX, out iGridY)) {

					int iSelectSerial = 0;

					if (0 < GameMain.Instance.BuildingSerial) {
						iSelectSerial = GameMain.Instance.BuildingSerial;
						GameMain.Instance.BuildingSerial = 0;
					} else {
						foreach (DataItem data_item in DataManager.Instance.m_ItemDataList) {
							if (GameMain.GridHit (iGridX, iGridY, data_item)) {
								iSelectSerial = data_item.item_serial;
							}
						}
					}

					if (0 < iSelectSerial) {
						Debug.Log ("hit:serial=" + iSelectSerial.ToString ());

						GameMain.Instance.m_iSettingItemSerial = iSelectSerial;
						m_selectItem = GameMain.dbItem.Select (iSelectSerial);
						int iCategory = m_selectItem.category;
						if (iCategory == (int)Define.Item.Category.SHOP) {
							SoundManager.Instance.PlaySE (SoundName.BUTTON_SELECT);
							CtrlFieldItem field_item = GameMain.ParkRoot.GetFieldItem (iSelectSerial);
							if (field_item.IsReady ()) {
								m_iNokoriTime = field_item.GetNokoriTime ();
								m_eStep = STEP.SPEED_BUILD_CHECK;
							} else {
								m_eStep = STEP.SHOP;
							}
						} else if (iCategory == (int)Define.Item.Category.CAGE) {
							SoundManager.Instance.PlaySE (SoundName.BUTTON_SELECT);
							CtrlFieldItem field_item = GameMain.ParkRoot.GetFieldItem (iSelectSerial);
							if (field_item.IsReady ()) {
								m_iNokoriTime = field_item.GetNokoriTime ();
								m_eStep = STEP.SPEED_BUILD_CHECK;
							} else {
								GameMain.Instance.SetStatus (GameMain.STATUS.CAGE_DETAIL);
							}

						} else if (iCategory == (int)Define.Item.Category.OFFICE) {
							SoundManager.Instance.PlaySE (SoundName.BUTTON_SELECT);
							CtrlFieldItem field_item = GameMain.ParkRoot.GetFieldItem (iSelectSerial);
							if (field_item.IsReady ()) {
								m_iNokoriTime = field_item.GetNokoriTime ();
								m_eStep = STEP.SPEED_BUILD_CHECK;
							} else {
								GameMain.Instance.SetStatus (GameMain.STATUS.OFFICE_DETAIL);
							}
						} else {
						}
						break;
					}
				}
			} else if (m_parkMain.m_bInputTrigger == false && InputManager.Instance.m_TouchInfo.TouchON) {
				m_parkMain.m_bInputTrigger = true;
				Vector2 pos = InputManager.Instance.m_TouchInfo.TouchPoint;

				Vector2 offset = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f - 40.0f);

				Vector2 use = InputManager.Instance.m_TouchInfo.TouchPoint - offset;

			} else if (InputManager.Instance.m_TouchInfo.TouchON == false) {
				m_parkMain.m_bInputTrigger = false;
			} else if (InputManager.Instance.m_TouchInfo.TouchON && m_bLongTapCheck== false ) {
				if (m_fLongTapTime < Define.LONG_TAP_TIME) {
					m_fLongTapTime += Time.deltaTime;
				} else {
					int iGridX = 0;
					int iGridY = 0;
					if (GameMain.GetGrid (InputManager.Instance.m_TouchInfo.TouchPoint, out iGridX, out iGridY)) {

						int iSelectSerial = 0;
						foreach (DataItem data_item in DataManager.Instance.m_ItemDataList) {
							if (GameMain.GridHit (iGridX, iGridY, data_item)) {
								iSelectSerial = data_item.item_serial;
							}
						}
						if (0 < iSelectSerial) {
							Debug.Log ("hit:serial=" + iSelectSerial.ToString ());

							GameMain.Instance.m_iSettingItemSerial = iSelectSerial;
							m_selectItem = GameMain.dbItem.Select (iSelectSerial);
							int iCategory = m_selectItem.category;
							if (iCategory == (int)Define.Item.Category.SHOP) {
							} else if (iCategory == (int)Define.Item.Category.CAGE) {
							} else if (iCategory == (int)Define.Item.Category.OFFICE) {
							} else {
							}
							m_eStep = STEP.END;
							break;
						}
					}
				}
			} else {
			}

			break;
		case STEP.SWIPE:
			m_parkMain.goParkRoot.transform.localPosition += new Vector3 (InputManager.Instance.m_TouchInfo.SwipeAdd.x, InputManager.Instance.m_TouchInfo.SwipeAdd.y, 0.0f);

			float fMaxX = (DataManager.user.m_iWidth ) * Define.CELL_X_DIR.x;
			float fMinX = fMaxX * -1.0f;

			if (m_parkMain.goParkRoot.transform.localPosition.x < fMinX) {
				m_parkMain.goParkRoot.transform.localPosition = new Vector3 (fMinX, m_parkMain.goParkRoot.transform.localPosition.y, m_parkMain.goParkRoot.transform.localPosition.z);
			} else if (fMaxX < m_parkMain.goParkRoot.transform.localPosition.x) {
				m_parkMain.goParkRoot.transform.localPosition = new Vector3 (fMaxX, m_parkMain.goParkRoot.transform.localPosition.y, m_parkMain.goParkRoot.transform.localPosition.z);
			} else {
			}

			float fMaxY = 0.0f;
			float fMinY = (DataManager.user.m_iHeight*2) * Define.CELL_X_DIR.y * -1.0f;
			if (m_parkMain.goParkRoot.transform.localPosition.y < fMinY) {
				m_parkMain.goParkRoot.transform.localPosition = new Vector3 (m_parkMain.goParkRoot.transform.localPosition.x, fMinY , m_parkMain.goParkRoot.transform.localPosition.z);
			} else if (fMaxY < m_parkMain.goParkRoot.transform.localPosition.y) {
				m_parkMain.goParkRoot.transform.localPosition = new Vector3 (m_parkMain.goParkRoot.transform.localPosition.x, fMaxY , m_parkMain.goParkRoot.transform.localPosition.z);
			} else {
			}


			if (InputManager.Instance.m_TouchInfo.Swipe == false) {
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.PINCH:
			if (bInit) {
				m_fPinchValueBase = InputManager.Instance.m_TouchInfo.PinchPos.magnitude;
				m_fPinchValue = m_fPinchValueBase;
				//m_fPinchValue = InputManager.Instance.m_TouchInfo.PinchDelta;
				Debug.Log (InputManager.Instance.m_TouchInfo.PinchPos);

			}

			//Debug.Log (string.Format ("base={0} now={1} rate={2} ", InputManager.Instance.m_TouchInfo.PinchDelta, m_fPinchValue, (InputManager.Instance.m_TouchInfo.PinchDelta / m_fPinchValue)));
			Debug.Log (string.Format ("base={0} now={1} delta={2} rate={3} ", m_fPinchValueBase, m_fPinchValue,InputManager.Instance.m_TouchInfo.PinchDelta, (m_fPinchValue / m_fPinchValueBase)));

			// ここの判定危険かも
			if (InputManager.Instance.IsPinch () == false && InputManager.Instance.m_TouchInfo.TouchUp) {
				m_eStep = STEP.IDLE;
			} else {

				m_fPinchValue = InputManager.Instance.m_TouchInfo.PinchDelta;
				m_fPinchValue *= 0.001f;

				float fRate = GameMain.ParkRoot.myTransform.localScale.x;
				fRate += m_fPinchValue;

				if (fRate < 0.5f) {
					fRate = 0.5f;
				} else if (2.0f < fRate) {
					fRate = 2.0f;
				}
				GameMain.ParkRoot.myTransform.localScale = new Vector3 (fRate, fRate, fRate);

			}
			break;

		case STEP.SHOP:
			if (bInit) {

				GameObject objShopDetail = PrefabManager.Instance.MakeObject ("prefab/PrefShopDetail", gameObject);
				m_ctrlShopDetail = objShopDetail.GetComponent<CtrlShopDetail> ();
				m_ctrlShopDetail.Init (m_selectItem , m_parkMain.m_csParkRoot );
				//m_selectItem;
			}
			if (m_ctrlShopDetail.IsEnd ()) {
				Destroy (m_ctrlShopDetail.gameObject);
				m_ctrlShopDetail = null;
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.SPEED_BUILD_CHECK:
			if (bInit) {

				//PrefItemCheck
				GameObject objOjisan = PrefabManager.Instance.MakeObject ("prefab/PrefItemCheck", gameObject);
				m_itemCheck = objOjisan.GetComponent<CtrlItemCheck> ();
				string strNokoriTime = makeNokoriTime (m_iNokoriTime);
				int iNeedTicket = RequireTicketNum (m_iNokoriTime);
				m_itemCheck.Initialize ( m_selectItem.item_id , string.Format ("この施設は完成まで\n{0}です\n\nチケット{1}枚で\nすぐに完成します。", strNokoriTime, iNeedTicket));

				if (DataManager.user.m_iTicket < iNeedTicket) {
					m_itemCheck.YesOrNo.EnableButtonYes (false);
				}
			}
			if (m_itemCheck.IsYes ()) {
				m_eStep = STEP.SPEED_BUILD;
			} else if (m_itemCheck.IsNo ()) {
				Destroy (m_itemCheck.gameObject);
				m_eStep = STEP.IDLE;

			} else {
			}
			break;

		case STEP.SPEED_BUILD:
			Destroy (m_itemCheck.gameObject);

			CsvItemData csv_item_data = DataManager.GetItem (m_selectItem.item_id);

			Dictionary< string , string > dict = new Dictionary< string , string > ();
			//dict.Add ("create_time", "\"1900-01-01 00:00:00\""); 
			//dict.Add ("setting_time", "\""+ TimeManager.StrGetTime() + "\""); 
			dict.Add ("create_time", "\""+ TimeManager.StrGetTime(-1*csv_item_data.production_time) + "\""); 

			GameMain.dbItem.Update (m_selectItem.item_serial, dict);
			DataManager.user.AddTicket (-1 * RequireTicketNum (m_iNokoriTime));

			// 仕事の確認
			DataWork.WorkCheck ();
			GameMain.Instance.HeaderRefresh ();
			m_eStep = STEP.IDLE;
			break;


		default:
			break;

		}

			
	}

	// if文の書き方で少しわかりにくくなってしまった
	public int RequireTicketNum( int _iNokoriSec ){

		int iRet = 0;
		if (99999 < _iNokoriSec) {
			iRet = 5;
		} else if (3600 < _iNokoriSec) {
			iRet = 5;
		} else if (1800 < _iNokoriSec) {
			iRet = 4;
		} else if (900 < _iNokoriSec) {
			iRet = 3;
		} else if (300 < _iNokoriSec) {
			iRet = 2;
		} else {
			iRet = 1;
		}
		return iRet;

	}

	private string makeNokoriTime( int _iNokoriSec ){
		string strNokoriTime = "あと";
		int iHour = _iNokoriSec / 3600;
		int iMinute = (_iNokoriSec % 3600) / 60;
		int iSec = (_iNokoriSec % 60);

		if (3600 <= _iNokoriSec) {
			strNokoriTime += iHour.ToString () + "時間" + iMinute.ToString () + "分";
		} else if (60 <= _iNokoriSec) {
			strNokoriTime += iMinute.ToString () + "分" + iSec.ToString () + "秒";
		} else {
			strNokoriTime += iSec.ToString () + "秒";
		}
		return strNokoriTime;
	}


}




























