using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements; 
using System;
 
namespace UTK
{
    public class UTKWinTool : EditorWindow
    {
        [MenuItem("UTK/Samples")]
        static void UTKSample()
        {
            var win = GetWindow<UTKWinTool>(typeof(UTKWinTool));
        }
        [MenuItem("UTK/Tests")]
        static void UTKTest()
        {
            
        }
        public VisualElement root;
        void OnEnable()
        {
            this.SetAntiAliasing(4);
            root = rootVisualElement;
            DrawContainer();
        }
        void OnDisable()
        {

        }
        private void DrawContainer()
        {
            var rt = new VisualElement().Size(100, 100, true, true).FlexRow().FlexGrow(1);
            var left = new VisualElement().Size(50, 100, true, true);
            var right = new VisualElement().Size(50, 100, true, true);

            var topLeftPane = new VisualElement().Size(100, 100, true, true).Padding(2, true);
            var bottomLeftPane = new VisualElement().Size(100, 100, true, true).Padding(2, true);

            var topRightPane = new VisualElement().Size(100, 100, true, true).Padding(2, true);
            var bottomRightPane = new VisualElement().Size(100, 100, true, true).Padding(2, true);

            left.AddChild(topLeftPane).AddChild(bottomLeftPane);
            right.AddChild(topRightPane).AddChild(bottomRightPane);

            rt.AddChild(left).AddChild(right);
            root.Add(rt);

            //UTKTab sample
            var tabcon = new UTKTab(new List<string>{"Foldout", "Toggle", "Accordion", "Inline"});
            topLeftPane.AddChild(tabcon);

            var scr = new ScrollView();
            scr.mode = ScrollViewMode.Vertical;
            scr.horizontalScrollerVisibility = ScrollerVisibility.Hidden;

            //UTKFoldout
            var fold = new UTKFoldout("Foldout test 1");
            scr.Add(fold);

            var vis1 = new Label().BcgColor(Color.blue).Size(100, 30, true, false).Text("Test 1");
            var vis2 = new Label().BcgColor(Color.green).Size(100, 30, true, false).Text("Test 2");
            var vis3 = new Label().BcgColor(Color.red).Size(100, 30, true, false).Text("Test 3");

            fold.AddToContainer(vis1);
            fold.AddToContainer(vis2);
            fold.AddToContainer(vis3);

            var fold2 = new UTKFoldout("Foldout test 2");
            scr.Add(fold2);

            var vis11 = new Label().BcgColor(Color.blue).Size(100, 30, true, false).Text("Test 3");
            var vis22 = new Label().BcgColor(Color.green).Size(100, 30, true, false).Text("Test 4");
            var vis33 = new Label().BcgColor(Color.red).Size(100, 30, true, false).Text("Test 5");

            //Inser to container
            fold2.AddToContainer(vis11);
            fold2.AddToContainer(vis22);
            fold2.AddToContainer(vis33);

            var fold3 = new UTKFoldoutSlim("Foldout slim 3");
            scr.Add(fold3);

            var vis111 = new Label().BcgColor(Color.blue).Size(100, 30, true, false).Text("Test 3");
            var vis222 = new Label().BcgColor(Color.green).Size(100, 30, true, false).Text("Test 4");
            var vis333 = new Label().BcgColor(Color.red).Size(100, 30, true, false).Text("Test 5");

            var fold4 = new UTKFoldoutSlim("Foldout slim 4");
            scr.Add(fold4);

            var vis1111 = new Label().BcgColor(Color.blue).Size(100, 30, true, false).Text("Test 3");
            var vis2222 = new Label().BcgColor(Color.green).Size(100, 30, true, false).Text("Test 4");
            var vis3333 = new Label().BcgColor(Color.red).Size(100, 30, true, false).Text("Test 5");

            //Inser to container
            fold3.AddToContainer(vis111);
            fold3.AddToContainer(vis222);
            fold3.AddToContainer(vis333);

            //Inser to container
            fold4.AddToContainer(vis1111);
            fold4.AddToContainer(vis2222);
            fold4.AddToContainer(vis3333);
            tabcon.InsertToTab(1, scr);

            //UTKCarousel
            var clis = new List<string>{"Test", "Test", "Test", "Test", "Test", "Test", "Test", "Test", "Test", "Test"};

            var cmakeItem = new Func<VisualElement>(() =>
            {
                return new Label().Size(100, 100).BcgColor(Color.red).Color(Color.white).TextAlignment(TextAnchor.MiddleCenter);
            });

            var cbindItem = new Action<VisualElement, int>((element, i)=>
            {
                var visualElement = element as Label;
                visualElement.text = "Test " + i;
            });

            //Inline carousel
            var utcar = new UTKCarouselInline(clis, cmakeItem, cbindItem);
            tabcon.InsertToTab(4, utcar);

            //Simple accordion
            var accr = new UTKFlexible();
            accr.AddElement(new Label().Text("Test").Size(100, 100));
            accr.AddElement(new Label().Text("Test").Size(100, 100));
            accr.AddElement(new Label().Text("Test").Size(100, 100));
            tabcon.InsertToTab(3, accr);

            var accrA = new UTKAccordion().Top(10);
            accrA.AddElement(new Label().Text("Test").Size(100, 100));
            accrA.AddElement(new Label().Text("Test").Size(100, 100));
            accrA.AddElement(new Label().Text("Test").Size(100, 100));
            accrA.AddElement(new Label().Text("Test").Size(100, 100));
            accrA.AddElement(new Label().Text("Test").Size(100, 100));
            tabcon.InsertToTab(3, accrA);

            var uttog1 = new UTKToggle();
            tabcon.InsertToTab(2, uttog1);

            var uttog2 = new UTKToggle();
            tabcon.InsertToTab(2, uttog2);

            var uttog4 = new UTKRoundToggle();
            tabcon.InsertToTab(2, uttog4);

            var uttog5 = new UTKRoundToggle();
            tabcon.InsertToTab(2, uttog5); 

            var stog1 = new UTKRoundSlimToggle();
            tabcon.InsertToTab(2, stog1);

            var stog2 = new UTKRoundSlimToggle();
            tabcon.InsertToTab(2, stog2);

            //Side tab
            var tabVert = new UTKTab(new List<string>{"Title", "Browser", "AssetBrowse"}, true);
            var title1 = new UTKTitle("Title 1");
            var title2 = new UTKTitle("Title 2");
            var title3 = new UTKTitle("Title 3");
            var title4 = new UTKTitle("Title 4");
            tabVert.InsertToTab(1, title1);
            tabVert.InsertToTab(1, title2);
            tabVert.InsertToTab(1, title3);
            tabVert.InsertToTab(1, title4);
            bottomLeftPane.AddChild(tabVert);

            var imgl = new UTKImage();
            tabVert.InsertToTab(2, imgl);
            var imgl1 = new UTKImage();
            tabVert.InsertToTab(2, imgl1);
            var imgl2 = new UTKImage();
            tabVert.InsertToTab(2, imgl2);

            var btnOpen = new Button().Size(100, Utk.ScreenRatio(50), true, false);
            btnOpen.text = "AssetBrowse";
            tabVert.InsertToTab(3, btnOpen);

            btnOpen.clicked +=()=>
            {
                var browserAsset = new UTKBrowseOverlay(typeof(Sprite), "Browse");

                ///NOTE : Must use the rootVisualElement/ancestor to show it on top of everything.
                this.rootVisualElement.AddChild(browserAsset);
            };

            var spacedTab = new UTKSpacedTab(new List<string>{"SpacedTab", "DropDown", "Slider", "Menu"});
            topRightPane.AddChild(spacedTab);

            var spacedTab2 = new UTKSpacedTab(new List<string>{"ProgressBar", "Template", "Group", "FieldPair"});
            bottomRightPane.AddChild(spacedTab2);

            var dvis = new VisualElement().FlexRow().FlexGrow(1).Position(Position.Absolute).Top(Utk.ScreenRatio(30)).Left(Utk.ScreenRatio(30));

            var group1 = new UTKGroup();
            var gtxt = new UTKDropDown();
            gtxt.AddMenu("Menu 1", ()=>{Debug.Log("Square Corner");});
            gtxt.AddMenu("Menu 2", ()=>{Debug.Log("Square Corner");});
            gtxt.AddMenu("Menu 3", ()=>{Debug.Log("Square Corner");});
            group1.AddElement(gtxt);
            spacedTab2.InsertToTab(3, group1);

            var drop = new UTKDropDown("DropDown");
            drop.AddMenu("Menu 1", ()=>{Debug.Log("Square Corner");});
            drop.AddMenu("Menu 2", ()=>{Debug.Log("Square Corner");});
            drop.AddMenu("Menu 3", ()=>{Debug.Log("Square Corner");});
            drop.MarginRight(5);

            var dropRound = new UTKDropDown("MyDropDown", roundCorner:true);
            dropRound.AddMenu("Menu 1", ()=> {Debug.Log("Round Corner 1");});
            dropRound.AddMenu("Menu 2", ()=> {Debug.Log("Round Corner 2");});
            dropRound.AddMenu("Menu 3", ()=>{Debug.Log("Round Corner 3");});

            dvis.AddChild(drop).AddChild(dropRound);
            spacedTab.InsertToTab(2, dvis);

            var normalSlider = new UTKSlider().Top(10);
            spacedTab.InsertToTab(3, normalSlider);

            var floatingSlider = new UTKSliderValue().Top(40);
            spacedTab.InsertToTab(3, floatingSlider);
            
            var fillSlider = new UTKSliderFill().Top(50);
            spacedTab.InsertToTab(3, fillSlider); 

            var sliderBold = new UTKSliderFillBold().Top(60);
            spacedTab.InsertToTab(3, sliderBold);

            var menuToolbar = new UTKMenuToolbar().Top(Utk.ScreenRatio(80)); 
            
            var menuplain = new UTKDropDownMenu().Size(100, 60);
            menuplain.AddMenu("Menu 1", ()=>{Debug.Log("Menu 1");});
            menuplain.AddMenu("Menu 2", ()=>{Debug.Log("Menu 2");});
            menuplain.AddMenu("Menu 3", ()=>{Debug.Log("Menu 3");});

            var menuplain1 = new UTKDropDownMenu().Size(100, 60);
            menuplain1.AddMenu("Menu 1", ()=>{Debug.Log("Menu 1");});
            menuplain1.AddMenu("Menu 2", ()=>{Debug.Log("Menu 2");});
            menuplain1.AddMenu("Menu 3", ()=>{Debug.Log("Menu 3");});

            var menuplain2 = new UTKDropDownMenu().Size(100, 60);
            menuplain2.AddMenu("Menu 1", ()=>{Debug.Log("Menu 1");});
            menuplain2.AddMenu("Menu 2", ()=>{Debug.Log("Menu 2");});
            menuplain2.AddMenu("Menu 3", ()=>{Debug.Log("Menu 3");});

            var menuplain3 = new UTKDropDownMenu().Size(100, 60);
            menuplain3.AddMenu("Menu 1", ()=>{Debug.Log("Menu 1");}); 
            menuplain3.AddMenu("Menu 2", ()=>{Debug.Log("Menu 2");});
            menuplain3.AddMenu("Menu 3", ()=>{Debug.Log("Menu 3");}); 

            menuToolbar.AddMenu(menuplain).AddMenu(menuplain1).AddMenu(menuplain2).AddMenu(menuplain3);
            spacedTab.InsertToTab(2, menuToolbar);
            menuToolbar.PlaceBehind(dvis);

            var mnContainer = new VisualElement().FlexRow();
            var gt = new UTKMenu("Menu Test");
            gt.AddMenus(new List<(string, Action)>{("Test1/A", ()=>{Debug.Log("Test1");}), ("Test3", ()=>{Debug.Log("Test3");}), ("Test4", ()=>{Debug.Log("Test1");}), ("Test1/A/RR", ()=>{Debug.Log("Test1");}), ("Test1/A/RR/QQ", ()=>{Debug.Log("Test1");}), ("Test1/B", ()=>{Debug.Log("Test1");}), ("Test1/C", ()=>{Debug.Log("Test1");}), ("Test2/te2", ()=>{Debug.Log("Test 1");}), ("Test2/te1", ()=>{Debug.Log("Test 1");}), ("Test2/te4", ()=>{Debug.Log("Test 1");}), ("Test2/te4/t2", ()=>{Debug.Log("Test 1");})});
            mnContainer.AddChild(gt);

            var gt1 = new UTKMenu("Menu Test");
            gt1.AddMenus(new List<(string, Action)>{("Test1/A", ()=>{Debug.Log("Test1");}), ("Test3", ()=>{Debug.Log("Test3");}), ("Test4", ()=>{Debug.Log("Test1");}), ("Test1/A/RR", ()=>{Debug.Log("Test1");}), ("Test1/A/RR/QQ", ()=>{Debug.Log("Test1");}), ("Test1/B", ()=>{Debug.Log("Test1");}), ("Test1/C", ()=>{Debug.Log("Test1");}), ("Test2/te2", ()=>{Debug.Log("Test 1");}), ("Test2/te1", ()=>{Debug.Log("Test 1");}), ("Test2/te4", ()=>{Debug.Log("Test 1");}), ("Test2/te4/t2", ()=>{Debug.Log("Test 1");})});
            mnContainer.AddChild(gt1);

            var gt2 = new UTKMenu("Menu Test");
            gt2.AddMenus(new List<(string, Action)>{("Test1/A", ()=>{Debug.Log("Test1");}), ("Test3", ()=>{Debug.Log("Test3");}), ("Test4", ()=>{Debug.Log("Test1");}), ("Test1/A/RR", ()=>{Debug.Log("Test1");}), ("Test1/A/RR/QQ", ()=>{Debug.Log("Test1");}), ("Test1/B", ()=>{Debug.Log("Test1");}), ("Test1/C", ()=>{Debug.Log("Test1");}), ("Test2/te2", ()=>{Debug.Log("Test 1");}), ("Test2/te1", ()=>{Debug.Log("Test 1");}), ("Test2/te4", ()=>{Debug.Log("Test 1");}), ("Test2/te4/t2", ()=>{Debug.Log("Test 1");})});
            mnContainer.AddChild(gt2);

            var mn2Container = new UTKMenuContainer().Center(true);
            var mn2 = new UTKMenuSideBorder("Menu").SetRightBorderStyle(Utk.ScreenRatio(4), Utk.HexColor(Color.clear, "#8e8cd8"));
            mn2.AddMenus(new List<(string, Action)>{("Test1/A", ()=>{Debug.Log("Test1");}), ("Test3", ()=>{Debug.Log("Test3");}), ("Test4", ()=>{Debug.Log("Test1");}), ("Test1/A/RR", ()=>{Debug.Log("Test1");}), ("Test1/A/RR/QQ", ()=>{Debug.Log("Test1");}), ("Test1/B", ()=>{Debug.Log("Test1");}), ("Test1/C", ()=>{Debug.Log("Test1");}), ("Test2/te2", ()=>{Debug.Log("Test 1");}), ("Test2/te1", ()=>{Debug.Log("Test 1");}), ("Test2/te4", ()=>{Debug.Log("Test 1");}), ("Test2/te4/t2", ()=>{Debug.Log("Test 1");})});
            mn2Container.AddMenuComponent(mn2 as VisualElement);

            var mn3 = new UTKMenuSideBorder("Menu").SetRightBorderStyle(Utk.ScreenRatio(4), Utk.HexColor(Color.clear, "#8e8cd8"));
            mn3.AddMenus(new List<(string, Action)>{("Test1/A", ()=>{Debug.Log("Test1");}), ("Test3", ()=>{Debug.Log("Test3");}), ("Test4", ()=>{Debug.Log("Test1");}), ("Test1/A/RR", ()=>{Debug.Log("Test1");}), ("Test1/A/RR/QQ", ()=>{Debug.Log("Test1");}), ("Test1/B", ()=>{Debug.Log("Test1");}), ("Test1/C", ()=>{Debug.Log("Test1");}), ("Test2/te2", ()=>{Debug.Log("Test 1");}), ("Test2/te1", ()=>{Debug.Log("Test 1");}), ("Test2/te4", ()=>{Debug.Log("Test 1");}), ("Test2/te4/t2", ()=>{Debug.Log("Test 1");})});
            mn2Container.AddMenuComponent(mn3 as VisualElement);

            var mn4 = new UTKMenuSideBorder("Menu", rightBorder: false);
            mn4.AddMenus(new List<(string, Action)>{("Test1/A", ()=>{Debug.Log("Test1");}), ("Test3", ()=>{Debug.Log("Test3");}), ("Test4", ()=>{Debug.Log("Test1");}), ("Test1/A/RR", ()=>{Debug.Log("Test1");}), ("Test1/A/RR/QQ", ()=>{Debug.Log("Test1");}), ("Test1/B", ()=>{Debug.Log("Test1");}), ("Test1/C", ()=>{Debug.Log("Test1");}), ("Test2/te2", ()=>{Debug.Log("Test 1");}), ("Test2/te1", ()=>{Debug.Log("Test 1");}), ("Test2/te4", ()=>{Debug.Log("Test 1");}), ("Test2/te4/t2", ()=>{Debug.Log("Test 1");})});
            mn2Container.AddMenuComponent(mn4 as VisualElement);
            mn2Container.SetMenuColor(Utk.HexColor(Color.clear, "#744da9"));

            ///Open close logic
            gt.onExpandChanged += x=>
            {
                if(x)
                {
                    gt1.CloseMenu();
                    gt2.CloseMenu();
                }
            };
            gt1.onExpandChanged += x=>
            {
                if(x)
                {
                    gt.CloseMenu();
                    gt2.CloseMenu();
                }
            };
            gt2.onExpandChanged += x=>
            {
                if(x)
                {
                    gt.CloseMenu();
                    gt1.CloseMenu();
                }
            };
            ////

            spacedTab.InsertToTab(4, mn2Container);
            spacedTab.InsertToTab(4, mnContainer);
            var vvt = new VisualElement().Size(100, 60, false, false).Top(250).FlexRow();
            var btnNext = new Button().Size(50, 30).Text("Next");
            var btnPrev = new Button().Size(50, 30).Text("Prev");
            var btnMoveBack = new Button().Size(50, 30).Text("Back");
            var btnSelect = new Button().Size(50, 30).Text("Select");
            
            btnSelect.clicked += ()=>
            {
                gt.Select();
            };

            btnNext.clicked += ()=>
            {
                gt.MoveNext();
            };

            btnPrev.clicked += ()=>
            {
                gt.MovePrevious();
            };

            btnMoveBack.clicked += ()=>
            {
                gt.MoveBack();
            };

            vvt.AddChild(btnNext).AddChild(btnPrev).AddChild(btnSelect).AddChild(btnMoveBack);
            spacedTab.InsertToTab(4, vvt);

            /// UTKTextField + Label 
            var txtF = new UTKTextField();
            spacedTab2.InsertToTab(2, txtF);

            var txtF1 = new UTKTextField().Top(10);
            spacedTab2.InsertToTab(2, txtF1);
            txtF1.label.BcgColor(Color.white).Color(Color.black);

            var valFloat = new UTKValueFloatField().Top(20);
            spacedTab2.InsertToTab(2, valFloat);

            var valint = new UTKValueIntField().Top(30);
            spacedTab2.InsertToTab(2, valint);

            var valVec3 = new UTKValueVector3Field().Top(40);
            spacedTab2.InsertToTab(2, valVec3);

            var txtPair = new UTKPairTextField().Top(10);
            spacedTab2.InsertToTab(4, txtPair);

            var intPair = new UTKPairFloatField().Top(20);
            spacedTab2.InsertToTab(4, intPair); 

            var vec3Pair = new UTKPairVector3Field().Top(30);
            spacedTab2.InsertToTab(4, vec3Pair);

            var vec2Pair = new UTKPairVector2Field().Top(40);
            spacedTab2.InsertToTab(4, vec2Pair);

            //Clear background vector3
            var vec3Clear = new UTKPairVector3Field().Top(50);
            vec3Clear.SetBackgroundColor(Color.clear);
            vec3Clear.TextColor(Color.white);
            spacedTab2.InsertToTab(4, vec3Clear);

            /// UTKDropDown + Label
            var sliderPair = new UTKPairDropDown("DropDownPair").Top(60);
            spacedTab2.InsertToTab(4, sliderPair);

            sliderPair.objectElement.AddMenu("Menu 1", ()=>{Debug.Log("Square Corner");});
            sliderPair.objectElement.AddMenu("Menu 2", ()=>{Debug.Log("Square Corner");});
            sliderPair.objectElement.AddMenu("Menu 3", ()=>{Debug.Log("Square Corner");});

            var progressBar = new UTKProgressBar(0f, 2f);
            var progressFlat = new UTKProgressBarFlat(0, 5f).Top(20);
            var progressLine = new UTKProgressBarLine(0, 10).Top(30);
            var progressMove = new UTKProgressBarFlexibleLine(0, 15).Top(50);

            progressFlat.duration = 1.5f;
            progressLine.duration = 3f;
            progressMove.duration = 5f;
            spacedTab2.InsertToTab(1, progressBar);
            spacedTab2.InsertToTab(1, progressFlat);
            spacedTab2.InsertToTab(1, progressLine);
            spacedTab2.InsertToTab(1, progressMove);

            var playCon = new VisualElement().FlexRow().Top(70);
            var btnPlay = new Button().Size(50, 30).Text("Play");
            var btnStop = new Button().Size(50, 30).Text("Stop");
            playCon.AddChild(btnPlay).AddChild(btnStop);
            spacedTab2.InsertToTab(1, playCon);

            btnPlay.clicked += ()=>
            {
                progressBar.Play();
                progressFlat.Play();
                progressLine.Play();
                progressMove.Play();
            };

            btnStop.clicked += ()=>
            {
                progressBar.Stop();
                progressFlat.Stop();
                progressLine.Stop();
                progressMove.Stop();
            };   
        }
    }
}