using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterLoadSprite : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public string	 filename;
		public int		 version;
		public int		 pre_load;
		public string	 path;
		public int		 del_flg;

	}

}

