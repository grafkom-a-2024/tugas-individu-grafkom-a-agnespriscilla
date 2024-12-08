using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PointManagement : MonoBehaviour
{
    public int scoreGameplay;

    [SerializeField]
    Text scoreText;

    

    int playerScoreLoaded;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MenuUtama")
        {
            LoadScore();
            scoreGameplay = playerScoreLoaded;
            SetTextScore();
        }
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTextScore()
    {
        scoreText.text = scoreGameplay.ToString();
       
    }
    public void SaveScoreNow()
    {
        //load score
        LoadScore();
        if(scoreGameplay > playerScoreLoaded)//jika lebih besar dari sebelumnya
        {
            MethodSaveScore(scoreGameplay);//save
        }
    }

    public void MethodSaveScore(int setScore)
    {
        // Simpan nilai dengan kunci "PlayerScore"
        PlayerPrefs.SetInt("PlayerScore", setScore);
    }

    public void LoadScore()
    {
        // Ambil nilai dengan kunci "PlayerScore" (default value: 0 jika kunci tidak ditemukan)
        playerScoreLoaded = PlayerPrefs.GetInt("PlayerScore", 0);

    }
}
