using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterQuiz : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 quiz_id;
		public int		 quiz_category;
		public int		 quiz_type;
		public int		 quiz_difficulty;
		public int		 answer_id;
		public string	 question;
		public string	 answer_1;
		public string	 answer_2;
		public string	 answer_3;
		public string	 answer_4;
		public int		 del_flg;

	}

}

