using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class InAppPurchasing : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    private IAppleExtensions m_AppleExtensions;
    private IGooglePlayStoreExtensions m_GoogleExtensions;
    private static string buy6Lives = "buy6lives";
    private const string buyToken100 = "100tokens";
    private const string buyToken250 = "tokens250";
    private const string buyToken500 = "500tokens";
    private const string buyToken1000 = "1000tokens";
    private const string buyAnUnlimitedNumberOfLivesInOneDay = "buyanunlimitednumberoflivesinoneday";
    private const string withoutAdsPerWeek = "withoutadsperweek";

    private void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }
    private void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(buy6Lives, ProductType.Consumable);
        builder.AddProduct(buyToken100, ProductType.Consumable);
        builder.AddProduct(buyToken250, ProductType.Consumable);
        builder.AddProduct(buyToken500, ProductType.Consumable);
        builder.AddProduct(buyToken1000, ProductType.Consumable);
        builder.AddProduct(buyAnUnlimitedNumberOfLivesInOneDay, ProductType.Consumable);
        builder.AddProduct(withoutAdsPerWeek, ProductType.Subscription);
        UnityPurchasing.Initialize(this, builder);
    }
    public void Buy6Lives()
    {
        BuyProductID(buy6Lives);
    }
    public void BuyToken100()
    {
        BuyProductID(buyToken100);
    }
    public void BuyToken250()
    {
        BuyProductID(buyToken250);
    }
    public void BuyToken500()
    {
        BuyProductID(buyToken500);
    }
    public void BuyToken1000()
    {
        BuyProductID(buyToken1000);
    }
    public void BuyUnlimitiedLives()
    {
        BuyProductID(buyAnUnlimitedNumberOfLivesInOneDay);
    }
    public void BuyWithoutAds()
    {
        BuyProductID(withoutAdsPerWeek);
    }
    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            //Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            //Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();

            apple.RestoreTransactions((result) =>
            {
                //Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            //Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }
    private void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                //Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                //Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            //Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }
    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }
    public void OnPurchaseDeferred(Product product)
    {
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
        m_GoogleExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();

        m_GoogleExtensions?.SetDeferredPurchaseListener(OnPurchaseDeferred);

        Dictionary<string, string> dict = m_AppleExtensions.GetIntroductoryPriceDictionary();
        foreach (Product item in controller.products.all)
        {

            if (item.receipt != null)
            {
                string intro_json = (dict == null || !dict.ContainsKey(item.definition.storeSpecificId)) ? null : dict[item.definition.storeSpecificId];

                if (item.definition.type == ProductType.Subscription)
                {
                    SubscriptionManager p = new SubscriptionManager(item, intro_json);
                    SubscriptionInfo info = p.getSubscriptionInfo();
                    if (info.isSubscribed().HasFlag(Result.True))
                    {
                        //if (info.isAutoRenewing().HasFlag(Result.True))
                        //{
                            //Debug.Log("next time: " + info.getExpireDate());
                        //}
                    }
                }
            }
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        //Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        //Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        if (string.Equals(purchaseEvent.purchasedProduct.definition.id, buyToken100, StringComparison.Ordinal))
        {

            SavedData.instance.tokensCount += 100;
            EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, 0);
            SavedData.instance.Saved();
        }
        else if (string.Equals(purchaseEvent.purchasedProduct.definition.id, buyToken250, StringComparison.Ordinal))
        {
            SavedData.instance.tokensCount += 250;
            EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, 0);
            SavedData.instance.Saved();
        }
        else if (string.Equals(purchaseEvent.purchasedProduct.definition.id, buyToken500, StringComparison.Ordinal))
        {
            SavedData.instance.tokensCount += 500;
            EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, 0);
            SavedData.instance.Saved();
        }
        else if (string.Equals(purchaseEvent.purchasedProduct.definition.id, buyToken1000, StringComparison.Ordinal))
        {
            SavedData.instance.tokensCount += 1000;
            EventManager.instance.CheckCountTokens(SavedData.instance.tokensCount, 0);
            SavedData.instance.Saved();
        }
        else if (string.Equals(purchaseEvent.purchasedProduct.definition.id, withoutAdsPerWeek, StringComparison.Ordinal))
        {
            SavedData.instance.removeAd = true;
            SavedData.instance.lastBuyWithoutAdsPerWeek = (ulong)DateTime.Now.Ticks;
            ShopManager.instance.CheckStatusWithoutAds();
            SavedData.instance.Saved();
        }
        else if (string.Equals(purchaseEvent.purchasedProduct.definition.id, buyAnUnlimitedNumberOfLivesInOneDay, StringComparison.Ordinal))
        {
            SavedData.instance.lastBuyUnlimitedLives = (ulong)DateTime.Now.Ticks;
            SavedData.instance.unlimitedLives = true;
            EventManager.instance.CheckCountLives(' ', SavedData.instance.livesCount);
            SavedData.instance.Saved();
        }
        else if (string.Equals(purchaseEvent.purchasedProduct.definition.id, buy6Lives, StringComparison.Ordinal))
        {
            SavedData.instance.livesCount += 6;
            SavedData.instance.Saved();
        }
        return PurchaseProcessingResult.Complete;
    }
}
