using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_ANDROID
using Prime31;
#endif

public class PurchasesManager : MonoBehaviour {

	protected static PurchasesManager instance = null;
	public static PurchasesManager Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("PurchasesManager");
				if (obj == null) {
					obj = new GameObject("PurchasesManager");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<PurchasesManager> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<PurchasesManager>() as PurchasesManager;
				}
				instance.initialize ();
			}
			return instance;
		}
	}
	public void DummyCall(){
		Debug.Log ("dummy");
	}
	public enum STATUS {
		NONE		= 0,
		BUYING		,
		SUCCESS		,
		FAILD		,
		MAX			,
	}
	private STATUS m_eStatus;
	public STATUS Status{
		get{ return m_eStatus; }
		set{ m_eStatus = value; }
	}
	public bool m_bPurchased = false;
	private static bool IsInitialized = false;

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	public const string TICKET_010 	=  "ticket010";
	//public const string TICKET_010 	=  "comicticket100";
	public const string TICKET_055 	=  "ticket055";
	public const string TICKET_125 	=  "ticket125";
	public const string TICKET_350 	=  "ticket350";
	public const string TICKET_800 	=  "ticket800";

	public bool IsPurchased(){
		return m_bPurchased;
	}

	public static void buyItem(string productId) {

		instance.m_bPurchased = false;
		instance.m_eStatus = STATUS.BUYING;

		#if UNITY_IPHONE
		IOSInAppPurchaseManager.Instance.BuyProduct(productId);
		#elif UNITY_ANDROID
		GoogleIAB.purchaseProduct( productId);
		#endif
	}

	public void initialize(){
		if (!IsInitialized) {
			DontDestroyOnLoad(gameObject);

			m_bPurchased = false;

			#if UNITY_IPHONE
			//You do not have to add products by code if you already did it in seetings guid
			//Windows -> IOS Native -> Edit Settings
			//Billing tab.
			IOSInAppPurchaseManager.Instance.AddProductId(TICKET_010);
			IOSInAppPurchaseManager.Instance.AddProductId(TICKET_055);
			IOSInAppPurchaseManager.Instance.AddProductId(TICKET_125);
			IOSInAppPurchaseManager.Instance.AddProductId(TICKET_350);
			IOSInAppPurchaseManager.Instance.AddProductId(TICKET_800);

			IOSInAppPurchaseManager.OnVerificationComplete += HandleOnVerificationComplete;
			IOSInAppPurchaseManager.OnStoreKitInitComplete += OnStoreKitInitComplete;

			IOSInAppPurchaseManager.OnTransactionComplete += OnTransactionComplete;
			IOSInAppPurchaseManager.OnRestoreComplete += OnRestoreComplete;
			IOSInAppPurchaseManager.instance.LoadStore ();
			#elif UNITY_ANDROID

			GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
			GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
			GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
			GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
			GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
			GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
			GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
			GoogleIAB.enableLogging (true);

			string key = "your public key from the Android developer portal here";
			// jp.app.bokunozoo
			key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsqFXrg2t62dru/VFQYyxd2m1kORBbrAGxDxiSAkh3ybaXJtJWNcej/YAxKx7Orrtfq+pU965U2FnU3K54xddts2UGCI9O6TSU0AoKbwFYj+okfF21firsEqZd4aYtVYQ471flWj3ZEG9u2YpIzjGykUQadsxO4Y/OcRbdUn9289Mc0JAbdepmN9yRnvgBJWKZF/c0mBrM4ISfF5TVip2Tp+BXACqblOb+TQZjOB0OeVPxYpdy5k3eJTcQuwiLmYxgpEBL3tIT7grxVROgk8YYncncaZR7Q/wWlsFgFTNMRaF2bPI8apLiA7eIyKv5zbmhbE7YLBXUvkuoHbAqDQrLQIDAQAB";
			// jp.app.bokuno.zoo
			key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA3xplcFnhYvPYqsxu5D2hP/DwOYHTOUgy59ZUhLaUyEKyK9HZ5hpeDFFe2WqopiR2tM5aD6zRs1WFJqLKjoqoeMi4BwbtAcGOK1I5BDAe9YmDlGN+YEkG6nBPwEm+IZ1C9pLkAi9EoCc28xS/pUlwIPP8/PSMjTpTixO5S0lbKk5tY3VJyt454khCE/XFJMZd6C0j2sBiLwxi7vpZ3i5X0bl75sMr3fvIFdS7WT+m9slwsEZ9qDw/H0Uh01yA5gJn8CkNQ0x04gw+OrtepalTDvE4Lb/nzs6+1QwAi7jbvzPwCD9KDyhfEfLjjj/iOI9nkfjDMXt9d6+n6TCKROfsUwIDAQAB";
			Debug.Log( key );
			//下はテスト用
			//key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArGLKSb92Imt43S40ArCXfTmQ31c+pFQTM0Dza3j/Tn4cqjwccjQ/jej68GgVyGXGC2gT/EtbcVVA+bHugXmyv73lGBgmQlzBL41WYTKolO8Z6pVWTeHBtsT7RcHKukoKiONZ7NiQ9P5t6CCPBB2sXQOp1y3ryVbv01xXlM+hB6HkkKxrT6lIjTbtiVXCHAJvqPexPbqVIfGjBaXH/oHKxEBxYDaa6PTUsU3OP3MTx63ycTEnEMsQlA1W6ZuTFIa5Jd3cVlfQI7uovEzAbIlUfwcnxVOUWASiYe81eQiD1BMl+JeCRhfd5e8D4n0LOA12rHm1F3fC9ZoIEjpNB+BRhwIDAQAB";
			GoogleIAB.init( key );

			#endif

		}
		IsInitialized = true;
	}

	// Use this for initialization
	void Start () {
		initialize ();
		Instance.DummyCall ();
	}

	//public Action<IOSStoreKitResult> OnTransactionComplete = delegate{};
	//public Action<ISN_Result> OnStoreKitInitComplete = delegate{};


	private static void OnStoreKitInitComplete (ISN_Result result) {
		IOSInAppPurchaseManager.OnStoreKitInitComplete -= OnStoreKitInitComplete;

		if(result.IsSucceeded) {
			Debug.Log("Inited successfully, Available products count: " + IOSInAppPurchaseManager.Instance.Products.Count.ToString());
			foreach(IOSProductTemplate tpl in IOSInAppPurchaseManager.Instance.Products) {
				Debug.Log (string.Format ("id:{0} title:{1} description:{2} price:{3} localizedPrice:{4} currencySymbol:{5} currencyCode:{6}" ,
					tpl.Id,
					tpl.Title,
					tpl.Description,
					tpl.Price,
					tpl.LocalizedPrice,
					tpl.CurrencySymbol,
					tpl.CurrencyCode));
				Debug.Log("-------------");
			}
		} else {
			Debug.LogError("StoreKit Init Failed.  Error code: " + result.Error.Code + "\n" + "Error description:" + result.Error.Description);
		}
	}
	/*
	private static void OnStoreKitInitComplete(ISN_Result result) {
		if(result.IsSucceeded) {
			int avaliableProductsCount = 0;
			foreach(IOSProductTemplate tpl in IOSInAppPurchaseManager.instance.Products) {
				if(tpl.IsAvaliable) {
					avaliableProductsCount++;
				}
			}
			IOSNativePopUpManager.showMessage("StoreKit Init Succeeded", "Available products count: " + avaliableProductsCount);
			Debug.Log("StoreKit Init Succeeded Available products count: " + avaliableProductsCount);
		} else {
			IOSNativePopUpManager.showMessage("StoreKit Init Failed",  "Error code: " + result.Error.Code + "\n" + "Error description:" + result.Error.Description);
		}
	}
	*/



	private static void UnlockProducts(string productIdentifier) {
		Debug.Log (string.Format ("UnlockProducts:{0}", productIdentifier));
		switch(productIdentifier) {
		/*
		case SMALL_PACK:
			//code for adding small game money amount here
			break;
		case NC_PACK:
			//code for unlocking cool item here
			break;
		*/
		}
	}

	private static void OnTransactionComplete (IOSStoreKitResult result) {

		Debug.Log("OnTransactionComplete: " + result.ProductIdentifier);
		Debug.Log("OnTransactionComplete: state: " + result.State);
		instance.m_bPurchased = true;
		switch(result.State) {
		case InAppPurchaseState.Purchased:
		case InAppPurchaseState.Restored:
			//Our product been succsesly purchased or restored
			//So we need to provide content to our user depends on productIdentifier
			UnlockProducts(result.ProductIdentifier);
			instance.m_eStatus = STATUS.SUCCESS;
			break;
		case InAppPurchaseState.Deferred:
			//iOS 8 introduces Ask to Buy, which lets parents approve any purchases initiated by children
			//You should update your UI to reflect this deferred state, and expect another Transaction Complete  to be called again with a new transaction state 
			//reflecting the parent’s decision or after the transaction times out. Avoid blocking your UI or gameplay while waiting for the transaction to be updated.
			instance.m_eStatus = STATUS.FAILD;
			break;
		case InAppPurchaseState.Failed:
			//Our purchase flow is failed.
			//We can unlock intrefase and repor user that the purchase is failed. 
			Debug.Log("Transaction failed with error, code: " + result.Error.Code);
			Debug.Log("Transaction failed with error, description: " + result.Error.Description);
			instance.m_eStatus = STATUS.FAILD;
			break;
		}

		if(result.State == InAppPurchaseState.Failed) {
		IOSNativePopUpManager.showMessage("Transaction Failed", "Error code: " + result.Error.Code + "\n" + "Error description:" + result.Error.Description);
		} else {
		IOSNativePopUpManager.showMessage("Store Kit Response", "product " + result.ProductIdentifier + " state: " + result.State.ToString());
		}

	}

	static void HandleOnVerificationComplete (IOSStoreKitVerificationResponse response) {
		IOSNativePopUpManager.showMessage("Verification", "Transaction verification status: " + response.status.ToString());

		Debug.Log("ORIGINAL JSON: " + response.originalJSON);
	}

	private static void OnRestoreComplete (IOSStoreKitRestoreResult res) {
		if(res.IsSucceeded) {
			IOSNativePopUpManager.showMessage("Success", "Restore Compleated");
		} else {
			IOSNativePopUpManager.showMessage("Error: " + res.Error.Code, res.Error.Description);
		}
	}	

	#region Android

	#if UNITY_ANDROID

	void billingSupportedEvent()
	{
		Debug.Log( "billingSupportedEvent" );
	}


	void billingNotSupportedEvent( string error )
	{
		Debug.Log( "billingNotSupportedEvent: " + error );
	}


	void queryInventorySucceededEvent( List<GooglePurchase> purchases, List<GoogleSkuInfo> skus )
	{
		Debug.Log( string.Format( "queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count ) );
		Prime31.Utils.logObject( purchases );
		Prime31.Utils.logObject( skus );
	}


	void queryInventoryFailedEvent( string error )
	{
		Debug.Log( "queryInventoryFailedEvent: " + error );
	}


	void purchaseCompleteAwaitingVerificationEvent( string purchaseData, string signature )
	{
		Debug.Log( "purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature );
	}


	void purchaseSucceededEvent( GooglePurchase purchase )
	{
		Debug.Log( "purchaseSucceededEvent: " + purchase );

		GoogleIAB.consumeProduct(purchase.productId);

	}


	void purchaseFailedEvent( string error, int response )
	{
		Debug.Log( "purchaseFailedEvent: " + error + ", response: " + response );

		instance.m_bPurchased = true;
		instance.m_eStatus = STATUS.FAILD;

	}


	void consumePurchaseSucceededEvent( GooglePurchase purchase )
	{
		Debug.Log( "consumePurchaseSucceededEvent: " + purchase );
		instance.m_eStatus = STATUS.SUCCESS;
		m_bPurchased = true;
	}


	void consumePurchaseFailedEvent( string error )
	{
		Debug.Log( "consumePurchaseFailedEvent: " + error );
		instance.m_bPurchased = true;
		instance.m_eStatus = STATUS.FAILD;
	}
	#endif

	#endregion


	// Update is called once per frame
	void Update () {



	
	}
}
