using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManagement : MonoBehaviour
{
    [SerializeField]
    bool disableAllDebug;

    [SerializeField]
    float cookingTimerDefault = 15;

    float cookTime;

    [SerializeField]
    int cooking_CountReceipt; // jumlah receipt yang benar oleh pengguna nantinya

    int cooking_CountWrong;

    [SerializeField]
    bool cookRunning; // status memasak sedang berjalan

    bool cookingResultSuccess;

    public int MenuNoUrut;

    int scoreInFinish;

    [SerializeField]
    Text scoreTextInFinish;

    [SerializeField]
    PointManagement pointScoreManagement;

    [SerializeField]
    Text realtimeScoreText;

    [SerializeField]
    Image judulBar;

    [SerializeField]
    Image ThumbnailBar;

    [SerializeField]
    Text timeCookTextUI;

    [SerializeField]
    List<GameObject> listObject;

    [SerializeField]
    List<GameObject> startObject;

    // jumlah receip yang harus terpenuhi
    [SerializeField]
    List<int> receipt_CookCount;//index jadi urutan makanan index 0 klepon dst

    [SerializeField]
    List<AudioSource> audioGameList;

    [SerializeField]
    List<Sprite> imageForTitle;

    [SerializeField] 
    List<Sprite> ThumbnailForTitle;

    [SerializeField]
    List<Cooking_ProcessValue> bahanReceiptReset;

    // Start is called before the first frame update
    void Start()
    {
        realtimeScoreText.text = pointScoreManagement.scoreGameplay.ToString();

        cookTime = cookingTimerDefault;

        listObject[0].SetActive(true); // button mulai memasak
        listObject[1].SetActive(false); //Ui GagaL
        listObject[2].SetActive(false); //UI berhasil
        listObject[3].SetActive(false); // Buton Okay UI Notif 
        listObject[4].SetActive(false); // UI Group notif

        // for start UI group berhasil
        startObject[0].SetActive(false);
        startObject[1].SetActive(false);
        startObject[2].SetActive(false);

        //default start Menu no urut 1
        UpdateTitleCook();
    }

    // Update is called once per frame
    void Update()
    {
        if(cookTime > 0 && cookRunning)
        {
            cookTime -=1 * Time.deltaTime;
            timeCookTextUI.text = cookTime.ToString();
            //if(!disableAllDebug) Debug.Log(cookTime.ToString());
        }
        else
        {

            if (cookRunning)
            {
                //fail
                CookFailedNotif();
                cookRunning = false;
            }
            
        }
        
    }
    void CookFailedNotif() // time habis belum memulai memasak
    {
        //audio
        audioGameList[1].Play();

        //hidden button mulai memasak
        listObject[0].gameObject.SetActive(false);


        //hidden berhasil
        HiddenOrShowNotifBerhasil(false);

        //show notif gagal
        HiddenOrShowNotifGagal(true);
    }

    public void CheckCookResult()// button mulai memasak
    {
        //Kondisi berhasil :
        //-> waktu > 0
        //cek kondisi benar
        //calculasi persentase
        if(cookTime > 0 && cooking_CountWrong == 0)//cek time first
        {
            //stop running cooking
            cookRunning = false;

            //cek presentase sekarang
            float cook_presentase = ((float)cooking_CountReceipt / receipt_CookCount[MenuNoUrut]) * 100; // hitung presentase sesuai menu urutnya
            if (!disableAllDebug) Debug.Log(cook_presentase.ToString() + "%");
            //-> minimal benar receipt (misal 4) start 3 full benar, benar diatas 60 % bintang 1, benar 80% receipt bintang 2, 100% bintang 3
            //-> point untuk waktu <10 dan >0 = 2,
            //> 10 dan kurang 30 = 5,
            //>30 dan <60 = 10
            //->point untuk start
            // 3 = 15
            // 2 = 10
            // 1 = 5
            if(cook_presentase >= 60 &&  cook_presentase < 80)//bintang 1
            {
                //show notif berhasil
                HiddenOrShowNotifGagal(false);
                HiddenOrShowNotifBerhasil(true);

                //show star
                startObject[0].gameObject.SetActive(true);
                startObject[1].gameObject.SetActive(false);
                startObject[2].gameObject.SetActive(false);

                //update status cooking
                cookingResultSuccess = true;

                //audio
                audioGameList[0].Play();


                //jika final menu
                
                
                  
                    //set score
                    TimeConvertionToScoreAndScore(5);
                
                

            }
            else if (cook_presentase >= 80 && cook_presentase < 100)//bintang 2
            {
                //show notif berhasil
                HiddenOrShowNotifGagal(false);
                HiddenOrShowNotifBerhasil(true);

                //show star
                startObject[0].gameObject.SetActive(true);
                startObject[1].gameObject.SetActive(true);
                startObject[2].gameObject.SetActive(false);

                //update status cooking
                cookingResultSuccess = true;

                //audio
                audioGameList[0].Play();

                
                   
                    //set score
                    TimeConvertionToScoreAndScore(10);
                
                
            }
            else if (cook_presentase == 100)//bintang 3
            {
                //show notif berhasil
                HiddenOrShowNotifGagal(false);
                HiddenOrShowNotifBerhasil(true);

                //show star
                startObject[0].gameObject.SetActive(true);
                startObject[1].gameObject.SetActive(true);
                startObject[2].gameObject.SetActive(true);

                //update status cooking
                cookingResultSuccess = true;

                //audio
                audioGameList[0].Play();

                
                    
                    //set score
                    TimeConvertionToScoreAndScore(15);
                
                
            }
            else // gagal
            {
                //hidden notif berhasil
                HiddenOrShowNotifBerhasil(false);
                //show notif berhasil
                HiddenOrShowNotifGagal(true);

                //update status cooking
                cookingResultSuccess = false;
            }
        }
        //Kondisi else 
        //-> waktu habis
        else
        {
            // stop condition 
            cookRunning = false;

            //update status cooking
            cookingResultSuccess = false;

            //hidden notif berhasil
            HiddenOrShowNotifBerhasil(false);
            //show notif berhasil
            HiddenOrShowNotifGagal(true);
        }
        
    }
    void TimeConvertionToScoreAndScore(int score)
    {
        //convertwaktu menjadi int score
        int roundedIntValue = Mathf.RoundToInt(cookTime);//sisa waktu terbanyak adalah score terbanyak dan max 60
        //maximal score dari waktu =60
        if(roundedIntValue > 60) {
            roundedIntValue = 60;
        }

        //score + star
        roundedIntValue += score;

        //setScore
        pointScoreManagement.scoreGameplay += roundedIntValue;

        ScoreViewPlus(score);

        int countListLevelTerakhir = receipt_CookCount.Count - 1;
        if(MenuNoUrut >= countListLevelTerakhir)
        {
            //save score
            pointScoreManagement.SaveScoreNow();
        }
        

    }
    void ScoreViewPlus(int score)
    {
        //convertwaktu menjadi int score
        int roundedIntValue = Mathf.RoundToInt(cookTime);//sisa waktu terbanyak adalah score terbanyak dan max 60
        //maximal score dari waktu =60
        if (roundedIntValue > 60)
        {
            roundedIntValue = 60;
        }

        //score + star
        roundedIntValue += score;

        //setScore
        scoreInFinish = roundedIntValue;

        scoreTextInFinish.text = scoreInFinish.ToString();

    }
    public void btnOkay_finish()
    {
        //reset time
        cookTime = cookingTimerDefault;

        HiddenOrShowAllNotif(false);

        //final update status
        if (cookingResultSuccess) // jika berhasil
        {
            int jumlahLevel = receipt_CookCount.Count - 1;
            if (MenuNoUrut < jumlahLevel) // berjalan tidak lebih dari level yang di tentukan
            {
                MenuNoUrut += 1; // next level menu masakan
                UpdateTitleCook();

                //reset kondisi
                ResetCondition();
            }
            else // jika sudah lebih
            {
                if(!disableAllDebug) Debug.Log("game finish");
                LoadScene("MenuUtama");
            }
            
        }
        else
        {
            //enable button memasak
            listObject[0].gameObject.SetActive(true);

            cookRunning = true;
            //reset kondition
            //ResetCondition();

            if(!disableAllDebug) Debug.Log("Silahkan ulang memasak lagi");

        }
    }

    void HiddenOrShowNotifGagal(bool show) //hidden/show notif gagal
    {
        //hidden/show finish UI Group
        listObject[4].gameObject.SetActive(show);

        //hidden/show gagal 
        listObject[1].gameObject.SetActive(show);

        //hidden/show okay
        listObject[3].gameObject.SetActive(show);
    }

    void HiddenOrShowNotifBerhasil(bool show) //hidden/show notif berhasil
    {
        //hidden/show finish UI Group
        listObject[4].gameObject.SetActive(show);

        //hidden/show berhasil 
        listObject[2].gameObject.SetActive(show);

        //hidden/show okay
        listObject[3].gameObject.SetActive(show);

        //set UI Score
        //pointScoreManagement.SetTextScore();
        realtimeScoreText.text = pointScoreManagement.scoreGameplay.ToString();
        scoreTextInFinish.text = scoreInFinish.ToString();
    }

    public void HiddenOrShowAllNotif(bool hidden)
    {
        
        HiddenOrShowNotifBerhasil(hidden);
        
        HiddenOrShowNotifGagal(hidden);
    }

    public void CookCountPlus()// tambah jumlah memasak receipt
    {
        cooking_CountReceipt += 1;
        if (!disableAllDebug) Debug.Log(cooking_CountReceipt.ToString());
    }

    public void CookCountMinus()// kurangi jumlah memasak receipt
    {
        cooking_CountReceipt -= 1;
        if (!disableAllDebug) Debug.Log(cooking_CountReceipt.ToString());
    }

    void UpdateTitleCook()
    {
        judulBar.sprite = imageForTitle[MenuNoUrut];
        ThumbnailBar.sprite = ThumbnailForTitle[MenuNoUrut];
    }

    public void CookWrongMethodPlus()
    {
        cooking_CountWrong += 1;
    }

    public void CookWrongMethodMinus()
    {
        cooking_CountWrong -= 1;
    }

    void ResetCondition()
    {
        //cook running
        cookRunning = true;

        //jumlah receipt benar
        cooking_CountReceipt = 0;

        //jumlah receipt salah
        cooking_CountWrong = 0;

        //reset time
        cookTime = cookingTimerDefault;

        //reset button select
        foreach (Cooking_ProcessValue getImageComponent in bahanReceiptReset)
        {
            if (getImageComponent != null)
            {
                getImageComponent.selectReceipt = false;
                getImageComponent.GetComponent<Image>().sprite = getImageComponent.selectReceiptMenuImageDefault;
            }
        }
    }
    void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        // Mulai asynchronous loading scene
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);

        // Tunggu hingga selesai loading
        while (!asyncOperation.isDone)
        {
            // Dapatkan progress loading (0.0 - 1.0)
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            // Contoh: Menampilkan progress loading
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            yield return null;
        }

        // Scene telah selesai loading
        Debug.Log("Scene selesai loading");
    }
}
