using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



using Prime31;
public class GoogleIABEventListener : MonoBehaviour
	{
#if UNITY_ANDROID

		void Start(){
			DontDestroyOnLoad(gameObject);
		}

		void OnEnable()
		{
			Debug.Log ("GoogleIABEventListener OnEnable");

			// Listen to all events for illustration purposes
			GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
			GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
			GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
			GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
			GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
			GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
			GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
		}
	
	
		void OnDisable()
		{
			Debug.Log ("GoogleIABEventListener OnDisable");
			// Remove all event handlers
			GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
			GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
			GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent -= purchaseCompleteAwaitingVerificationEvent;
			GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
			GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
			GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
			GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
		}
	
	
	
		void billingSupportedEvent()
		{
			Debug.LogError( "billingSupportedEvent" );
		}
	
	
		void billingNotSupportedEvent( string error )
		{
			Debug.LogError( "billingNotSupportedEvent: " + error );
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
			Debug.LogError( "purchaseSucceededEvent: " + purchase );
			GoogleIAB.consumeProduct (purchase.productId);
		}
	
	
		void purchaseFailedEvent( string error, int response )
		{
			Debug.LogError( "purchaseFailedEvent: " + error + ", response: " + response );
		}
	
	
		void consumePurchaseSucceededEvent( GooglePurchase purchase )
		{
			Debug.Log( "consumePurchaseSucceededEvent: " + purchase );
		}
	
	
		void consumePurchaseFailedEvent( string error )
		{
			Debug.Log( "consumePurchaseFailedEvent: " + error );
		}
	
	
#endif
	}

	
	
