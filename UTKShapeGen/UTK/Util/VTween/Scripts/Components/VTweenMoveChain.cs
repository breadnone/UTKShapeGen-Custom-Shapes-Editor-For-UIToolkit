using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Breadnone;
using UnityEngine.Events;

namespace Breadnone.Extension
{
    [System.Serializable]
    public class VChainProperty
    {
        public Transform point;
        public float time;
    }
    ///<summary>Moves object in sequence.</summary>
    public class VTweenMoveChain : MonoBehaviour
    {
        [field: SerializeField] public GameObject obj;
        [field: SerializeField] public int loopCount;
        [field: SerializeField] public List<VChainProperty> targetPoints { get; set; }
        [field: SerializeField] public bool setInactiveOnComplete { get; set; }
        [field: SerializeField] public Ease ease { get; set; }
        [field: SerializeField] public bool infiniteLoop{get;set;}
        [field: SerializeField] public bool playImmediately{get;set;} = true;
        [SerializeField] public UnityEvent onComplete;
        private Vector3 defPos;
        private VTweenQueue queue;
        private int cacheLoopCount;
        void Start()
        {
            cacheLoopCount = loopCount;

            if(playImmediately)
                Play();
        }
        public void Cancel(){queue.stop();}
        public void ResetPos(){obj.transform.position = defPos;}
        public void Play()
        {
            if (targetPoints.Count == 0)
                return;

            var t = VTween.queue;

            for (int i = 0; i < targetPoints.Count; i++)
            {
                if (targetPoints[i] == null)
                    continue;

                t.add(VTween.move(obj, targetPoints[i].point.position, targetPoints[i].time).setEase(ease));
            }

            t.setOnComplete(onComplete.Invoke).setOnComplete(()=>
            {
                if(infiniteLoop && !setInactiveOnComplete)
                {
                    ResetPos();
                    Play();
                }
                if(!infiniteLoop && loopCount > 0)
                {
                    loopCount--;
                    ResetPos();
                    Play();
                }
                if(setInactiveOnComplete && !infiniteLoop && loopCount == 0) 
                {
                    obj.SetActive(false);
                    loopCount = cacheLoopCount;
                }
            }).start();
        }
    }
}