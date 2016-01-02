using UnityEngine;
using System.Collections;

public class Fuwafuwa : MonoBehaviourEx {

	public float m_fTimer;
	public float m_fSaveY;
	public float m_fDeltaY;

	public float m_fSpeed = 2.0f;

	public Vector3 m_v3Save;

	private float SWING = 10.0f;

	void OnEnable(){
		m_v3Save = myTransform.localPosition;
		m_fSaveY = myTransform.localPosition.y;
		m_fTimer = 0.0f;
	}


	// Update is called once per frame
	void Update () {

		m_fTimer += Time.deltaTime * m_fSpeed;
		m_fDeltaY = SWING * Mathf.Sin(0.5f * Mathf.PI * m_fTimer);

		myTransform.localPosition = new Vector3 (m_v3Save.x, m_v3Save.y + m_fDeltaY, m_v3Save.z);

	
	}
}
