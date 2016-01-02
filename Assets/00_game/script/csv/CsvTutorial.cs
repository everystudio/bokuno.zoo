using UnityEngine;
using System.Collections;

public class CsvTutorial : MasterTableBase<CsvTutorialData> {

	private static readonly string FilePath = "csv/tutorial";
	public void Load() { Load(FilePath); }
}

[System.Serializable]
public class CsvTutorialData : MasterBase
{
	public int tutorial_parent_id { get{ return m_tutorial_parent_id;} private set{m_tutorial_parent_id = value; } }
	public int tutorial_child_id { get{return m_tutorial_child_id; } private set{ m_tutorial_child_id=value; } }
	public string command { get{return m_command; } private set{ m_command=value; } }
	public string string_param { get{return m_string_param; } private set{ m_string_param=value; } }
	public int param1 { get{return m_param1; } private set{ m_param1=value; } }
	public int param2 { get{return m_param2; } private set{ m_param2=value; } }
	public int param3 { get{return m_param3; } private set{ m_param3=value; } }
	public int param4 { get{return m_param4; } private set{ m_param4=value; } }
	public int param5 { get{return m_param5; } private set{ m_param5=value; } }
	public int next_tutorial_child_id { get{return m_next_tutorial_child_id; } private set{ m_next_tutorial_child_id=value; } }
	public string comm { get{return m_comm; } private set{ m_comm=value; } }

	public int m_tutorial_parent_id;
	public int m_tutorial_child_id;
	public string m_command;
	public string m_string_param;
	public int m_param1;
	public int m_param2;
	public int m_param3;
	public int m_param4;
	public int m_param5;
	public int m_next_tutorial_child_id;
	public string m_comm;




}

