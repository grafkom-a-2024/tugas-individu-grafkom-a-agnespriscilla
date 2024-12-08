using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooking_ProcessValue : MonoBehaviour
{
    
    public GameManagement gMClass;

    public bool selectReceipt;// sudah di selek atau belum

    [SerializeField]
    bool disableAllDebug;

    [SerializeField]
    Image selectReceiptMenu;

    [SerializeField]
    Sprite receiptSelectedImage;// terselect

    
    public Sprite selectReceiptMenuImageDefault; // backup image default

    [SerializeField]
    List<bool> selectIndexLevelCookActive;// pilih di level masakan mana saja dipakai sesuai menu urut 


    
    // Start is called before the first frame update
    void Start()
    {
        selectReceiptMenu = GetComponent<Image>();
        selectReceiptMenuImageDefault = selectReceiptMenu.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectedReceive()
    {
        if(selectReceipt)// jika sudah pernah di selek maka un select
        {
            //cek apaka aktif
            // berfungsi jika sudah saat nya menggunakan bahan ini sesuai level masakan
            if (selectIndexLevelCookActive[gMClass.MenuNoUrut] == true)
            {
                gMClass.CookCountMinus();
                
            }
            else
            {
                gMClass.CookWrongMethodMinus();
                if (!disableAllDebug) Debug.Log("Unselect ! tapi receipt ini tidak dipakai di level masakan ini");
            }
            //receipt unselect
            selectReceipt = false;

            //update UI
            selectReceiptMenu.sprite = selectReceiptMenuImageDefault;
        }
        else // jika belum di select maka selected !!!
        {
            //cek apaka aktif
            // berfungsi jika sudah saat nya menggunakan bahan ini sesuai level masakan
            if (selectIndexLevelCookActive[gMClass.MenuNoUrut] == true)
            {
                gMClass.CookCountPlus();
                
            }
            else
            {
                gMClass.CookWrongMethodPlus();
                if (!disableAllDebug) Debug.Log("selected tapi receipt ini tidak dipakai di level masakan ini");
            }
            //receipt selected
            selectReceipt = true;

            //UpdateUI
            selectReceiptMenu.sprite = receiptSelectedImage;
        }
    }
}
