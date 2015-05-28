using UnityEngine;
using System.Collections;
using Game.Space;
using System.Collections.Generic;


public class AnimationManager : Singleton<AnimationManager> {
    private TimeModifierType testTimeModifier = TimeModifierType.Linear;
    private int testModifierIndex = 0;

    private Dictionary<string, AnimationNode> animations = new Dictionary<string, AnimationNode>();
    private List<CompletedAnimation> completedAnimations = new List<CompletedAnimation>();
    private List<string> removedAnimations = new List<string>();
    private List<string> replacedAnimations = new List<string>();
    public class CompletedAnimation {
        public string animationName;
        public string animationId;
    }

    public void OnAnimationCompleted(string animationName, string animationId) {
        /*
        if (animations.ContainsKey(animationName)) {
            AnimationNode node = animations[animationName];
            if (node.AnimationId == animationId)
            {
                if (node.GetNext != null)
                {
                    animations[animationName] = node.GetNext;
                }
                else
                {
                    print("remove animation: " + animationName);
                    animations.Remove(animationName);
                }
            }
            else
            {
                Debug.LogError(string.Format("Animation: {0} first node id: {1} but completed node id: {2}", animationName, node.AnimationId, animationId));
            }
        }*/
        completedAnimations.Add(new CompletedAnimation { animationName = animationName, animationId = animationId });
    }

    void Update() {
        foreach (KeyValuePair<string, AnimationNode> de in animations) {
            if( de.Value.IsStarted)
                de.Value.Update();
            if (!de.Value.IsStarted && !de.Value.IsCompleted) {
                if (Time.time >= de.Value.StartTime)
                    de.Value.Start();
            }
        }

        if (completedAnimations.Count > 0) {
            foreach (CompletedAnimation ca in completedAnimations) {
                if (animations.ContainsKey(ca.animationName))
                {
                    if (animations[ca.animationName].AnimationId == ca.animationId)
                    {
                        if (animations[ca.animationName].GetNext != null)
                            replacedAnimations.Add(ca.animationName);
                        else
                            removedAnimations.Add(ca.animationName);
                    }
                    else
                    {
                        Debug.LogError(string.Format("First anim node of animation {0} is {1}, but completed animation is: {2}",
                            ca.animationName, animations[ca.animationName].AnimationId, ca.animationId));
                    }
                }
                else {
                    Debug.LogError(string.Format("not founded animation: {0}", ca.animationName));
                }
            }

            if (replacedAnimations.Count > 0) {
                foreach (string animName in replacedAnimations) {
                    animations[animName] = animations[animName].GetNext;
                }
                replacedAnimations.Clear();
            }
            if (removedAnimations.Count > 0) {
                foreach (string animName in removedAnimations) {
                    animations.Remove(animName);
                }
                removedAnimations.Clear();
            }
            completedAnimations.Clear();
        }
    }

    public void AddSingleAnimationNode(string animationName, string animationNodeId, Transform t, AnimationInfo info, float startTime) {
        if (animations.ContainsKey(animationName))
        {
            if (!ChainContainsSameId( animationNodeId, animations[animationName]))
            {
                AnimationNode node = animations[animationName];
                while (node.GetNext != null)
                {
                    node = node.GetNext;
                }
                SingleAnimationNode singleNode = new SingleAnimationNode(animationName, animationNodeId, t, info, startTime);
                node.SetNext(singleNode);
            }
        }
        else {
            SingleAnimationNode singleNode = new SingleAnimationNode(animationName, animationNodeId, t, info, startTime);
            singleNode.SetNext(null);
            animations.Add(animationName, singleNode);
        }
    }

    public void AddParallelAnimationNode(string animationName, string animationNodeId, Transform t, AnimationInfo[] infoArr, float startTime) {
        if (animations.ContainsKey(animationName))
        {
            if (!ChainContainsSameId(animationNodeId, animations[animationName]))
            {
                AnimationNode node = animations[animationName];
                while (node.GetNext != null)
                {
                    node = node.GetNext;
                }
                ParallelAnimationNode parallelNode = new ParallelAnimationNode(animationName, animationNodeId, t, infoArr, startTime);
                node.SetNext(parallelNode);
            }
        }
        else
        {
            ParallelAnimationNode parallelNode = new ParallelAnimationNode(animationName, animationNodeId, t, infoArr, startTime);
            parallelNode.SetNext(null);
            animations.Add(animationName, parallelNode);
        }      
    }

    private bool ChainContainsSameId(string animationNodeId, AnimationNode node) {
        if (node.AnimationId == animationNodeId)
            return true;
        while (node != null) {
            node = node.GetNext;
            if (node != null)
                if (node.AnimationId == animationNodeId)
                    return true;
        }
        return false;
    }

    void OnGUI() {
      /*
        if (GUI.Button(new Rect(0, 0, 200, 100), "Update time modifier"))
        {
            List<MapGalaxyData> result = XmlLoader.GetMapGalaxies();
            foreach (var d in result) {
                print(d.Id + ":" + d.Name);
            }
        }
       */
    }
}

public abstract class AnimationNode {
    private Transform transform;
    private Renderer renderer;
    private AnimationNode next;
    private float startTime;
    private string animationName;
    private string animationId;

    public AnimationNode(string animationName, string animationId, Transform t, float startTime = 0.0f) {
        transform = t;
        this.startTime = startTime;
        this.animationName = animationName;
    }
    public void SetNext(AnimationNode n) {
        next = n;
    }
    public abstract void Update();
    public abstract void SetCompleted();
    public abstract void Start();
    public abstract bool IsStarted { get; }
    public abstract bool IsCompleted { get;  }

    public AnimationNode GetNext {
        get {
            return next;
        }
    }

    public Transform Transform {
        get {
            return transform;
        }
    }

    public Renderer Renderer {
        get {
            if (!renderer)
                renderer = transform.GetComponent<Renderer>();
            return renderer;
        }
    }

    public string AnimationName {
        get {
            return animationName;
        }
    }
    public string AnimationId {
        get {
            return animationId;
        }
    }
    public float StartTime {
        get { return startTime; }
    }
}

public class SingleAnimationNode : AnimationNode {
    private AnimationInfo info;

    public SingleAnimationNode(string animationName, string animationId, Transform t, AnimationInfo inf, float startTime = 0.0f) 
        : base(animationName, animationId, t, startTime){
        info = inf;
    }

    public override void Update()
    {
        if (info.IsStarted) {
            info.Update();
            switch (info.ValueType) { 
                case AnimatedValue.Position:
                    Transform.localPosition = ((VectorAnimationInfo)info).CurrentValue;
                    break;
                case AnimatedValue.Scale:
                    Transform.localScale = ((VectorAnimationInfo)info).CurrentValue;
                    break;
                case AnimatedValue.Rotation:
                    Transform.localRotation = Quaternion.Euler(((VectorAnimationInfo)info).CurrentValue);
                    break;
                case AnimatedValue.Color:
                    ColorAnimationInfo inf = (ColorAnimationInfo)info;
                    Renderer.material.SetColor(inf.ColorName, inf.CurrentValue);
                    break;
            }

            if (info.IsComepleted) {
                AnimationManager.Get.OnAnimationCompleted(AnimationName, AnimationId);
            }
        }
    }

    public override void Start()
    {
        VectorAnimationInfo vecInfo = null;
        ColorAnimationInfo colorInfo = null;
        switch (info.ValueType) { 
            case AnimatedValue.Position:
                vecInfo = (VectorAnimationInfo)info;
                vecInfo.Start(vecInfo.Behaviour == AnimationBehaviour.FromTo ? Vector3.zero : Transform.localPosition);
                break;
            case AnimatedValue.Scale:
                vecInfo = (VectorAnimationInfo)info;
                vecInfo.Start(vecInfo.Behaviour == AnimationBehaviour.FromTo ? Vector3.zero : Transform.localScale);
                break;
            case AnimatedValue.Rotation:
                vecInfo = (VectorAnimationInfo)info;
                vecInfo.Start(vecInfo.Behaviour == AnimationBehaviour.FromTo ? Vector3.zero : Transform.localEulerAngles);
                break;
            case AnimatedValue.Color:
                colorInfo = (ColorAnimationInfo)info;
                colorInfo.Start(colorInfo.Behaviour == AnimationBehaviour.FromTo ? Color.black : Renderer.material.GetColor(colorInfo.ColorName));
                break;
        }
    }

    public override void SetCompleted()
    {
        info.SetCompleted();
    }

    public override bool IsStarted
    {
        get { return info.IsStarted; }
    }

    public override bool IsCompleted
    {
        get { return info.IsComepleted; }
    }
}

public class ParallelAnimationNode : AnimationNode {
    private AnimationInfo[] infoArray;


    public ParallelAnimationNode(string animationName, string animationId, Transform t, AnimationInfo[] infArr, float startTime = 0.0f) 
        : base(animationName, animationId, t, startTime) {
        infoArray = infArr;
    }

    public override void Update()
    {
        if (IsStarted)
        {
            foreach (AnimationInfo info in infoArray)
            {
                if (info.IsStarted)
                {
                    info.Update();
                    switch (info.ValueType)
                    {
                        case AnimatedValue.Position:
                            Transform.localPosition = ((VectorAnimationInfo)info).CurrentValue;
                            break;
                        case AnimatedValue.Scale:
                            Transform.localScale = ((VectorAnimationInfo)info).CurrentValue;
                            break;
                        case AnimatedValue.Rotation:
                            Transform.localRotation = Quaternion.Euler(((VectorAnimationInfo)info).CurrentValue);
                            break;
                        case AnimatedValue.Color:
                            ColorAnimationInfo inf = (ColorAnimationInfo)info;
                            Renderer.material.SetColor(inf.ColorName, inf.CurrentValue);
                            break;
                    }
                }
            }
            if (IsCompleted) {
                AnimationManager.Get.OnAnimationCompleted(AnimationName, AnimationId);
            }
        }

    }

    public override bool IsCompleted {
        get {
            bool completed = true;
            foreach(AnimationInfo info in infoArray)
                if (!info.IsComepleted) {
                    completed = false;
                    break;
                }
            return completed;      
        }
    }

    public override bool IsStarted {
        get {
            bool started = false;
            foreach (AnimationInfo info in infoArray) {
                if (info.IsStarted) {
                    started = true;
                    break;
                }
            }
            return started;
        }
    }
    public override void SetCompleted()
    {
        foreach (AnimationInfo info in infoArray) {
            info.SetCompleted();
        }
    }
    public override void Start()
    {
        foreach (AnimationInfo info in infoArray) {
            VectorAnimationInfo vecInfo = null;
            ColorAnimationInfo colorInfo = null;
            switch (info.ValueType)
            {
                case AnimatedValue.Position:
                    vecInfo = (VectorAnimationInfo)info;
                    vecInfo.Start(vecInfo.Behaviour == AnimationBehaviour.FromTo ? Vector3.zero : Transform.localPosition);
                    break;
                case AnimatedValue.Scale:
                    vecInfo = (VectorAnimationInfo)info;
                    vecInfo.Start(vecInfo.Behaviour == AnimationBehaviour.FromTo ? Vector3.zero : Transform.localScale);
                    break;
                case AnimatedValue.Rotation:
                    vecInfo = (VectorAnimationInfo)info;
                    vecInfo.Start(vecInfo.Behaviour == AnimationBehaviour.FromTo ? Vector3.zero : Transform.localEulerAngles);
                    break;
                case AnimatedValue.Color:
                    colorInfo = (ColorAnimationInfo)info;
                    colorInfo.Start(colorInfo.Behaviour == AnimationBehaviour.FromTo ? Color.black : Renderer.material.GetColor(colorInfo.ColorName));
                    break;
            }           
        }
    }
}

public abstract class AnimationInfo
{
    private AnimatedValue valueType;
    private AnimationBehaviour behaviour;
    private TimeModifierType timeModifier;
    private AnimationType animationType;
    private bool started;
    private bool completed;
    private float duration;

    protected float timer;

    public AnimationInfo(AnimatedValue vt, AnimationBehaviour ab, TimeModifierType tmt, AnimationType animType,  float dur) {
        valueType = vt;
        behaviour = ab;
        timeModifier = tmt;
        //Debug.Log(string.Format("set time modifier in ctor to: {0}", timeModifier));
        started = false;
        completed = false;
        duration = dur;
        timer = 0.0f;
        animationType = animType;
    } 

    public abstract void Update();
    public AnimatedValue ValueType {
        get {
            return valueType;
        }
    }

    public AnimationBehaviour Behaviour {
        get {
            return behaviour;
        }
    }

    public TimeModifierType TimeModifier {
        get {
            return timeModifier;
        }
    }

    public bool IsStarted {
        get {
            return started;
        }
    }

    public bool IsComepleted {
        get {
            return completed;
        }
    }

    public float Duration {
        get {
            return duration;
        }
    }

    public void SetStarted() {
        started = true;
    }

    public void SetNotStarted() {
        started = false;
    }

    public void SetCompleted() {
        completed = true;
    }

    public void SetNotComepleted() {
        completed = false;
    }

    public AnimationType AnimationType {
        get {
            return animationType;
        }
    }
}

public class VectorAnimationInfo : AnimationInfo {
    private Vector3 startValue;
    private Vector3 endValue;
    private Vector3 currentValue;
    private Vector3 change;

    public VectorAnimationInfo(Vector3 start, Vector3 end, AnimatedValue vt, AnimationBehaviour ab, TimeModifierType tmt, AnimationType animType, float dur)
        : base(vt, ab, tmt, animType, dur )
    {
        startValue = start;
        endValue = end;
    }

    public void Start(Vector3 start) {
        if (!IsStarted)
        {
            if (Behaviour == AnimationBehaviour.To)
                startValue = start;
            currentValue = startValue;
            SetStarted();
            SetNotComepleted();
            timer = 0.0f;
            change = endValue - startValue;
        }
    }

    public override void Update()
    {
        if (IsStarted)
        {
            //Debug.Log(string.Format("time modifier in animationInfo: <color=orange>{0}</color>", TimeModifier));
            timer += Time.deltaTime;
            float nt = 0;
            switch (TimeModifier) { 
                case TimeModifierType.Linear:
                    nt = timer / Duration;
                    currentValue = change * nt + startValue;
                    break;
                case TimeModifierType.QuadraticIn:
                    nt = timer / Duration;
                    currentValue = change * nt * nt + startValue;
                    break;
                case TimeModifierType.QuadraticOut:
                    nt = timer / Duration;
                    currentValue = -change * nt * (nt - 2) + startValue;
                    break;
                case TimeModifierType.QuadraticInOut:
                    nt = timer / (Duration * 0.5f);
                    if (nt < 1.0f)
                        currentValue = 0.5f * change * nt * nt + startValue;
                    else {
                        nt--;
                        currentValue = -0.5f * change * (nt * (nt - 2) - 1) + startValue;
                    }
                    break;
                case TimeModifierType.CubicIn:
                    nt = timer / Duration;
                    currentValue = change * nt * nt * nt + startValue;
                    break;
                case TimeModifierType.CubicOut:
                    nt = timer / Duration;
                    nt--;
                    currentValue = change * (nt * nt * nt + 1) + startValue;
                    break;
                case TimeModifierType.CubicInOut:
                    nt = timer / (Duration * 0.5f);
                    if (nt < 1.0f)
                    {
                        currentValue = 0.5f * change * nt * nt * nt + startValue;
                    }
                    else {
                        nt -= 2.0f;
                        currentValue = 0.5f * change * (nt * nt * nt + 2) + startValue;
                    }
                    break;
                case TimeModifierType.QuarticIn:
                    nt = timer / Duration;
                    currentValue = change * nt * nt * nt * nt + startValue;
                    break;
                case TimeModifierType.QuarticOut:
                    nt = timer / Duration;
                    nt--;
                    currentValue = (1.0f - nt * nt * nt * nt) * change + startValue;
                    break;
                case TimeModifierType.QuarticInOut:
                    nt = timer / (Duration * 0.5f);
                    if (nt < 1.0f)
                    {
                        currentValue = 0.5f * nt * nt * nt * nt * change + startValue;
                    }
                    else {
                        nt -= 2.0f;
                        currentValue = 0.5f * (2.0f - nt * nt * nt * nt) * change + startValue;
                    }
                    break;
                case TimeModifierType.QuinticIn:
                    nt = timer / Duration;
                    currentValue = change * nt * nt * nt * nt * nt + startValue;
                    break;
                case TimeModifierType.QuinticOut:
                    nt = timer / Duration;
                    nt--;
                    currentValue = change * (nt * nt * nt * nt * nt + 1.0f) + startValue;
                    break;
                case TimeModifierType.QuinticInOut:
                    nt = timer / (Duration * 0.5f);
                    if (nt < 1.0f)
                    {
                        currentValue = 0.5f * nt * nt * nt * nt * nt * change + startValue;
                    }
                    else {
                        nt -= 2.0f;
                        currentValue = 0.5f * (nt * nt * nt * nt * nt + 2.0f) * change + startValue;
                    }
                    break;
                case TimeModifierType.SinIn:
                    nt = timer / Duration;
                    currentValue = -change * Mathf.Cos(nt * Mathf.PI * 0.5f) + change + startValue;
                    break;
                case TimeModifierType.SinOut:
                    nt = timer / Duration;
                    currentValue = change * Mathf.Sin(nt * Mathf.PI * 0.5f) + startValue;
                    break;
                case TimeModifierType.SinInOut:
                    nt = timer / Duration;
                    currentValue = -0.5f * (Mathf.Cos(Mathf.PI * nt) - 1.0f) * change + startValue;
                    break;
                case TimeModifierType.ExpIn:
                    nt = timer / Duration;
                    currentValue = change * Mathf.Pow(2.0f, 10.0f * (nt - 1.0f)) + startValue;
                    break;
                case TimeModifierType.ExpOut:
                    nt = timer / Duration;
                    currentValue = (-Mathf.Pow(2.0f, -10.0f * nt) + 1.0f) * change + startValue;
                    break;
                case TimeModifierType.ExpInOut:
                    nt = timer / (Duration * 0.5f);
                    if (nt < 1.0f)
                    {
                        currentValue = 0.5f * Mathf.Pow(2.0f, 10.0f * (nt - 1.0f)) * change + startValue;
                    }
                    else {
                        nt--;
                        currentValue = -0.5f * (Mathf.Pow(2.0f, -10.0f * nt) + 2.0f) * change + startValue;
                    }
                    break;
                case TimeModifierType.CircularIn:
                    nt = timer / Duration;
                    currentValue = -(Mathf.Sqrt(1.0f - nt * nt) - 1) * change + startValue;
                    break;
                case TimeModifierType.CircularOut:
                    nt = timer / Duration;
                    nt--;
                    currentValue = Mathf.Sqrt(1.0f - nt * nt) * change + startValue;
                    break;
                case TimeModifierType.CircularInOut:
                    nt = timer / (Duration * 0.5f);
                    if (nt < 1.0f)
                    {
                        currentValue = 0.5f * (1.0f - Mathf.Sqrt(1.0f - nt * nt)) * change + startValue;
                    }
                    else {
                        nt -= 2.0f;
                        currentValue = 0.5f * (Mathf.Sqrt(1.0f - nt * nt) + 1.0f) * change + startValue;
                    }
                    break;
            }
            if (timer >= Duration) {
                if (AnimationType == global::AnimationType.Single)
                {
                    SetNotStarted();
                    SetCompleted();
                    currentValue = endValue;
                }
                else if (AnimationType == global::AnimationType.Loop) {
                    Start(startValue);
                }
                else if (AnimationType == global::AnimationType.PingPong) {
                    Vector3 temp = startValue;
                    startValue = endValue;
                    endValue = temp;
                    timer = 0;
                    SetStarted();
                    SetNotComepleted();
                    currentValue = startValue;
                } 
            }
        }
    }

    public Vector3 CurrentValue {
        get {
            return currentValue;
        }
    }
}

public class ColorAnimationInfo : AnimationInfo {
    private Color startValue;
    private Color endValue;
    private Color currentValue;
    private Color change;
    private string propertyName;

    public ColorAnimationInfo( Color start, Color end, AnimatedValue vt, AnimationBehaviour ab, TimeModifierType tmt, AnimationType animType, float dur,
        string propName = "_Color") : base(vt, ab, tmt, animType, dur) {
        startValue = start;
        endValue = end;
        propertyName = propName;
    }

    public void Start(Color start) {
        if (!IsStarted)
        {
            if (Behaviour == AnimationBehaviour.To)
                startValue = start;
            currentValue = startValue;
            SetStarted();
            SetNotComepleted();
            timer = 0.0f;
            change = endValue - startValue;
        }
    }

    public override void Update()
    {
        if (IsStarted)
        {
            timer += Time.deltaTime;
            float nt = 0;
            switch (TimeModifier)
            {
                case TimeModifierType.Linear:
                    nt = timer / Duration;
                    currentValue = change * nt + startValue;
                    break;
                case TimeModifierType.QuadraticIn:
                    nt = timer / Duration;
                    currentValue = change * nt * nt + startValue;
                    break;
                case TimeModifierType.QuadraticOut:
                    nt = timer / Duration;
                    currentValue = change * nt * (2.0f - nt) + startValue;
                    break;
                case TimeModifierType.QuadraticInOut:
                    nt = timer / (Duration * 0.5f);
                    if (nt < 1.0f)
                        currentValue = 0.5f * change * nt * nt + startValue;
                    else
                    {
                        nt--;
                        currentValue = -0.5f * change * (nt * (nt - 2) - 1) + startValue;
                    }
                    break;
                case TimeModifierType.CubicIn:
                    nt = timer / Duration;
                    currentValue = change * nt * nt * nt + startValue;
                    break;
                case TimeModifierType.CubicOut:
                    nt = timer / Duration;
                    nt--;
                    currentValue = change * (nt * nt * nt + 1) + startValue;
                    break;
                case TimeModifierType.CubicInOut:
                    nt = timer / (Duration * 0.5f);
                    if (nt < 1.0f)
                    {
                        currentValue = 0.5f * change * nt * nt * nt + startValue;
                    }
                    else
                    {
                        nt -= 2.0f;
                        currentValue = 0.5f * change * (nt * nt * nt + 2) + startValue;
                    }
                    break;
                case TimeModifierType.QuarticIn:
                    nt = timer / Duration;
                    currentValue = change * nt * nt * nt * nt + startValue;
                    break;
                case TimeModifierType.QuarticOut:
                    nt = timer / Duration;
                    nt--;
                    currentValue = (1.0f - nt * nt * nt * nt) * change + startValue;
                    break;
                case TimeModifierType.QuarticInOut:
                    nt = timer / (Duration * 0.5f);
                    if (nt < 1.0f)
                    {
                        currentValue = 0.5f * nt * nt * nt * nt * change + startValue;
                    }
                    else
                    {
                        nt -= 2.0f;
                        currentValue = 0.5f * (2.0f - nt * nt * nt * nt) * change + startValue;
                    }
                    break;
                case TimeModifierType.QuinticIn:
                    nt = timer / Duration;
                    currentValue = change * nt * nt * nt * nt * nt + startValue;
                    break;
                case TimeModifierType.QuinticOut:
                    nt = timer / Duration;
                    nt--;
                    currentValue = change * (nt * nt * nt * nt * nt + 1.0f) + startValue;
                    break;
                case TimeModifierType.QuinticInOut:
                    nt = timer / (Duration * 0.5f);
                    if (nt < 1.0f)
                    {
                        currentValue = 0.5f * nt * nt * nt * nt * nt * change + startValue;
                    }
                    else
                    {
                        nt -= 2.0f;
                        currentValue = 0.5f * (nt * nt * nt * nt * nt + 2.0f) * change + startValue;
                    }
                    break;
                case TimeModifierType.SinIn:
                    nt = timer / Duration;
                    currentValue = change * (-Mathf.Cos(nt * Mathf.PI * 0.5f)) + change + startValue;
                    break;
                case TimeModifierType.SinOut:
                    nt = timer / Duration;
                    currentValue = change * Mathf.Sin(nt * Mathf.PI * 0.5f) + startValue;
                    break;
                case TimeModifierType.SinInOut:
                    nt = timer / Duration;
                    currentValue = -0.5f * (Mathf.Cos(Mathf.PI * nt) - 1.0f) * change + startValue;
                    break;
                case TimeModifierType.ExpIn:
                    nt = timer / Duration;
                    currentValue = change * Mathf.Pow(2.0f, 10.0f * (nt - 1.0f)) + startValue;
                    break;
                case TimeModifierType.ExpOut:
                    nt = timer / Duration;
                    currentValue = (-Mathf.Pow(2.0f, -10.0f * nt) + 1.0f) * change + startValue;
                    break;
                case TimeModifierType.ExpInOut:
                    nt = timer / (Duration * 0.5f);
                    if (nt < 1.0f)
                    {
                        currentValue = 0.5f * Mathf.Pow(2.0f, 10.0f * (nt - 1.0f)) * change + startValue;
                    }
                    else
                    {
                        nt--;
                        currentValue = -0.5f * (Mathf.Pow(2.0f, -10.0f * nt) + 2.0f) * change + startValue;
                    }
                    break;
                case TimeModifierType.CircularIn:
                    nt = timer / Duration;
                    currentValue = -(Mathf.Sqrt(1.0f - nt * nt) - 1) * change + startValue;
                    break;
                case TimeModifierType.CircularOut:
                    nt = timer / Duration;
                    nt--;
                    currentValue = Mathf.Sqrt(1.0f - nt * nt) * change + startValue;
                    break;
                case TimeModifierType.CircularInOut:
                    nt = timer / (Duration * 0.5f);
                    if (nt < 1.0f)
                    {
                        currentValue = 0.5f * (1.0f - Mathf.Sqrt(1.0f - nt * nt)) * change + startValue;
                    }
                    else
                    {
                        nt -= 2.0f;
                        currentValue = 0.5f * (Mathf.Sqrt(1.0f - nt * nt) + 1.0f) * change + startValue;
                    }
                    break;
            }
            if (timer >= Duration)
            {
                SetNotStarted();
                SetCompleted();
                currentValue = endValue;
            }
        }
    }

    public Color CurrentValue {
        get {
            return currentValue;
        }
    }

    public string ColorName {
        get {
            return propertyName;
        }
    }
}

public enum AnimatedValue { Position, Scale, Rotation, Color }
public enum AnimationBehaviour { FromTo, To }
public enum TimeModifierType { Linear, QuadraticIn, QuadraticOut, QuadraticInOut, CubicIn, CubicOut, CubicInOut, 
    QuarticIn, QuarticOut, QuarticInOut, QuinticIn, QuinticOut, QuinticInOut, SinIn, SinOut, SinInOut, ExpIn, ExpOut, ExpInOut,
    CircularIn, CircularOut, CircularInOut};
public enum AnimationType { Single, PingPong, Loop }

