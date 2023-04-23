using UnityEngine;
using UnityEngine.UIElements;


namespace UTK
{
    public class UTKGroup : VisualElement
    {
        public (UTKTitle title, VisualElement mainContainer) containers = (new UTKTitle(), new VisualElement());
        public UTKGroup(string title = "Group")
        {
            containers.title.Text(title);
            this.FlexGrow(1).RoundCorner(Utk.ScreenRatio(20)).Border(Utk.ScreenRatio(6), Utk.HexColor(Color.clear, "#0063b1")).Padding(Utk.ScreenRatio(3));
            this.AddChild(containers.title as VisualElement).AddChild(containers.mainContainer).FlexGrow(1);
            containers.mainContainer.PaddingTop(Utk.ScreenRatio(3)).Size(100, 100, true, true).FlexGrow(1);
            containers.title.containers.leftLine.Height(Utk.ScreenRatio(6)).BcgColor("#0063b1");
            containers.title.containers.rightLine.Height(Utk.ScreenRatio(6)).BcgColor("#0063b1");
        }
        public void AddElement(VisualElement visualElement)
        {
            containers.mainContainer.AddChild(visualElement);
        }
        public UTKGroup SetSharpEdges(bool state)
        {
            if(state)
                this.RoundCorner(0);
            else
                this.RoundCorner(Utk.ScreenRatio(20));

            return this;
        }
        public UTKGroup SetBorderStyle(float width, Color color)
        {
            this.Border(Utk.ScreenRatio(width), color);
            return this;
        } 
        public UTKGroup SetBackgroundColor(Color color)
        {
            this.BcgColor(color);
            return this;
        }
    }
}