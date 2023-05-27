using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    BulletScript bulletScript = null;
    [SerializeField]
    GameObject enemySpawn = null;
    [SerializeField]
    Canvas gameClear = null;
    [SerializeField]
    Canvas gameOver = null;
    [SerializeField]
    Text countText = null;
    [SerializeField,Min(1)]
    int maxCount = 30;

    bool isGameClear = false;
    bool isGameOver = false;
    int count = 0;

    public int Count
    {
        set
        {
            count = Mathf.Max(0, value);
            UpdateCountText();

            if(count >= maxCount)
            {
                GameClear();
            }
        }
        get
        {
            return count;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        count = 0;
        UpdateCountText();
    }

    public void GameClear()
    {
        if(isGameClear || isGameOver)
        {
            return;
        }

        Time.timeScale = 0f;
        bulletScript.enabled = false;
        enemySpawn.SetActive(false);
        gameClear.enabled = true;
        Cursor.lockState = CursorLockMode.None;

        isGameClear = true;
    }

    public void GameOver()
    {
        if(isGameClear || isGameOver)
        {
            return;
        }

        Time.timeScale = 0f;
        bulletScript.enabled = false;
        enemySpawn.SetActive(false);
        gameOver.enabled = true;
        Cursor.lockState = CursorLockMode.None;

        isGameOver = true;
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateCountText()
    {
        countText.text = count.ToString() + " / " + maxCount.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
