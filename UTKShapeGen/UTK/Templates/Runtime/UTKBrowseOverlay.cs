
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

///<summary>Asset browser overlay.</summary>
namespace UTK
{
    public class UTKBrowseOverlay : UnityEngine.UIElements.PopupWindow
    {
        public UTKBrowse utkBrowse { get; set; }
        private DragManipulator mouseEvent;
        public Type objectType
        {
            get{return utkBrowse.objectType;}
            set{utkBrowse.objectType = value;}
        }

        public UTKBrowseOverlay(Type type, string title, string okButtonText = "Ok", string cancelButtonText = "Close")
        {
            var utk = new UTKBrowse(type, title, okButtonText, cancelButtonText);
            this.AddChild(utk as VisualElement);

            this.Position(Position.Absolute).Size(Screen.currentResolution.width / 3f, Screen.currentResolution.height / 2f, false, false);
            utkBrowse = utk;

            RegisterManipulator(true).RoundCorner(10, true).schedule.Execute(() =>{this.AlignSelf(Align.Center);}); 
            utk.OnCancelPressed(() =>{this.RemoveFromHierarchy();});
            this.OnDetachedFromPanel(x =>{UnregisterManipulator();});
            utk.RegisterSelectorLogic(()=>{UnregisterManipulator();}, mouseDown: true);//mouse down
            utk.RegisterSelectorLogic(()=>{RegisterManipulator(true);}, mouseDown: false);//mouse up
        }
        private UTKBrowseOverlay RegisterManipulator(bool disregardParentLayout)
        {
            if(mouseEvent != null)
                this.RemoveManipulator(mouseEvent);

            mouseEvent = new DragManipulator(disregardParentLayout);
            this.AddManipulator(mouseEvent);
            return this;
        }
        private UTKBrowseOverlay UnregisterManipulator()
        {
            if(mouseEvent != null)
                this.RemoveManipulator(mouseEvent);
                
            return this;
        }
    }
}
#endif