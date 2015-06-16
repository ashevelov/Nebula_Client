using UnityEngine;

public class InvasionPortalEnemy : BaseSpaceObject {

    private float _updatePropertiesNextTime;

    public override void Start() {
        base.Start();
    }
    public override void OnDestroy() {
    }
    public override void Update() {
        if (Item != null) {
            UpdateProperties();
        }
        base.Update();
    }
    private void UpdateProperties() {
        if (Time.time > _updatePropertiesNextTime) {
            _updatePropertiesNextTime = Time.time + 1.0f;
            Item.GetProperties();
        }
    }
}
