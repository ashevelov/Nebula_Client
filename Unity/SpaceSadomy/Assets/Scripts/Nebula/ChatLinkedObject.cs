namespace Nebula {
    using UnityEngine;
    using System.Collections;
    using Common;
    using System;
    using ServerClientCommon;

    public class ChatLinkedObject : IInfo {

        public int linkID { get; set; }
        public string displayName { get; set; }
        public Hashtable objectInfo { get; set; }

        public Hashtable GetInfo() {
            return new Hashtable {
                { (int)SPC.Id, linkID },
                { (int)SPC.DisplayName, displayName },
                { (int)SPC.AttachedObject, objectInfo }
            };
        }

        public void ParseInfo(Hashtable info) {
            linkID          = info.Value<int>((int)SPC.Id, 0);
            displayName     = info.Value<string>((int)SPC.DisplayName);
            objectInfo      = info.Value<Hashtable>((int)SPC.AttachedObject);
        }
    }
}
