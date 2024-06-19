using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gley.EasyIAP;
using TMPro;
public class MatchIAPManager : MonoBehaviour
{
	string whirlypool="Whirlypool";
	string pinwheel="Pinwheel";
	string cigar="Cigar";
	[Header("AIP BUTTONS")]
	[SerializeField]GameObject whirlyAipButton;
	[SerializeField]GameObject pinwheelAipButton;
	[SerializeField]GameObject cigarAipButton;
	[Header("START BUTTONS")]
	[SerializeField]GameObject whirlyStartButton;
	[SerializeField]GameObject pinwheelStartButton;
	[SerializeField]GameObject cigarStartButton;
	
	private string isAdsActive="isAdsActive";//1 reklamlar aktif demek, 0 ise reklamlar aktif değil demek IAP'ye göre
	void Start()
	{
		
		
		Gley.EasyIAP.API.Initialize(InitializationComplete);
		
	}
	 public void PinwheelBuy()
	{
		
		Gley.EasyIAP.API.BuyProduct(ShopProductNames.Pinwheel, ProductBought);
	}
	 public void WhirlypoolBuy()
	{
		
		Gley.EasyIAP.API.BuyProduct(ShopProductNames.Whirlypool, ProductBought);
	}
	 public void CigarBuy()
	{
		
		Gley.EasyIAP.API.BuyProduct(ShopProductNames.Cigar, ProductBought);
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
				
				PlayerPrefs.SetInt(isAdsActive,0);
			}
			if (product.productName == cigar)
			{
				cigarAipButton.SetActive(false);
				cigarStartButton.SetActive(true);
			}
			if (product.productName == whirlypool)
			{
				whirlyAipButton.SetActive(false);
				whirlyStartButton.SetActive(true);
			}
			if (product.productName == pinwheel)
			{
				pinwheelAipButton.SetActive(false);
				pinwheelStartButton.SetActive(true);
			}
			
		}
	}
	public void RestorePurchases()
	{
		
		Gley.EasyIAP.API.RestorePurchases(ProductRestored);
	}

	private void ProductRestored(IAPOperationStatus status, string message, StoreProduct product)
	{
		if (status == IAPOperationStatus.Success)
		{
			if (product.productName == "RemoveAds")
			{
				PlayerPrefs.SetInt(isAdsActive,0);
			}
			if (product.productName == cigar)
			{
				cigarAipButton.SetActive(false);
				cigarStartButton.SetActive(true);
			}
			if (product.productName == whirlypool)
			{
				whirlyAipButton.SetActive(false);
				whirlyStartButton.SetActive(true);
			}
			if (product.productName == pinwheel)
			{
				pinwheelAipButton.SetActive(false);
				pinwheelStartButton.SetActive(true);
			}
		}
		else
		{
			Debug.Log("Error occurred: " + message);
		}
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
						
						PlayerPrefs.SetInt(isAdsActive,0);
					}
				}
				if (shopProducts[i].productName == cigar)
				{
					//if the active property is true, the product is bought
					if (shopProducts[i].active)
					{
						//cigar aç
						cigarAipButton.SetActive(false);
						cigarStartButton.SetActive(true);
					}
				}
				if (shopProducts[i].productName == whirlypool)
				{
					//if the active property is true, the product is bought
					if (shopProducts[i].active)
					{
						//whirly ac
						whirlyAipButton.SetActive(false);
						whirlyStartButton.SetActive(true);
					}
				}
				if (shopProducts[i].productName == pinwheel)
				{
					//if the active property is true, the product is bought
					if (shopProducts[i].active)
					{
						//pinwheel aç
						pinwheelAipButton.SetActive(false);
						pinwheelStartButton.SetActive(true);
					}
				}
				Debug.Log("Menüdeki reklam değeri"+PlayerPrefs.GetInt("isAdsActive"));
			}
		}
		else
		{
			Debug.Log("Error occurred: " + message);
		}
		whirlyAipButton.GetComponentInChildren<TextMeshProUGUI>().text=$"Unlock For {Gley.EasyIAP.API.GetLocalizedPriceString(ShopProductNames.Whirlypool)}";
		cigarAipButton.GetComponentInChildren<TextMeshProUGUI>().text=$"Unlock For {Gley.EasyIAP.API.GetLocalizedPriceString(ShopProductNames.Cigar)}";
		pinwheelAipButton.GetComponentInChildren<TextMeshProUGUI>().text=$"Unlock For {Gley.EasyIAP.API.GetLocalizedPriceString(ShopProductNames.Pinwheel)}";
	}
	
	
	
}
	

