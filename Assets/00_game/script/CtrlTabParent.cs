using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlTabParent : ButtonManager {

	#region serializefield対応
	[SerializeField]
	private GameObject m_posGrid;
	#endregion

	private int m_iSelectingTabIndex;
	private Tab.TYPE m_eSelectingTabType;
	private List<CtrlTabChild> m_tabChildList = new List<CtrlTabChild>();

	public void Init( Tab[] _tabArr , int _iSelectingTabIndex = 0 ){

		myTransform.localPosition = new Vector3 (0.0f, 280.0f, 0.0f);

		// 先に配列を確保（この後のadd関数でエラーとか詐欺っぽい作りじゃな・・・）
		ButtonRefresh (_tabArr.Length);

		int iButtonIndex = 0;
		foreach (Tab tab in _tabArr) {
			GameObject obj = PrefabManager.Instance.MakeObject ("prefab/PrefTabChild", m_posGrid);
			CtrlTabChild child = obj.GetComponent<CtrlTabChild> ();
			child.Init (tab);
			m_tabChildList.Add (child);
			AddButtonBase (iButtonIndex,obj);
			iButtonIndex += 1;
		}
		SelectTab (m_iSelectingTabIndex);
		// インデックスふってあるからやらなくてもいいんですけどね
		ButtonInit ();
		Index = _iSelectingTabIndex;
	}

	public Tab.TYPE GetSelectingTabType(){
		return m_eSelectingTabType;
	}

	private void SelectTab( int _iIndex ){
		m_eSelectingTabType = m_tabChildList [m_iSelectingTabIndex].GetTabType();
		//Debug.Log ("SelectTab:" + _iIndex.ToString ());
		int iIndex = 0;
		foreach (CtrlTabChild child in m_tabChildList) {
			bool bFlag = false;
			if (iIndex == _iIndex) {
				bFlag = true;
			} else {
			}
			child.Switch (bFlag);
			iIndex += 1;
		}
		return;
	}

	// Update is called once per frame
	void Update () {

		if (ButtonPushed || (m_iSelectingTabIndex != Index)) {
			m_iSelectingTabIndex = Index;
			SelectTab (m_iSelectingTabIndex);
			TriggerClearAll ();
		}


	}
}

















