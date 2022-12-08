using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace TAR
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text t;
        [SerializeField]
        private GameManager gm;


        public void SetText(string text)
        {
            t.text = text;        
        }
        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void Quit()
        {
            Application.Quit();
        }
        public void Resume()
        {
            gm.isRunning = true;
            this.gameObject.SetActive(false);
        }
    }
}
