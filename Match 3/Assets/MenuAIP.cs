using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gley.EasyIAP;
using TMPro;
public class MenuAIP : MonoBehaviour
{
	[SerializeField]Settings settings;
	private string isAdsActive="isAdsActive";//1 reklamlar aktif demek, 0 ise reklamlar aktif değil demek IAP'ye göre

	[SerializeField]GameObject removeAdsPanel;
	[SerializeField]GameObject removeAdsButton;
	[SerializeField]TextMeshProUGUI removeAdsPriceText;
	private void Start()
	{
		if (PlayerPrefs.GetInt(isAdsActive,1)==0)
		{
			removeAdsButton.SetActive(false);
		}
		Gley.EasyIAP.API.Initialize(InitializationComplete);
		Debug.Log("Menüdeki reklam değeri"+PlayerPrefs.GetInt("isAdsActive"));
	}
	public void RestorePurchases()
	{
		Gley.EasyIAP.API.RestorePurchases(ProductRestored);
	}
	private void InitializationComplete(IAPOperationStatus status, string message, List<StoreProduct> shopProducts)
	{
		if (status == IAPOperationStatus.Success)
		{
			//IAP was successfully initialized
			//loop through all products
			for (int i = 0; i < shopProducts.Count; i++)
			{
				if (shopProducts[i].productName == "RemoveAds")
				{
					//if the active property is true, the product is bought
					if (shopProducts[i].active)
					{
						PlayerPrefs.SetInt("isAdsActive",0);
						removeAdsButton.SetActive(false);
					}
				}
			}
		}
		else
		{
			Debug.Log("Error occurred: " + message);
		}
		removeAdsPriceText.text = $" {Gley.EasyIAP.API.GetLocalizedPriceString(ShopProductNames.RemoveAds)}";

	}
	
	public void RemoveAdsPanel()
	{
		
		bool isActive = removeAdsPanel.activeSelf;
	
	// Eğer aktif ise pasif yap, pasif ise aktif yap
		removeAdsPanel.SetActive(!isActive);
	}
	
	public void RemoveAdsBuy()
	{
		
		Gley.EasyIAP.API.BuyProduct(ShopProductNames.RemoveAds, ProductBought);
	}
	private void ProductBought(IAPOperationStatus status, string message, StoreProduct product)
	{
		if (status == IAPOperationStatus.Success)
		{
			//since all consumable products reward the same coin, a simple type check is enough 
			
			if (product.productName == "RemoveAds")
			{
				PlayerPrefs.SetInt("isAdsActive",0);
				RemoveAdsPanel();
				removeAdsButton.SetActive(false);
				Debug.Log("Satın aldıktan sonra reklam değeri"+PlayerPrefs.GetInt("isAdsActive"));
				//disable ads here
			}
		}
	}

	private void ProductRestored(IAPOperationStatus status, string message, StoreProduct product)
	{
		if (status == IAPOperationStatus.Success)
		{
			if (product.productName == "RemoveAds")
			{
				PlayerPrefs.SetInt("isAdsActive",0);
				removeAdsButton.SetActive(false);
				//disable ads here
			}
		}
		else
		{
			Debug.Log("Error occurred: " + message);
		}
	}
}
