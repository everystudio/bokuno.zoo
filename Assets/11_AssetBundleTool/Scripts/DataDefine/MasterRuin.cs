using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterRuin : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 ruin_id;
		public string	 ruin_name;
		public int		 ruin_type;
		public int		 difficulty;
		public int		 ruin_instance_num;
		public int		 ruin_instance_add_num;
		public int		 station_max;
		public int		 jackpot_max;
		public int		 jackpot_min;
		public int		 pool_rate;
		public int		 max_combi_play_num;
		public int		 del_flg;

	}

}

