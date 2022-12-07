using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TAR
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("ARScene");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
