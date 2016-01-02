using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlStaffSetting : MonoBehaviour {

	private CtrlDispOffice m_dispOffice;
	private bool m_bIsEnd;

	private CtrlYesNoButton m_buttonYesNo;

	public float m_fTimer;

	public DataItem m_dataOffice;
	public int m_iSetStaffId;
	public int m_iSetStaffSerial;

	/*
	 * どの場所に
	 * どの種類のスタッフを配置するか
	 * しかもお手持ちおスタッフかどうかも判断
	 * */
	public void Initialize( DataItem _dataOffice , int _iStaffId , int _iStaffSerial ){
		/*
		Debug.Log ("Initialize");
		Debug.Log (_dataOffice);
		Debug.Log (_iStaffId);
		Debug.Log (_iStaffSerial);
		*/

		m_dataOffice = _dataOffice;
		m_iSetStaffId = _iStaffId;
		m_iSetStaffSerial = _iStaffSerial;

		GameObject dispRoot = gameObject.transform.parent.parent.parent.parent.gameObject;
	
		//GameObject obj = PrefabManager.Instance.MakeObject ("prefab/PrefDispOffice", gameObject.transform.parent.parent.parent.parent.gameObject);
		GameObject obj = PrefabManager.Instance.MakeObject ("prefab/PrefDispOffice", dispRoot );
		m_dispOffice = obj.GetComponent< CtrlDispOffice> ();
		m_dispOffice.Initialize (m_dataOffice.item_serial , GameMain.PanelFront );
		m_dispOffice.SetColor (Define.Item.Category.SHOP, new Color (0.75f, 0.75f, 0.75f));
		m_dispOffice.SetColor (Define.Item.Category.NONE, new Color (0.75f, 0.75f, 0.75f));

		m_iSelectingCageSerial = m_dispOffice.SelectingCageSerial;
		m_iSelectingCageSerialPre = m_iSelectingCageSerial;

		m_bIsEnd = false;

		m_fTimer = 0.0f;
		GameObject objYesNo = PrefabManager.Instance.MakeObject ("prefab/PrefYesNoButton", GameMain.PanelFront );
		objYesNo.transform.localPosition = new Vector3 (0.0f, -400.0f, 0.0f);
		m_buttonYesNo = objYesNo.GetComponent<CtrlYesNoButton> ();


		m_buttonYesNo.EnableButtonYes (false);

		return;
	}

	public void Close(){
		m_dispOffice.Close ();
		Destroy (m_buttonYesNo.gameObject);
	}

	public bool IsEnd(){
		return m_bIsEnd;
	}

	public int m_iSelectingCageSerial;
	public int m_iSelectingCageSerialPre;

	// Update is called once per frame
	void Update () {
		//m_fTimer += Time.deltaTime;
		if (10.0f < m_fTimer) {
			m_bIsEnd = true;
		}

		if (m_buttonYesNo.IsYes ()) {
			m_bIsEnd = true;
			DataStaff staff;
			int iStaffCost = 0;
			if (0 < m_iSetStaffSerial) {
				//GameMain.dbStaff.Update(
				Debug.Log (string.Format( "set staff from backyard:{0}", m_iSetStaffSerial));
				string strNow = TimeManager.StrNow ();
				Dictionary< string , string > dict = new Dictionary< string , string > ();
				dict.Add( "office_serial" , m_dataOffice.item_serial.ToString() ); 
				dict.Add( "item_serial" , m_dispOffice.SelectingCageSerial.ToString() ); 
				dict.Add ("setting_time", "\"" + strNow + "\"");
				staff = GameMain.dbStaff.Update (m_iSetStaffSerial,dict);

				CsvStaffData staff_data = DataManager.GetStaff (m_iSetStaffId);
				iStaffCost = staff_data.coin;
			} else {
				staff = GameMain.dbStaff.Insert ( m_iSetStaffId , m_dataOffice.item_serial , m_dispOffice.SelectingCageSerial);

				CsvStaffData staff_data = DataManager.GetStaff (m_iSetStaffId);
				if (0 < staff_data.coin) {
					DataManager.user.AddGold (-1 * staff_data.coin);
				} else if (0 < staff_data.ticket) {
					DataManager.user.AddTicket (-1 * staff_data.ticket); 
				} else {
					;// エラーちゃう？
				}
				iStaffCost = staff_data.cost;
			}

			CtrlFieldItem fielditem = GameMain.ParkRoot.GetFieldItem (m_dispOffice.SelectingCageSerial);
			GameObject objIcon = PrefabManager.Instance.MakeObject ("prefab/PrefIcon", fielditem.gameObject);
			CtrlIconRoot iconRoot = objIcon.GetComponent<CtrlIconRoot> ();
			iconRoot.Initialize (staff , fielditem );
			fielditem.Add (iconRoot);

			// 仕事の確認
			DataWork.WorkCheck ();
			GameMain.Instance.HeaderRefresh ();
			GameMain.ListRefresh = true;

			GameMain.Instance.m_iCostNokori -= iStaffCost;

		} else if (m_buttonYesNo.IsNo ()) {
			m_bIsEnd = true;
		} else {
			;// 
		}

		m_iSelectingCageSerial = m_dispOffice.SelectingCageSerial;

		if (m_iSelectingCageSerialPre != m_iSelectingCageSerial) {
			if (0 < m_iSelectingCageSerialPre) {
				CtrlFieldItem script = GameMain.ParkRoot.GetFieldItem (m_iSelectingCageSerialPre);
				script.SetColor (Color.white);
			}
			if (0 < m_iSelectingCageSerial) {
				m_buttonYesNo.EnableButtonYes (true);

				CtrlFieldItem script = GameMain.ParkRoot.GetFieldItem (m_iSelectingCageSerial);
				script.SetColor (Color.red);
				m_iSelectingCageSerialPre = m_iSelectingCageSerial;
			}

		}




	
	}
}
