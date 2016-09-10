using UnityEngine;
using System.Collections;

public class BlockSecret : MonoBehaviour {

    public enum Item { None, Magnet, Aqua, PlusJump, BigPoint, Point, SpringBlock, EnemyMove, EnemyStaticMove };
    
    public Item item;
    public bool isItemBlock, isEnemyForward, isItemRespawn;
    public float SpringForce;

    private GameObject ItemObject;
    private Collider thisCollider;
    private bool isItemSpawned;
    private float posY;

    // Use this for initialization
    void Start () {
        if (item == Item.None) UnityEngine.Debug.LogError("Item is None.");
        thisCollider = GetComponent<Collider>();
        posY = transform.position.y - 0.95f;

        if (SpringForce == 0) SpringForce = 300;

        ItemObject = null;
    }
	
	// Update is called once per frame
	void Update () {
        if (isItemBlock) return;

	    if(IsTouchCube()) thisCollider.isTrigger = false;
        else              thisCollider.isTrigger = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Cube" || (isItemBlock && !IsTouchCube()) || ItemObject != null) return;

        World.audioSource.PlayOneShot(World.findItemSE);

        GetComponent<Renderer>().material = World.materialBlockSecret;

        switch (item)
        {
            case Item.Magnet:
                ItemObject = (GameObject)Instantiate(World.Magnet, transform.position + new Vector3(0, 1.27f, 0), transform.rotation);
                break;
            case Item.Aqua:
                ItemObject = (GameObject)Instantiate(World.Aqua, transform.position + new Vector3(0, 1.27f, 0), transform.rotation);
                break;
            case Item.PlusJump:
                ItemObject = (GameObject)Instantiate(World.PlusJump, transform.position + new Vector3(0, 1.27f, 0), transform.rotation);
                break;
            case Item.BigPoint:
                ItemObject = (GameObject)Instantiate(World.BigPoint, transform.position + new Vector3(0, 1.27f, 0), transform.rotation);
                break;
            case Item.Point:
                ItemObject = (GameObject)Instantiate(World.Point, transform.position + new Vector3(0, 1.27f, 0), transform.rotation);
                break;
            case Item.SpringBlock:
                ItemObject = (GameObject)Instantiate(World.SpringBlock, transform.position + new Vector3(0, 1.77f, 0), transform.rotation);
                ItemObject.GetComponent<SpringManager>().force = SpringForce;
                break;
            case Item.EnemyMove:
                ItemObject = (GameObject)Instantiate(World.EnemyMove, transform.position + new Vector3(0, 1.3f, 0), transform.rotation);
                GameObject Sensor = ItemObject.transform.FindChild("Sensor").gameObject;
                Sensor.GetComponent<EnemyManager>().isForward = isEnemyForward;
                break;
            case Item.EnemyStaticMove:
                ItemObject = (GameObject)Instantiate(World.EnemyStaticMove, transform.position + new Vector3(0, 1.3f, 0), transform.rotation);
                ItemObject.GetComponent<EnemyManager>().isForward = isEnemyForward;
                break;
        }

        if (!isItemBlock) GameDataManager.AddDataValue(GameDataManager.Data.SecretBlock);

        if(!isItemRespawn) Destroy(this);
    }

    bool IsTouchCube()
    {
        if (World.cubeManager.pos.y <= posY && World.cubeManager.speed.y >= 0) return true;

        return false;
    }
}
