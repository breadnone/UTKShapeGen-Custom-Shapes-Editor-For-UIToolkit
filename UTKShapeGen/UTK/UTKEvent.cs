/*
MIT License
Copyright 2023 Stevphanie Ricardo

Permission is hereby granted, free of charge, to any person obtaining a copy of this
software and associated documentation files (the "Software"), to deal in the Software
without restriction, including without limitation the rights to use, copy, modify,
merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEditor;
#endif

///<summary>All events</summary>
namespace UTK
{
    public static partial class Utk
    {
        public static T OnGeometryChanged<T>(this T visualElement, EventCallback<GeometryChangedEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<GeometryChangedEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<GeometryChangedEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnMouseDown<T>(this T visualElement, EventCallback<MouseDownEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<MouseDownEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<MouseDownEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnClickEvent<T>(this T visualElement, EventCallback<ClickEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<ClickEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<ClickEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnTooltipEvent<T>(this T visualElement, EventCallback<TooltipEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<TooltipEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<TooltipEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnMouseCapture<T>(this T visualElement, EventCallback<MouseCaptureEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<MouseCaptureEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<MouseCaptureEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnMouseCaptureOut<T>(this T visualElement, EventCallback<MouseCaptureOutEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<MouseCaptureOutEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<MouseCaptureOutEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnMouseEnter<T>(this T visualElement, EventCallback<MouseEnterEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<MouseEnterEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<MouseEnterEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnMouseExit<T>(this T visualElement, EventCallback<MouseLeaveEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<MouseLeaveEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<MouseLeaveEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnMouseMove<T>(this T visualElement, EventCallback<MouseMoveEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<MouseMoveEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<MouseMoveEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnMouseOut<T>(this T visualElement, EventCallback<MouseOutEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<MouseOutEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<MouseOutEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnMouseUp<T>(this T visualElement, EventCallback<MouseUpEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<MouseUpEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<MouseUpEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnMouseOver<T>(this T visualElement, EventCallback<MouseOverEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<MouseOverEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<MouseOverEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnKeyDown<T>(this T visualElement, EventCallback<KeyDownEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<KeyDownEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<KeyDownEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnKeyUp<T>(this T visualElement, EventCallback<KeyUpEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<KeyUpEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<KeyUpEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnFocusIn<T>(this T visualElement, EventCallback<FocusInEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<FocusInEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<FocusInEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnFocusOut<T>(this T visualElement, EventCallback<FocusOutEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<FocusOutEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<FocusOutEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnFocusEvent<T>(this T visualElement, EventCallback<FocusEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<FocusEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<FocusEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnInputEvent<T>(this T visualElement, EventCallback<InputEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : TextField
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<InputEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<InputEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnBlurEvent<T>(this T visualElement, EventCallback<BlurEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<BlurEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<BlurEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnWheelEvent<T>(this T visualElement, EventCallback<WheelEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<WheelEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<WheelEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnCustomStyleResolved<T>(this T visualElement, EventCallback<CustomStyleResolvedEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<CustomStyleResolvedEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<CustomStyleResolvedEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnDragEnter<T>(this T visualElement, EventCallback<DragEnterEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<DragEnterEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<DragEnterEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnDragExit<T>(this T visualElement, EventCallback<DragExitedEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<DragExitedEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<DragExitedEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnDragPerformed<T>(this T visualElement, EventCallback<DragPerformEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<DragPerformEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<DragPerformEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnDragUpdated<T>(this T visualElement, EventCallback<DragUpdatedEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<DragUpdatedEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<DragUpdatedEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnDragLeave<T>(this T visualElement, EventCallback<DragLeaveEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<DragLeaveEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<DragLeaveEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnPointerUp<T>(this T visualElement, EventCallback<PointerUpEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<PointerUpEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<PointerUpEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnPointerCancel<T>(this T visualElement, EventCallback<PointerCancelEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<PointerCancelEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<PointerCancelEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnPointerCapture<T>(this T visualElement, EventCallback<PointerCaptureEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<PointerCaptureEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<PointerCaptureEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnPointerCaptureOut<T>(this T visualElement, EventCallback<PointerCaptureOutEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<PointerCaptureOutEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<PointerCaptureOutEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnPointerDown<T>(this T visualElement, EventCallback<PointerDownEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<PointerDownEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<PointerDownEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnPointerMove<T>(this T visualElement, EventCallback<PointerMoveEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<PointerMoveEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<PointerMoveEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnPointerOver<T>(this T visualElement, EventCallback<PointerOverEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<PointerOverEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<PointerOverEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnPointerOut<T>(this T visualElement, EventCallback<PointerOutEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<PointerOutEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<PointerOutEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnContextClicked<T>(this T visualElement, EventCallback<ContextClickEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<ContextClickEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<ContextClickEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnAttachedToPanel<T>(this T visualElement, EventCallback<AttachToPanelEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<AttachToPanelEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<AttachToPanelEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnTransitionRun<T>(this T visualElement, EventCallback<TransitionRunEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<TransitionRunEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<TransitionRunEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnTransitionStart<T>(this T visualElement, EventCallback<TransitionStartEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<TransitionStartEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<TransitionStartEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnTransitionEnd<T>(this T visualElement, EventCallback<TransitionEndEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<TransitionEndEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<TransitionEndEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnTransitionCancelled<T>(this T visualElement, EventCallback<TransitionCancelEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<TransitionCancelEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<TransitionCancelEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnNavigationMove<T>(this T visualElement, EventCallback<NavigationMoveEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<NavigationMoveEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<NavigationMoveEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnNavigationCancel<T>(this T visualElement, EventCallback<NavigationCancelEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<NavigationCancelEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<NavigationCancelEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnNavigationSubmit<T>(this T visualElement, EventCallback<NavigationSubmitEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<NavigationSubmitEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<NavigationSubmitEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnDetachedFromPanel<T>(this T visualElement, EventCallback<DetachFromPanelEvent> callback) where T : VisualElement
        {
            visualElement.RegisterCallback<DetachFromPanelEvent>(x=> 
            {
                callback.Invoke(x); 
                visualElement.UnregisterCallback<DetachFromPanelEvent>(callback);
            });
            return visualElement;
        }
        public static T OnMouseEnterWindow<T>(this T visualElement, EventCallback<MouseEnterWindowEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<MouseEnterWindowEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<MouseEnterWindowEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
        public static T OnPointerUp<T>(this T visualElement, EventCallback<MouseLeaveWindowEvent> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where T : VisualElement
        {
            visualElement.OnDetachedFromPanel(y => {visualElement.UnregisterCallback<MouseLeaveWindowEvent>(x => callback.Invoke(x), trickleDown);});
            visualElement.RegisterCallback<MouseLeaveWindowEvent>((x)=> callback.Invoke(x), trickleDown);
            return visualElement;
        }
    }
}