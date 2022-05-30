using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] float panSpeed;
        [Range(0f, 1f)]
        [SerializeField] float startPanX;
        [Range(0f, 1f)]
        [SerializeField] float stopPanX;
        [Range(0f, 1f)]
        [SerializeField] float startPanY;
        [Range(0f, 1f)]
        [SerializeField] float stopPanY;

        private bool isPanningX, isPanningY;

        private Player player;
        private Camera _camera;

        private readonly Vector2 CENTER = new Vector2(0.5f, 0.5f);


        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (player == null)
            {
                player = FindObjectOfType<Player>(); // dear god this is an awful way to do this
                return;
            }

            Vector2 pos = _camera.WorldToViewportPoint(player.transform.position);

            if (!isPanningX)
            {
                isPanningX = pos.x < startPanX || pos.x > 1f - startPanX;
            }
            else 
            {
                isPanningX = pos.x < stopPanX || pos.x > 1f - stopPanX;
            }

            if (!isPanningY)
            {
                isPanningY = pos.y < startPanY || pos.y > 1f - startPanY;
            }
            else
            {
                isPanningY = pos.y < stopPanY || pos.y > 1f - stopPanY;
            }

            var panVec = new Vector2(
                isPanningX ? pos.x - CENTER.x : 0f,
                isPanningY ? pos.y - CENTER.y : 0f
                );
            transform.Translate(panSpeed * Time.deltaTime * panVec);
        }

        private void OnDrawGizmosSelected()
        {
            if (_camera == null) _camera = GetComponent<Camera>();
            Gizmos.color = Color.cyan;

            var worldPanMin = _camera.ViewportToWorldPoint(new Vector2(startPanX, startPanY));
            var worldPanMax = _camera.ViewportToWorldPoint(new Vector2(1f - startPanX, 1f - startPanY));

            Gizmos.DrawLine(new Vector2(worldPanMin.x, worldPanMax.y), new Vector2(worldPanMin.x, worldPanMin.y));
            Gizmos.DrawLine(new Vector2(worldPanMax.x, worldPanMax.y), new Vector2(worldPanMax.x, worldPanMin.y));
            Gizmos.DrawLine(new Vector2(worldPanMin.x, worldPanMax.y), new Vector2(worldPanMax.x, worldPanMax.y));
            Gizmos.DrawLine(new Vector2(worldPanMin.x, worldPanMin.y), new Vector2(worldPanMax.x, worldPanMin.y));

            Gizmos.color = Color.gray;

            worldPanMin = _camera.ViewportToWorldPoint(new Vector2(stopPanX, stopPanY));
            worldPanMax = _camera.ViewportToWorldPoint(new Vector2(1f - stopPanX, 1f - stopPanY));

            Gizmos.DrawLine(new Vector2(worldPanMin.x, worldPanMax.y), new Vector2(worldPanMin.x, worldPanMin.y));
            Gizmos.DrawLine(new Vector2(worldPanMax.x, worldPanMax.y), new Vector2(worldPanMax.x, worldPanMin.y));
            Gizmos.DrawLine(new Vector2(worldPanMin.x, worldPanMax.y), new Vector2(worldPanMax.x, worldPanMax.y));
            Gizmos.DrawLine(new Vector2(worldPanMin.x, worldPanMin.y), new Vector2(worldPanMax.x, worldPanMin.y));
        }
    }
}

