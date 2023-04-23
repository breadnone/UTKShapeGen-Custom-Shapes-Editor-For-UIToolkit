using UnityEngine;
using UnityEngine.UIElements;

namespace UTK
{
    public class UTKTitle : VisualElement
    {
        public string value
        {
            get
            {
                return this.containers.label.text;
            }
            set
            {
                this.containers.label.text = value;
            }
        }
        public StyleColor lineColor 
        {
            get
            {
                return  this.containers.leftLine.style.backgroundColor;
            }
            set
            {
                this.containers.leftLine.BcgColor(value);
                this.containers.rightLine.BcgColor(value);
            }
        }
        public float lineWidth
        {
            get
            {
                return this.containers.leftLine.resolvedStyle.height;
            }
            set
            {
                this.containers.leftLine.Height(value);
                this.containers.rightLine.Height(value);
            }
        }
        public (VisualElement mainContainer, VisualElement leftLine, Label label, VisualElement rightLine) containers;
        public UTKTitle(string text = "Title")
        {
            this.FlexRow().FlexGrow(1);
            var dummy = new VisualElement();
            this.containers = (new VisualElement(), new VisualElement(), new Label(), new VisualElement());
            this.containers.mainContainer.FlexRow().Size(100, Utk.ScreenRatio(50), true, false).AlignItems(Align.Center);
            dummy.FlexGrow(1);

            containers.label.MarginLeft(2).MarginRight(2);
            containers.leftLine.Height(Utk.ScreenRatio(5)).BcgColor("#744da9").FlexGrow(1);
            containers.rightLine.Height(Utk.ScreenRatio(5)).BcgColor("#744da9").FlexGrow(1); 
            
            this.containers.mainContainer.AddChild(containers.leftLine).AddChild(containers.label).AddChild(containers.rightLine);
            this.AddChild(containers.mainContainer).AddChild(dummy);
            containers.label.Text(text);
        }
    }
}