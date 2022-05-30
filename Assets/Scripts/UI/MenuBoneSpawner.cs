using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class MenuBoneSpawner : MonoBehaviour
    {
        [SerializeField] GameObject[] bones;
        [Min(0f)]
        [SerializeField] float minSpawnTime;
        [SerializeField] float maxSpawnTime;
        [SerializeField] float minSpawnX;
        [SerializeField] float maxSpawnX;

        private float nextBone = 0f;
        private float elapsed = 0f;


        private void Update()
        {
            elapsed += Time.deltaTime;

            if (elapsed >= nextBone)
            {
                var bone = Instantiate(bones[Random.Range(0, bones.Length)]);
                bone.transform.position = new Vector3(
                    transform.position.x + Random.Range(minSpawnX, maxSpawnX),
                    transform.position.y
                    );
                elapsed = 0f;
                nextBone = Random.Range(minSpawnTime, maxSpawnTime);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position + Vector3.right * minSpawnX, 0.4f);
            Gizmos.DrawSphere(transform.position + Vector3.right * maxSpawnX, 0.4f);
            Gizmos.DrawLine(transform.position + Vector3.right * minSpawnX, transform.position + Vector3.right * maxSpawnX);
        }
    }
}

