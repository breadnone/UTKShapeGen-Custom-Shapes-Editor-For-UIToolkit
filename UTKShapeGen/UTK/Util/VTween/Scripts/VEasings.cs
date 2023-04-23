using UnityEngine;
using System.Runtime.CompilerServices;

namespace Breadnone
{
    public enum Ease
    {
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc,
        Linear,
        Spring,
        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,
        EaseInElastic,
        EaseOutElastic,
        EaseInOutElastic,
    }

    public sealed class VEasings
    {
        private const float NATURAL_LOG_OF_2 = 0.693147181f;

        //
        // Easing functions
        //

        public static float Linear(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, value);
        }

        public static float Spring(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return start + (end - start) * value;
        }

        public static float EaseInQuad(float start, float end, float value)
        {
            end -= start;
            return end * value * value + start;
        }

        public static float EaseOutQuad(float start, float end, float value)
        {
            end -= start;
            return -end * value * (value - 2) + start;
        }

        public static float EaseInOutQuad(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value + start;
            value--;
            return -end * 0.5f * (value * (value - 2) - 1) + start;
        }

        public static float EaseInCubic(float start, float end, float value)
        {
            end -= start;
            return end * value * value * value + start;
        }

        public static float EaseOutCubic(float start, float end, float value)
        {
            value--;
            end -= start;
            return end * (value * value * value + 1) + start;
        }

        public static float EaseInOutCubic(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value + start;
            value -= 2;
            return end * 0.5f * (value * value * value + 2) + start;
        }

        public static float EaseInQuart(float start, float end, float value)
        {
            end -= start;
            return end * value * value * value * value + start;
        }

        public static float EaseOutQuart(float start, float end, float value)
        {
            value--;
            end -= start;
            return -end * (value * value * value * value - 1) + start;
        }

        public static float EaseInOutQuart(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value * value + start;
            value -= 2;
            return -end * 0.5f * (value * value * value * value - 2) + start;
        }

        public static float EaseInQuint(float start, float end, float value)
        {
            end -= start;
            return end * value * value * value * value * value + start;
        }

        public static float EaseOutQuint(float start, float end, float value)
        {
            value--;
            end -= start;
            return end * (value * value * value * value * value + 1) + start;
        }

        public static float EaseInOutQuint(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value * value * value + start;
            value -= 2;
            return end * 0.5f * (value * value * value * value * value + 2) + start;
        }

        public static float EaseInSine(float start, float end, float value)
        {
            end -= start;
            return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
        }

        public static float EaseOutSine(float start, float end, float value)
        {
            end -= start;
            return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
        }

        public static float EaseInOutSine(float start, float end, float value)
        {
            end -= start;
            return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
        }

        public static float EaseInExpo(float start, float end, float value)
        {
            end -= start;
            return end * Mathf.Pow(2, 10 * (value - 1)) + start;
        }

        public static float EaseOutExpo(float start, float end, float value)
        {
            end -= start;
            return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
        }

        public static float EaseInOutExpo(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;
            value--;
            return end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;
        }

        public static float EaseInCirc(float start, float end, float value)
        {
            end -= start;
            return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
        }

        public static float EaseOutCirc(float start, float end, float value)
        {
            value--;
            end -= start;
            return end * Mathf.Sqrt(1 - value * value) + start;
        }

        public static float EaseInOutCirc(float start, float end, float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return -end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;
            value -= 2;
            return end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;
        }

        public static float EaseInBounce(float start, float end, float value)
        {
            end -= start;
            float d = 1f;
            return end - EaseOutBounce(0, end, d - value) + start;
        }

        public static float EaseOutBounce(float start, float end, float value)
        {
            value /= 1f;
            end -= start;
            if (value < (1 / 2.75f))
            {
                return end * (7.5625f * value * value) + start;
            }
            else if (value < (2 / 2.75f))
            {
                value -= (1.5f / 2.75f);
                return end * (7.5625f * (value) * value + .75f) + start;
            }
            else if (value < (2.5 / 2.75))
            {
                value -= (2.25f / 2.75f);
                return end * (7.5625f * (value) * value + .9375f) + start;
            }
            else
            {
                value -= (2.625f / 2.75f);
                return end * (7.5625f * (value) * value + .984375f) + start;
            }
        }

        public static float EaseInOutBounce(float start, float end, float value)
        {
            end -= start;
            float d = 1f;
            if (value < d * 0.5f) return EaseInBounce(0, end, value * 2) * 0.5f + start;
            else return EaseOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
        }

        public static float EaseInBack(float start, float end, float value)
        {
            end -= start;
            value /= 1;
            float s = 1.70158f;
            return end * (value) * value * ((s + 1) * value - s) + start;
        }

        public static float EaseOutBack(float start, float end, float value)
        {
            float s = 1.70158f;
            end -= start;
            value = (value) - 1;
            return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
        }

        public static float EaseInOutBack(float start, float end, float value)
        {
            float s = 1.70158f;
            end -= start;
            value /= .5f;
            if ((value) < 1)
            {
                s *= (1.525f);
                return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
            }
            value -= 2;
            s *= (1.525f);
            return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
        }

        public static float EaseInElastic(float start, float end, float value)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return -(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
        }

        public static float EaseOutElastic(float start, float end, float value)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p * 0.25f;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
        }

        public static float EaseInOutElastic(float start, float end, float value)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d * 0.5f) == 2) return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (value < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
            return a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
        }
//////////
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Linear(ref Vector3 start, ref Vector3 end, ref float value)
        {
            return Vector3.Lerp(start, end, value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Spring(ref Vector3 start, ref Vector3 end, ref float value)
        {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return  new Vector3(start.x + (end.x - start.x), start.y + (end.y - start.y), start.z + (end.z - start.z));//start + (end - start) * value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInQuad(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
            return end * value * value + start;
            //return new Vector3(end.x * value * value + start.x, end.y * value * value + start.y, end.z * value * value + start.z);//end * value * value + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseOutQuad(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
            return -end * value * (value - 2) + start;
            //return new Vector3(-end.x * value * (value - 2) + start.x, -end.y * value * (value - 2) + start.y, -end.z * value * (value - 2) + start.z);//-end * value * (value - 2) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInOutQuad(ref Vector3 start, ref Vector3 end, ref float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value + start;//new Vector3(end.x * 0.5f * value * value + start.x, end.y * 0.5f * value * value + start.y, end.z * 0.5f * value * value + start.z);//end * 0.5f * value * value + start;
            value--;
            return -end * 0.5f * (value * (value - 2) - 1) + start;//new Vector3(-end.x * 0.5f * (value * (value - 2) - 1) + start.x, -end.y * 0.5f * (value * (value - 2) - 1) + start.y, -end.z * 0.5f * (value * (value - 2) - 1) + start.z);//-end * 0.5f * (value * (value - 2) - 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInCubic(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
            return end * value * value * value + start;
            //return new Vector3(end.x * value * value * value + start.x, end.y * value * value * value + start.y, end.z * value * value * value + start.z);//end * value * value * value + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseOutCubic(ref Vector3 start, ref Vector3 end, ref float value)
        {
            value--;
            end -= start;
            return end * (value * value * value + 1) + start;
            //return new Vector3(end.x * (value * value * value + 1) + start.x, end.y * (value * value * value + 1) + start.y, end.z * (value * value * value + 1) + start.z); //end * (value * value * value + 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInOutCubic(ref Vector3 start, ref Vector3 end, ref float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value + start;//new Vector3(end.x * 0.5f * value * value * value + start.x, end.y * 0.5f * value * value * value + start.y, end.z * 0.5f * value * value * value + start.z);//end * 0.5f * value * value * value + start;
            value -= 2;
            return end * 0.5f * (value * value * value + 2) + start;//new Vector3(end.x * 0.5f * (value * value * value + 2) + start.x, end.y * 0.5f * (value * value * value + 2) + start.y, end.z * 0.5f * (value * value * value + 2) + start.z);//end * 0.5f * (value * value * value + 2) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInQuart(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
            return end * value * value * value * value + start;//new Vector3(end.x * value * value * value * value + start.x, end.y * value * value * value * value + start.y, end.z * value * value * value * value + start.z);//end * value * value * value * value + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseOutQuart(ref Vector3 start, ref Vector3 end, ref float value)
        {
            value--;
            end -= start;
            return new Vector3(-end.x * (value * value * value * value - 1) + start.x, -end.y * (value * value * value * value - 1) + start.y, -end.z * (value * value * value * value - 1) + start.z);//-end * (value * value * value * value - 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInOutQuart(ref Vector3 start, ref Vector3 end, ref float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value * value + start;//new Vector3(end.x * 0.5f * value * value * value * value + start.x, end.y * 0.5f * value * value * value * value + start.y, end.z * 0.5f * value * value * value * value + start.z);//end * 0.5f * value * value * value * value + start;
            value -= 2;
            return -end * 0.5f * (value * value * value * value - 2) + start; //new Vector3(-end.x * 0.5f * (value * value * value * value - 2) + start.x, -end.y * 0.5f * (value * value * value * value - 2) + start.y, -end.z * 0.5f * (value * value * value * value - 2) + start.z);//-end * 0.5f * (value * value * value * value - 2) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInQuint(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
            return end * value * value * value * value * value + start;//new Vector3(end.x * value * value * value * value * value + start.x, end.y * value * value * value * value * value + start.y, end.z * value * value * value * value * value + start.z);//end * value * value * value * value * value + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseOutQuint(ref Vector3 start, ref Vector3 end, ref float value)
        {
            value--;
            end -= start;
            return end * (value * value * value * value * value + 1) + start;//new Vector3(end.x * (value * value * value * value * value + 1) + start.x, end.y * (value * value * value * value * value + 1) + start.y, end.z * (value * value * value * value * value + 1) + start.z);//end * (value * value * value * value * value + 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInOutQuint(ref Vector3 start, ref Vector3 end, ref float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value * value * value + start;//new Vector3(end.x * 0.5f * value * value * value * value * value + start.x, end.y * 0.5f * value * value * value * value * value + start.y, end.z * 0.5f * value * value * value * value * value + start.z);//end * 0.5f * value * value * value * value * value + start;
            value -= 2;
            return end * 0.5f * (value * value * value * value * value + 2) + start;//new Vector3(end.x * 0.5f * (value * value * value * value * value + 2) + start.x, end.y * 0.5f * (value * value * value * value * value + 2) + start.y, end.z * 0.5f * (value * value * value * value * value + 2) + start.z);//end * 0.5f * (value * value * value * value * value + 2) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInSine(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
            return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;//new Vector3(-end.x * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end.x + start.x, -end.y * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end.y + start.y, -end.z * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end.z + start.z);//-end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseOutSine(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
            return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;//new Vector3(end.x * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start.x, end.y * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start.y, end.z * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start.z);//end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInOutSine(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
            return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;// new Vector3(-end.x * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start.x, -end.y * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start.y, -end.z * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start.z);//-end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInExpo(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
            return end * Mathf.Pow(2, 10 * (value - 1)) + start;//new Vector3(end.x * Mathf.Pow(2, 10 * (value - 1)) + start.x, end.y * Mathf.Pow(2, 10 * (value - 1)) + start.y, end.z * Mathf.Pow(2, 10 * (value - 1)) + start.z);//end * Mathf.Pow(2, 10 * (value - 1)) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseOutExpo(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
            return end * (-Mathf.Pow(2, -10 * value) + 1) + start;//new Vector3(end.x * (-Mathf.Pow(2, -10 * value) + 1) + start.x, end.y * (-Mathf.Pow(2, -10 * value) + 1) + start.y, end.z * (-Mathf.Pow(2, -10 * value) + 1) + start.z);//end * (-Mathf.Pow(2, -10 * value) + 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]  
        public static Vector3 EaseInOutExpo(ref Vector3 start, ref Vector3 end, ref float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;//new Vector3(end.x * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start.x, end.y * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start.y, end.z * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start.z);//end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;
            value--;
            return end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;//new Vector3(end.x * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start.x, end.y * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start.y, end.z * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start.z);//end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInCirc(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
            return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
            //return new Vector3(-end.x * (Mathf.Sqrt(1 - value * value) - 1) + start.x, -end.y * (Mathf.Sqrt(1 - value * value) - 1) + start.y, -end.z * (Mathf.Sqrt(1 - value * value) - 1) + start.z);//-end * (Mathf.Sqrt(1 - value * value) - 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseOutCirc(ref Vector3 start, ref Vector3 end, ref float value)
        {
            value--;
            end -= start;
            return end * Mathf.Sqrt(1 - value * value) + start;//new Vector3(end.x * Mathf.Sqrt(1 - value * value) + start.x, end.y * Mathf.Sqrt(1 - value * value) + start.y, end.z * Mathf.Sqrt(1 - value * value) + start.z);//end * Mathf.Sqrt(1 - value * value) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInOutCirc(ref Vector3 start, ref Vector3 end, ref float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return -end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;//new Vector3(-end.x * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start.x, -end.y * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start.y, -end.z * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start.z);//-end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;
            value -= 2;
            return end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;//new Vector3(end.x * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start.x, end.y * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start.y, end.z * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start.z);//end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInBounce(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
            return new Vector3(end.x - EaseOutBounce(0, end.x, 1f) + start.x, end.y - EaseOutBounce(0, end.y, 1f) + start.y, end.z - EaseOutBounce(0, end.z, 1f) + start.z);//end - EaseOutBounce(0, end, d) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseOutBounce(ref Vector3 start, ref Vector3 end, ref float value)
        {
            value /= 1f;
            end -= start;

            if (value < (1 / 2.75f))
            {
                return end * (7.5625f * value * value) + start;//new Vector3(end.x * (7.5625f * value * value) + start.x, end.y * (7.5625f * value * value) + start.y, end.z * (7.5625f * value * value) + start.z);///end * (7.5625f * value * value) + start;
            }
            else if (value < (2 / 2.75f))
            {
                value -= (1.5f / 2.75f);
                return end * (7.5625f * (value) * value + .75f) + start;//new Vector3(end.x * (7.5625f * (value) * value + .75f) + start.x, end.y * (7.5625f * (value) * value + .75f) + start.y, end.z * (7.5625f * (value) * value + .75f) + start.z);//end * (7.5625f * (value) * value + .75f) + start;
            }
            else if (value < (2.5 / 2.75))
            {
                value -= (2.25f / 2.75f);
                return end * (7.5625f * (value) * value + .9375f) + start;//new Vector3(end.x * (7.5625f * (value) * value + .9375f) + start.x, end.y * (7.5625f * (value) * value + .9375f) + start.y, end.z * (7.5625f * (value) * value + .9375f) + start.z);//end * (7.5625f * (value) * value + .9375f) + start;
            }
            else
            {
                value -= (2.625f / 2.75f);
                return end * (7.5625f * (value) * value + .984375f) + start;//new Vector3(end.x * (7.5625f * (value) * value + .984375f) + start.x, end.y * (7.5625f * (value) * value + .984375f) + start.y, end.z * (7.5625f * (value) * value + .984375f) + start.z);//end * (7.5625f * (value) * value + .984375f) + start;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInOutBounce(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
            float d = 1f;
            if (value < d * 0.5f) return new Vector3(EaseInBounce(0, end.x, value * 2) * 0.5f + start.x, EaseInBounce(0, end.y, value * 2) * 0.5f + start.y, EaseInBounce(0, end.z, value * 2) * 0.5f + start.z);//EaseInBounce(0, end, value * 2) * 0.5f + start;
            else return new Vector3(EaseOutBounce(0, end.x, value * 2 - d) * 0.5f + end.x * 0.5f + start.x, EaseOutBounce(0, end.y, value * 2 - d) * 0.5f + end.y * 0.5f + start.y, EaseOutBounce(0, end.z, value * 2 - d) * 0.5f + end.z * 0.5f + start.z);//EaseOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInBack(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
            value /= 1;
            float s = 1.70158f;
            return end * (value) * value * ((s + 1) * value - s) + start;//new Vector3(end.x * (value) * value * ((s + 1) * value - s) + start.x, end.y * (value) * value * ((s + 1) * value - s) + start.y, end.z * (value) * value * ((s + 1) * value - s) + start.z);//end * (value) * value * ((s + 1) * value - s) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseOutBack(ref Vector3 start, ref Vector3 end, ref float value)
        {
            float s = 1.70158f;
            end -= start;
            value = (value) - 1;
            return end * ((value) * value * ((s + 1) * value + s) + 1) + start;//new Vector3(end.x * ((value) * value * ((s + 1) * value + s) + 1) + start.x, end.y * ((value) * value * ((s + 1) * value + s) + 1) + start.y, end.z * ((value) * value * ((s + 1) * value + s) + 1) + start.z);//end * ((value) * value * ((s + 1) * value + s) + 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInOutBack(ref Vector3 start, ref Vector3 end, ref float value)
        {
            float s = 1.70158f;
            end -= start;
            value /= .5f;
            if ((value) < 1)
            {
                s *= (1.525f);
                return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;//new Vector3(end.x * 0.5f * (value * value * (((s) + 1) * value - s)) + start.x, end.y * 0.5f * (value * value * (((s) + 1) * value - s)) + start.y, end.z * 0.5f * (value * value * (((s) + 1) * value - s)) + start.z);//end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
            }
            value -= 2;
            s *= (1.525f);
            return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;//new Vector3(end.x * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start.x, end.y * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start.y, end.z * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start.z);//end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInElastic(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
//x
            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (value == 0) return start;
            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end.x))
            {
                a = end.x;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end.x / a);
            }
//y
            float dd = 1f;
            float pp = dd * .3f;
            float ss;
            float aa = 0;

            if ((value /= dd) == 1) return start + end;

            if (aa == 0f || aa < Mathf.Abs(end.y))
            {
                aa = end.y;
                ss = pp / 4;
            }
            else
            {
                ss = pp / (2 * Mathf.PI) * Mathf.Asin(end.y / aa);
            }
//z
            float ddd = 1f;
            float ppp = ddd * .3f;
            float sss;
            float aaa = 0;

            if ((value /= ddd) == 1) return start + end;

            if (aaa == 0f || aaa < Mathf.Abs(end.z))
            {
                aaa = end.y;
                sss = ppp / 4;
            }
            else
            {
                sss = ppp / (2 * Mathf.PI) * Mathf.Asin(end.z / aaa);
            }

            return new Vector3(-(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start.x, -(aa * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * dd - ss) * (2 * Mathf.PI) / pp)) + start.y, -(aaa * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * ddd - sss) * (2 * Mathf.PI) / ppp)) + start.z);//-(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseOutElastic(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;
//x
            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end.x))
            {
                a = end.x;
                s = p * 0.25f;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end.x / a);
            }

//y
            float dd = 1f;
            float pp = dd * .3f;
            float ss;
            float aa = 0;

            if (value == 0) return start;

            if ((value /= dd) == 1) return start + end;

            if (aa == 0f || aa < Mathf.Abs(end.y))
            {
                aa = end.y;
                ss = pp * 0.25f;
            }
            else
            {
                ss = pp / (2 * Mathf.PI) * Mathf.Asin(end.y / aa);
            }

//z
            float ddd = 1f;
            float ppp = ddd * .3f;
            float sss;
            float aaa = 0;

            if (value == 0) return start;

            if ((value /= ddd) == 1) return start + end;

            if (aaa == 0f || aaa < Mathf.Abs(end.z))
            {
                aaa = end.z;
                sss = ppp * 0.25f;
            }
            else
            {
                sss = ppp / (2 * Mathf.PI) * Mathf.Asin(end.z / aaa);
            }

            return new Vector3((a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end.x + start.x), (aa * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * dd - ss) * (2 * Mathf.PI) / pp) + end.y + start.y), (aaa * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * ddd - sss) * (2 * Mathf.PI) / ppp) + end.z + start.z));//(a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 EaseInOutElastic(ref Vector3 start, ref Vector3 end, ref float value)
        {
            end -= start;

//x
            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d * 0.5f) == 2) return start + end;

            if (a == 0f || a < Mathf.Abs(end.x))
            {
                a = end.x;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end.x / a);
            }

//y
            float dd = 1f;
            float pp = dd * .3f;
            float ss;
            float aa = 0;

            if (value == 0) return start;

            if ((value /= dd * 0.5f) == 2) return start + end;

            if (aa == 0f || aa < Mathf.Abs(end.y))
            {
                aa = end.y;
                ss = pp / 4;
            }
            else
            {
                ss = pp / (2 * Mathf.PI) * Mathf.Asin(end.y / aa);
            }
//z
            float ddd = 1f;
            float ppp = ddd * .3f;
            float sss;
            float aaa = 0;

            if (value == 0) return start;

            if ((value /= ddd * 0.5f) == 2) return start + end;

            if (aaa == 0f || aaa < Mathf.Abs(end.z))
            {
                aaa = end.z;
                sss = ppp / 4;
            }
            else
            {
                sss = ppp / (2 * Mathf.PI) * Mathf.Asin(end.z / aaa);
            }

            if (value < 1) return new Vector3(-0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start.x, -0.5f * (aa * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * dd - ss) * (2 * Mathf.PI) / pp)) + start.y, -0.5f * (aaa * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * ddd - sss) * (2 * Mathf.PI) / ppp)) + start.z);
            return new Vector3(a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f, aa * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * dd - ss) * (2 * Mathf.PI) / pp) * 0.5f, aaa * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * ddd - sss) * (2 * Mathf.PI) / ppp) * 0.5f) + end + start;
        }

//////////

//////////
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Linear(ref Vector2 start, ref Vector2 end, ref float value)
        {
            return Vector2.Lerp(start, end, value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Spring(ref Vector2 start, ref Vector2 end, ref float value)
        {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return  new Vector2(start.x + (end.x - start.x), start.y + (end.y - start.y));//start + (end - start) * value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInQuad(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
            return end * value * value + start;
            //return new Vector3(end.x * value * value + start.x, end.y * value * value + start.y, end.z * value * value + start.z);//end * value * value + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseOutQuad(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
            return -end * value * (value - 2) + start;
            //return new Vector3(-end.x * value * (value - 2) + start.x, -end.y * value * (value - 2) + start.y, -end.z * value * (value - 2) + start.z);//-end * value * (value - 2) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInOutQuad(ref Vector2 start, ref Vector2 end, ref float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value + start;//new Vector3(end.x * 0.5f * value * value + start.x, end.y * 0.5f * value * value + start.y, end.z * 0.5f * value * value + start.z);//end * 0.5f * value * value + start;
            value--;
            return -end * 0.5f * (value * (value - 2) - 1) + start;//new Vector3(-end.x * 0.5f * (value * (value - 2) - 1) + start.x, -end.y * 0.5f * (value * (value - 2) - 1) + start.y, -end.z * 0.5f * (value * (value - 2) - 1) + start.z);//-end * 0.5f * (value * (value - 2) - 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInCubic(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
            return end * value * value * value + start;
            //return new Vector3(end.x * value * value * value + start.x, end.y * value * value * value + start.y, end.z * value * value * value + start.z);//end * value * value * value + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseOutCubic(ref Vector2 start, ref Vector2 end, ref float value)
        {
            value--;
            end -= start;
            return end * (value * value * value + 1) + start;
            //return new Vector3(end.x * (value * value * value + 1) + start.x, end.y * (value * value * value + 1) + start.y, end.z * (value * value * value + 1) + start.z); //end * (value * value * value + 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInOutCubic(ref Vector2 start, ref Vector2 end, ref float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value + start;//new Vector3(end.x * 0.5f * value * value * value + start.x, end.y * 0.5f * value * value * value + start.y, end.z * 0.5f * value * value * value + start.z);//end * 0.5f * value * value * value + start;
            value -= 2;
            return end * 0.5f * (value * value * value + 2) + start;//new Vector3(end.x * 0.5f * (value * value * value + 2) + start.x, end.y * 0.5f * (value * value * value + 2) + start.y, end.z * 0.5f * (value * value * value + 2) + start.z);//end * 0.5f * (value * value * value + 2) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInQuart(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
            return end * value * value * value * value + start;//new Vector3(end.x * value * value * value * value + start.x, end.y * value * value * value * value + start.y, end.z * value * value * value * value + start.z);//end * value * value * value * value + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseOutQuart(ref Vector2 start, ref Vector2 end, ref float value)
        {
            value--;
            end -= start;
            return new Vector2(-end.x * (value * value * value * value - 1) + start.x, -end.y * (value * value * value * value - 1) + start.y);//-end * (value * value * value * value - 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInOutQuart(ref Vector2 start, ref Vector2 end, ref float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value * value + start;//new Vector3(end.x * 0.5f * value * value * value * value + start.x, end.y * 0.5f * value * value * value * value + start.y, end.z * 0.5f * value * value * value * value + start.z);//end * 0.5f * value * value * value * value + start;
            value -= 2;
            return -end * 0.5f * (value * value * value * value - 2) + start; //new Vector3(-end.x * 0.5f * (value * value * value * value - 2) + start.x, -end.y * 0.5f * (value * value * value * value - 2) + start.y, -end.z * 0.5f * (value * value * value * value - 2) + start.z);//-end * 0.5f * (value * value * value * value - 2) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInQuint(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
            return end * value * value * value * value * value + start;//new Vector3(end.x * value * value * value * value * value + start.x, end.y * value * value * value * value * value + start.y, end.z * value * value * value * value * value + start.z);//end * value * value * value * value * value + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseOutQuint(ref Vector2 start, ref Vector2 end, ref float value)
        {
            value--;
            end -= start;
            return end * (value * value * value * value * value + 1) + start;//new Vector3(end.x * (value * value * value * value * value + 1) + start.x, end.y * (value * value * value * value * value + 1) + start.y, end.z * (value * value * value * value * value + 1) + start.z);//end * (value * value * value * value * value + 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInOutQuint(ref Vector2 start, ref Vector2 end, ref float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value * value * value + start;//new Vector3(end.x * 0.5f * value * value * value * value * value + start.x, end.y * 0.5f * value * value * value * value * value + start.y, end.z * 0.5f * value * value * value * value * value + start.z);//end * 0.5f * value * value * value * value * value + start;
            value -= 2;
            return end * 0.5f * (value * value * value * value * value + 2) + start;//new Vector3(end.x * 0.5f * (value * value * value * value * value + 2) + start.x, end.y * 0.5f * (value * value * value * value * value + 2) + start.y, end.z * 0.5f * (value * value * value * value * value + 2) + start.z);//end * 0.5f * (value * value * value * value * value + 2) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInSine(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
            return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;//new Vector3(-end.x * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end.x + start.x, -end.y * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end.y + start.y, -end.z * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end.z + start.z);//-end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseOutSine(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
            return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;//new Vector3(end.x * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start.x, end.y * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start.y, end.z * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start.z);//end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInOutSine(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
            return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;// new Vector3(-end.x * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start.x, -end.y * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start.y, -end.z * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start.z);//-end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInExpo(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
            return end * Mathf.Pow(2, 10 * (value - 1)) + start;//new Vector3(end.x * Mathf.Pow(2, 10 * (value - 1)) + start.x, end.y * Mathf.Pow(2, 10 * (value - 1)) + start.y, end.z * Mathf.Pow(2, 10 * (value - 1)) + start.z);//end * Mathf.Pow(2, 10 * (value - 1)) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseOutExpo(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
            return end * (-Mathf.Pow(2, -10 * value) + 1) + start;//new Vector3(end.x * (-Mathf.Pow(2, -10 * value) + 1) + start.x, end.y * (-Mathf.Pow(2, -10 * value) + 1) + start.y, end.z * (-Mathf.Pow(2, -10 * value) + 1) + start.z);//end * (-Mathf.Pow(2, -10 * value) + 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]  
        public static Vector2 EaseInOutExpo(ref Vector2 start, ref Vector2 end, ref float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;//new Vector3(end.x * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start.x, end.y * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start.y, end.z * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start.z);//end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;
            value--;
            return end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;//new Vector3(end.x * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start.x, end.y * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start.y, end.z * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start.z);//end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInCirc(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
            return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
            //return new Vector3(-end.x * (Mathf.Sqrt(1 - value * value) - 1) + start.x, -end.y * (Mathf.Sqrt(1 - value * value) - 1) + start.y, -end.z * (Mathf.Sqrt(1 - value * value) - 1) + start.z);//-end * (Mathf.Sqrt(1 - value * value) - 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseOutCirc(ref Vector2 start, ref Vector2 end, ref float value)
        {
            value--;
            end -= start;
            return end * Mathf.Sqrt(1 - value * value) + start;//new Vector3(end.x * Mathf.Sqrt(1 - value * value) + start.x, end.y * Mathf.Sqrt(1 - value * value) + start.y, end.z * Mathf.Sqrt(1 - value * value) + start.z);//end * Mathf.Sqrt(1 - value * value) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInOutCirc(ref Vector2 start, ref Vector2 end, ref float value)
        {
            value /= .5f;
            end -= start;
            if (value < 1) return -end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;//new Vector3(-end.x * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start.x, -end.y * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start.y, -end.z * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start.z);//-end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;
            value -= 2;
            return end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;//new Vector3(end.x * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start.x, end.y * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start.y, end.z * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start.z);//end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInBounce(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
            return new Vector2(end.x - EaseOutBounce(0, end.x, 1f) + start.x, end.y - EaseOutBounce(0, end.y, 1f) + start.y);//end - EaseOutBounce(0, end, d) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseOutBounce(ref Vector2 start, ref Vector2 end, ref float value)
        {
            value /= 1f;
            end -= start;

            if (value < (1 / 2.75f))
            {
                return end * (7.5625f * value * value) + start;//new Vector3(end.x * (7.5625f * value * value) + start.x, end.y * (7.5625f * value * value) + start.y, end.z * (7.5625f * value * value) + start.z);///end * (7.5625f * value * value) + start;
            }
            else if (value < (2 / 2.75f))
            {
                value -= (1.5f / 2.75f);
                return end * (7.5625f * (value) * value + .75f) + start;//new Vector3(end.x * (7.5625f * (value) * value + .75f) + start.x, end.y * (7.5625f * (value) * value + .75f) + start.y, end.z * (7.5625f * (value) * value + .75f) + start.z);//end * (7.5625f * (value) * value + .75f) + start;
            }
            else if (value < (2.5 / 2.75))
            {
                value -= (2.25f / 2.75f);
                return end * (7.5625f * (value) * value + .9375f) + start;//new Vector3(end.x * (7.5625f * (value) * value + .9375f) + start.x, end.y * (7.5625f * (value) * value + .9375f) + start.y, end.z * (7.5625f * (value) * value + .9375f) + start.z);//end * (7.5625f * (value) * value + .9375f) + start;
            }
            else
            {
                value -= (2.625f / 2.75f);
                return end * (7.5625f * (value) * value + .984375f) + start;//new Vector3(end.x * (7.5625f * (value) * value + .984375f) + start.x, end.y * (7.5625f * (value) * value + .984375f) + start.y, end.z * (7.5625f * (value) * value + .984375f) + start.z);//end * (7.5625f * (value) * value + .984375f) + start;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInOutBounce(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
            float d = 1f;
            if (value < d * 0.5f) return new Vector2(EaseInBounce(0, end.x, value * 2) * 0.5f + start.x, EaseInBounce(0, end.y, value * 2) * 0.5f + start.y);//EaseInBounce(0, end, value * 2) * 0.5f + start;
            else return new Vector3(EaseOutBounce(0, end.x, value * 2 - d) * 0.5f + end.x * 0.5f + start.x, EaseOutBounce(0, end.y, value * 2 - d) * 0.5f + end.y * 0.5f + start.y);//EaseOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInBack(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
            value /= 1;
            float s = 1.70158f;
            return end * (value) * value * ((s + 1) * value - s) + start;//new Vector3(end.x * (value) * value * ((s + 1) * value - s) + start.x, end.y * (value) * value * ((s + 1) * value - s) + start.y, end.z * (value) * value * ((s + 1) * value - s) + start.z);//end * (value) * value * ((s + 1) * value - s) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseOutBack(ref Vector2 start, ref Vector2 end, ref float value)
        {
            float s = 1.70158f;
            end -= start;
            value = (value) - 1;
            return end * ((value) * value * ((s + 1) * value + s) + 1) + start;//new Vector3(end.x * ((value) * value * ((s + 1) * value + s) + 1) + start.x, end.y * ((value) * value * ((s + 1) * value + s) + 1) + start.y, end.z * ((value) * value * ((s + 1) * value + s) + 1) + start.z);//end * ((value) * value * ((s + 1) * value + s) + 1) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInOutBack(ref Vector2 start, ref Vector2 end, ref float value)
        {
            float s = 1.70158f;
            end -= start;
            value /= .5f;
            if ((value) < 1)
            {
                s *= (1.525f);
                return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;//new Vector3(end.x * 0.5f * (value * value * (((s) + 1) * value - s)) + start.x, end.y * 0.5f * (value * value * (((s) + 1) * value - s)) + start.y, end.z * 0.5f * (value * value * (((s) + 1) * value - s)) + start.z);//end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
            }
            value -= 2;
            s *= (1.525f);
            return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;//new Vector3(end.x * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start.x, end.y * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start.y, end.z * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start.z);//end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInElastic(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
//x
            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (value == 0) return start;
            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end.x))
            {
                a = end.x;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end.x / a);
            }
//y
            float dd = 1f;
            float pp = dd * .3f;
            float ss;
            float aa = 0;

            if ((value /= dd) == 1) return start + end;

            if (aa == 0f || aa < Mathf.Abs(end.y))
            {
                aa = end.y;
                ss = pp / 4;
            }
            else
            {
                ss = pp / (2 * Mathf.PI) * Mathf.Asin(end.y / aa);
            }

            return new Vector2(-(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start.x, -(aa * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * dd - ss) * (2 * Mathf.PI) / pp)) + start.y);//-(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseOutElastic(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;
//x
            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (value == 0) return start;
            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end.x))
            {
                a = end.x;
                s = p * 0.25f;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end.x / a);
            }

//y
            float dd = 1f;
            float pp = dd * .3f;
            float ss;
            float aa = 0;

            if (value == 0) return start;

            if ((value /= dd) == 1) return start + end;

            if (aa == 0f || aa < Mathf.Abs(end.y))
            {
                aa = end.y;
                ss = pp * 0.25f;
            }
            else
            {
                ss = pp / (2 * Mathf.PI) * Mathf.Asin(end.y / aa);
            }

            return new Vector2((a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end.x + start.x), (aa * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * dd - ss) * (2 * Mathf.PI) / pp) + end.y + start.y));//(a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 EaseInOutElastic(ref Vector2 start, ref Vector2 end, ref float value)
        {
            end -= start;

//x
            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d * 0.5f) == 2) return start + end;

            if (a == 0f || a < Mathf.Abs(end.x))
            {
                a = end.x;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end.x / a);
            }

//y
            float dd = 1f;
            float pp = dd * .3f;
            float ss;
            float aa = 0;

            if (value == 0) return start;

            if ((value /= dd * 0.5f) == 2) return start + end;

            if (aa == 0f || aa < Mathf.Abs(end.y))
            {
                aa = end.y;
                ss = pp / 4;
            }
            else
            {
                ss = pp / (2 * Mathf.PI) * Mathf.Asin(end.y / aa);
            }

            if (value < 1) return new Vector2(-0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start.x, -0.5f * (aa * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * dd - ss) * (2 * Mathf.PI) / pp)) + start.y);
            return new Vector2(a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f, aa * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * dd - ss) * (2 * Mathf.PI) / pp) * 0.5f) + end + start;
        }

//////////
        //
        // These are derived functions that the motor can use to get the speed at a specific time.
        //
        // The easing functions all work with a normalized time (0 to 1) and the returned value here
        // reflects that. Values returned here should be divided by the actual time.
        //
        // TODO: These functions have not had the testing they deserve. If there is odd behavior around
        //       dash speeds then this would be the first place I'd look.

        public static float LinearD(float start, float end, float value)
        {
            return end - start;
        }

        public static float EaseInQuadD(float start, float end, float value)
        {
            return 2f * (end - start) * value;
        }

        public static float EaseOutQuadD(float start, float end, float value)
        {
            end -= start;
            return -end * value - end * (value - 2);
        }

        public static float EaseInOutQuadD(float start, float end, float value)
        {
            value /= .5f;
            end -= start;

            if (value < 1)
            {
                return end * value;
            }

            value--;

            return end * (1 - value);
        }

        public static float EaseInCubicD(float start, float end, float value)
        {
            return 3f * (end - start) * value * value;
        }

        public static float EaseOutCubicD(float start, float end, float value)
        {
            value--;
            end -= start;
            return 3f * end * value * value;
        }

        public static float EaseInOutCubicD(float start, float end, float value)
        {
            value /= .5f;
            end -= start;

            if (value < 1)
            {
                return (3f / 2f) * end * value * value;
            }

            value -= 2;

            return (3f / 2f) * end * value * value;
        }

        public static float EaseInQuartD(float start, float end, float value)
        {
            return 4f * (end - start) * value * value * value;
        }

        public static float EaseOutQuartD(float start, float end, float value)
        {
            value--;
            end -= start;
            return -4f * end * value * value * value;
        }

        public static float EaseInOutQuartD(float start, float end, float value)
        {
            value /= .5f;
            end -= start;

            if (value < 1)
            {
                return 2f * end * value * value * value;
            }

            value -= 2;

            return -2f * end * value * value * value;
        }

        public static float EaseInQuintD(float start, float end, float value)
        {
            return 5f * (end - start) * value * value * value * value;
        }

        public static float EaseOutQuintD(float start, float end, float value)
        {
            value--;
            end -= start;
            return 5f * end * value * value * value * value;
        }

        public static float EaseInOutQuintD(float start, float end, float value)
        {
            value /= .5f;
            end -= start;

            if (value < 1)
            {
                return (5f / 2f) * end * value * value * value * value;
            }

            value -= 2;

            return (5f / 2f) * end * value * value * value * value;
        }

        public static float EaseInSineD(float start, float end, float value)
        {
            return (end - start) * 0.5f * Mathf.PI * Mathf.Sin(0.5f * Mathf.PI * value);
        }

        public static float EaseOutSineD(float start, float end, float value)
        {
            end -= start;
            return (Mathf.PI * 0.5f) * end * Mathf.Cos(value * (Mathf.PI * 0.5f));
        }

        public static float EaseInOutSineD(float start, float end, float value)
        {
            end -= start;
            return end * 0.5f * Mathf.PI * Mathf.Sin(Mathf.PI * value);
        }
        public static float EaseInExpoD(float start, float end, float value)
        {
            return (10f * NATURAL_LOG_OF_2 * (end - start) * Mathf.Pow(2f, 10f * (value - 1)));
        }

        public static float EaseOutExpoD(float start, float end, float value)
        {
            end -= start;
            return 5f * NATURAL_LOG_OF_2 * end * Mathf.Pow(2f, 1f - 10f * value);
        }

        public static float EaseInOutExpoD(float start, float end, float value)
        {
            value /= .5f;
            end -= start;

            if (value < 1)
            {
                return 5f * NATURAL_LOG_OF_2 * end * Mathf.Pow(2f, 10f * (value - 1));
            }

            value--;

            return (5f * NATURAL_LOG_OF_2 * end) / (Mathf.Pow(2f, 10f * value));
        }

        public static float EaseInCircD(float start, float end, float value)
        {
            return ((end - start) * value) / Mathf.Sqrt(1f - value * value);
        }

        public static float EaseOutCircD(float start, float end, float value)
        {
            value--;
            end -= start;
            return (-end * value) / Mathf.Sqrt(1f - value * value);
        }

        public static float EaseInOutCircD(float start, float end, float value)
        {
            value /= .5f;
            end -= start;

            if (value < 1)
            {
                return (end * value) / (2f * Mathf.Sqrt(1f - value * value));
            }

            value -= 2;

            return (-end * value) / (2f * Mathf.Sqrt(1f - value * value));
        }

        public static float EaseInBounceD(float start, float end, float value)
        {
            end -= start;
            float d = 1f;

            return EaseOutBounceD(0, end, d - value);
        }

        public static float EaseOutBounceD(float start, float end, float value)
        {
            value /= 1f;
            end -= start;

            if (value < (1 / 2.75f))
            {
                return 2f * end * 7.5625f * value;
            }
            else if (value < (2 / 2.75f))
            {
                value -= (1.5f / 2.75f);
                return 2f * end * 7.5625f * value;
            }
            else if (value < (2.5 / 2.75))
            {
                value -= (2.25f / 2.75f);
                return 2f * end * 7.5625f * value;
            }
            else
            {
                value -= (2.625f / 2.75f);
                return 2f * end * 7.5625f * value;
            }
        }

        public static float EaseInOutBounceD(float start, float end, float value)
        {
            end -= start;
            float d = 1f;

            if (value < d * 0.5f)
            {
                return EaseInBounceD(0, end, value * 2) * 0.5f;
            }
            else
            {
                return EaseOutBounceD(0, end, value * 2 - d) * 0.5f;
            }
        }

        public static float EaseInBackD(float start, float end, float value)
        {
            float s = 1.70158f;

            return 3f * (s + 1f) * (end - start) * value * value - 2f * s * (end - start) * value;
        }

        public static float EaseOutBackD(float start, float end, float value)
        {
            float s = 1.70158f;
            end -= start;
            value = (value) - 1;

            return end * ((s + 1f) * value * value + 2f * value * ((s + 1f) * value + s));
        }

        public static float EaseInOutBackD(float start, float end, float value)
        {
            float s = 1.70158f;
            end -= start;
            value /= .5f;

            if ((value) < 1)
            {
                s *= (1.525f);
                return 0.5f * end * (s + 1) * value * value + end * value * ((s + 1f) * value - s);
            }

            value -= 2;
            s *= (1.525f);
            return 0.5f * end * ((s + 1) * value * value + 2f * value * ((s + 1f) * value + s));
        }

        public static float EaseInElasticD(float start, float end, float value)
        {
            return EaseOutElasticD(start, end, 1f - value);
        }

        public static float EaseOutElasticD(float start, float end, float value)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p * 0.25f;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return (a * Mathf.PI * d * Mathf.Pow(2f, 1f - 10f * value) *
                Mathf.Cos((2f * Mathf.PI * (d * value - s)) / p)) / p - 5f * NATURAL_LOG_OF_2 * a *
                Mathf.Pow(2f, 1f - 10f * value) * Mathf.Sin((2f * Mathf.PI * (d * value - s)) / p);
        }

        public static float EaseInOutElasticD(float start, float end, float value)
        {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (value < 1)
            {
                value -= 1;

                return -5f * NATURAL_LOG_OF_2 * a * Mathf.Pow(2f, 10f * value) * Mathf.Sin(2 * Mathf.PI * (d * value - 2f) / p) -
                    a * Mathf.PI * d * Mathf.Pow(2f, 10f * value) * Mathf.Cos(2 * Mathf.PI * (d * value - s) / p) / p;
            }

            value -= 1;

            return a * Mathf.PI * d * Mathf.Cos(2f * Mathf.PI * (d * value - s) / p) / (p * Mathf.Pow(2f, 10f * value)) -
                5f * NATURAL_LOG_OF_2 * a * Mathf.Sin(2f * Mathf.PI * (d * value - s) / p) / (Mathf.Pow(2f, 10f * value));
        }

        public static float SpringD(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            end -= start;

            // Damn... Thanks http://www.derivative-calculator.net/
            // TODO: And it's a little bit wrong
            return end * (6f * (1f - value) / 5f + 1f) * (-2.2f * Mathf.Pow(1f - value, 1.2f) *
                Mathf.Sin(Mathf.PI * value * (2.5f * value * value * value + 0.2f)) + Mathf.Pow(1f - value, 2.2f) *
                (Mathf.PI * (2.5f * value * value * value + 0.2f) + 7.5f * Mathf.PI * value * value * value) *
                Mathf.Cos(Mathf.PI * value * (2.5f * value * value * value + 0.2f)) + 1f) -
                6f * end * (Mathf.Pow(1 - value, 2.2f) * Mathf.Sin(Mathf.PI * value * (2.5f * value * value * value + 0.2f)) + value
                / 5f);

        }

        public delegate float Function(float s, float e, float v);

        /// <summary>
        /// Returns the function associated to the easingFunction enum. This value returned should be cached as it allocates memory
        /// to return.
        /// </summary>
        /// <param name="easingFunction">The enum associated with the easing function.</param>
        /// <returns>The easing function</returns>
        public static Function GetEasingFunction(Ease easingFunction)
        {
            if (easingFunction == Ease.EaseInQuad)
            {
                return EaseInQuad;
            }

            if (easingFunction == Ease.EaseOutQuad)
            {
                return EaseOutQuad;
            }

            if (easingFunction == Ease.EaseInOutQuad)
            {
                return EaseInOutQuad;
            }

            if (easingFunction == Ease.EaseInCubic)
            {
                return EaseInCubic;
            }

            if (easingFunction == Ease.EaseOutCubic)
            {
                return EaseOutCubic;
            }

            if (easingFunction == Ease.EaseInOutCubic)
            {
                return EaseInOutCubic;
            }

            if (easingFunction == Ease.EaseInQuart)
            {
                return EaseInQuart;
            }

            if (easingFunction == Ease.EaseOutQuart)
            {
                return EaseOutQuart;
            }

            if (easingFunction == Ease.EaseInOutQuart)
            {
                return EaseInOutQuart;
            }

            if (easingFunction == Ease.EaseInQuint)
            {
                return EaseInQuint;
            }

            if (easingFunction == Ease.EaseOutQuint)
            {
                return EaseOutQuint;
            }

            if (easingFunction == Ease.EaseInOutQuint)
            {
                return EaseInOutQuint;
            }

            if (easingFunction == Ease.EaseInSine)
            {
                return EaseInSine;
            }

            if (easingFunction == Ease.EaseOutSine)
            {
                return EaseOutSine;
            }

            if (easingFunction == Ease.EaseInOutSine)
            {
                return EaseInOutSine;
            }

            if (easingFunction == Ease.EaseInExpo)
            {
                return EaseInExpo;
            }

            if (easingFunction == Ease.EaseOutExpo)
            {
                return EaseOutExpo;
            }

            if (easingFunction == Ease.EaseInOutExpo)
            {
                return EaseInOutExpo;
            }

            if (easingFunction == Ease.EaseInCirc)
            {
                return EaseInCirc;
            }

            if (easingFunction == Ease.EaseOutCirc)
            {
                return EaseOutCirc;
            }

            if (easingFunction == Ease.EaseInOutCirc)
            {
                return EaseInOutCirc;
            }

            if (easingFunction == Ease.Linear)
            {
                return Linear;
            }

            if (easingFunction == Ease.Spring)
            {
                return Spring;
            }

            if (easingFunction == Ease.EaseInBounce)
            {
                return EaseInBounce;
            }

            if (easingFunction == Ease.EaseOutBounce)
            {
                return EaseOutBounce;
            }

            if (easingFunction == Ease.EaseInOutBounce)
            {
                return EaseInOutBounce;
            }

            if (easingFunction == Ease.EaseInBack)
            {
                return EaseInBack;
            }

            if (easingFunction == Ease.EaseOutBack)
            {
                return EaseOutBack;
            }

            if (easingFunction == Ease.EaseInOutBack)
            {
                return EaseInOutBack;
            }

            if (easingFunction == Ease.EaseInElastic)
            {
                return EaseInElastic;
            }

            if (easingFunction == Ease.EaseOutElastic)
            {
                return EaseOutElastic;
            }

            if (easingFunction == Ease.EaseInOutElastic)
            {
                return EaseInOutElastic;
            }

            return null;
        }

        //Easy to manage this way
        public static Vector2 ValEase(Ease easingFunction, Vector2 a, Vector2 b, float value)
        {
            switch (easingFunction)
            {
                case Ease.EaseInQuad:
                    return EaseInQuad(ref a, ref b, ref value);
                case Ease.EaseOutQuad:
                    return EaseOutQuad(ref a, ref b, ref value);
                case Ease.EaseInOutQuad:
                    return EaseInOutQuad(ref a, ref b, ref value);
                case Ease.EaseInCubic:
                    return EaseInCubic(ref a, ref b, ref value);
                case Ease.EaseOutCubic:
                    return EaseOutCubic(ref a, ref b, ref value);
                case Ease.EaseInOutCubic:
                    return EaseInOutCubic(ref a, ref b, ref value);
                case Ease.EaseInQuart:
                    return EaseInQuart(ref a, ref b, ref value);
                case Ease.EaseOutQuart:
                    return EaseOutQuart(ref a, ref b, ref value);
                case Ease.EaseInOutQuart:
                    return EaseInOutQuart(ref a, ref b, ref value);
                case Ease.EaseInQuint:
                    return EaseInQuint(ref a, ref b, ref value);
                case Ease.EaseOutQuint:
                    return EaseOutQuint(ref a, ref b, ref value);
                case Ease.EaseInOutQuint:
                    return EaseInOutQuint(ref a, ref b, ref value);
                case Ease.EaseInSine:
                    return EaseInSine(ref a, ref b, ref value);
                case Ease.EaseOutSine:
                    return EaseOutSine(ref a, ref b, ref value);
                case Ease.EaseInOutSine:
                    return EaseInOutSine(ref a, ref b, ref value);
                case Ease.EaseInExpo:
                    return EaseInExpo(ref a, ref b, ref value);
                case Ease.EaseOutExpo:
                    return EaseOutExpo(ref a, ref b, ref value);
                case Ease.EaseInOutExpo:
                    return EaseInOutExpo(ref a, ref b, ref value);
                case Ease.EaseInCirc:
                    return EaseInCirc(ref a, ref b, ref value);
                case Ease.EaseOutCirc:
                    return EaseOutCirc(ref a, ref b, ref value);
                case Ease.EaseInOutCirc:
                    return EaseInOutCirc(ref a, ref b, ref value);
                case Ease.Linear:
                    return Linear(ref a, ref b, ref value);
                case Ease.Spring:
                    return Spring(ref a, ref b, ref value);
                case Ease.EaseInBounce:
                    return EaseInBounce(ref a, ref b, ref value);
                case Ease.EaseOutBounce:
                    return EaseOutBounce(ref a, ref b, ref value);
                case Ease.EaseInOutBounce:
                    return EaseInOutBounce(ref a, ref b, ref value);
                case Ease.EaseInBack:
                    return EaseInBack(ref a, ref b, ref value);
                case Ease.EaseOutBack:
                    return EaseOutBack(ref a, ref b, ref value);
                case Ease.EaseInOutBack:
                    return EaseInOutBack(ref a, ref b, ref value);
                case Ease.EaseInElastic:
                    return EaseInElastic(ref a, ref b, ref value);
                case Ease.EaseOutElastic:
                    return EaseOutElastic(ref a, ref b, ref value);
                case Ease.EaseInOutElastic:
                    return EaseInOutElastic(ref a, ref b, ref value);
            }
            return Vector3.zero;
        }
        public static Vector3 ValEase(Ease easingFunction, Vector3 a, Vector3 b, float value)
        {
            switch (easingFunction)
            {
                case Ease.EaseInQuad:
                    return EaseInQuad(ref a, ref b, ref value);
                case Ease.EaseOutQuad:
                    return EaseOutQuad(ref a, ref b, ref value);
                case Ease.EaseInOutQuad:
                    return EaseInOutQuad(ref a, ref b, ref value);
                case Ease.EaseInCubic:
                    return EaseInCubic(ref a, ref b, ref value);
                case Ease.EaseOutCubic:
                    return EaseOutCubic(ref a, ref b, ref value);
                case Ease.EaseInOutCubic:
                    return EaseInOutCubic(ref a, ref b, ref value);
                case Ease.EaseInQuart:
                    return EaseInQuart(ref a, ref b, ref value);
                case Ease.EaseOutQuart:
                    return EaseOutQuart(ref a, ref b, ref value);
                case Ease.EaseInOutQuart:
                    return EaseInOutQuart(ref a, ref b, ref value);
                case Ease.EaseInQuint:
                    return EaseInQuint(ref a, ref b, ref value);
                case Ease.EaseOutQuint:
                    return EaseOutQuint(ref a, ref b, ref value);
                case Ease.EaseInOutQuint:
                    return EaseInOutQuint(ref a, ref b, ref value);
                case Ease.EaseInSine:
                    return EaseInSine(ref a, ref b, ref value);
                case Ease.EaseOutSine:
                    return EaseOutSine(ref a, ref b, ref value);
                case Ease.EaseInOutSine:
                    return EaseInOutSine(ref a, ref b, ref value);
                case Ease.EaseInExpo:
                    return EaseInExpo(ref a, ref b, ref value);
                case Ease.EaseOutExpo:
                    return EaseOutExpo(ref a, ref b, ref value);
                case Ease.EaseInOutExpo:
                    return EaseInOutExpo(ref a, ref b, ref value);
                case Ease.EaseInCirc:
                    return EaseInCirc(ref a, ref b, ref value);
                case Ease.EaseOutCirc:
                    return EaseOutCirc(ref a, ref b, ref value);
                case Ease.EaseInOutCirc:
                    return EaseInOutCirc(ref a, ref b, ref value);
                case Ease.Linear:
                    return Linear(ref a, ref b, ref value);
                case Ease.Spring:
                    return Spring(ref a, ref b, ref value);
                case Ease.EaseInBounce:
                    return EaseInBounce(ref a, ref b, ref value);
                case Ease.EaseOutBounce:
                    return EaseOutBounce(ref a, ref b, ref value);
                case Ease.EaseInOutBounce:
                    return EaseInOutBounce(ref a, ref b, ref value);
                case Ease.EaseInBack:
                    return EaseInBack(ref a, ref b, ref value);
                case Ease.EaseOutBack:
                    return EaseOutBack(ref a, ref b, ref value);
                case Ease.EaseInOutBack:
                    return EaseInOutBack(ref a, ref b, ref value);
                case Ease.EaseInElastic:
                    return EaseInElastic(ref a, ref b, ref value);
                case Ease.EaseOutElastic:
                    return EaseOutElastic(ref a, ref b, ref value);
                case Ease.EaseInOutElastic:
                    return EaseInOutElastic(ref a, ref b, ref value);
            }
            return Vector3.zero;
        }
        public static float ValEase(Ease easingFunction, float a, float b, float value)
        {
            switch (easingFunction)
            {
                case Ease.EaseInQuad:
                    return EaseInQuad(a, b, value);
                case Ease.EaseOutQuad:
                    return EaseOutQuad(a, b, value);
                case Ease.EaseInOutQuad:
                    return EaseInOutQuad(a, b, value);
                case Ease.EaseInCubic:
                    return EaseInCubic(a, b, value);
                case Ease.EaseOutCubic:
                    return EaseOutCubic(a, b, value);
                case Ease.EaseInOutCubic:
                    return EaseInOutCubic(a, b, value);
                case Ease.EaseInQuart:
                    return EaseInQuart(a, b, value);
                case Ease.EaseOutQuart:
                    return EaseOutQuart(a, b, value);
                case Ease.EaseInOutQuart:
                    return EaseInOutQuart(a, b, value);
                case Ease.EaseInQuint:
                    return EaseInQuint(a, b, value);
                case Ease.EaseOutQuint:
                    return EaseOutQuint(a, b, value);
                case Ease.EaseInOutQuint:
                    return EaseInOutQuint(a, b, value);
                case Ease.EaseInSine:
                    return EaseInSine(a, b, value);
                case Ease.EaseOutSine:
                    return EaseOutSine(a, b, value);
                case Ease.EaseInOutSine:
                    return EaseInOutSine(a, b, value);
                case Ease.EaseInExpo:
                    return EaseInExpo(a, b, value);
                case Ease.EaseOutExpo:
                    return EaseOutExpo(a, b, value);
                case Ease.EaseInOutExpo:
                    return EaseInOutExpo(a, b, value);
                case Ease.EaseInCirc:
                    return EaseInCirc(a, b, value);
                case Ease.EaseOutCirc:
                    return EaseOutCirc(a, b, value);
                case Ease.EaseInOutCirc:
                    return EaseInOutCirc(a, b, value);
                case Ease.Linear:
                    return Linear(a, b, value);
                case Ease.Spring:
                    return Spring(a, b, value);
                case Ease.EaseInBounce:
                    return EaseInBounce(a, b, value);
                case Ease.EaseOutBounce:
                    return EaseOutBounce(a, b, value);
                case Ease.EaseInOutBounce:
                    return EaseInOutBounce(a, b, value);
                case Ease.EaseInBack:
                    return EaseInBack(a, b, value);
                case Ease.EaseOutBack:
                    return EaseOutBack(a, b, value);
                case Ease.EaseInOutBack:
                    return EaseInOutBack(a, b, value);
                case Ease.EaseInElastic:
                    return EaseInElastic(a, b, value);
                case Ease.EaseOutElastic:
                    return EaseOutElastic(a, b, value);
                case Ease.EaseInOutElastic:
                    return EaseInOutElastic(a, b, value);
            }
            return 0;
        }
        /// <summary>
        /// Gets the derivative function of the appropriate easing function. If you use an easing function for position then this
        /// function can get you the speed at a given time (normalized).
        /// </summary>
        /// <param name="easingFunction"></param>
        /// <returns>The derivative function</returns>
        public static Function GetEasingFunctionDerivative(Ease easingFunction)
        {
            if (easingFunction == Ease.EaseInQuad)
            {
                return EaseInQuadD;
            }

            if (easingFunction == Ease.EaseOutQuad)
            {
                return EaseOutQuadD;
            }

            if (easingFunction == Ease.EaseInOutQuad)
            {
                return EaseInOutQuadD;
            }

            if (easingFunction == Ease.EaseInCubic)
            {
                return EaseInCubicD;
            }

            if (easingFunction == Ease.EaseOutCubic)
            {
                return EaseOutCubicD;
            }

            if (easingFunction == Ease.EaseInOutCubic)
            {
                return EaseInOutCubicD;
            }

            if (easingFunction == Ease.EaseInQuart)
            {
                return EaseInQuartD;
            }

            if (easingFunction == Ease.EaseOutQuart)
            {
                return EaseOutQuartD;
            }

            if (easingFunction == Ease.EaseInOutQuart)
            {
                return EaseInOutQuartD;
            }

            if (easingFunction == Ease.EaseInQuint)
            {
                return EaseInQuintD;
            }

            if (easingFunction == Ease.EaseOutQuint)
            {
                return EaseOutQuintD;
            }

            if (easingFunction == Ease.EaseInOutQuint)
            {
                return EaseInOutQuintD;
            }

            if (easingFunction == Ease.EaseInSine)
            {
                return EaseInSineD;
            }

            if (easingFunction == Ease.EaseOutSine)
            {
                return EaseOutSineD;
            }

            if (easingFunction == Ease.EaseInOutSine)
            {
                return EaseInOutSineD;
            }

            if (easingFunction == Ease.EaseInExpo)
            {
                return EaseInExpoD;
            }

            if (easingFunction == Ease.EaseOutExpo)
            {
                return EaseOutExpoD;
            }

            if (easingFunction == Ease.EaseInOutExpo)
            {
                return EaseInOutExpoD;
            }

            if (easingFunction == Ease.EaseInCirc)
            {
                return EaseInCircD;
            }

            if (easingFunction == Ease.EaseOutCirc)
            {
                return EaseOutCircD;
            }

            if (easingFunction == Ease.EaseInOutCirc)
            {
                return EaseInOutCircD;
            }

            if (easingFunction == Ease.Linear)
            {
                return LinearD;
            }

            if (easingFunction == Ease.Spring)
            {
                return SpringD;
            }

            if (easingFunction == Ease.EaseInBounce)
            {
                return EaseInBounceD;
            }

            if (easingFunction == Ease.EaseOutBounce)
            {
                return EaseOutBounceD;
            }

            if (easingFunction == Ease.EaseInOutBounce)
            {
                return EaseInOutBounceD;
            }

            if (easingFunction == Ease.EaseInBack)
            {
                return EaseInBackD;
            }

            if (easingFunction == Ease.EaseOutBack)
            {
                return EaseOutBackD;
            }

            if (easingFunction == Ease.EaseInOutBack)
            {
                return EaseInOutBackD;
            }

            if (easingFunction == Ease.EaseInElastic)
            {
                return EaseInElasticD;
            }

            if (easingFunction == Ease.EaseOutElastic)
            {
                return EaseOutElasticD;
            }

            if (easingFunction == Ease.EaseInOutElastic)
            {
                return EaseInOutElasticD;
            }

            return null;
        }
    }
}