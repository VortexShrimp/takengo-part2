using TMPro;
using UnityEngine;

namespace UserInterface
{
    /// <summary>
    /// Controls the UI in the "Game Over" screen.
    /// </summary>
    public class GameOverController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreTextMesh;

        private void Start()
        {
            scoreTextMesh.text = $"You died. Final score: {GameManager.Instance.score}";

            if (GameManager.Instance.bossActive)
            {
                scoreTextMesh.text = $"You're a dud. The boss was on his way.";

                if (GameManager.Instance.bossSpawned)
                {
                    scoreTextMesh.text = $"The boss killed you. You're a bot.";

                    if (GameManager.Instance.bossKilled)
                    {
                        scoreTextMesh.text = $"You killed Johannes. No one liked him anyway...";
                    }
                }
            }
        }
    } 
}

