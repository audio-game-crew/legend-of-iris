using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    /// <summary>
    /// Source: http://www.gizma.com/easing/
    /// </summary>
    class Ease
    {
        /// <summary>
        /// Assuming t is positive, result will bounce from 0 to 1 to 0 to 1 to 0 etc.
        /// </summary>
        /// <param name="t">The time value (positive)</param>
        /// <param name="d">The duration of a single forward/backward bounce</param>
        /// <returns></returns>
        public static float loopBounce(float t, float d)
        {
            return 1f - Mathf.Abs(((t / d) % 2f) - 1f);
        }
        /// <summary>
        /// Assuming t is positive, result will bounce from 0 to 1 to 0 to 1 to 0 etc.
        /// </summary>
        /// <param name="t">The time value (positive)</param>
        /// <param name="d">The duration of a single forward/backward bounce</param>
        /// <returns></returns>
        public static float loopBounce(float t)
        {
            return 1f - Mathf.Abs((t % 2f) - 1f);
        }
        /// <summary>
        /// Assuming t is positive, result will loop from 0 to 1, from 0 to 1 etc.
        /// </summary>
        /// <param name="t">The time value (positive)</param>
        /// <param name="d">The duration of a single forward/backward bounce</param>
        /// <returns></returns>
        public static float loop(float t, float d = 1f)
        {
            return (t/d) % 1f;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float linear(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            return c * t / d + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float ioQuad(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t + b;
            t--;
            return -c / 2 * (t * (t - 2) - 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float iQuad(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d;
            return c * t * t + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float oQuad(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d;
            return -c * t * (t - 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float ioCubic(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t + b;
            t -= 2;
            return c / 2 * (t * t * t + 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float iCubic(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d;
            return c * t * t * t + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float oCubic(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d;
            t--;
            return c * (t * t * t + 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float ioQuartic(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t * t + b;
            t -= 2;
            return -c / 2 * (t * t * t * t - 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float iQuartic(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d;
            return c * t * t * t * t + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float oQuartic(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d;
            t--;
            return -c * (t * t * t * t - 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float ioQuintic(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t * t * t + b;
            t -= 2;
            return c / 2 * (t * t * t * t * t + 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float iQuintic(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d;
            return c * t * t * t * t * t + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float oQuintic(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d;
            t--;
            return c * (t * t * t * t * t + 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float ioSinusoidal(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            return -c / 2 * (Mathf.Cos(Mathf.PI * t / d) - 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float iSinusoidal(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float oSinusoidal(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float ioExponential(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b;
            t--;
            return c / 2 * (-Mathf.Pow(2, -10 * t) + 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float oExponential(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            return c * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float iExponential(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            return c * Mathf.Pow(2, 10 * (t / d - 1)) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float ioCircular(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d / 2;
            if (t < 1) return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;
            t -= 2;
            return c / 2 * (Mathf.Sqrt(1 - t * t) + 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float oCircular(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d;
            t--;
            return c * Mathf.Sqrt(1 - t * t) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static float iCircular(float t, float b = 0f, float c = 1f, float d = 1f)
        {
            t /= d;
            return -c * (Mathf.Sqrt(1 - t * t) - 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        /// <param name="s">Overease</param>
        public static float iBack(float t, float b = 0f, float c = 1f, float d = 1f, float s = 1.70158f)
        {
            return c * (t /= d) * t * ((s + 1) * t - s) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        /// <param name="s">Overease</param>
        public static float oBack(float t, float b = 0f, float c = 1f, float d = 1f, float s = 1.70158f)
        {
            return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        /// <param name="s">Overease</param>
        public static float ioBack(float t, float b = 0f, float c = 1f, float d = 1f, float s = 1.70158f)
        {
            if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
            return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        /// <param name="s">Overease</param>
        public static float oiBack(float t, float b = 0f, float c = 1f, float d = 1f, float s = 1.70158f)
        {
            if (t < d / 2) return oBack(t * 2, b, c / 2, d, s);
            return iBack((t * 2) - d, b + c / 2, c / 2, d, s);
        }

        /*********************************************************************/

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 linear(float t, Vector3 b, Vector3 c, float d)
        {
            return c * t / d + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 ioQuad(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t + b;
            t--;
            return -c / 2 * (t * (t - 2) - 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 iQuad(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d;
            return c * t * t + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 oQuad(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d;
            return -c * t * (t - 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 ioCubic(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t + b;
            t -= 2;
            return c / 2 * (t * t * t + 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 iCubic(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d;
            return c * t * t * t + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 oCubic(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d;
            t--;
            return c * (t * t * t + 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 ioQuartic(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t * t + b;
            t -= 2;
            return -c / 2 * (t * t * t * t - 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 iQuartic(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d;
            return c * t * t * t * t + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 oQuartic(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d;
            t--;
            return -c * (t * t * t * t - 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 ioQuintic(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t * t * t + b;
            t -= 2;
            return c / 2 * (t * t * t * t * t + 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 iQuintic(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d;
            return c * t * t * t * t * t + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 oQuintic(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d;
            t--;
            return c * (t * t * t * t * t + 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 ioSinusoidal(float t, Vector3 b, Vector3 c, float d)
        {
            return -c / 2 * (Mathf.Cos(Mathf.PI * t / d) - 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 iSinusoidal(float t, Vector3 b, Vector3 c, float d)
        {
            return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 oSinusoidal(float t, Vector3 b, Vector3 c, float d)
        {
            return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 ioExponential(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b;
            t--;
            return c / 2 * (-Mathf.Pow(2, -10 * t) + 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 oExponential(float t, Vector3 b, Vector3 c, float d)
        {
            return c * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 iExponential(float t, Vector3 b, Vector3 c, float d)
        {
            return c * Mathf.Pow(2, 10 * (t / d - 1)) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 ioCircular(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d / 2;
            if (t < 1) return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;
            t -= 2;
            return c / 2 * (Mathf.Sqrt(1 - t * t) + 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 oCircular(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d;
            t--;
            return c * Mathf.Sqrt(1 - t * t) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector3 iCircular(float t, Vector3 b, Vector3 c, float d)
        {
            t /= d;
            return -c * (Mathf.Sqrt(1 - t * t) - 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        /// <param name="s">Overease</param>
        public static Vector3 iBack(float t, Vector3 b, Vector3 c, float d, Vector3 s)
        {
            return (c * (t /= d) * t).times((s + Vector3.one) * t - s) + b;
        }
        public static Vector3 iBack(float t, Vector3 b, Vector3 c, float d)
        {
            return iBack(t, b, c, d, Vector3.one * 1.70158f);
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        /// <param name="s">Overease</param>
        public static Vector3 oBack(float t, Vector3 b, Vector3 c, float d, Vector3 s)
        {
            return c.times((t = t / d - 1) * t * ((s + Vector3.one) * t + s) + Vector3.one) + b;
        }
        public static Vector3 oBack(float t, Vector3 b, Vector3 c, float d)
        {
            return oBack(t, b, c, d, Vector3.one * 1.70158f);
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        /// <param name="s">Overease</param>
        public static Vector3 ioBack(float t, Vector3 b, Vector3 c, float d, Vector3 s)
        {
            if ((t /= d / 2) < 1) return (c / 2).times(t * t * (((s *= (1.525f)) + Vector3.one) * t - s)) + b;
            return (c / 2).times((t -= 2) * t * (((s *= (1.525f)) + Vector3.one) * t + s) + Vector3.one + Vector3.one) + b;
        }
        public static Vector3 ioBack(float t, Vector3 b, Vector3 c, float d)
        {
            return ioBack(t, b, c, d, Vector3.one * 1.70158f);
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        /// <param name="s">Overease</param>
        public static Vector3 oiBack(float t, Vector3 b, Vector3 c, float d, Vector3 s)
        {
            if (t < d / 2) return oBack(t * 2, b, c / 2, d, s);
            return iBack((t * 2) - d, b + c / 2, c / 2, d, s);
        }
        public static Vector3 oiBack(float t, Vector3 b, Vector3 c, float d)
        {
            return oiBack(t, b, c, d, Vector3.one * 1.70158f);
        }

        /*********************************************************************/

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 linear(float t, Vector2 b, Vector2 c, float d)
        {
            return c * t / d + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 ioQuad(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t + b;
            t--;
            return -c / 2 * (t * (t - 2) - 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 iQuad(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d;
            return c * t * t + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 oQuad(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d;
            return -c * t * (t - 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 ioCubic(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t + b;
            t -= 2;
            return c / 2 * (t * t * t + 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 iCubic(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d;
            return c * t * t * t + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 oCubic(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d;
            t--;
            return c * (t * t * t + 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 ioQuartic(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t * t + b;
            t -= 2;
            return -c / 2 * (t * t * t * t - 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 iQuartic(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d;
            return c * t * t * t * t + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 oQuartic(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d;
            t--;
            return -c * (t * t * t * t - 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 ioQuintic(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t * t * t + b;
            t -= 2;
            return c / 2 * (t * t * t * t * t + 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 iQuintic(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d;
            return c * t * t * t * t * t + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 oQuintic(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d;
            t--;
            return c * (t * t * t * t * t + 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 ioSinusoidal(float t, Vector2 b, Vector2 c, float d)
        {
            return -c / 2 * (Mathf.Cos(Mathf.PI * t / d) - 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 iSinusoidal(float t, Vector2 b, Vector2 c, float d)
        {
            return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 oSinusoidal(float t, Vector2 b, Vector2 c, float d)
        {
            return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 ioExponential(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b;
            t--;
            return c / 2 * (-Mathf.Pow(2, -10 * t) + 2) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 oExponential(float t, Vector2 b, Vector2 c, float d)
        {
            return c * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 iExponential(float t, Vector2 b, Vector2 c, float d)
        {
            return c * Mathf.Pow(2, 10 * (t / d - 1)) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 ioCircular(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d / 2;
            if (t < 1) return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;
            t -= 2;
            return c / 2 * (Mathf.Sqrt(1 - t * t) + 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 oCircular(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d;
            t--;
            return c * Mathf.Sqrt(1 - t * t) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        public static Vector2 iCircular(float t, Vector2 b, Vector2 c, float d)
        {
            t /= d;
            return -c * (Mathf.Sqrt(1 - t * t) - 1) + b;
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        /// <param name="s">Overease</param>
        public static Vector2 iBack(float t, Vector2 b, Vector2 c, float d, Vector2 s)
        {
            return (c * (t /= d) * t).times((s + Vector2.one) * t - s) + b;
        }
        public static Vector2 iBack(float t, Vector2 b, Vector2 c, float d)
        {
            return iBack(t, b, c, d, Vector2.one * 1.70158f);
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        /// <param name="s">Overease</param>
        public static Vector2 oBack(float t, Vector2 b, Vector2 c, float d, Vector2 s)
        {
            return c.times((t = t / d - 1) * t * ((s + Vector2.one) * t + s) + Vector2.one) + b;
        }
        public static Vector2 oBack(float t, Vector2 b, Vector2 c, float d)
        {
            return oBack(t, b, c, d, Vector2.one * 1.70158f);
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        /// <param name="s">Overease</param>
        public static Vector2 ioBack(float t, Vector2 b, Vector2 c, float d, Vector2 s)
        {
            if ((t /= d / 2) < 1) return (c / 2).times(t * t * (((s *= (1.525f)) + Vector2.one) * t - s)) + b;
            return (c / 2).times((t -= 2) * t * (((s *= (1.525f)) + Vector2.one) * t + s) + Vector2.one + Vector2.one) + b;
        }
        public static Vector2 ioBack(float t, Vector2 b, Vector2 c, float d)
        {
            return ioBack(t, b, c, d, Vector2.one * 1.70158f);
        }

        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">Change in value</param>
        /// <param name="d">Duration</param>
        /// <param name="s">Overease</param>
        public static Vector2 oiBack(float t, Vector2 b, Vector2 c, float d, Vector2 s)
        {
            if (t < d / 2) return oBack(t * 2, b, c / 2, d, s);
            return iBack((t * 2) - d, b + c / 2, c / 2, d, s);
        }
        public static Vector2 oiBack(float t, Vector2 b, Vector2 c, float d)
        {
            return oiBack(t, b, c, d, Vector2.one * 1.70158f);
        }


    }
