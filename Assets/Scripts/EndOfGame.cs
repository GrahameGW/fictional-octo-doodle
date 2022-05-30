using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FictionalOctoDoodle.Core
{
    public class EndOfGame : MonoBehaviour
    {
        [SerializeField] SpriteRenderer endOfGameOverlay;
        [SerializeField] float fadeOutDuration;
        [SerializeField] float textFadeInDuration;
        [SerializeField] TextMeshProUGUI thanksText;
        [SerializeField] GameObject returnBtn;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.gameObject.GetComponentInParent<Player>();
            
            if (player != null)
            {

                StartCoroutine(FadeOut(player));
            }
        }

        private IEnumerator FadeOut(Player player)
        {
            float elapsed = 0f;
            endOfGameOverlay.gameObject.SetActive(true);
            player.enabled = false;

            do
            {
                endOfGameOverlay.color = new Color(1f, 1f, 1f, Mathf.Lerp(0, 1, elapsed / fadeOutDuration));
                elapsed += Time.deltaTime;
                yield return null;
            } while (elapsed <= fadeOutDuration);

            thanksText.gameObject.SetActive(true);
            elapsed = 0f;
            do
            {
                thanksText.color = new Color(1f, 1f, 1f, Mathf.Lerp(0, 1, elapsed / textFadeInDuration));
                elapsed += Time.deltaTime;
                yield return null;
            } while (elapsed <= textFadeInDuration);

            returnBtn.SetActive(true);
            Time.timeScale = 0f;
        }

        public void ReturnToMain()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}


