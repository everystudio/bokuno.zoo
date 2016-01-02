using UnityEngine;
using System.Collections;

abstract public class DebugNumBase : MonoBehaviour {

	void OnEnable(){
		//Debug.LogError ("OnEnable");
		Initialize ();
	}

	[SerializeField]
	public UILabel m_lbNum;

	[SerializeField]
	public UILabel m_lbRate;

	public int m_iRate;

	[SerializeField]
	public ButtonBase m_btnPlus;
	[SerializeField]
	public ButtonBase m_btnMinus;
	[SerializeField]
	public ButtonBase m_btnRate;

	abstract protected void initialize ();
	abstract protected void func_plus ();
	abstract protected void func_minus();

	void Start(){

		m_btnPlus.TriggerClear ();
		m_btnMinus.TriggerClear ();

		m_iRate = 100;
		m_lbRate.text = "百";
	}

	public void Initialize(){
		initialize ();
	}

	// Update is called once per frame
	void Update () {

		if (m_btnPlus.ButtonPushed) {
			SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);
			m_btnPlus.TriggerClear ();
			func_plus ();
		}
		if (m_btnMinus.ButtonPushed) {
			SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);
			m_btnMinus.TriggerClear ();
			func_minus ();
		}

		if (m_btnRate.ButtonPushed) {
			SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);
			m_btnRate.TriggerClear ();
			string strRate = "";

			switch (m_iRate) {
			case 1:
				m_iRate = 10;
				strRate = "拾";
				break;
			case 10:
				m_iRate = 100;
				strRate = "百";
				break;
			case 100:
				m_iRate = 1000;
				strRate = "千";
				break;
			case 1000:
				m_iRate = 10000;
				strRate = "万";
				break;
			case 10000:
			case 100000:
			default:
				m_iRate = 1;
				strRate = "壱";
				break;
			}
			m_lbRate.text = strRate;
		}


	
	}
}
