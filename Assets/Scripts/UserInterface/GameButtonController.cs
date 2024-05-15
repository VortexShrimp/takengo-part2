using UnityEngine;
using UnityEngine.SceneManagement;

namespace UserInterface
{
    /// <summary>
    /// A simple class used to handle button presses in the "Game Over" scene.
    /// </summary> 
    public class GameButtonController : MonoBehaviour
    {
        public void OnStartButtonPressed()
        {
            // Load the game's main scene to start the game.
            SceneManager.LoadSceneAsync("Main Game", LoadSceneMode.Single);
        }
        
        public void OnRestartButtonPressed()
        {
            // Load the game's main scene to restart the game.
            SceneManager.LoadSceneAsync("Main Game", LoadSceneMode.Single);
        }

        public void OnExitButtonPressed()
        {
            // Exit the Unity application.
            Application.Quit();
        }
    }
}



