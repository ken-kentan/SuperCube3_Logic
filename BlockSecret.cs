using UnityEngine;
using System.Collections;

public class BlockSecret : MonoBehaviour {

    public enum Item { None, Magnet, Aqua, PlusJump, BigPoint, Point, SpringBlock, EnemyMove, EnemyStaticMove };
    
    public Item item;
    public bool isItemBlock, isEnemyForward;
    public float SpringForce;

    private Collider thisCollider;
    private float posY;

    // Use this for initialization
    void Start () {
        if (item == Item.None) UnityEngine.Debug.LogError("Item is None.");
        thisCollider = GetComponent<Collider>();
        posY = transform.position.y - 1.0f;

        if (SpringForce == 0) SpringForce = 300;
    }
	
	// Update is called once per frame
	void Update () {
        if (isItemBlock) return;

	    if(IsTouchCube()) thisCollider.isTrigger = false;
        else                        thisCollider.isTrigger = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Cube" || (isItemBlock && !IsTouchCube())) return;

        World.audioSource.PlayOneShot(World.findItemSE);

        GetComponent<Renderer>().material = World.materialBlockSecret;

        GameObject Temp;

        switch (item)
        {
            case Item.Magnet:
                Instantiate(World.Magnet, transform.position + new Vector3(0, 1.27f, 0), transform.rotation);
                break;
            case Item.Aqua:
                Instantiate(World.Aqua, transform.position + new Vector3(0, 1.27f, 0), transform.rotation);
                break;
            case Item.PlusJump:
                Instantiate(World.PlusJump, transform.position + new Vector3(0, 1.27f, 0), transform.rotation);
                break;
            case Item.BigPoint:
                Instantiate(World.BigPoint, transform.position + new Vector3(0, 1.27f, 0), transform.rotation);
                break;
            case Item.Point:
                Instantiate(World.Point, transform.position + new Vector3(0, 1.27f, 0), transform.rotation);
                break;
            case Item.SpringBlock:
                Temp = (GameObject)Instantiate(World.SpringBlock, transform.position + new Vector3(0, 1.77f, 0), transform.rotation);
                Temp.GetComponent<SpringManager>().force = SpringForce;
                break;
            case Item.EnemyMove:
                Temp = (GameObject)Instantiate(World.EnemyMove, transform.position + new Vector3(0, 1.3f, 0), transform.rotation);
                GameObject Sensor = Temp.transform.FindChild("Sensor").gameObject;
                Sensor.GetComponent<EnemyManager>().isForward = isEnemyForward;
                break;
            case Item.EnemyStaticMove:
                Temp = (GameObject)Instantiate(World.EnemyStaticMove, transform.position + new Vector3(0, 1.3f, 0), transform.rotation);
                Temp.GetComponent<EnemyManager>().isForward = isEnemyForward;
                break;
        }

        if (!isItemBlock)
        {
            GameDataManager.AddDataValue(GameDataManager.Data.SecretBlock);
        }

        Destroy(this);
    }

    bool IsTouchCube()
    {
        if (CubeManager.pos.y <= posY && CubeManager.speedY >= 0) return true;

        return false;
    }
}
