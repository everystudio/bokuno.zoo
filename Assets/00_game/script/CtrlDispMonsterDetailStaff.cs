using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlDispMonsterDetailStaff : MonoBehaviour {

	public void Initialize( List<DataStaff> _staff_list ){

		m_staffClean.Initialize ("cleanstaff_no");
		m_staffMeal.Initialize ("eatstaff_no");

		foreach (DataStaff staff in _staff_list) {
			CsvStaffData csvData = DataManager.GetStaff (staff.staff_id);
			if (csvData.effect_param == 1 || csvData.effect_param == 3) {
				m_staffClean.Initialize ( string.Format("staff_icon{0}" , staff.staff_id));
			}
			if (csvData.effect_param == 2 || csvData.effect_param == 3) {
				m_staffMeal.Initialize (string.Format("staff_icon{0}" , staff.staff_id));
			}
		}
		return;
	}

	#region SerializeField
	[SerializeField]
	private CtrlDispMonsterDetailStaffSub m_staffClean;
	[SerializeField]
	private CtrlDispMonsterDetailStaffSub m_staffMeal;
	#endregion

	// Update is called once per frame
	void Update () {
	
	}
}
