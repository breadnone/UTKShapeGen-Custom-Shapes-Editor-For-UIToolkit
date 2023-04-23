using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{
    ///<summary>Tabbed VisualElement.</summary>
    public class UTKSpacedTab : VisualElement
    {
        public (VisualElement mainContainer, VisualElement header) containers;
        public Action<int> onTabChanged;
        public int selectedTab { get; set; } = 0;
        public List<(VisualElement tab, VisualElement content, int index, bool enableScroll, bool enable)> tabs;
        private List<string> _choices;
        private int _defaultTab;
        public int defaultTab
        {
            get { return _defaultTab; }
            set
            {
                _defaultTab = value;
                selectedTab = value;
                Construct();
            }
        }
        public UTKSpacedTab(List<string> tabs)
        {
            this.focusable = true;
            this.FlexGrow(1);
            containers = (new VisualElement(), new VisualElement());
            containers.header.Size(100, Utk.ScreenRatio(50), true, false).FlexRow().FlexShrink(0).JustifyContent(Justify.Center);
            containers.mainContainer.FlexGrow(1).RoundCorner(new Vector4(0, 0, 20, 20));

            if (tabs == null)
            {
                tabs = new List<string> { "Tab 1", "Tab 2", "Tab 3" };
            }

            this._choices = tabs;
            this.AddChild(containers.header).AddChild(containers.mainContainer);
            Construct(true);
        }
        private void Construct(bool init = false)
        {
            containers.mainContainer.Clear();
            containers.header.Clear();
            GenerateTabs();

            schedule.Execute(x =>
            {
                if (init)
                {
                    selectedTab = 0;
                    ShowTab(selectedTab);
                }
                else
                {
                    ShowTab(selectedTab);
                }
            });
        }
        private VisualElement sideTabRoot;
        private void GenerateTabs()
        {
            tabs = new();

            if (sideTabRoot != null)
            {
                sideTabRoot.Clear();
                sideTabRoot.RemoveFromHierarchy();
                sideTabRoot = null;
            }
            containers.header.Size(100, Utk.ScreenRatio(50), true, false).FlexRow().FlexShrink(0);

            if (!this.Contains(containers.mainContainer))
                this.AddChild(containers.mainContainer);

            float space = 2 * _choices.Count;

            for (int i = 0; i < _choices.Count; i++)
            {
                VisualElement vis = null;
                vis = new VisualElement().FlexGrow(1).BcgColor("#4c4a48").Padding(2);

                float width = 100 / (float)_choices.Count;
                float height = 100;

                var container = new VisualElement().FlexGrow(1).Size(width, height, true, true);
                var lbl = new Label().Text(_choices[i]).TextAlignment(TextAnchor.MiddleCenter).BcgColor("#4c4a48").FlexGrow(1).Size(100, 100, true, true).RoundCorner(new Vector4(5,5,0,0), false);
                container.Add(lbl);

                tabs.Add((container, vis, i, false, true));

                var tmp = tabs[tabs.Count - 1];
                this.containers.header.AddChild(tmp.tab);
                this.containers.mainContainer.AddChild(tmp.content);
                var idx = i;

                if (i != _choices.Count - 1)
                    tmp.tab.PaddingRight(1, true);

                tmp.content.RoundCorner(new Vector4(0, 0, 5, 5));

                tmp.tab.FlexGrow(1);
                tmp.tab.OnMouseDown((x) => { ShowTab(idx); });
            }
        }
        public UTKSpacedTab ShowTab(int index)
        {
            IterateTab(new Action<(VisualElement tab, VisualElement content, int index, bool scrollMode, bool enable)>(x =>
            {
                var tab = x;

                if (tab.index == index)
                {
                    if (selectedTab != tab.index)
                    {
                        onTabChanged?.Invoke(tab.index);
                    }

                    selectedTab = tab.index;
                    tab.tab.Opacity(1f).Display(DisplayStyle.Flex);
                    tab.content.Display(DisplayStyle.Flex);
                }
                else
                {
                    tab.tab.Opacity(0.5f);
                    tab.content.Display(DisplayStyle.None);
                }
            }));

            return this;
        }
        private void IterateTab(Action<(VisualElement tab, VisualElement content, int index, bool enableScroll, bool enable)> tabCallback)
        {
            for (int i = 0; i < tabs.Count; i++) { tabCallback(tabs[i]); }
        }

        private (VisualElement tab, VisualElement content, int index, bool scrollMode, bool enable) GetTab(int tabIndex)
        {
            if (tabs == null || tabs.Count == 0)
                return (null, null, 0, false, false);

            for (int i = 0; i < tabs.Count; i++)
            {
                if (tabs[i].index == tabIndex - 1)
                    return tabs[i];
            }

            return (null, null, 0, false, false);
        }
        ///<summary>Reconstruct the hierarchy and clear previous elements.</summary>
        public UTKSpacedTab Rebuild()
        {
            Construct();
            return this;
        }
        ///<summary>Select next tab.</summary>
        public UTKSpacedTab Next()
        {
            if (selectedTab != tabs.Count - 1)
            {
                ShowTab(selectedTab - 1);
            }
            return this;
        }
        ///<summary>Selects previous tab.</summary>
        public UTKSpacedTab Pervious()
        {
            if (selectedTab != tabs.Count - 1)
            {
                ShowTab(selectedTab - 1);
            }
            return this;
        }
        ///<summary>Jump to tab.</summary>
        public UTKSpacedTab JumpTo(int tabIndex)
        {
            tabIndex -= 1;

            if (tabIndex > -1 && tabIndex <= tabs.Count - 1)
            {
                ShowTab(tabIndex);
            }
            return this;
        }
        ///<summary>Inserts to tab container.</summary>
        public UTKSpacedTab InsertToTab(int tabIndex, VisualElement visualElement)
        {
            var tab = GetTab(tabIndex);
            tab.content.AddChild(visualElement);
            return this;
        }
    }
}