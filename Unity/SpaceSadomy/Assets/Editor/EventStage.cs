using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using System.Linq;

namespace Game.Space.Editor
{
    using System.Collections.Generic;

    public class EventStage 
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string startText;

        public string StartText
        {
            get { return startText; }
            set { startText = value; }
        }
        private string taskText;

        public string TaskText
        {
            get { return taskText; }
            set { taskText = value; }
        }
        private bool isFinal;

        public bool IsFinal
        {
            get { return isFinal; }
            set { isFinal = value; }
        }
        private bool isSuccess;

        public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }
        private EventStageTimeout timeout;

        public EventStageTimeout Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }
        private List<Transition> transitions;

        public List<Transition> Transitions
        {
            get { return transitions; }
            set { transitions = value; }
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, Start Text: {1}, Task Text: {2}, Is Final: {3}, Is Success: {4}, Timeout: {5}, Transitions Count: {6}",
                id, startText, taskText, isFinal, isSuccess, timeout, transitions.Count);
        }

        public EventStage()
        {
            id = 0;
            startText = string.Empty;
            taskText = string.Empty;
            isFinal = false;
            isSuccess = false;
            timeout = EventStageTimeout.YES;
            transitions = new List<Transition>();
        }

        public XElement ToXElement()
        {
            XElement result = new XElement("stage");
            result.SetAttributeValue("id", this.Id.ToString());
            result.SetAttributeValue("start_text", this.StartText);
            result.SetAttributeValue("task_text", this.TaskText);
            result.SetAttributeValue("is_final", this.IsFinal.ToString());
            result.SetAttributeValue("is_success", this.IsSuccess.ToString());
            result.SetAttributeValue("timeout", ((int)this.Timeout).ToString());
            XElement transitions = new XElement("transitions");
            result.Add(transitions);
            foreach(var t in this.Transitions)
            {
                transitions.Add(t.ToXElement());
            }
            return result;
        }

        public static EventStage Parse(XElement stageElement)
        {
            int sId = int.Parse(stageElement.Attribute("id").Value);
            string sStartText = stageElement.Attribute("start_text").Value;
            string sTaskText = stageElement.Attribute("task_text").Value;
            bool sIsFinal = bool.Parse(stageElement.Attribute("is_final").Value);
            bool sIsSuccess = bool.Parse(stageElement.Attribute("is_success").Value);
            EventStageTimeout timeout = (EventStageTimeout)int.Parse(stageElement.Attribute("timeout").Value);

            List<Transition> sTransitions = new List<Transition>();

            if(stageElement.Element("transitions") != null )
            {
                sTransitions = stageElement.Element("transitions").Elements("transition").Select(e =>
                    {
                        return Transition.Parse(e);
                    }).ToList();
            }

            EventStage result = new EventStage
            {
                Id = sId,
                StartText = sStartText,
                TaskText = sTaskText,
                IsFinal = sIsFinal,
                IsSuccess = sIsSuccess,
                Timeout = timeout,
                Transitions = sTransitions
            };
            return result;
        }
    }
}

