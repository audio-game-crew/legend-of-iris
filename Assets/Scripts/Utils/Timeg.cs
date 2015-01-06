using UnityEngine;

    public class Timeg
    {
        /// <summary>
        /// Maximum can only be 1
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static float safeDelta(float speed)
        {
            return Mathf.Min(1f, Time.deltaTime * speed);
        }
        /// <summary>
        /// Maximum can only be 1
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static float safeDelta()
        {
            return Mathf.Min(1f, Time.deltaTime);
        }
        /// <summary>
        /// Maximum can only be 1
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static float safeFixedDelta(float speed)
        {
            return Mathf.Min(1f, Time.fixedDeltaTime * speed);
        }
        /// <summary>
        /// Maximum can only be 1
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static float safeFixedDelta()
        {
            return Mathf.Min(1f, Time.fixedDeltaTime);
        }
    }