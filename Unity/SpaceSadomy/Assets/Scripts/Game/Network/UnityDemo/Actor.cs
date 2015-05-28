
/*
public class Actor : MonoBehaviour
{
    private readonly Vector3 actorTextOffset = new Vector3(0, 2.6f, 0);
    private Item actor;
    private GameObject actorText;
    private float camHeight;
    private int color;

    public void Destroy()
    {
        Destroy(this.actorText);
        Destroy(this.gameObject);
        Destroy(this);
        this.actorText = null;
    }

    public void Initialize(Item actor, float camHeight)
    {
        this.name = "Item" + actor.Id;
        this.actor = actor;
        this.camHeight = camHeight;

        this.actorText = Instantiate(Resources.Load("ActorName")) as GameObject;
        this.actorText.name = "ActorText" + actor.Id;

        this.actorText.transform.renderer.material.color = Color.white;
        this.ShowActor(false);
        this.transform.localScale = new Vector3(1, 3f, 1);
        this.transform.renderer.material = Resources.Load("ActorMaterial") as Material;
    }

    public void Start() { }

    public void Update()
    {
        if (this.actor == null || this.actor.IsVisible == false) {
            this.ShowActor(false);
            return;
        }
        TextMesh textMesh = this.actorText.GetComponent<TextMesh>();
        textMesh.text = this.actor.Text;
        if (this.color != this.actor.Color)
        {
            byte[] colorBytes = BitConverter.GetBytes(this.actor.Color);
            this.color = this.actor.Color;
            this.SetActorColor(new Color((float)colorBytes[2] / byte.MaxValue, (float)colorBytes[1]/byte.MaxValue, (float)colorBytes[0]/byte.MaxValue));
        }
        this.transform.position = this.GetPosition(this.actor.Position);
        if (this.actor.Rotation != null)
        {
            this.transform.rotation = this.GetRotation(this.actor.Rotation);
        }
        this.actorText.transform.position = this.transform.position + this.actorTextOffset;
        Vector3 camDiff = this.actorText.transform.position - Camera.main.transform.position;
        Vector3 lookAt = this.actorText.transform.position + camDiff;
        this.actorText.transform.LookAt(lookAt);
        this.ShowActor(true);
    }

    private Vector3 GetPosition(float[] pos)
    {
        float x = pos[0] / MmoEngine.PositionFactorHorizonal;
        float y = pos[1] / MmoEngine.PositionFactorVertical;
        return new Vector3(x, this.transform.position.y, y);
    }
    private Quaternion GetRotation(float[] rotationValue)
    {
        Vector3 vector = new Vector3(rotationValue[0], rotationValue[1], rotationValue[2]);
        return Quaternion.Euler(vector);
    }
    private void SetActorColor(Color actorColor)
    {
        this.transform.renderer.material.color = actorColor;
    }

    private bool ShowActor(bool show)
    {
        if (this.transform.renderer.enabled != show)
        {
            this.transform.renderer.enabled = show;
            this.actorText.transform.renderer.enabled = show;
            return true;
        }
        return false;
    }
}
*/
