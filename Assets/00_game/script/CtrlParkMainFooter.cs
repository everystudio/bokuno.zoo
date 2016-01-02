using UnityEngine;
using System.Collections;

public class CtrlParkMainFooter : MonoBehaviourEx {

	public void TriggerClearAll(){
		m_PageButton.ButtonInit ();
		m_PageButton.TriggerClearAll();

		m_CollectButton.TriggerClear ();
	}

	[SerializeField]
	private ButtonManager m_PageButton;

	[SerializeField]
	private ButtonBase m_CollectButton;

	void Update(){
		if (m_PageButton.ButtonPushed) {
			//Debug.Log (m_PageButton.Index);
			m_PageButton.TriggerClearAll ();

			SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);

			GameMain.STATUS status = GameMain.STATUS.NONE;
			switch (m_PageButton.Index) {
			case 0:
				SoundManager.Instance.PlayBGM (SoundName.BGM_SHOP);
				status = GameMain.STATUS.BOOK;
				break;
			case 1:
				status = GameMain.STATUS.WORK;
				SoundManager.Instance.PlayBGM (SoundName.BGM_WORK);
				break;
			case 2:
				SoundManager.Instance.PlayBGM (SoundName.BGM_SHOP);
				status = GameMain.STATUS.ITEM;
				break;
			default:
				break;
			}


			m_PageButton.TriggerClearAll ();
			GameMain.Instance.SetStatus (status);
		}

		/*
		if (m_CollectButton.ButtonPushed) {
			m_CollectButton.TriggerClear ();
			Debug.Log ("collect start");
		}
		*/
	}

}
