using UnityEngine;
using System.Collections;

public class ParkMainController : MonoBehaviourEx {
	protected ParkMain m_parkMain;

	virtual protected void initialize(){
		return;
	}

	public void Initialize( ParkMain _parkMain ){
		m_parkMain = _parkMain;
		enabled = true;
		initialize ();
	}

	virtual public void Clear(){
		Debug.Log ("if you need clear action");
	}

	virtual public bool IsEnd(){
		return false;
	}

}
