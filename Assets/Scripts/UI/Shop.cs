using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    #region Serialized

    [SerializeField]
    private Transform itemsHolder;

    #endregion

    #region Private

    private string boughtSkins;
    private int selectedIndex;
    private ShopItem[] shopItems = new ShopItem[6];
    private List<SkinItem> allSkins;

    private static Shop _instance;

    #endregion

    #region Methods

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {

            _instance = this;
            selectedIndex = PlayerStorage.SkinSelected;
            boughtSkins = PlayerStorage.BoughtSkins;
            allSkins = DataHolder.Instance.AllSkins;
            for (int i = 0; i < allSkins.Count; i++)
            {
                int ind = i;
                shopItems[i] = itemsHolder.GetChild(i).GetComponent<ShopItem>();
                shopItems[i].Initialize(allSkins[i], () => Select(ind));
                if (boughtSkins.IndexOf(i.ToString()) != -1 || i == 0)
                {
                    shopItems[i].HidePrice();
                }
                if (i == selectedIndex)
                    shopItems[i].Select();
                else
                    shopItems[i].UnSelect();
            }
        }
    }

    public void Buy()
    {
        if (selectedIndex == 0 || boughtSkins.IndexOf(allSkins[selectedIndex].skinNumber.ToString()) != -1 )
            return;
        int availableCoins = PlayerStorage.CoinsCollected;
        if(availableCoins >= allSkins[selectedIndex].price)
        {
            PlayerStorage.CoinsCollected -= allSkins[selectedIndex].price;
            boughtSkins += allSkins[selectedIndex].skinNumber.ToString();
            shopItems[selectedIndex].HidePrice();
            PlayerStorage.BoughtSkins = boughtSkins;
            PlayerStorage.SkinSelected = allSkins[selectedIndex].skinNumber;
            EventsPool.UpdateUIEvent.Invoke();
            EventsPool.UpdateSkinEvent.Invoke(allSkins[selectedIndex]);
        }
    }

    public void Select(int index)
    {
        if (boughtSkins.IndexOf(index.ToString()) != -1 || index == 0)
        {
            PlayerStorage.SkinSelected = index;
            EventsPool.UpdateSkinEvent.Invoke(allSkins[index]);
        }
        for (int i = 0;i < shopItems.Length; i++) 
        {
            if(i == index)
            {
                shopItems[i]?.Select();
            }
            else
            {
                shopItems[i]?.UnSelect();
            }
        }
        selectedIndex = index;
    }

    #endregion
}
