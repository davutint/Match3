#if GLEY_IAP_IOS || GLEY_IAP_GOOGLEPLAY || GLEY_IAP_AMAZON || GLEY_IAP_MACOS || GLEY_IAP_WINDOWS
#define GleyIAPEnabled
#endif
#if GleyIAPEnabled
namespace Gley.EasyIAP
{
	public enum ShopProductNames
	{
		Pinwheel,
		Cigar,
		Whirlypool,
		RemoveAds,
	}
}
#endif