using UnityEngine;
using System.Collections;

public class CtrlDispHungry : MonoBehaviour {

	[SerializeField]
	private UISprite[] m_sprTripeArr;

	public void Set( float _iMealLevel ){
		for (int i = 0; i < m_sprTripeArr.Length; i++) {
			if ( _iMealLevel < (i + 1) ) {
				m_sprTripeArr [i].spriteName = "icon_manpuku3";
			}
		}
	}

}
