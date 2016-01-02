using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlDispOffice : CtrlItemDetailBase {

	private List<CtrlFieldItem> m_areaFieldItem = new List<CtrlFieldItem> ();
	private GameObject m_goRootPosition;

	private GameObject m_goBlackBack;

	override protected void close(){
		m_goRootPosition.transform.localScale = Vector3.one;

		m_ctrlFieldItem.gameObject.transform.parent = GameMain.ParkRoot.transform;
		m_ctrlFieldItem.gameObject.transform.localScale = Vector3.one;
		m_ctrlFieldItem.SetPos (m_dataItem.x, m_dataItem.y);
		foreach (CtrlFieldItem field_item in m_areaFieldItem) {
			field_item.gameObject.transform.parent = GameMain.ParkRoot.transform;
			field_item.gameObject.transform.localScale = Vector3.one;
			field_item.ResetPos ();
			field_item.SetColor (Color.white);
		}
		Destroy (m_goRootPosition);
		Release (m_goBlackBack);

		return;
	}

	public void SetColor( Define.Item.Category _category , Color _color ){
		foreach (CtrlFieldItem script in m_areaFieldItem) {
			if (script.m_dataItem.category == (int)_category) {
				script.SetColor (_color);
			}
		}
		return;
	}

	// これを利用して初期化する
	public void Initialize( int _iItemSerial , GameObject _rootPosition ){
		if (m_goRootPosition == null) {
			m_goRootPosition = new GameObject ();
		}
		m_goRootPosition.name = "ctrldispoffice_rootposition";		// 別に名付ける必要はなかったんですけどね
		m_goRootPosition.transform.parent = _rootPosition.transform;
		m_goRootPosition.transform.localScale = Vector3.one;
		//m_goRootPosition.transform.localScale = new Vector3( 0.5f , 0.5f , 0.5f );
		m_goRootPosition.transform.localPosition = Vector3.zero;
		// ここ別にbaseいらないね
		m_goBlackBack = PrefabManager.Instance.MakeObject ("prefab/PrefBlackBack", m_goRootPosition);
		base.Initialize (_iItemSerial);

		return;
	}

	override protected void initialize ()
	{
		m_ctrlFieldItem.gameObject.transform.parent = m_goRootPosition.transform;
		m_ctrlFieldItem.transform.localScale = Vector3.one;
		m_ctrlFieldItem.ResetPos ();

		DataItemMaster master_data = GameMain.dbItemMaster.Select( m_dataItem.item_id );

		Color color = new Color (0.75f, 0.75f, 0.75f);

		for( int x = m_dataItem.x - (master_data.area ) ; x < m_dataItem.x + master_data.size + (master_data.area ) ; x++ ){
			for( int y = m_dataItem.y - (master_data.area ) ; y < m_dataItem.y + master_data.size + (master_data.area ) ; y++ ){
				//Debug.Log ("x=" + x.ToString () + " y=" + y.ToString ());

				//foreach (DataItem data_item in DataManager.Instance.m_ItemDataList) {
				foreach (CtrlFieldItem field_item in GameMain.ParkRoot.m_fieldItemList) {

					// xyが合ってて、シリアルは別
					if (field_item.m_dataItem.x == x && field_item.m_dataItem.y == y && m_dataItem.item_serial != field_item.m_dataItem.item_serial ) {
						//CtrlFieldItem script = GameMain.ParkRoot.GetFieldItem (data_item.item_serial);
						CtrlFieldItem script = field_item;
						m_areaFieldItem.Add (script);
						script.gameObject.transform.parent = m_goRootPosition.transform;
						script.gameObject.transform.localScale = Vector3.one;
						script.ResetPos ();
					}
				}
			}
		}


		/*
		for( int x = m_dataItem.x - (master_data.area ) ; x < m_dataItem.x + master_data.size + (master_data.area ) ; x++ ){
			for( int y = m_dataItem.y - (master_data.area ) ; y < m_dataItem.y + master_data.size + (master_data.area ) ; y++ ){
				//Debug.Log ("x=" + x.ToString () + " y=" + y.ToString ());
				foreach (DataItem data_item in DataManager.Instance.m_ItemDataList) {

					// xyが合ってて、シリアルは別
					if (data_item.x == x && data_item.y == y && m_dataItem.item_serial != data_item.item_serial ) {
						CtrlFieldItem script = GameMain.ParkRoot.GetFieldItem (data_item.item_serial);
						m_areaFieldItem.Add (script);
						script.gameObject.transform.parent = m_goRootPosition.transform;
						script.ResetPos ();
					}
				}
			}
		}
		*/

		float fScale = 0.5f;
		m_goRootPosition.transform.localScale = new Vector3 (fScale, fScale, fScale);
		m_goRootPosition.transform.localPosition = (-1.0f * Define.CELL_X_DIR.normalized * Define.CELL_X_LENGTH * m_dataItem.x) + (-1.0f * Define.CELL_Y_DIR.normalized * Define.CELL_Y_LENGTH * m_dataItem.y + new Vector3(0.0f, -240.0f,0.0f ));
		m_goRootPosition.transform.localPosition *= fScale;

		return;
	}

	private int m_iSelectingCageSerial;
	public int SelectingCageSerial{
		get{ return m_iSelectingCageSerial; }
	}

	// Update is called once per frame
	void Update () {
		if (InputManager.Instance.m_TouchInfo.TouchON) {
			int iGridX = 0;
			int iGridY = 0;

			int iSelectCageSerial = 0;

			if (m_goRootPosition) {
				if (GameMain.GetGrid (m_goRootPosition, InputManager.Instance.m_TouchInfo.TouchPoint, out iGridX, out iGridY)) {
					//Debug.Log ("x=" + iGridX.ToString () + " y=" + iGridY.ToString ());

					foreach (DataItem data_item in DataManager.Instance.m_ItemDataList) {
						if (GameMain.GridHit (iGridX, iGridY, data_item)) {

							if (data_item.category == (int)Define.Item.Category.CAGE) {
								iSelectCageSerial = data_item.item_serial;
								m_iSelectingCageSerial = iSelectCageSerial;
								break;
							}
						}
					}
					/*
				foreach (DataItem data_item in DataManager.Instance.m_ItemDataList) {
					// xyが合ってて、シリアルは別
					if (data_item.x == iGridX && data_item.y == iGridY && data_item.category == (int)Define.Item.Category.CAGE ) {
						iSelectCageSerial = data_item.item_serial;
						m_iSelectingCageSerial = iSelectCageSerial;
						break;
					}
				}
				*/
					/*
				if (0 < m_iSelectingCageSerial) {
					CtrlFieldItem script = GameMain.ParkRoot.GetFieldItem (m_iSelectingCageSerial);
					script.SetColor (Color.white);
					if (0 < iSelectCageSerial) {
						CtrlFieldItem script = GameMain.ParkRoot.GetFieldItem (iSelectCageSerial);
						script.SetColor (Color.red);
						m_iSelectingCageSerial = iSelectCageSerial;
					}
				}
				*/
				}
			}
		}


		// 勝手にちっさくなるのでここで再度修正。苛ついてるのでずっとスケールかけてます
		// こういうのがあるからUnity嫌いなんだよね
		//m_goBlackBack.transform.localScale = new Vector3 (100.0f, 100.0f, 100.0f);

	}
}
