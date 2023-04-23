using System.Collections.Generic;
using UnityEngine;
using Breadnone;
using Breadnone.Extension;
using UnityEngine.UIElements;
using System.Collections;

///<summary>VTween MonoBehavior move component.</summary>
public class VTweenMoveTo : MonoBehaviour
{
    [field: SerializeField] public VAxis axis = VAxis.XYZ;
    [field: SerializeField] public bool isGameObject { get; set; }
    [field: SerializeField] public GameObject objectToMove { get; set; }
    [field: SerializeField] public UIDocument visualElement { get; set; }
    [field: SerializeField] public UIDocument targetVisualElement { get; set; }
    [field: SerializeField] public float duration { get; set; } = 1f;
    [field: SerializeField] public float delay { get; set; }
    [field: SerializeField] public int loopCount{get;set;}
    [field: SerializeField] public Vector3 to { get; set; }
    [field: SerializeField] public Transform toTarget { get; set; }
    [field: SerializeField] public Ease ease { get; set; } = Ease.Linear;
    [field: SerializeField] public bool pingpong { get; set; }
    [field: SerializeField] public bool isLocal { get; set; }
    [field: SerializeField] public bool setActive { get; set; }
    [field: SerializeField] public bool unscaledTime { get; set; }
    private Vector3 defaultPos;
    void Awake()
    {
        if (objectToMove != null)
            defaultPos = objectToMove.transform.position;

        if (visualElement != null)
        {
            defaultPos = targetVisualElement.transform.position;
        }
    }
    public void StartTween()
    {
        if(delay > 0)
            StartCoroutine(StartDelay());
        else
            MoveToTargetObject();
    }
    private IEnumerator StartDelay()
    {
        if(!unscaledTime)
        yield return new WaitForSeconds(delay);
        else
        yield return new WaitForSecondsRealtime(delay);
        MoveToTargetObject();
    }
    public void MoveToTargetObject()
    {
        if (isGameObject)
        {
            if(axis == VAxis.XYZ)
            {
                if (toTarget == null)
                {
                    VTween.move(objectToMove, to, duration).setLoop(loopCount).setInactiveOnComplete(setActive).setEase(ease).setPingPong(pingpong).setUnscaledTime(unscaledTime);
                }
                else
                {
                    VTween.move(objectToMove, toTarget, duration).setLoop(loopCount).setInactiveOnComplete(setActive).setEase(ease).setPingPong(pingpong).setUnscaledTime(unscaledTime);
                }
            }
            else if(axis == VAxis.X)
            {
                if (toTarget == null)
                {
                    VTween.moveX(objectToMove, to.x, duration).setLoop(loopCount).setInactiveOnComplete(setActive).setEase(ease).setPingPong(pingpong).setUnscaledTime(unscaledTime);
                }
                else
                {
                    VTween.moveX(objectToMove, toTarget.transform.position.x, duration).setLoop(loopCount).setInactiveOnComplete(setActive).setEase(ease).setPingPong(pingpong).setUnscaledTime(unscaledTime);
                }
            }
            else if(axis == VAxis.Y)
            {
                if (toTarget == null)
                {
                    VTween.moveY(objectToMove, to.y, duration).setLoop(loopCount).setInactiveOnComplete(setActive).setEase(ease).setPingPong(pingpong).setUnscaledTime(unscaledTime);
                }
                else
                {
                    VTween.moveY(objectToMove, toTarget.transform.position.y, duration).setLoop(loopCount).setInactiveOnComplete(setActive).setEase(ease).setPingPong(pingpong).setUnscaledTime(unscaledTime);
                }
            }
            else if(axis == VAxis.Z)
            {
                if (toTarget == null)
                {
                    VTween.moveZ(objectToMove, to.z, duration).setLoop(loopCount).setInactiveOnComplete(setActive).setEase(ease).setPingPong(pingpong).setUnscaledTime(unscaledTime);
                }
                else
                {
                    VTween.moveZ(objectToMove, toTarget.transform.position.z, duration).setLoop(loopCount).setInactiveOnComplete(setActive).setEase(ease).setPingPong(pingpong).setUnscaledTime(unscaledTime);
                }
            }
        }
        else
        {
            if(axis == VAxis.XYZ)
            {
                if (targetVisualElement == null)
                {
                    VTween.move(visualElement.rootVisualElement, to, duration).setLoop(loopCount).setInactiveOnComplete(setActive).setEase(ease).setPingPong(pingpong).setUnscaledTime(unscaledTime);
                }
                else
                {
                    VTween.move(visualElement.rootVisualElement, targetVisualElement.rootVisualElement.transform.position, duration).setLoop(loopCount).setInactiveOnComplete(setActive).setUnscaledTime(unscaledTime).setEase(ease).setPingPong(pingpong);
                }
            }
            else if(axis == VAxis.X)
            {
                if (targetVisualElement == null)
                {
                    VTween.moveX(visualElement.rootVisualElement, to.x, duration).setLoop(loopCount).setInactiveOnComplete(setActive).setEase(ease).setPingPong(pingpong).setUnscaledTime(unscaledTime);
                }
                else
                {
                    VTween.moveX(visualElement.rootVisualElement, targetVisualElement.rootVisualElement.transform.position.x, duration).setLoop(loopCount).setInactiveOnComplete(setActive).setUnscaledTime(unscaledTime).setEase(ease).setPingPong(pingpong);
                }
            }
            else if(axis == VAxis.Y)
            {
                if (targetVisualElement == null)
                {
                    VTween.moveY(visualElement.rootVisualElement, to.y, duration).setLoop(loopCount).setInactiveOnComplete(setActive).setEase(ease).setPingPong(pingpong).setUnscaledTime(unscaledTime);
                }
                else
                {
                    VTween.moveY(visualElement.rootVisualElement, targetVisualElement.rootVisualElement.transform.position.y, duration).setLoop(loopCount).setInactiveOnComplete(setActive).setUnscaledTime(unscaledTime).setEase(ease).setPingPong(pingpong);
                }
            }
            else if(axis == VAxis.Z)
            {
                throw new VTweenException("UIElements doesn't support Z orde yet!");
            }
        }
    }
    public void SetToDefaultPosition()
    {
        objectToMove.transform.position = defaultPos;
    }
}
