using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using Game.Space.Editor;

public class DrawEventEditor : EditorWindow {

    private static List<WndInfo> windows;

    public static void ShowEventDrawer(WorldEvent evt)
    {
        Init(evt);
        EditorWindow w = EditorWindow.GetWindow<DrawEventEditor>();
        w.Show();
    }

    void curveFromTo(Rect rectStart, Rect rectEnd, Color color, Color shadow)
    {
        Drawing.bezierLine(
            new Vector2(rectStart.x + rectStart.width, rectStart.y + 3 + rectStart.height / 2),
            new Vector2(rectStart.x + rectStart.width + Mathf.Abs(rectEnd.x - (rectStart.x + rectStart.width)) / 2, rectStart.y + 3 + rectStart.height / 2),
            new Vector2(rectEnd.x, rectEnd.y + 3 + rectEnd.height / 2),
            new Vector2(rectEnd.x - Mathf.Abs(rectEnd.x - (rectStart.x + rectStart.width)) / 2, rectEnd.y + 3 + rectEnd.height / 2), shadow, 5, true, 20);

        /*
        Drawing.bezierLine(
            new Vector2(wr.x + wr.width, wr.y + wr.height / 2),
            new Vector2(wr.x + wr.width + Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr.y + wr.height / 2),
            new Vector2(wr2.x, wr2.y + wr2.height / 2),
            new Vector2(wr2.x - Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr2.y + wr2.height / 2), color, 2, true, 20);*/

    }

    private static void Init(WorldEvent evt)
    {
        windows = new List<WndInfo>();

        foreach(var stage in evt.Stages)
        {
            List<int> connectedWindows = new List<int>();
            foreach(var t in stage.Transitions)
            {
                connectedWindows.Add(t.ToStage);
            }

            windows.Add(new WndInfo(stage.Id, connectedWindows));
        }
    }

    private static WndInfo GetConnectedWindow(int id )
    {
        foreach(var w in windows)
        {
            if (w.id == id)
                return w;
        }
        return null;
    }

    void OnGUI()
    {
        if(windows != null )
        {
            Color s = new Color(0.4f, 0.4f, 0.5f);

            foreach(var w in windows)
            {
                foreach(int cId in w.connectedWindows )
                {
                    var cw = GetConnectedWindow(cId);
                    if(cw != null )
                    {
                        curveFromTo(w.rect, cw.rect, new Color(0.3f, 0.7f, 0.4f), s);
                    }
                }
            }

            BeginWindows();
            foreach(var w in windows)
            {
                w.Draw();
            }
            EndWindows();
        }
    }
}

public class WndInfo
{
    public Rect rect;
    public int id;
    public List<int> connectedWindows;

    public WndInfo(int id, List<int> connectedWindows)
    {
        this.id = id;
        this.rect = new Rect(10 + id * 1, 100, 80, 80);
        this.connectedWindows = connectedWindows;
    }

    private void doWindow(int wndId )
    {
        GUI.Button(new Rect(0, 0, rect.width, rect.height / 2), id.ToString());
        GUI.DragWindow();
    }

    public void Draw()
    {
        rect = GUI.Window(id, rect, doWindow, id.ToString());
    }
}
