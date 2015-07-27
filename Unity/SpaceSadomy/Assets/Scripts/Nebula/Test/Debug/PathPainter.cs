namespace Nebula.Test {
    using UnityEngine;
    using System.Collections;
    using Common;
    using ServerClientCommon;

    public class PathPainter : MonoBehaviour {

        void OnDrawGizmos() {
            Gizmos.color = Color.green;
            for(int i = 0; i < transform.childCount; i++) {
                if(i < transform.childCount - 1) {
                    Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
                } else if(i == transform.childCount - 1) {
                    Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(0).position);
                }
            }
        }

        void test(Hashtable fireProperties) {
            //Случился выстрел или нет(нужно в самом начале в Fire проверят это, и если false выстрел не создавать)
            bool fireALLOWED = fireProperties.GetValue<bool>((int)SPC.FireAllowed, false);

            //ID того кто стреляет
            string sourceID = fireProperties.GetValue<string>((int)SPC.Source, string.Empty);

            //Type того кто стреляет
            ItemType sourceTYPE = (ItemType)fireProperties.GetValue<byte>((int)SPC.SourceType, (byte)ItemType.Avatar);

            //ID того в кого стреляют
            string targetID = fireProperties.GetValue<string>((int)SPC.Target, string.Empty);

            //Type того в кого выстрклили
            ItemType targetTYPE = (ItemType)fireProperties.GetValue<byte>((int)SPC.TargetType, (byte)ItemType.Avatar);

            //Мастерская того кто стреляет
            Workshop sourceWorkshop = (Workshop)fireProperties.GetValue<byte>((int)SPC.Workshop, (byte)Workshop.Arlen);

            //Skill ID которым инициирован выстрел ( для ботов -1)
            int skillID = fireProperties.GetValue<int>((int)SPC.Skill, -1);

            //Попал или нет в цель ( если не попал надо чтоб ракеты взрывались в воздухе или пролетали мимо)
            bool isHitted = fireProperties.GetValue<bool>((int)SPC.IsHitted, false);

            //Урон нанесенный выстрелом (нужно показывать текстом при взрыве рактеы в том случае если стреляет игрок)
            float actualDamage = fireProperties.GetValue<float>((int)SPC.ActualDamage, 0f);

            //Крит урон или нет ( если крит урон нужно взрыв усиленный делать)
            bool isCritical = fireProperties.GetValue<bool>((int)SPC.IsCritical, false); 

        }
    }
}
