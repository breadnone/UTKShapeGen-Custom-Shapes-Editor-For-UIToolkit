using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UTKShape;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UTK
{
    public class UTKShapeGen : EditorWindow
    {
        public UTKPropSettings prop;
        public UTKScriptableShapeGen instance;
        private VisualElement canvas;
        private List<VisualElement> containers = new();
        private VisualElement rightPanel;
        private ScrollView leftPanel;
        private List<VisualElement> roots = new();
        private VisualElement previewWindow;
        private Image previewImage;
        private VisualElement browserContainer;
        private List<(VisualElement, int index)> AllRoots = new();
        private VisualElement selectedDot;
        private List<UTKMesh> overlays = new();
        private (int row, int column) grids = (60, 50);
        private Func<Vector2> mouseEvent;

        [MenuItem("UTKShapeGen/UTKShapeGen")]
        static void WindowShapeGen()
        {
            var window = EditorWindow.GetWindow<UTKShapeGen>(typeof(UTKShapeGen));
        }

        private void DrawPreviewOverlay(bool show)
        {
            if (show)
            {
                if (browserContainer != null)
                    browserContainer.Clear();

                previewWindow = new VisualElement().Size(100, 100, true, true).FlexGrow(1).Position(Position.Absolute).BcgColor(Color.white);
                var img = new Image().Size(100, 100, true, true).FlexGrow(1);
                img.scaleMode = ScaleMode.ScaleToFit;
                previewWindow.AddChild(img);
                previewImage = img;
                rightPanel.AddChild(previewWindow);
                browserContainer.AddChild(DrawBrowse());
            }
            else
            {
                if (previewWindow != null)
                {
                    if (browserContainer != null)
                        browserContainer.Clear();

                    previewWindow.RemoveFromHierarchy();
                    previewWindow = null;
                    previewImage = null;
                }
            }
        }

        void OnEnable()
        {
            instance = UTKScriptableShapeGen.instance;

            if(mouseEvent == null)
            {
                rootVisualElement.OnPointerMove(x=>
                {
                    mouseEvent += ()=>
                    {
                        Debug.Log("Mouse position is : " + Event.current.mousePosition);
                        return x.localPosition;
                    };
                });
            }

            AssemblyReloadEvents.beforeAssemblyReload += () =>
            {
                if (overlays == null)
                    return;

                for (int i = 0; i < overlays.Count; i++)
                {
                    overlays[i].RemoveFromHierarchy();
                }
            };

            if (instance.settings == null)
            {
                instance.settings = new UTKPropSettings();
            }

            prop = instance.settings;

            if (String.IsNullOrEmpty(prop.id))
            {
                prop.id = UnityEngine.Random.Range(int.MinValue, int.MaxValue) + "-utk-" + this.GetHashCode();
            }

            canvas = new VisualElement().Size(100, 100, true, true).SetOverflow(Overflow.Hidden);

            containers = new();
            rightPanel = new VisualElement().Size(65, 100, true, true);
            leftPanel = new ScrollView().Size(30, 100, true, true).Padding(Utk.ScreenRatio(5));

            rootVisualElement.OnGeometryChanged(x =>
            {
                canvas.schedule.Execute(() =>
                {
                    if (prop.points.Count == 0)
                        return;

                    for (int i = 0; i < prop.points.Count; i++)
                    {
                        for (int j = 0; j < roots.Count; j++)
                        {
                            if (prop.points[i].root == (int)roots[j].userData)
                            {
                                var siz = roots[j].ElementAt(0).ChangeCoordinatesTo(canvas, roots[j].ElementAt(0).transform.position);
                                prop.points[i] = (siz, prop.points[i].root);
                            }

                        }
                    }

                    ReDraw(1, true);

                }).ExecuteLater(5);
            });

            DrawCanvas();
            DrawTab();
        }
        void OnDisable()
        {
            instance.SaveThis();
            Clear();
        }
        private List<VisualElement> mirrors = new();
        private List<(Vector3 point, int root, int order, Vector3 rootPair)> mirrorPoint = new();
        private void DrawMirror(bool state)
        {
            Clear();
            showMirror = state;

            if (state)
            {
                var mid = (int)40 / 2f;
                Vector2 defSiz = new Vector2(AllRoots[0].Item1.resolvedStyle.width, AllRoots[0].Item1.resolvedStyle.height);
                var dummy = AllRoots[0].Item1;
                int counter = 0;

                for (int i = 0; i < AllRoots.Count; i++)
                {
                    if (i % mid == 0 && counter == 0)
                    {
                        counter++;
                        var rect = AllRoots[i].Item1.worldBound.size;
                        var vis = new VisualElement().Size(defSiz.x / 2.3f, defSiz.y, false, false).Opacity(0.5f).BcgColor(Color.white).Position(Position.Absolute);
                        vis.transform.position = new Vector2(AllRoots[i].Item1.worldBound.position.x - (rect.x * 2f), AllRoots[i].Item1.worldBound.position.y - (rect.y * 2f));
                        canvas.AddChild(vis);
                        mirrors.Add(vis);
                        vis.PickingMode(PickingMode.Ignore);
                        continue;
                    }

                    if (counter != 2)
                    {
                        counter++;
                    }
                    else
                    {
                        counter = 0;
                    }
                }

                mirrorButton.Border(2, Color.magenta);
            }
            else
            {
                if (mirrors.Count == 0)
                    return;

                for (int i = 0; i < mirrors.Count; i++)
                {
                    mirrors[i].RemoveFromHierarchy();
                }

                mirrorPoint.Clear();
                mirrors.Clear();
            }
        }
        private VisualElement mirrorButton;

        private VisualElement DrawToolbar()
        {
            var vis = new VisualElement().Size(5, 100, true, true).BorderLeft(2, Color.black).BorderRight(2, Color.black);
            var btnLine = new Image().Size(100, Utk.ScreenRatio(70), true, false).BorderBottom(2, Color.black).Border(2, Color.white);
            var btnCurve = new Image().Size(100, Utk.ScreenRatio(70), true, false).BorderBottom(2, Color.black);
            var btnBezier = new Image().Size(100, Utk.ScreenRatio(70), true, false).BorderBottom(2, Color.black);
            var btnShowPoints = new Image().Size(100, Utk.ScreenRatio(70), true, false).BorderBottom(2, Color.black);
            var btnMirror = new Image().Size(100, Utk.ScreenRatio(70), true, false).BorderBottom(2, Color.black);

            //TODO: Hide this for now... both Quadratic and ArcTo are a buggy mess.  
            btnBezier.Display(DisplayStyle.None);
            mirrorButton = btnMirror;

            vis.AddChild(btnLine).AddChild(btnCurve).AddChild(btnBezier).AddChild(Separator().Size(100, Utk.ScreenRatio(120), true, false).BorderBottom(2, Color.black)).AddChild(btnShowPoints).AddChild(btnMirror);

            btnLine.sprite = Resources.Load<Sprite>("Straight-utk");
            btnCurve.sprite = Resources.Load<Sprite>("Curve-utk");
            btnBezier.sprite = Resources.Load<Sprite>("Arc-utk");
            btnShowPoints.sprite = Resources.Load<Sprite>("Dot-utk");
            btnMirror.sprite = Resources.Load<Sprite>("Mirror-utk");

            btnLine.scaleMode = ScaleMode.ScaleToFit;
            btnCurve.scaleMode = ScaleMode.ScaleToFit;
            btnBezier.scaleMode = ScaleMode.ScaleToFit;
            btnShowPoints.scaleMode = ScaleMode.ScaleToFit;
            btnMirror.scaleMode = ScaleMode.ScaleToFit;

            if (showPoint == true)
            {
                btnShowPoints.Border(2, Color.magenta);
            }
            else
            {
                btnShowPoints.Border(0, Color.black).BorderTop(2, Color.black);
            }

            if (showMirror == true)
            {
                btnMirror.Border(2, Color.magenta);
            }
            else
            {
                btnMirror.Border(0, Color.black).BorderBottom(2, Color.black).BorderTop(2, Color.black);
            }

            if (prop.drawType == UTKDrawType.Line)
            {
                btnLine.Border(2, Color.white);
                btnCurve.Border(0, Color.white).BorderBottom(2, Color.black);
                btnBezier.Border(0, Color.white).BorderBottom(2, Color.black);
            }
            else if (prop.drawType == UTKDrawType.Curve)
            {
                btnBezier.Border(0, Color.white).BorderBottom(2, Color.black);
                btnCurve.Border(2, Color.white);
                btnLine.Border(0, Color.white).BorderBottom(2, Color.black);
            }
            else if (prop.drawType == UTKDrawType.Bezier)
            {
                btnLine.Border(0, Color.white).BorderBottom(2, Color.black);
                btnCurve.Border(0, Color.white).BorderBottom(2, Color.black);
                btnBezier.Border(2, Color.white);
            }

            btnLine.OnMouseDown(x =>
            {
                btnBezier.Border(0, Color.white).BorderBottom(2, Color.black);
                btnCurve.Border(0, Color.white).BorderBottom(2, Color.black);
                btnLine.Border(2, Color.white);
                prop.drawType = UTKDrawType.Line;
                ReDraw(1, true);

                x.StopImmediatePropagation();
            });

            btnCurve.OnMouseDown(x =>
            {
                btnBezier.Border(0, Color.white).BorderBottom(2, Color.black);
                btnLine.Border(0, Color.white).BorderBottom(2, Color.black);
                btnCurve.Border(2, Color.white);
                prop.drawType = UTKDrawType.Curve;
                ReDraw(1, true);
                x.StopImmediatePropagation();
            });

            btnBezier.OnMouseDown(x =>
            {
                btnLine.Border(0, Color.white).BorderBottom(2, Color.black);
                btnCurve.Border(0, Color.white).BorderBottom(2, Color.black);
                btnBezier.Border(2, Color.white);
                prop.drawType = UTKDrawType.Bezier;
                ReDraw(1, true);
                x.StopImmediatePropagation();
            });

            btnShowPoints.OnMouseDown(x =>
            {
                if (!showPoint == true)
                {
                    btnShowPoints.Border(2, Color.magenta);
                }
                else
                {
                    btnShowPoints.Border(0, Color.black).BorderTop(2, Color.black).BorderBottom(2, Color.black);
                }

                ShowPoints(!showPoint);
                x.StopImmediatePropagation();
            });

            btnMirror.OnMouseDown(x =>
            {
                if (!showMirror == true)
                {
                    btnMirror.Border(2, Color.magenta);
                }
                else
                {
                    btnMirror.Border(0, Color.black).BorderTop(2, Color.black).BorderBottom(2, Color.black);
                }

                DrawMirror(!showMirror);
                x.StopImmediatePropagation();
            });
            
            return vis;
        }
        private bool showPoint = false;
        private bool showMirror = false;
        private bool mouseDown = false;
        private void IterateBlue(Action<VisualElement> visualElement)
        {
            var dots = canvas.Query("anchor").ToList();

            for (int i = 0; i < dots.Count; i++)
            {
                visualElement(dots[i]);
            }
        }

        private void ShowPoints(bool state)
        {
            showPoint = state;
            mouseDown = false;

            if (state)
            {
                for (int j = 0; j < prop.points.Count; j++)
                {
                    var img = new Image().Size(Utk.ScreenRatio(40), Utk.ScreenRatio(40), false, false).Position(Position.Absolute).Name("anchor").FlexShrink(0).JustifyContent(Justify.Center);
                    var dummy = new VisualElement().Size(0f, 0f, false, false).Name("dummy").AlignSelf(Align.Center);
                    img.AddChild(dummy);
                    img.sprite = Resources.Load<Sprite>("dot-blue-utk");
                    img.scaleMode = ScaleMode.ScaleToFit;
                    VisualElement child = roots[j].ElementAt(0);

                    Vector3 startPoint = Vector3.zero;
                    var index = j;
                    img.pickingMode = PickingMode.Position;
                    AssignContextMenu(img, (child.parent, (int)child.parent.userData));

                    img.OnPointerDown(x =>
                    {
                        if (mouseDown || x.button == 1)
                            return;

                        mouseDown = true;

                        if (selectedDot != null)
                            selectedDot.Border(0, Color.clear);

                        selectedDot = img;
                        img.Border(2, Color.green);
                        startPoint = x.localPosition;
                        x.StopImmediatePropagation();
                    });

                    img.OnPointerMove(x =>
                    {
                        if (!mouseDown)
                            return;

                        Vector3 diff = x.localPosition - startPoint;
                        img.style.top = img.layout.y + diff.y;
                        img.style.left = img.layout.x + diff.x;
                        x.StopImmediatePropagation();
                    });

                    img.OnPointerOut(x =>
                    {
                        if (mouseDown)
                        {
                            mouseDown = false;
                            RefreshPointPosition(dummy.worldBound.center, index, child);
                            x.target.ReleasePointer(x.pointerId);
                        }
                        else
                        {
                            return;
                        }
                        x.StopImmediatePropagation();
                    });

                    img.OnPointerUp(x =>
                    {
                        if (!mouseDown)
                            return;

                        mouseDown = false;
                        selectedDot.Border(0, Color.clear);
                        selectedDot = null;
                        RefreshPointPosition(dummy.worldBound.center, index, child);
                        x.StopImmediatePropagation();
                    });

                    canvas.AddChild(img);
                    var rect = child.worldBound.size;
                    img.transform.position = new Vector2(prop.points[j].point.x - (rect.x * 2f), prop.points[j].point.y - (rect.y * 2f));
                }
            }
            else
            {
                IterateBlue(x =>
                {
                    x.RemoveFromHierarchy();
                });
            }
        }

        private void ClearColorStates()
        {
            for (int i = 0; i < AllRoots.Count; i++)
            {
                AllRoots[i].Item1.BcgColor(Color.clear);
            }

            for (int z = 0; z < roots.Count; z++)
            {
                roots[z].BcgColor(Color.yellow);
            }
        }
        private void RefreshPointPosition(Vector2 pos, int index, VisualElement oldRoot)
        {
            for (int j = 0; j < AllRoots.Count; j++)
            {
                if (AllRoots[j].Item1.worldBound.Contains(pos))
                {
                    var siz = AllRoots[j].Item1.ElementAt(0).ChangeCoordinatesTo(canvas, AllRoots[j].Item1.ElementAt(0).transform.position);
                    AllRoots[j].Item1.BcgColor(Color.yellow);

                    var oldData = prop.points[index]; //Used by mirror mode only

                    prop.points[index] = (siz, AllRoots[j].Item2);
                    roots[index] = AllRoots[j].Item1;

                    if (showMirror)
                    {
                        var idx = AllRoots[j].Item2;
                        (int start, int end, int nextIndex) range = (0, 0, 0);
                        int tracking = grids.row;

                        for (int a = 0; a < grids.row * grids.column; a++)
                        {
                            if (a % grids.row == 0)
                            {
                                (int, int) num = (a, tracking);

                                if (Enumerable.Range(a, num.Item2).Contains(idx))
                                {
                                    int max = num.Item2;

                                    for (int b = num.Item1; b < num.Item2; b++)
                                    {
                                        if (b == idx)
                                        {
                                            break;
                                        }

                                        max--;
                                    }

                                    range = (num.Item1, num.Item2, max);
                                }

                                tracking += grids.row;
                            }
                        }

                        var getDistance = AllRoots.Find(x => x.index == range.nextIndex);
                        var sizSub = getDistance.Item1.ElementAt(0).ChangeCoordinatesTo(canvas, getDistance.Item1.ElementAt(0).transform.position);
                        var oldPos = oldRoot.ChangeCoordinatesTo(canvas, oldRoot.transform.position);

                        for (int i = 0; i < mirrorPoint.Count; i++)
                        {
                            if (new Vector2(mirrorPoint[i].rootPair.x, mirrorPoint[i].rootPair.y) == oldPos)
                            {
                                mirrorPoint[i] = (sizSub, getDistance.index, mirrorPoint[i].order, siz);
                            }

                            if (mirrorPoint[i].root == oldData.root)
                            {
                                mirrorPoint[i] = (siz, prop.points[index].root, mirrorPoint[i].order, sizSub);
                            }
                        }

                        roots.Clear();
                        prop.points.Clear();

                        for (int d = 0; d < mirrorPoint.Count; d++)
                        {
                            roots.Add(AllRoots[mirrorPoint[d].root].Item1);
                            prop.points.Add((mirrorPoint[d].point, mirrorPoint[d].root));
                        }
                    }

                    Draw();

                    canvas.schedule.Execute(() =>
                    {
                        if (showPoint)
                        {
                            bool tmp = showPoint;
                            ShowPoints(false);
                            ShowPoints(true);
                        }

                        instance.SaveThis();
                    });

                    ClearColorStates();
                    return;
                }
            }

        }
        private VisualElement Separator()
        {
            return new VisualElement().Size(100, Utk.ScreenRatio(25), true, false);
        }
        private void DrawTab()
        {
            var tb = new UTKTab(new List<string> { "Edit", "SaveAs", "Browse" });

            leftPanel.AddChild(tb as VisualElement);

            tb.onTabChanged += x =>
            {
                if (x == 2)
                {
                    DrawPreviewOverlay(true);
                }
                else
                {
                    DrawPreviewOverlay(false);
                }
            };

            var btnCon = new VisualElement().FlexRow().Size(100, Utk.ScreenRatio(50), true, false);
            var btnDrawLines = new Button().Size(20, 100, true, true).Text("D.Line").BcgColor(Color.clear).Name("btnLine");
            var btnFillLines = new Button().Size(20, 100, true, true).Text("Fill").BcgColor(Color.clear).Name("btnFill");
            var btnClear = new Button().Size(20, 100, true, true).Text("Clear").BcgColor(Color.clear);
            btnCon.AddChild(btnDrawLines).AddChild(btnFillLines).AddChild(btnClear);
            tb.InsertToTab(1, btnCon);

            var savAs = new VisualElement().FlexRow().Size(100, Utk.ScreenRatio(50), true, false);
            var txtPath = new TextField().Size(75, 100, true, true);
            txtPath.SetValueWithoutNotify("<EnterName>");
            var btnSave = new Button().Size(20, 100, true, true).Text("Export").BcgColor(Color.clear);
            savAs.AddChild(txtPath).AddChild(btnSave);
            tb.InsertToTab(2, savAs);
            browserContainer = new VisualElement();
            tb.InsertToTab(3, browserContainer);

            btnSave.clicked += () =>
            {
                if (String.IsNullOrEmpty(txtPath.value) || txtPath.value == "<EnterName>")
                    return;

                var vimg = ScriptableObject.CreateInstance<VectorImage>();
                AssetDatabase.CreateAsset(vimg, "Assets/UTKShapeGen/Resources/" + txtPath.value + ".asset");
                vimg.name = txtPath.value;
                prop.utkMesh.meshProperty.painter.SaveToVectorImage(vimg);
                AssetDatabase.SaveAssets();
            };

            if (prop.fill)
                btnFillLines.BcgColor(Color.blue);

            if (prop.line)
                btnDrawLines.BcgColor(Color.blue);

            btnDrawLines.clicked += () =>
            {
                prop.line = !prop.line;

                if (prop.line)
                {
                    btnDrawLines.BcgColor(Color.blue);
                }
                else
                {
                    btnDrawLines.BcgColor(Color.clear);
                }

                ReDraw(5, showPoint);
            };

            btnFillLines.clicked += () =>
            {
                prop.fill = !prop.fill;

                if (prop.fill)
                {
                    btnFillLines.BcgColor(Color.blue);
                }
                else
                {
                    btnFillLines.BcgColor(Color.clear);
                }

                ReDraw(1, showPoint);

                if (showPoint)
                {
                    ShowPoints(false);
                    canvas.schedule.Execute(() => ShowPoints(true)).StartingIn(10);
                }
            };

            btnClear.clicked += () =>
            {
                canvas.schedule.Execute(() => Clear());
            };

            //////
            var lineCon = new VisualElement().Size(100, Utk.ScreenRatio(50), true, false).FlexRow().AlignItems(Align.FlexStart);
            var fillCon = new VisualElement().Size(100, Utk.ScreenRatio(50), true, false).FlexRow().AlignItems(Align.FlexStart);
            var ruleCon = new VisualElement().Size(100, Utk.ScreenRatio(50), true, false).FlexRow().AlignItems(Align.FlexStart);
            var pathCon = new VisualElement().Size(100, Utk.ScreenRatio(50), true, false).FlexRow().AlignItems(Align.FlexStart);
            var capCon = new VisualElement().Size(100, Utk.ScreenRatio(50), true, false).FlexRow().AlignItems(Align.FlexStart);

            var lblLine = new Label().Text("Line Color").Size(40, 100, true, true).TextAlignment(TextAnchor.MiddleLeft);
            var lblfill = new Label().Text("Fill Color").Size(40, 100, true, true).TextAlignment(TextAnchor.MiddleLeft);
            var lineCol = new ColorField().Size(60, 100, true, true).Name("lineColor");
            var fillCol = new ColorField().Size(60, 100, true, false).Name("fillColor");
            fillCol.value = prop.fillColor;
            lineCol.value = prop.lineColor;

            var lblRule = new Label().Text("Fill Rule").Size(40, 100, true, true).TextAlignment(TextAnchor.MiddleLeft);
            var ruleCol = new DropdownField().Size(60, 100, true, true);
            ruleCon.AddChild(lblRule).AddChild(ruleCol);
            ruleCol.SetValueWithoutNotify("NonZero");
            ruleCol.choices = new List<string> { "OddEven", "NonZero" };

            var lblCap = new Label().Text("Line Cap").Size(40, 100, true, true).TextAlignment(TextAnchor.MiddleLeft);
            var capCol = new DropdownField().Size(60, 100, true, true);
            capCon.AddChild(lblCap).AddChild(capCol);
            capCol.SetValueWithoutNotify("Round");
            capCol.choices = Enum.GetNames(typeof(LineCap)).ToList();

            if(prop != null)
            capCol.SetValueWithoutNotify(prop.lineCap.ToString());

            capCol.OnValueChanged(x=>
            {
                if(x.newValue == "Butt")
                {
                    prop.lineCap = LineCap.Butt;
                }
                else if(x.newValue == "Round")
                {
                    prop.lineCap = LineCap.Round;
                }

                ReDraw(1, showPoint);
                instance.SaveThis();
            });

            var lblPath = new Label().Text("Close Path").Size(40, 100, true, true).TextAlignment(TextAnchor.MiddleLeft);
            var objPath = new DropdownField().Size(60, 100, true, true);
            objPath.choices = new List<string> { "Auto", "Disable" };
            pathCon.AddChild(lblPath).AddChild(objPath);
            objPath.SetValueWithoutNotify(prop.closePath ? "Auto" : "Disable");

            var layerParent = new VisualElement().Size(100, Utk.ScreenRatio(50 * 3));

            for (int i = 0; i < overlays.Count; i++)
            {
                var layers = new Label().Text("").Size(40, 100, true, true).TextAlignment(TextAnchor.MiddleLeft);
                var layerObj = new VisualElement().Size(60, 100, true, true);
                objPath.choices = new List<string> { "Auto", "Disable" };
                pathCon.AddChild(lblPath).AddChild(objPath);
                objPath.SetValueWithoutNotify(prop.closePath ? "Auto" : "Disable");
            }
            objPath.OnValueChanged(x =>
            {
                if(prop == null)
                    return;
                    
                if (x.newValue == "Auto")
                {
                    prop.closePath = true;
                    prop.utkMesh.meshProperty.closePath = true;
                }
                else
                {
                    prop.closePath = false;
                    prop.utkMesh.meshProperty.closePath = false;
                }

                ReDraw(1, showPoint);
                instance.SaveThis();
            });


            ruleCol.OnValueChanged(x =>
            {
                if (prop == null)
                    return;

                if (x.newValue == "OddEven")
                {
                    prop.fillRule = FillRule.OddEven;
                }
                else
                {
                    prop.fillRule = FillRule.NonZero;

                }

                ReDraw(1, showPoint);
                instance.SaveThis();
            });

            lineCon.AddChild(lblLine as VisualElement).AddChild(lineCol);
            fillCon.AddChild(lblfill as VisualElement).AddChild(fillCol);
            tb.InsertToTab(1, Separator()).InsertToTab(1, pathCon).InsertToTab(1, lineCon).InsertToTab(1, fillCon).InsertToTab(1, ruleCon);


            var dcon = new VisualElement().Size(100, Utk.ScreenRatio(50), true, false).FlexRow();
            var lbld = new Label().Text("Join Type").Size(40, 100, true, true).TextAlignment(TextAnchor.MiddleLeft);
            var dropLine = new DropdownField().Size(60, 100, true, true);
            dcon.AddChild(lbld).AddChild(dropLine);
            dropLine.choices = new List<string> { "Mitter", "Round", "Bevel" };
            tb.InsertToTab(1, Separator()).InsertToTab(1, dcon);

            dropLine.SetValueWithoutNotify(prop.lineJoin.ToString());

            var lcon = new VisualElement().Size(100, Utk.ScreenRatio(50), true, false).FlexRow();
            var lbll = new Label().Size(40, 100, true, true).Text("Line Width").TextAlignment(TextAnchor.MiddleLeft);
            var sliderl = new Slider().Size(60, 100, true, true).Name("lineWidth");
            sliderl.LowValue(0).HighValue(100).ShowInputField(true);
            lcon.AddChild(lbll).AddChild(sliderl);
            tb.InsertToTab(1, Separator()).InsertToTab(1, lcon);
            tb.InsertToTab(1, capCon);
            sliderl.value = prop.lineRadius;

            var mcon = new VisualElement().Size(100, Utk.ScreenRatio(50), true, false).FlexRow();
            var lblm = new Label().Size(40, 100, true, true).Text("Line Mitter").TextAlignment(TextAnchor.MiddleLeft);
            var sliderm = new Slider().Size(60, 100, true, true).Name("Mitter Limit");
            sliderm.LowValue(0).HighValue(100).ShowInputField(true);
            mcon.AddChild(lblm).AddChild(sliderm);
            tb.InsertToTab(1, Separator()).InsertToTab(1, mcon);
            sliderm.value = prop.mitterLimit;

            var ccon = new VisualElement().Size(100, Utk.ScreenRatio(50), true, false).FlexRow();
            ccon.Display(DisplayStyle.Flex);
            var lblc = new Label().Size(40, 100, true, true).TextAlignment(TextAnchor.MiddleLeft).Text("Curve Radius");
            var cslide = new Slider().LowValue(0).HighValue(150).ShowInputField(true).Size(60, 100, true, true);
            ccon.AddChild(lblc).AddChild(cslide);
            tb.InsertToTab(1, Separator()).InsertToTab(1, ccon);

            cslide.SetValueWithoutNotify(prop.curveRadius);

            cslide.OnValueChanged(x =>
            {
                prop.curveRadius = x.newValue;
                ReDraw(1, showPoint);
                instance.SaveThis();
            });

            var profCon = new VisualElement().Size(100, Utk.ScreenRatio(50), true, false).FlexRow();
            var btnprof = new Button().Size(20, 100, true, true).Text("Save");
            var lblprof = new Label().Size(40, 100, true, true).Text("Save Preset").TextAlignment(TextAnchor.MiddleLeft);
            var prof = new TextField().Size(35, 100, true, true);
            prof.SetValueWithoutNotify("<NO_NAME>");
            profCon.AddChild(lblprof).AddChild(prof).AddChild(btnprof);
            tb.InsertToTab(1, Separator().MarginBottom(5)).InsertToTab(1, profCon);

            var profListCon = new VisualElement().Size(100, Utk.ScreenRatio(50), true, false).FlexRow();
            var lblprofList = new Label().Size(40, 100, true, true).Text("Load Preset").TextAlignment(TextAnchor.MiddleLeft);
            var profList = new DropdownField().Size(60, 100, true, true);
            profListCon.AddChild(lblprofList).AddChild(profList);
            tb.InsertToTab(1, profListCon);

            List<string> lis = new();

            if (instance.profiles.Count > 0)
            {
                foreach (var str in instance.profiles)
                {
                    lis.Add(str.name);
                }
            }

            lis.Add("<None>");
            profList.choices = lis;
            profList.SetValueWithoutNotify("<SelectProfile>");

            profList.OnValueChanged(x =>
            {
                if (String.IsNullOrEmpty(x.newValue))
                    return;

                if (instance.profiles.Exists(y => y.name == x.newValue))
                {
                    Clear();

                    var found = instance.profiles.Find(z => z.name == x.newValue);

                    if (found == null || found.points == null || found.points.Count == 0)
                        return;

                    var newprop = new UTKPropSettings();
                    newprop.name = prof.value;
                    newprop.fill = found.fill;
                    newprop.fillColor = found.fillColor;
                    newprop.line = found.line;
                    newprop.lineCap = found.lineCap;
                    newprop.lineColor = found.lineColor;
                    newprop.lineJoin = found.lineJoin;
                    newprop.lineRadius = found.lineRadius;
                    newprop.points = new List<(Vector3 point, int root)>(found.points);
                    newprop.fillRule = found.fillRule;
                    newprop.drawType = found.drawType;
                    newprop.closePath = found.closePath;
                    newprop.curveRadius = found.curveRadius;

                    prop = newprop;

                    for (int i = 0; i < AllRoots.Count; i++)
                    {
                        for (int j = 0; j < prop.points.Count; j++)
                        {
                            if (AllRoots[i].index == prop.points[j].root)
                            {
                                roots.Add(AllRoots[i].Item1);
                            }
                        }
                    }

                    rootVisualElement.schedule.Execute(() =>
                    {
                        var colline = rootVisualElement.Query<ColorField>("lineColor").First();
                        colline.SetValueWithoutNotify(prop.lineColor);

                        var colfill = rootVisualElement.Query<ColorField>("fillColor").First();
                        colfill.SetValueWithoutNotify(prop.fillColor);

                        var bfill = rootVisualElement.Query<Button>("btnFill").First();
                        var bline = rootVisualElement.Query<Button>("btnLine").First();

                        if (prop.fill)
                            bfill.BcgColor(Color.blue);
                        else
                            bfill.BcgColor(Color.clear);

                        if (prop.line)
                            bline.BcgColor(Color.blue);
                        else
                            bline.BcgColor(Color.clear);

                        Draw();
                        canvas.schedule.Execute(() =>
                        {
                            ShowPoints(showPoint);
                        }).StartingIn(5);
                    }).ExecuteLater(5);
                }

                instance.SaveThis();
            });

            btnprof.clicked += () =>
            {
                if (String.IsNullOrEmpty(prof.value) || prof.value == "<NO_NAME>" || instance.profiles.Exists(x => x.name == prof.value))
                {
                    Debug.LogWarning("UTKShapeGen : Profile name can't be empty or it already existed. Aborting!");
                    return;
                }

                var newprop = new UTKPropSettings();
                newprop.name = prof.value;
                newprop.fill = prop.fill;
                newprop.fillColor = prop.fillColor;
                newprop.line = prop.line;
                newprop.lineCap = prop.lineCap;
                newprop.lineColor = prop.lineColor;
                newprop.lineJoin = prop.lineJoin;
                newprop.lineRadius = prop.lineRadius;
                newprop.points = new List<(Vector3 point, int root)>(prop.points);
                newprop.fillRule = prop.fillRule;
                newprop.closePath = prop.closePath;
                newprop.drawType = prop.drawType;
                newprop.curveRadius = prop.curveRadius;

                instance.profiles.Add(newprop);
                List<string> lis = new();

                if (instance.profiles.Count > 0)
                {
                    foreach (var str in instance.profiles)
                    {
                        lis.Add(str.name);
                    }
                }

                lis.Add("<None>");
                profList.choices = lis;
                profList.SetValueWithoutNotify(profList.value);

                prof.value = string.Empty;
                instance.SaveThis();
            };

            sliderl.OnValueChanged(x =>
            {
                prop.lineRadius = x.newValue;
                ReDraw(2, showPoint);
                instance.SaveThis();
            });

            sliderm.OnValueChanged(x =>
            {
                prop.mitterLimit = x.newValue;
                ReDraw(2, showPoint);
                instance.SaveThis();
            });

            if (prop.lineJoin != LineJoin.Miter)
                mcon.SetEnabled(false);

            dropLine.OnValueChanged(x =>
            {
                if (x.newValue == "Mitter")
                {
                    prop.lineJoin = LineJoin.Miter;
                    mcon.SetEnabled(true);

                }
                else if (x.newValue == "Round")
                {
                    prop.lineJoin = LineJoin.Round;
                    mcon.SetEnabled(false);
                }
                else
                {
                    prop.lineJoin = LineJoin.Bevel;
                    mcon.SetEnabled(false);
                }

                ReDraw(1, showPoint);
                instance.SaveThis();
            });

            lineCol.OnValueChanged(x =>
            {
                prop.lineColor = x.newValue;
                ReDraw(2, showPoint);
                instance.SaveThis();
            });

            fillCol.OnValueChanged(x =>
            {
                prop.fillColor = x.newValue;
                ReDraw(1, showPoint);
                instance.SaveThis();
            });
        }
        private void ClearOverlays()
        {
            if (overlays.Count == 0)
                return;

            for (int i = 0; i < overlays.Count; i++)
            {
                overlays[i].RemoveFromHierarchy();
            }
        }
        private void Clear()
        {
            if (prop.utkMesh != null)
                prop.utkMesh.RemoveFromHierarchy();

            if (prop.points.Count > 0)
            {
                for (int i = prop.points.Count; i-- > 0;)
                {
                    for (int j = 0; j < roots.Count; j++)
                    {
                        if (prop.points[i].root == (int)roots[j].userData)
                            roots[j].BcgColor(Color.clear);
                    }
                }

                roots.Clear();
                prop.points.Clear();
                bool tmp = showPoint;
                ShowPoints(false);
                showPoint = tmp;
                DrawMirror(false);
            }

            ClearOverlays();
            ClearColorStates();

            canvas.schedule.Execute(() =>
            {
                if (!showMirror)
                    mirrorButton.Border(0, Color.black).BorderBottom(2, Color.black).BorderTop(2, Color.black);
                else
                {
                    mirrorButton.Border(2, Color.magenta);
                }
            }).StartingIn(5);
        }
        private void DrawCanvas()
        {
            this.rootVisualElement.AddChild(leftPanel).AddChild(DrawToolbar()).AddChild(rightPanel).FlexRow();
            rightPanel.AddChild(canvas);

            rootVisualElement.schedule.Execute(() =>
            {
                DrawGrid(grids.row, grids.column);
            }).ExecuteLater(1);
        }
        private void DrawGrid(int row, int column)
        {
            float width = 100 / (float)row;
            float height = 100 / (float)column;

            for (int i = 0; i < column; i++)
            {
                var vis = new VisualElement().FlexRow().AlignItems(Align.FlexStart).Size(100, height, true, true).Name(i.ToString());
                canvas.AddChild(vis);
                containers.Add(vis);
            }

            for (int i = 0; i < row * column; i++)
            {
                for (int j = 0; j < containers.Count; j++)
                {
                    if (containers[j].childCount < row)
                    {
                        var root = new VisualElement().Size(100, 100, true, true).Border(Utk.ScreenRatio(1f), Color.white).Opacity(0.3f).Name("root");
                        var sub = new VisualElement().Size(50, 50, true, true).BcgColor(Color.clear).Name("point").Position(Position.Absolute).Bottom(0).Right(0);
                        var idx = i;
                        root.userData = i;
                        AllRoots.Add((root, idx));

                        root.OnPointerDown(x =>
                        {
                            VisualElement froot = roots.Find(x => x == root);

                            if (froot == null)
                            {
                                var siz = sub.ChangeCoordinatesTo(canvas, sub.transform.position);
                                root.BcgColor(Color.yellow);
                                bool ignoreAdd = false;

                                if (showMirror)
                                {
                                    ignoreAdd = true;
                                    (int start, int end, int nextIndex) range = (0, 0, 0);
                                    int tracking = row;

                                    for (int a = 0; a < row * column; a++)
                                    {
                                        if (a % row == 0)
                                        {
                                            (int, int) num = (a, tracking);

                                            if (Enumerable.Range(a, num.Item2).Contains(idx))
                                            {
                                                int max = num.Item2;

                                                for (int b = num.Item1; b < num.Item2; b++)
                                                {
                                                    if (b == idx)
                                                    {
                                                        break;
                                                    }

                                                    max--;
                                                }

                                                range = (num.Item1, num.Item2, max);
                                            }

                                            tracking += row;
                                        }
                                    }

                                    var getDistance = AllRoots.Find(x => x.index == range.nextIndex);
                                    var sizSub = getDistance.Item1.ElementAt(0).ChangeCoordinatesTo(canvas, getDistance.Item1.ElementAt(0).transform.position);

                                    mirrorPoint.Add((siz, idx, mirrorPoint.Count, sizSub));
                                    mirrorPoint.Add((sizSub, getDistance.index, -mirrorPoint.Count, siz));

                                    ClearColorStates();
                                    roots.Clear();
                                    prop.points.Clear();

                                    List<(Vector3 point, int root, int order, Vector3 rootPair)> sorted = mirrorPoint.OrderBy(o => o.order).ToList();
                                    mirrorPoint = new List<(Vector3 point, int root, int order, Vector3 rootPair)>(sorted);

                                    for (int d = 0; d < mirrorPoint.Count; d++)
                                    {
                                        roots.Add(AllRoots[mirrorPoint[d].root].Item1);
                                        prop.points.Add((mirrorPoint[d].point, mirrorPoint[d].root));
                                    }
                                }
                                else
                                {
                                    ignoreAdd = false;
                                }

                                if (!ignoreAdd)
                                {
                                    roots.Add(root);
                                    prop.points.Add((siz, idx));
                                }

                                ReDraw(1, showPoint);
                            }
                            else
                            {
                                if (showPoint || showMirror)
                                    return;

                                root.BcgColor(Color.clear);

                                for (int u = 0; u < prop.points.Count; u++)
                                {
                                    if (prop.points[u].root == idx)
                                    {
                                        prop.points.RemoveAt(u);
                                        roots.Remove(froot);
                                        ReDraw(1, showPoint);
                                        return;
                                    }
                                }
                            }
                        });

                        root.AddChild(sub);
                        containers[j].AddChild(root);
                        break;
                    }
                }
            }
        }

        private void Draw()
        {
            if (this.prop.points.Count == 0)
                return;

            List<Vector3> p = new();
            UTKMeshProp prop = new UTKMeshProp();

            foreach (var f in this.prop.points)
            {
                p.Add(f.point);
            }

            if (this.prop.utkMesh != null)
                this.prop.utkMesh.RemoveFromHierarchy();

            prop.lineJoin = this.prop.lineJoin;
            prop.lineCap = this.prop.lineCap;
            prop.fillRule = this.prop.fillRule;
            prop.closePath = this.prop.closePath;
            prop.curveRadius = this.prop.curveRadius;
            prop.lineColor = this.prop.lineColor;
            prop.drawType = this.prop.drawType;
            prop.closePath = this.prop.closePath;
            prop.mitterLimit = this.prop.mitterLimit;

            if (this.prop.line)
            {
                prop.lineRadius = this.prop.lineRadius;
            }
            else
            {
                prop.lineRadius = 0;
            }

            if (this.prop.fill)
                prop.fillColor = this.prop.fillColor;

            this.prop.utkMesh = new UTKMesh(p, prop);
            this.prop.utkMesh.Position(Position.Absolute).Opacity(0.7f);
            canvas.AddChild(this.prop.utkMesh);
        }
        private void ReDraw(long waitTime, bool clearPoints)
        {
            canvas.schedule.Execute(() =>
            {
                Draw();

                if (clearPoints && showPoint)
                {
                    ShowPoints(false);
                    ShowPoints(true);
                }
            }).ExecuteLater(waitTime);
        }
        private VisualElement DrawBrowse()
        {
            var vis = new VisualElement().FlexGrow(1).Size(100, 90, true, true);
            var assets = FindAssetsByType();

            Func<VisualElement> makeItem = () =>
            {
                var vis = new VisualElement().FlexRow().Size(100, Utk.ScreenRatio(50), true, false);
                var lbl = new Label().Size(80, 100, true, true).TextAlignment(TextAnchor.MiddleLeft).MarginLeft(5);
                var img = new Image().Size(20, 100, true, true);
                vis.AddChild(lbl).AddChild(img);
                return vis;
            };

            Action<VisualElement, int> bindItem = (vis, i) =>
            {
                (vis.ElementAt(0) as Label).Text(assets[i].obj.name);
                (vis.ElementAt(1) as Image).image = assets[i].thumbnail;
                (vis.ElementAt(1) as Image).scaleMode = ScaleMode.ScaleToFit;

                void Down(MouseDownEvent evt)
                {
                    previewImage.vectorImage = assets[i].obj;
                    previewImage.MarkDirtyRepaint();
                }
                vis.userData = new Action(() => vis.UnregisterCallback<MouseDownEvent>(Down));
                vis.RegisterCallback<MouseDownEvent>(Down);
            };

            Action<VisualElement, int> unbindItem = (vis, i) =>
            {
                var act = vis.userData as Action;
                act?.Invoke();
            };

            const int itemHeight = 30;
            var listview = new ListView(assets, itemHeight, makeItem, bindItem);
            listview.unbindItem = unbindItem;
            vis.Add(listview);
            return vis;
        }

        private List<(VectorImage obj, Texture2D thumbnail)> FindAssetsByType()
        {
            string[] guids;
            List<(VectorImage, Texture2D)> lis = new();
            guids = AssetDatabase.FindAssets("t:VectorImage", new[] { "Assets/UTKShapeGen/Resources" });

            if (guids == null || guids.Length == 0)
                return lis;

            foreach (string guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var obj = AssetDatabase.LoadAssetAtPath(path, typeof(VectorImage)) as VectorImage;
                var prev = AssetPreview.GetAssetPreview(obj);
                lis.Add((obj, prev));
            }

            return lis;
        }

        private void AssignContextMenu(VisualElement img, (VisualElement root, int index) prop)
        {
            if (showMirror)
                return;

            void Menus(ContextualMenuPopulateEvent menu)
            {
                menu.menu.AppendAction("Delete", (x) =>
                {
                    if (showPoint && !showMirror)
                    {
                        VisualElement froot = roots.Find(x => x == prop.root);
                        prop.root.BcgColor(Color.clear);

                        for (int u = 0; u < this.prop.points.Count; u++)
                        {
                            if (this.prop.points[u].root == prop.index)
                            {
                                this.prop.points.RemoveAt(u);
                                roots.Remove(froot);
                                ReDraw(1, showPoint);
                                return;
                            }
                        }
                    }

                }, DropdownMenuAction.AlwaysEnabled);

                menu.menu.AppendAction("Cancel", (x) => { }, DropdownMenuAction.AlwaysEnabled);
            }

            var con = new ContextualMenuManipulator(Menus);
            con.target = img;
        }
        private void AssignOverlayContextMenu<T>(T mesh, Painter2D painter) where T : UTKMesh
        {
            if (showMirror)
                return;

            void Menus(ContextualMenuPopulateEvent menu)
            {
                menu.menu.AppendAction("Merge", (x) =>
                {
                    prop.utkMesh.meshProperty.painter.Insert(0, painter);
                }, DropdownMenuAction.AlwaysEnabled);

                menu.menu.AppendAction("Delete", (x) =>
                {
                    var target = mesh;
                    target.RemoveFromHierarchy();
                    overlays.Remove(target);
                }, DropdownMenuAction.AlwaysEnabled);

                menu.menu.AppendAction("Cancel", (x) => { }, DropdownMenuAction.AlwaysEnabled);
            }

            var con = new ContextualMenuManipulator(Menus);
            con.target = mesh;
        }
    }
}