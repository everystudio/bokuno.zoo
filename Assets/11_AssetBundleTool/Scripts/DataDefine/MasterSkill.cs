using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterSkill : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 skill_id;
		public int		 skill_type;
		public int		 skill_category;
		public int		 skill_attribute;
		public string	 skill_name;
		public int		 skill_cost;
		public int		 max_level;
		public int		 param1;
		public int		 param2;
		public string	 description;
		public string	 prefab_name;
		public string	 select_prefab_name;
		public int		 del_flg;

	}

}

