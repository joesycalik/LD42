using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Level
{
    public class WinLossMenu : MonoBehaviour
    {

        public LevelManager levelManager;

        public Text levelResultText;

        bool isWin;

        public bool IsWin
        {
            get
            {
                return isWin;
            }

            set
            {

                if (value)
                {
                    isWin = true;
                    levelResultText.text = "Complete!";
                    levelResultText.color = Color.green;
                }
                else
                {
                    isWin = false;
                    levelResultText.text = "Failed!";
                    levelResultText.color = Color.red;
                }
            }
        }


        public void ReturnToMenu()
        {
            Helpers.ReturnToMenu();
        }

        public void RetryLevel()
        {
            
        }
    }
}