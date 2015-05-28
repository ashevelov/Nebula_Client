using UnityEngine;
using Common;
using Nebula;


public class ServerAPI : MonoBehaviour {


	void Start () {
	    
        //чтобы получить ссылку на игру
        var game = MmoEngine.Get.Game;

        //чтобы получить игрока
        var avatar = MmoEngine.Get.Game.Avatar;

        //чтобы получить ссылку на скрипт корабля игрока
        BaseSpaceObject playerShipComponent = MmoEngine.Get.Game.Avatar.Component;
        //или на сам GameObject
        GameObject playerShipGameObject = MmoEngine.Get.Game.Avatar.View;
        //все сервеные объекты, которые представлены GameObject-ом в юнити при создании
        // будут иметь компонент, который наследуется от BaseSpaceObject. Основные общие действия(типа шитов и тд) нужно скриптовать
        // в нем или переопределятьв наследниках

        //Получить список серверных объектов( игроков, ботов, можно из списка)
        var items = MmoEngine.Get.Game.Items;

        //or
        Item item;
        if (MmoEngine.Get.Game.TryGetItem((byte)ItemType.Bot, "QWERT12323", out item)) { }

        //выполнить функцию на сервере примерно как и раньше
        Operations.ExecAction(MmoEngine.Get.Game, MmoEngine.Get.Game.Avatar.Id, "TestAction", new object[] { "one", 2 });

        //запросить определенное свойство с сервера, запрашиваются только группами
        Operations.GetProperties(MmoEngine.Get.Game, MmoEngine.Get.Game.Avatar.Id, MmoEngine.Get.Game.Avatar.Type, null, new string[] { GroupProps.SHIP_BASE_STATE});

        //когда свойства вернутся вызовется Item.SetProperties(string group, Hashtable properties)
        //который вызовет виртуальный метод Item.OnSettedProperty(string group, string propName, object newValue, object oldValue) - переопределяя этот метод
        //иожно обратывать свойства, например итем игрока class MyItem : Item {} переопределяет его 

        /*
        public override void OnSettedProperty(string group, string propName, object newValue, object oldValue)
        {
            base.OnSettedProperty(group, propName, newValue, oldValue);
            switch(group)
            {
                case GroupProps.DEFAULT_STATE:
                    _targetState.ParseProp(propName, newValue);
                    break;
                case GroupProps.MECHANICAL_SHIELD_STATE:
                    _ship.MechanicalShield.ParseProp(propName, newValue);
                    break;
                case GroupProps.POWER_FIELD_SHIELD_STATE:
                    _ship.PowerShield.ParseProp(propName, newValue);
                    break;
                case GroupProps.SHIP_BASE_STATE:
                    _ship.ParseProp(propName, newValue);
                    _aiState.ParseProp(propName, newValue);
                    break;
                case GroupProps.SHIP_WEAPON_STATE:
                    _ship.Weapon.ParseProp(propName, newValue);
                    break;
            }
        }
         */
    }
	

}
