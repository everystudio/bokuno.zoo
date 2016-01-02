using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterLeaderSkill : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 lskill_id;
		public int		 lskill_tyoe;
		public int		 lskill_category;
		public int		 lskill_attribute;
		public int		 lskill_attribute_bit;
		public string	 lskill_name;
		public int		 calc_type;
		public int		 param1;
		public int		 param2;
		public string	 description;
		public int		 del_flg;

	}

}

