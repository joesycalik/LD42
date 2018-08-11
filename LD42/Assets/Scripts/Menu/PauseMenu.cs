using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Level
{
    public class PauseMenu : MonoBehaviour
    {

        public LevelManager levelManager;

        public void Open()
        {
            this.gameObject.SetActive(true);
        }

        public void Close()
        {
            this.gameObject.SetActive(false);
        }

        public void ReturnToMainMenu()
        {
            Helpers.ReturnToMenu();
        }
    }
}