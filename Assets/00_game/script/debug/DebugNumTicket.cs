using UnityEngine;
using System.Collections;

public class DebugNumTicket : DebugNumBase {

	override protected void initialize (){
		m_lbNum.text = DataManager.user.m_iTicket.ToString ();
	}

	override protected void func_plus (){
		DataManager.user.AddTicket (m_iRate);
		m_lbNum.text = DataManager.user.m_iTicket.ToString ();
		GameMain.Instance.HeaderRefresh ();
	}

	override protected void func_minus(){
		DataManager.user.AddTicket (m_iRate * -1);
		m_lbNum.text = DataManager.user.m_iTicket.ToString ();
		GameMain.Instance.HeaderRefresh ();
	}
}
