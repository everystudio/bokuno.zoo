using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterGacha : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 gacha_id;
		public string	 gacha_name;
		public int		 gacha_type;
		public string	 start_date;
		public string	 end_date;
		public int		 ticket_type;
		public int		 ticket_num;
		public int		 friend_point;
		public int		 need_star;
		public int		 need_diamond;
		public int		 card_type;
		public int		 card_category;
		public int		 prob_index;
		public int		 del_flg;

	}

}

