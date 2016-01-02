using UnityEngine;
using System.Collections;

public class CtrlItemDetailDisp : CtrlItemDetailBase {

	#region SerializeField設定
	[SerializeField]
	private UISprite m_sprBlack;
	#endregion
	/*
	protected ButtonBase m_btnClose;
	protected CtrlCloseButton m_closeButton;
	*/

	override protected void initialize(){

		m_ctrlFieldItem.gameObject.transform.parent = gameObject.transform;
		m_ctrlFieldItem.gameObject.transform.localPosition = new Vector3 (0.0f, -240.0f, 0.0f);

		makeCloseButton ( false );

		AddDepth (m_ctrlFieldItem.gameObject, m_sprBlack.depth);
		AddDepth (m_btnClose.gameObject, m_sprBlack.depth);

		return;
	}

	override protected void close(){
		// 閉じる的な終了時
		m_ctrlFieldItem.gameObject.transform.parent = GameMain.ParkRoot.transform;
		m_ctrlFieldItem.SetPos (m_dataItem.x, m_dataItem.y);
		return;
	}


	protected CtrlCloseButton makeCloseButton(bool _bAuto){
		GameObject objCloseButton = PrefabManager.Instance.MakeObject ("prefab/PrefCloseButton", gameObject);
		objCloseButton.transform.localPosition = new Vector3 (0.0f, -427.0f, 0.0f);
		m_btnClose = objCloseButton.GetComponent<ButtonBase> ();
		m_btnClose.TriggerClear ();

		m_closeButton = objCloseButton.GetComponent<CtrlCloseButton> ();
		m_closeButton.enabled = _bAuto;
		return m_closeButton;
	}
	public bool IsClose(){
		if (m_btnClose != null) {
			return m_btnClose.ButtonPushed;
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
