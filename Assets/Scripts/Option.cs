using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TAR
{
    public class Option : MonoBehaviour
    {
        [SerializeField]
        private GameManager gm;
        [SerializeField]
        private GameObject OptionUI;

        public void OpenOption()
        {
            if (!gm.isRunning) return;
            gm.isRunning = false;
            OptionUI.gameObject.SetActive(true);
        }

        public void Resume()
        {
            gm.isRunning = true;
            OptionUI.gameObject.SetActive(false);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
