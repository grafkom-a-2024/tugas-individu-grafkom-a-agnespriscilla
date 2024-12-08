using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagement : MonoBehaviour
{
    [SerializeField]
    List<GameObject> objList;
    // Start is called before the first frame update
    void Start()
    {
        if (objList.Count > 0)
        {
            objList[0].SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Menangani tombol Escape pada Android
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackMenuUtama();
        }
    }
    public void SceneLoad(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Mulai asynchronous loading scene
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

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
    
    public void BackMenuUtama()
    {
        objList[0].SetActive(true);
    }
    public void HideUIBackMenuUtama()
    {
        objList[0].SetActive(false);
    }
}
