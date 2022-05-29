using System;
using UnityEngine;

namespace FictionalOctoDoodle.Core
{
    public class AIDiveBomb : IAIBehavior
    {
        private Transform transform;
        private Vector3 nadir;
        private float speed;

        private float cTwoPiFreq;
        private float amp;
        private float t = 0f;
        private float neg;
        private Vector3 start;
        private Vector3 end;

        private readonly Action OnDiveComplete;


        public AIDiveBomb(Vector3 target, float speed, Action diveCompleteAction)
        {
            nadir = target;
            this.speed = speed;
            OnDiveComplete = diveCompleteAction;
        }
        
        public void Initialize(Transform transform)
        {
            this.transform = transform;
            var len = 2f * Vector3.Distance(nadir, transform.position);
            // a single sinewave is close to a straight line and faster benefits us anyway
            var f = speed / len;
            cTwoPiFreq = 4f * Mathf.PI * f * (1f / Mathf.Abs(nadir.x - transform.position.x));
            
            neg = Mathf.Sign(nadir.x - transform.position.x);
            amp = Mathf.Abs(transform.position.y - nadir.y) * 0.5f;
            start = transform.position;
            end = new Vector3(
                2f * (nadir.x - transform.position.x) + transform.position.x,
                transform.position.y
                );
        }

        public void Update()
        {
            t += Time.deltaTime;
            var x = t * neg * speed;
            var y = amp * Mathf.Cos(cTwoPiFreq * x);
            transform.position = start + new Vector3(x, y - amp);

            if (Vector3.Distance(end, transform.position) <= 0.05f)
            {
                OnDiveComplete();
            }
        }

    }
}


