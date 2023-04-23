using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace Exts
{
    public static class Ext
    {
        ///<summary>List<T> ext: Custom extension to iterate.</summary>
        public static List<T> GetRange<T>(this List<T> lis, int start, int length, Predicate<T> predicate) where T : List<T>
        {
            var list = new List<T>();

            for (int i = start; i < length; i++)
            {
                if (lis[i] == null)
                    continue;

                if (predicate(lis[i]))
                    list.Add(lis[i]);
            }
            return list;
        }
        ///<summary>List<T> ext: Custom extension to iterate.</summary>
        public static List<T> GetRange<T>(this List<T> lis, int start, int length) where T : List<T>
        {
            var list = new List<T>();

            for (int i = start; i < length; i++)
            {
                list.Add(lis[i]);
            }
            return list;
        }
        ///<summary>List<T> ext: Backward loops with predicate.</summary>
        public static List<T> ReverseLoop<T>(this List<T> lis, Action<T> callback) where T : List<T>
        {
            for (int i = lis.Count; i-- > 0;)
            {
                callback(lis[i]);
            }
            return lis;
        }
        ///<summary>List<T> ext: Regular for loop</summary>
        public static List<T> Loop<T>(this List<T> lis, Action<T> callback) where T : List<T>
        {
            for (int i = 0; i < lis.Count; i++)
            {
                callback(lis[i]);
            }
            return lis;
        }
        public static List<T> RemoveFirst<T>(this List<T> lis, Predicate<T> predicate) where T : List<T>
        {
            for (int i = lis.Count; i-- > 0;)
            {
                if (predicate(lis[i]))
                {
                    lis.RemoveAt(i);
                    return lis;
                }
            }
            return lis;
        }
        public static List<T> RemoveAll<T>(this List<T> lis, Predicate<T> predicate) where T : List<T>
        {
            for (int i = lis.Count; i-- > 0;)
            {
                if (predicate(lis[i]))
                    lis.RemoveAt(i);
            }
            return lis;
        }
        public static T FindFirst<T>(this List<T> lis, Predicate<T> predicate) where T : List<T>
        {
            for (int i = 0; i < lis.Count; i++)
            {
                if (predicate(lis[i]))
                    return lis[i];
            }
            return null;
        }
        public static List<T> FindLast<T>(this List<T> lis, Predicate<T> predicate) where T : List<T>
        {
            for (int i = lis.Count; i-- > 0;)
            {
                if (predicate(lis[i]))
                    return lis[i];
            }
            return null;
        }
        ///<summary>List<T> ext: Fisher yates randomizer.</summary>
        public static List<T> Randomize<T>(this List<T> lis) where T : List<T>
        {
            System.Random rng = new System.Random();
            int n = lis.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = lis[k];
                lis[k] = lis[n];
                lis[n] = value;
            }

            return lis;
        }
        public static List<T> Reverse<T>(this List<T> lis) where T : List<T>
        {
            List<T> tmp = new();

            for (int i = lis.Count; i-- > 0;)
            {
                tmp.Add(lis[i]);
            }
            return tmp;
        }
        ///<summary>Swaps List item.</summary>
        public static List<T> SwapItem<T>(this List<T> t, int a, int b) where T:List<T>
        {
            T source = t[a];
            T target = t[b];

            t[a] = target;
            t[b] = source;
            return t;
        }
        ///<summary>Value T ext: Swaps structs value.</summary>
        public static void Swap<T>(ref this T t, ref T target) where T:struct
        {
            T a = t;
            T b = target;
            t = b;
            target = a;
        }
        public static void Repeat(int repeatAmount, Action<int> callback)
        {
            for(int i = 0; i < repeatAmount; i++){callback(i);}
        }
    }
}