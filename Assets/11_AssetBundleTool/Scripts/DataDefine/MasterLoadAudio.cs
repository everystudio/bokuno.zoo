using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterLoadAudio : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public string	 filename;
		public int		 version;
		public string	 path;
		public int		 audio_type;
		public int		 del_flg;

	}

}

