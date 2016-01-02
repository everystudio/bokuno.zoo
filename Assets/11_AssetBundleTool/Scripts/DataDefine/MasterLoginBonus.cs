using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterLoginBonus : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 login_bonus_id;
		public string	 login_bonus_name;
		public int		 login_bonus_type;
		public int		 priority;
		public int		 last_day_num;
		public string	 start_date;
		public string	 end_date;
		public int		 del_flg;

	}

}

