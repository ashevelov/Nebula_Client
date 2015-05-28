
/*
public class RemoteCam : MonoBehaviour
{
    private InterestArea interestArea;
    private float nextMoveTime;
    private Vector3 direction;


    public void Destroy()
    {
        this.interestArea.Remove();
        Destroy(this.gameObject);
        Destroy(this);
        Debug.Log("destroy interest area");
    }

    public void Initialize(Game game, byte cameraId, Vector3 position, Vector3 moveDirection)
    {
        this.interestArea = new InterestArea(cameraId, game, Player.GetPosition(position));
        this.interestArea.ResetViewDistance();
        this.direction = moveDirection;
        this.nextMoveTime = Time.time + 0.05f;
        this.interestArea.Create();
        this.transform.position = position;
    }

    public void Update()
    {
        Vector3 targetPos = this.transform.position + this.direction;
        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.time / 2f);
        if (Time.time > this.nextMoveTime)
        {
            this.interestArea.Move(Player.GetPosition(this.transform.position));
            this.nextMoveTime = Time.time + 0.05f;
        }
    }
}
*/
