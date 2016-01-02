using UnityEngine;
using System.Collections;

public class CtrlItemDetailBase : MonoBehaviourEx {

	protected int m_iMainItemSerial;
	protected DataItem m_dataItem;
	protected CtrlFieldItem m_ctrlFieldItem;

	public ButtonBase m_btnClose;				// 面倒なので直接押させない
	public CtrlCloseButton m_closeButton;	// 外から終了させたい事情が出来たのでスクリプト追加

	virtual protected void initialize(){
		return;
	}
	virtual protected void close(){
		return;
	}
	virtual protected void remove(){
		return;
	}

	public void Initialize( int _iItemSerial ){
		m_iMainItemSerial = _iItemSerial;
		//GameMain.Instance.m_
		//Debug.Log (m_iMainItemSerial);
		m_dataItem = GameMain.dbItem.Select (m_iMainItemSerial);
		m_ctrlFieldItem = GameMain.ParkRoot.GetFieldItem (m_iMainItemSerial);

		initialize ();
		return;
	}

	public void Close(){
		close ();
		return;
	}

	public void Remove(){
		remove ();
		return;
	}


	protected CtrlCloseButton makeCloseButton(){
		GameObject objCloseButton = PrefabManager.Instance.MakeObject ("prefab/PrefCloseButton", gameObject);
		objCloseButton.transform.localPosition = new Vector3 (0.0f, -427.0f, 0.0f);
		m_btnClose = objCloseButton.GetComponent<ButtonBase> ();
		m_btnClose.TriggerClear ();

		m_closeButton = objCloseButton.GetComponent<CtrlCloseButton> ();
		return m_closeButton;
	}


}
