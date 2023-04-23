using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Breadnone;

namespace UTK
{
    public interface IAnim<T> where T : VisualElement
    {
        public List<(T VisualElement, int id)> animInstances {get;set;} 
        public int getId => UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        
        public void Play()
        {
            (this as T).schedule.Execute(()=>
            {
                
            });
        }
        public void PlayLater(float time)
        {
            (this as T).schedule.Execute(()=>
            {

            });
        }
        public void PlayDelayed(float time)
        {
            (this as T).schedule.Execute(()=>
            {

            });
        }
    }
}