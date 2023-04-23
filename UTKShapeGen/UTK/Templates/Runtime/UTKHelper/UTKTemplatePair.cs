using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{
    public interface ITemplatePair<T> where T : VisualElement
    {
        public Label title{get;set;}
        public VisualElement rightContainer{get;set;} 
        public T objectElement{get;set;}
        public void Init(string text = "Title text")
        {
            (this as VisualElement).FlexRow();
            title = new Label().Size(30, 100, true, true).TextAlignment(TextAnchor.MiddleLeft).Text(text).PaddingLeft(2); 
            rightContainer = new VisualElement().FlexRow().AlignItems(Align.Center).Size(70, 100, true, true);
            (this as VisualElement).AddChild(title).AddChild(rightContainer);
        }
        public void InsertToContainer(VisualElement visualElement)
        {
            visualElement.Left(30, true);
            rightContainer.AddChild(visualElement);
        }
        ///<summary>Spacing in percent. 100 percent is the highest assignable value.</summary>
        public virtual void SetSpacing(float value)
        {
            title.Size(value, 100, true, true);
            rightContainer.Size(100 - value, 100, true, true);
        }
        public virtual void OnEnable()
        {

        }
        public virtual void OnDisable()
        {

        }
    }
}