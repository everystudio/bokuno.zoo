using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UI2DSprite))]
public class Chikachika : MonoBehaviourEx {

	public UI2DSprite m_ui2dSprite;

	public float INTERVAL = 0.5f;
	float m_fTimer;
	bool m_bFlag;
	void OnEnable(){
		m_fTimer = 0.0f;
		m_ui2dSprite = GetComponent<UI2DSprite> ();
	}
	void OnDisable(){
		m_ui2dSprite = GetComponent<UI2DSprite> ();
		m_ui2dSprite.enabled = true;
	}

	// Update is called once per frame
	void Update () {

		m_fTimer += Time.deltaTime;
		if (INTERVAL < m_fTimer) {
			m_fTimer -= INTERVAL;
			m_ui2dSprite.enabled = m_bFlag;
			m_bFlag = !m_bFlag;
		}

	}
}
