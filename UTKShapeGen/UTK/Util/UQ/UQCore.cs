using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

namespace ULite
{
    public static class UQCore
    {
        public static List<T> Select<T>(this T list, Predicate<T> predicate) where T: List<T>
        {
            List<T> tmp = new();

            for(int i = 0; i < list.Count; i++)
            {
                if(list[i] == null)
                    continue;
                    
                if(predicate(list[i]))
                    tmp.Add(list[i]);
            }
            return tmp;
        }
        public static T GetSingle<T>(this List<T> list, Predicate<T> predicate) where T: struct, IConvertible
        {
            if(list == null || list.Count == 0)
                return default;

            for(int i = 0; i < list.Count; i++)
            {
                if(predicate(list[i]))
                    return list[i];
            }
            return default;
        }
        public static T GetRandom<T>(this List<T> list, Predicate<T> predicate) where T: class
        {
            if(list == null || list.Count == 0)
                return null;
            
            int index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }
        public static List<T> Where<T>(this T list, Predicate<T> condition) where T: List<T>, IEnumerable, IEnumerable<T>
        {
            List<T> tmp = new();

            for(int i = 0; i < list.Count; i++)
            {
                if(list[i] == null)
                    continue;

                if(condition(list[i]))
                    tmp.Add(list[i]);
            }
            return tmp;
        }
        public static List<T> Then<T>(this T list, Predicate<T> condition) where T: List<T>
        {
            List<T> tmp = new();

            for(int i = 0; i < list.Count; i++)
            {
                if(list[i] == null)
                    continue;

                if(condition(list[i]))
                    tmp.Add(list[i]);
            }
            return tmp;
        }
        public static int GetIndex<T>(this T list, Predicate<T> condition) where T : List<T>
        {
            for(int i = 0; i < list.Count; i++)
            {
                if(list[i] == null)
                    continue;

                if(condition(list[i]))
                    return i;
            }

            return -1;
        }
        public static T SwapSingle<T>(this T list, int indexA, int indexB) where T : List<T>
        {
            var a = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = a; 
            return list;
        }
        public static T[] ToArray<T>(this T list) where T : List<T>, IEnumerable<T>, IEnumerable
        {
            T[] tmp = new T[list.Count];

            for(int i = 0; i < list.Count; i++)
            {
                if(list[i] == null)
                    continue;

                tmp[i] = list[i];
            } 

            return tmp;
        }

        public static List<T> ToList<T>(this T[] list) where T : IEnumerable, IEnumerable<T> 
        {
            List<T> tmp = new(list.Length);
            
            for(int i = 0; i < list.Length; i++)
            {
                if(list[i] == null)
                    continue;

                tmp.Add(list[i]);
            }
            return tmp;
        }
        public static List<T> ToList<T>(this T[] list, int t = 0) 
        {
            List<T> tmp = new(list.Length);
            
            for(int i = 0; i < list.Length; i++)
            {
                tmp.Add(list[i]);
            }
            return tmp;
        }
        public static List<T> ToList<T>(this IEnumerable<T> list, int t = 0) 
        {
            List<T> tmp = new();
            
            foreach(var i in  list)
            {
                tmp.Add(i);
            }
            return tmp;
        }
        public static T First<T>(this T list, Predicate<T> predicate) where T : List<T>
        {
            for(int i = 0; i < list.Count; i++)
            {
                if(list[i] == null)
                    continue;

                if(predicate(list[i]))
                    return list[i];
            }

            return null;
        }
        public static T Last<T>(this T list, Predicate<T> predicate) where T : List<T>
        {
            for (int i = list.Count; i --> 0; )
            {
                if(list[i] == null)
                    continue;

                if(predicate(list[i]))
                    return list[i];
            }

            return null;
        }
        private static System.Random rng = new System.Random();  
        ///<summary>Randomized List<T> with FisherYates shuffle algorithm.</summary>
        public static List<T> Shuffle<T>(this T list) where T : List<T>
        {
            int n = list.Count;  
            while (n > 1) 
            {  
                n--;  
                int k = rng.Next(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
            return list;
        }
        public static List<T> OfType<T, R>(this T list) where T : List<T> where R: Type
        {
            List<T> tmp = new();

            for(int i = 0; i < list.Count; i++)
            {
                if(list[i] is R)
                {
                    tmp.Add(list[i]);
                }
            }

            return tmp;
        }
        
        public static void ForLoop<T>(this T list, Action<T> callback) where T : List<T>
        {
            for(int i = 0; i < list.Count; i++)
            {
                callback(list[i]);
            }
        }
        public static void ForLoopBreak<T>(this T list, Func<T, bool> callback) where T : List<T>
        {
            for(int i = 0; i < list.Count; i++)
            {
                if(callback(list[i]))
                {
                    break;
                }
            }
        }
        public static void ForReverse<T>(this T list, Action<T> callback) where T : List<T>
        {
            for (int i = list.Count; i --> 0; )
            {
                callback(list[i]);
            }
        }
        public static void ForReverseBreak<T>(this T list, Func<T, bool> callback) where T : List<T>
        {
            for (int i = list.Count; i --> 0; )
            {
                if(callback(list[i]))
                {
                    break;
                }
            }
        }
        public static void ForRange<T>(this T list, int start, int end, Action<T> callback) where T : List<T>
        {
            for(int i = start; i < end; i++)
            {
                callback(list[i]);
            }
        }
        public static List<T> Combine<T>(this T list, T target) where T : List<T>
        {
            List<T> tmp = new();
            
            for(int i = 0; i < list.Count; i++)
            {
                if(list[i] == null)
                    continue;

                bool found = false;

                for(int j = 0; j < target.Count; j++)
                {
                    if(target[j] == null)
                        continue;

                    if(list[i] == target[j])
                        found = true;
                }

                if(!found)
                {
                    tmp.Add(list[i]);
                }
            }

            for(int i = 0; i < target.Count; i++)
            {
                if(target[i] == null)
                    continue;

                tmp.Add(target[i]);
            }

            return tmp;
        }
        public static int Count<T> (this IEnumerable<T> ienum) where T : IEnumerable<T>
        {
            int idx = 0;

            foreach(var i in ienum)
            {
                idx++;
            }
            return idx;
        }
        public static bool Any<T>(this T list, Predicate<T> predicate) where T : List<T>, IEnumerable<T>
        {
            foreach(var i in list)
            {
                if(predicate(i))
                    return true;
            }

            return false;
        }
        public static List<T> Repeat<T>(this T list, int loopCount, Action<int> callback) where T : List<T>, IEnumerable<T>
        {
            for(int i = 0; i < loopCount; i++)
            {
                callback(i);
            }

            return list;
        }
        public static List<T> AddFirst<T>(this T list, T value) where T : List<T>, IEnumerable<T>
        {
            list.Insert(0, value);
            return list;
        }
        public static List<T> AddLast<T>(this T list, T value) where T : List<T>, IEnumerable<T>
        {
            if(list.Count > 0)
                list.Insert(list.Count - 1, value);
            else
                list.Insert(0, list);
            return list;
        }
        public static void SwapValue<T>(this ref T source, ref T target) where T : struct, IConvertible
        {
            T a = source;
            source = target;
            target = a;
        }
    }
}