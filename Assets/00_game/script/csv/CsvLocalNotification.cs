using UnityEngine;
using System.Collections;

public class CsvLocalNotification : MasterTableBase<CsvLocalNotificationData> {

	private static readonly string FilePath = "csv/local_notification";
	public void Load() { Load(FilePath); }
}
	
public class CsvLocalNotificationData : MasterBase
{
	public int id { get; private set; }
	public int type { get; private set; }
	public int second { get; private set; }
	public string title { get; private set; }
	public string message { get; private set; }
}






