/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Common;
using Nebula;

namespace Game.Space {


    public class GroupView {

        protected Rect groupRect;

        public enum GroupAlign { MiddleLeft, UpperLeft, MiddleTop, RightTop, MiddleRight, RightBottom, MiddleBottom, LeftBottom, Center }

        public void SetGroup(Vector2 size, GroupAlign align) {
            switch (align)
            {
                case GroupAlign.Center:
                    groupRect = new Rect((Utils.NativeWidth - size.x) * 0.5f, (Utils.NativeHeight - size.y) * 0.5f, size.x, size.y);
                    break;
                case GroupAlign.LeftBottom:
                    groupRect = new Rect(0, Utils.NativeHeight - size.y, size.x, size.y);
                    break;
                case GroupAlign.MiddleBottom:
                    groupRect = new Rect((Utils.NativeWidth - size.x) * 0.5f, Utils.NativeHeight - size.y, size.x, size.y);
                    break;
                case GroupAlign.MiddleLeft:
                    groupRect = new Rect(0, (Utils.NativeHeight - size.y) * 0.5f, size.x, size.y);
                    break;
                case GroupAlign.MiddleRight:
                    groupRect = new Rect(Utils.NativeWidth - size.x, (Utils.NativeHeight - size.y) * 0.5f, size.x, size.y);
                    break;
                case GroupAlign.MiddleTop:
                    groupRect = new Rect((Utils.NativeWidth - size.x) * 0.5f, 0, size.x, size.y);
                    break;
                case GroupAlign.RightBottom:
                    groupRect = new Rect((Utils.NativeWidth - size.x), Utils.NativeHeight - size.y, size.x, size.y);
                    break;
                case GroupAlign.RightTop:
                    groupRect = new Rect(Utils.NativeWidth - size.x, 0, size.x, size.y);
                    break;
                case GroupAlign.UpperLeft:
                    groupRect = new Rect(0, 0, size.x, size.y);
                    break;
            }
        }

        public void SetGroup(Rect rect) {
            groupRect = rect;
        }

        public void StartGroup() {
            GUI.BeginGroup(groupRect, GUIStyle.none);
        }

        public void EndGroup() {
            GUI.EndGroup();
        }
    }
    
    [System.Serializable]
    public abstract class GuiElement {
        public static readonly Vector3 DEFAULT_SCALE = Vector3.one;
        public static readonly float DEFAULT_ROTATION = 0.0f;
        public static readonly Color DEFAULT_COLOR = Color.white;

        private Vector3 scale = Vector3.one;
        private float rotation = 0.0f;
        private Color color = Color.white;
        private Vector2 startCoord = Vector2.zero;
        private GUIStyle style = GUIStyle.none;

        public Rect Rect;

        public Vector3 Scale { get { return scale; } }
        public float Rotation { get { return rotation; } }
        public Color Color { get { return color; } }
        public Vector2 CoordinateOrigin { get { return startCoord; } }
        public GUIStyle Style { get { return style; } }

        public void SetScale(float x, float y) {
            scale = new Vector3(x, y, 1.0f);
        }
        public void SetScaleDefault() {
            scale = Vector3.one;
        }
        public void SetRotation(float ang) {
            rotation = ang;
        }
        public void SetRotationDefault() {
            rotation = 0.0f;
        }
        public void SetColor(Color c) {
            color = c;
        }
        public void SetColorDefault() {
            color = Color.white;
        }
        public void SetStyle(GUIStyle s) {
            style = s;
        }
        public void SetStyleDefault() {
            style = GUIStyle.none; ;
        }

        public void SetRectPos(float x, float y) {
            Rect.Set(x, y, Rect.width, Rect.height);
        }

        public void SetRectPos(Vector2 pos) {
            Rect.Set(pos.x, pos.y, Rect.width, Rect.height);
        }
        public void SetRectSize(float width, float height) {
            Rect.Set(Rect.x, Rect.y, width, height);
        }
        public void SetRect(Rect r) {
            Rect = r;
        }
        public void SetRect(float x, float y, float width, float height) {
            Rect.Set(x, y, width, height);
        }
        public abstract bool Draw();
        public void Init(Rect rect, GUIStyle s) {
            SetRect(rect);
            SetStyle(s);
        }

        protected bool IsDefaultDrawMode() {
            return scale == DEFAULT_SCALE && rotation == DEFAULT_ROTATION && color == DEFAULT_COLOR;
        }
    }

    [System.Serializable]
    public class Label : GuiElement{

        public string Text = string.Empty;

        public void SetText(string str) {
            Text = str;
        }

        public override bool Draw() {
            if (Scale == DEFAULT_SCALE && Rotation == DEFAULT_ROTATION && Color == DEFAULT_COLOR)
            {
                GUI.Label(Rect, Text, Style);
            }
            else {
                Matrix4x4 m = GUI.matrix;
                Color c = GUI.color;
                //Debug.Log("Rect center: " + Rect.center + " rect x,y: " + Rect.x + ", " + Rect.y);
                Vector3 targetPoint = new Vector3(CoordinateOrigin.x + Rect.center.x, CoordinateOrigin.y + Rect.center.y, 0);
                Matrix4x4 newMatr = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Utils.ResX, Utils.ResY, 1.0f));
                newMatr = newMatr * Matrix4x4.TRS(targetPoint, Quaternion.identity, Vector3.one);
                if( Scale != DEFAULT_SCALE )
                    newMatr = newMatr * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Scale);
                if (Rotation != DEFAULT_ROTATION)
                    newMatr = newMatr * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, Rotation), Vector3.one);
                newMatr = newMatr * Matrix4x4.TRS(-targetPoint, Quaternion.identity, Vector3.one);
                GUI.matrix = newMatr;
                GUI.color = Color;
                GUI.Label(Rect, Text, Style);
                GUI.color = c;
                GUI.matrix = m;
            }
            return true;
        }
    }

    [System.Serializable]
    public class Texture : GuiElement
    {
        public Texture2D texture;

        public void Init(Rect rect, GUIStyle style, Texture2D texture)
        {
            base.Init(rect, style);
            this.texture = texture;
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public override bool Draw()
        {
            if (IsDefaultDrawMode())
            {
                GUI.DrawTexture(Rect, texture);
            }
            else
            {
                bool useColor = false;
                bool useRotation = false;
                bool useScale = false;
                if (Color != DEFAULT_COLOR)
                    useColor = true;
                if (Rotation != DEFAULT_ROTATION)
                    useRotation = true;
                if (Scale != DEFAULT_SCALE)
                    useScale = true;
                Color oldColor = GUI.color;
                if (useColor)
                    GUI.color = Color;
                Matrix4x4 oldMatrix = GUI.matrix;

                if (useScale || useRotation)
                {
                    Vector3 targetPoint = new Vector3(CoordinateOrigin.x + Rect.center.x, CoordinateOrigin.y + Rect.center.y, 0);
                    Matrix4x4 newMatr = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Utils.ResX, Utils.ResY, 1.0f));
                    newMatr = newMatr * Matrix4x4.TRS(targetPoint, Quaternion.identity, Vector3.one);
                    if (Scale != DEFAULT_SCALE)
                        newMatr = newMatr * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Scale);
                    if (Rotation != DEFAULT_ROTATION)
                        newMatr = newMatr * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, Rotation), Vector3.one);
                    newMatr = newMatr * Matrix4x4.TRS(-targetPoint, Quaternion.identity, Vector3.one);
                    GUI.matrix = newMatr;
                }
                GUI.DrawTexture(Rect, texture);
                if (useScale || useRotation)
                    GUI.matrix = oldMatrix;
                if (useColor)
                    GUI.color = oldColor;
            }
            return true;
        }
    }

    [System.Serializable]
    public class Button : GuiElement {
        private object data;

        public string Text = string.Empty;
        public void SetText(string str) {
            Text = str;
        }

        public override bool Draw()
        {
            if (Scale == DEFAULT_SCALE && Rotation == DEFAULT_ROTATION && Color == DEFAULT_COLOR)
            {
                if (GUI.Button(Rect, Text, Style))
                {
                    return true; ;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                bool result = false;
                Matrix4x4 m = GUI.matrix;
                Color c = GUI.color;
                //Debug.Log("Rect center: " + Rect.center + " rect x,y: " + Rect.x + ", " + Rect.y);
                Vector3 targetPoint = new Vector3(CoordinateOrigin.x + Rect.center.x, CoordinateOrigin.y + Rect.center.y, 0);
                Matrix4x4 newMatr = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Utils.ResX, Utils.ResY, 1.0f));
                newMatr = newMatr * Matrix4x4.TRS(targetPoint, Quaternion.identity, Vector3.one);
                if (Scale != DEFAULT_SCALE)
                    newMatr = newMatr * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Scale);
                if (Rotation != DEFAULT_ROTATION)
                    newMatr = newMatr * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, Rotation), Vector3.one);
                newMatr = newMatr * Matrix4x4.TRS(-targetPoint, Quaternion.identity, Vector3.one);
                GUI.matrix = newMatr;
                GUI.color = Color;
                //GUI.Label(Rect, Text, Style);
                if (GUI.Button(Rect, Text, Style)) {
                    result = true;
                }
                GUI.color = c;
                GUI.matrix = m;
                return result;
            }
        }

        /// <summary>
        /// Set user specified data for button, for later use
        /// </summary>
        public void SetData(object data) {
            this.data = data;
        }

        /// <summary>
        /// Get user specified data for button
        /// </summary>
        public object Data {
            get {
                return this.data;
            }
        }
    }

    [System.Serializable]
    public class Slider : GuiElement {

        public string Text = string.Empty;
        private GUIStyle thumbStyle;
        private float value = 0.0f;
        private float leftValue;
        private float rightValue;

        public void SetText(string str) {
            Text = str;
        }
        public void SetThumbStyle(GUIStyle st) {
            thumbStyle = st;
        }

        public void SetValue(float v) {
            value = v;
        }

        public void SetRange(float left, float right) {
            leftValue = left;
            rightValue = right;
        }

        public float Value {
            get {
                return value;
            }
        }

        public float LeftValue {
            get {
                return leftValue;
            }
        }

        public float RightValue {
            get {
                return rightValue;
            }
        }

        public override bool Draw()
        {
            if (IsDefaultDrawMode())
            {
                value = GUI.HorizontalSlider(Rect, value, leftValue, rightValue, Style, thumbStyle);
            }
            else
            {
                Matrix4x4 m = GUI.matrix;
                Color c = GUI.color;
                //Debug.Log("Rect center: " + Rect.center + " rect x,y: " + Rect.x + ", " + Rect.y);
                Vector3 targetPoint = new Vector3(CoordinateOrigin.x + Rect.center.x, CoordinateOrigin.y + Rect.center.y, 0);
                Matrix4x4 newMatr = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Utils.ResX, Utils.ResY, 1.0f));
                newMatr = newMatr * Matrix4x4.TRS(targetPoint, Quaternion.identity, Vector3.one);
                if (Scale != DEFAULT_SCALE)
                    newMatr = newMatr * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Scale);
                if (Rotation != DEFAULT_ROTATION)
                    newMatr = newMatr * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, Rotation), Vector3.one);
                newMatr = newMatr * Matrix4x4.TRS(-targetPoint, Quaternion.identity, Vector3.one);
                GUI.matrix = newMatr;
                GUI.color = Color;
                value = GUI.HorizontalSlider(Rect, value, leftValue, rightValue, Style, thumbStyle);
                GUI.color = c;
                GUI.matrix = m;
            }
            return true;
        }
    }

    [System.Serializable]
    public class ReactorProgress
    {
        public Rect rect;
        public Texture2D emptyTexture;
        public Texture2D filledTexture;
        public Material material;
        public Color filledColor;
        public Color emptyColor;
        
        private float value;

        public void SetValue(float value)
        {
            this.value = Mathf.Clamp01(value);
            if (material)
            {
                material.SetFloat("_Value", this.value);
                Color color = Color.Lerp(emptyColor, filledColor, this.value);
                material.SetColor("_Color", color);
            }
        }

        public void Draw()
        {
            if (material)
                if(Event.current.type == EventType.Repaint)
                    Graphics.DrawTexture(rect, material.mainTexture, material);
            else
                GUI.DrawTexture(rect, filledTexture);
            GUI.DrawTexture(rect, emptyTexture);
        }
    }

    [System.Serializable]
    public class ModuleGroupProgress
    {
        public Rect rect;
        public Texture2D emptyTexture;
        public Texture2D filledTexture;
        public Material material;
        public Color filledColor;
        public Color emptyColor;

        private float value;

        public void SetValue(float value)
        {
            this.value = Mathf.Clamp01(value);
            if (material)
            {
                material.SetFloat("_Value", this.value);
                Color color = Color.Lerp(emptyColor, filledColor, this.value);
                material.SetColor("_Color", color);
            }
        }

        public void Draw()
        {
            if (material)
			{
				if (Event.current.type.Equals(EventType.Repaint))
                Graphics.DrawTexture(rect, material.mainTexture, material);
			}
            else
			{
				if (Event.current.type.Equals(EventType.Repaint))
                GUI.DrawTexture(rect, filledTexture);
			}
            GUI.DrawTexture(rect, emptyTexture);
        }
    }

    [System.Serializable]
    public class Box
    {
        public Rect rect;
        private GUIStyle style;
        public Color color = Color.white;

        public void Setup(GUIStyle style)
        {
            this.style = style;
        }

        public void Draw() {
            if (style == null)
                style = GUIStyle.none;
            Color oldColor = GUI.color;
            GUI.color = color;
            GUI.Box(rect, string.Empty, this.style);
            GUI.color = oldColor;
        }
    }

    [System.Serializable]
    public class Toggle
    {
        public Rect rect;
        private GUIStyle style;

        private bool isChecked;

        public void Setup(GUIStyle style)
        {
            this.style = style;
        }

        public bool Draw()
        {
            isChecked = GUI.Toggle(rect, isChecked, string.Empty, style);
            return isChecked;
        }

        public bool IsChecked
        {
            get { return isChecked;  }
        }

        public void SetChecked(bool value)
        {
            isChecked = value;
        }
    }

	[System.Serializable]
	public class Window
	{
		public Rect rect = new Rect(50,50,400,400);
		public string title;
		private Rect handleSize = new Rect(0,0, 20,20);
		public Rect handlePos = new Rect(0,0, 20,20);
		public Vector2 _minRectSize = new Vector2(100,100);
		public Vector2 _maxRectSize = new Vector2(1500,800);
		public Rect closeBtn = new Rect(0,0,20,20);
		private GUIStyle style;
		private bool switchWin = false;

		public int depth = -2;
		
		public List<WindowComponent> winComponents = new List<WindowComponent>();
		
		public void Setup(GUIStyle style)
		{
			this.style = style;
		}
		
		private int activeComponent = 0;
		
		public void Draw()
		{
			
			GUI.enabled = activeWin;
			GUI.BeginGroup(rect, GUI.skin.box);

			GUI.enabled = true;
			if(switchWin && !Utils.MouseOverGUI)
			{
				depth = 0;
				//GUIWinController.Get.UpdateDepth();

				Vector2 mousePos = Utils.ConvertedMousePosition;
				if(handlePos.addPos(rect).Contains(mousePos))
				{
					_handleClickedPos = true;
				}
				
				_clickedPosition = mousePos;
				_originalRect = rect;


			}
			switchWin = false;
			if ((Event.current.button == 0) && (Event.current.type == EventType.MouseDown) && !Utils.MouseOverGUI) {
				switchWin = true;
			}
			Event e = Event.current;
			//Check the event type and make sure it's left click.
			if( ( e.type == EventType.MouseDown||e.type == EventType.MouseDrag || e.type== EventType.MouseUp ) && e.button == 0 && activeWin)
			{
				Utils.MouseOverGUI = true;
			}


			GUI.enabled = activeWin;
			float width = (handlePos.width -60) / winComponents.Count;
			for(int i = 0; i < winComponents.Count; i++)
			{
				winComponents[i].handlePos.x = width*i;
				winComponents[i].handlePos.width = width;
				winComponents[i].handlePos.height = handlePos.height;
				if(GUI.Toggle(winComponents[i].handlePos,  (activeComponent == i), winComponents[i].title, GUI.skin.button))
				{
					activeComponent = i;
				}
				if(activeComponent == i)
				{
					winComponents[i].rect = new Rect(0, handlePos.height, rect.width, rect.height - handlePos.height);
					winComponents[i].Draw();
				}
			}
			
			handleSize.x = rect.width-handleSize.width;
			handleSize.y = rect.height-handleSize.height;
			handlePos.width = rect.width;
			closeBtn.x = rect.width-closeBtn.width;
			if(GUI.Button(closeBtn, "x"))
			{
				winComponents.Clear();
				//GUIWinController.Get.RemoveWin(this);
			}
			GUI.Box(handleSize, GUIContent.none);
			GUI.enabled = true;

			GUI.EndGroup();
		}
		

		private bool _handleClickedSize;
		private bool _HnadleClickedNewWin;
		[HideInInspector]
		public  bool _handleClickedPos;
		[HideInInspector]
		public Vector2 _clickedPosition;
		[HideInInspector]
		public  Rect _originalRect;
		private WindowComponent del_comp = null;
		
		public void Update()
		{
			if(activeWin)
			{
				CheckSizeAndPos();
			}
		}

		public bool activeWin
		{
			get
			{
				return depth >= -1;
			}
		}
		
		private void CheckSizeAndPos()
		{
			
			Vector2 mousePos = Utils.ConvertedMousePosition;
			
			if (Input.GetMouseButtonDown(0))
			{
				if(handleSize.addPos(rect).Contains(mousePos))
				{
					_handleClickedSize = true;
				}

				if(handlePos.addPos(rect).Contains(mousePos))
				{
					_handleClickedPos = true;

					if(winComponents.Count > 1)
					{
						winComponents.ForEach((c)=>{
							if(c.handlePos.addPos(rect).Contains(mousePos))
							{
								_HnadleClickedNewWin = true;
								del_comp = c;
							}
						});
					}
				}
				_clickedPosition = mousePos;
				_originalRect = rect;
			}
			
			if (_handleClickedSize)
			{
				if (Input.GetMouseButton(0))
				{
					rect.width = Mathf.Clamp(_originalRect.width + (mousePos.x - _clickedPosition.x), _minRectSize.x, _maxRectSize.x);
					rect.height = Mathf.Clamp(_originalRect.height + (mousePos.y - _clickedPosition.y), _minRectSize.y, _maxRectSize.y);
				}
				if (Input.GetMouseButtonUp(0))
				{
					_handleClickedSize = false;
				}
			}
			
			if (_handleClickedPos)
			{
				if (Input.GetMouseButton(0))
				{
					if(_HnadleClickedNewWin)
					{
						if(Vector2.Distance(mousePos, _clickedPosition) > 10 )
						{
							_handleClickedPos = false;
							_HnadleClickedNewWin = false;
							//GUIWinController.Get.AddWin(del_comp, rect);
							winComponents.Remove(del_comp);
							del_comp = null;
						}
					}else{
						rect = _originalRect.addPos(mousePos - _clickedPosition);
					}
				}
				if (Input.GetMouseButtonUp(0))
				{
					_handleClickedPos = false;
					_HnadleClickedNewWin = false;
					del_comp = null;
				}
			}
		}
	}
		
		[System.Serializable]
		public class WindowComponent
		{
			public Rect rect;
			public string title;
			[HideInInspector]
			public Rect handlePos = new Rect(0,0, 20,20);
			private GUIStyle style;
			
			public System.Action drawElements;
			
			public void Setup(GUIStyle style)
			{
				//this.style = style;
			}
			
			public void Draw()
			{
				GUI.BeginGroup(rect, GUI.skin.box);
				
				if(drawElements != null)
				{
					drawElements();
				}
				
				GUI.EndGroup();
			}
		}
        
        public class NotifLabel
        {
            public Rect rect;
            public Vector2 pos;
            public string text;
            public Color color;

            public GUIStyle style = new GUIStyle();

            public void Setup(string text, Color color)
            {
                this.text = text;
                this.color = color;
                style.alignment = TextAnchor.MiddleCenter;
                style.fontStyle = FontStyle.Bold;
                style.normal.textColor = Color.white;
                style.fontSize = 30;
            }

            public void Draw()
            {
                GUI.Label(rect, text, style);
            }

        }


}*/