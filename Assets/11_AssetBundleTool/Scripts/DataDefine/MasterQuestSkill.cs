using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterQuestSkill : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 qskill_id;
		public int		 qskill_type;
		public int		 qskill_category;
		public int		 qskill_attribute;
		public int		 qskill_attribute_bit;
		public string	 qskill_name;
		public int		 calc_type;
		public int		 param1;
		public int		 param2;
		public string	 description;
		public int		 del_flg;

	}

}

