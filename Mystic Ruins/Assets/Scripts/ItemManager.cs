using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class ItemManager : MonoBehaviour
{
    public GameObject[] itemImages = new GameObject[4];

    public Sprite[] itemSprites = new Sprite[4];
    //(0)바퀴 / (1)석탄 / (2)사다리 / (3)판자

    public void SetItems()
    {
        int i = 0;
        while (i < DataManager.Instance.gameData.items.Length)
        {
            if (DataManager.Instance.gameData.items[i] != 0)
            {                
                itemImages[i].GetComponent<Image>().sprite = itemSprites[DataManager.Instance.gameData.items[i] - 1];
                itemImages[i].GetComponent<Image>().enabled = true;
            }
            i++;
        }
    }

    public bool AddItem(int itemValue)
    {         
        int[] items = DataManager.Instance.gameData.items;
        bool isSaved = false;
        int i = 0;
        while(i < items.Length) 
        {
            if (items[i] == 0)
            {
                DataManager.Instance.gameData.items[i] = itemValue;
                itemImages[i].GetComponent<Image>().sprite = itemSprites[itemValue - 1];
                itemImages[i].GetComponent<Image>().enabled = true;
                isSaved = true;
                break;
            }
            i++;
        }

        return isSaved;
    }

    public bool UseItem(int pointerNum, int wantedItemNum)
    {
        if (itemImages[pointerNum].GetComponent<Image>().sprite == itemSprites[wantedItemNum])
        {
            DataManager.Instance.gameData.items[pointerNum] = 0;
            itemImages[pointerNum].GetComponent<Image>().sprite = null;
            itemImages[pointerNum].GetComponent<Image>().enabled = false;

            return true;
        }

        return false;
    }

    //아이템 사용시의 데이터 수정 기능 및 UI 수정 기능 구현 필요
}
