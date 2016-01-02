using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterRuinSugorokuGrid : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 ruin_id;
		public int		 sugoroku_id;
		public int		 grid;
		public int		 grid_type;
		public int		 value;
		public int		 is_move;
		public int		 next_sugoroku_id;
		public int		 next_grid;
		public int		 del_flg;

	}

}

