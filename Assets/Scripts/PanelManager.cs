using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Panel;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        Panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Pause();
    }
    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Panel.activeSelf)
            {
                Panel.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else
            {
                Panel.SetActive(true);
                Time.timeScale = 0.0f;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
