using UnityEngine;
using System.Collections;

public class DebugNumGold : DebugNumBase {

	override protected void initialize (){
		m_lbNum.text = DataManager.user.m_iGold.ToString ();
	}

	override protected void func_plus (){
		DataManager.user.AddGold (m_iRate);
		m_lbNum.text = DataManager.user.m_iGold.ToString ();
		GameMain.Instance.HeaderRefresh ();
	}
	override protected void func_minus(){
		DataManager.user.AddGold (m_iRate * -1);
		m_lbNum.text = DataManager.user.m_iGold.ToString ();
		GameMain.Instance.HeaderRefresh ();
	}

}
