using UnityEngine;
using System.Collections;

public class BlockSecret : MonoBehaviour {

    public enum Item { None, Magnet, Aqua, PlusJump, BigPoint, Point, SpringBlock, EnemyMove, EnemyStaticMove };
    
    public Item item;
    public bool isItemBlock, isEnemyForward, isItemRespawn;
    public float SpringForce;

    private GameObject ItemObject;
    private Collider thisCollider;
    private Vector2 pos;

    // Use this for initialization
    void Start () {
        if (item == Item.None) UnityEngine.Debug.LogError("Item is None.");
        thisCollider = GetComponent<Collider>();
        pos = transform.position;

        if (SpringForce == 0) SpringForce = 300;

        ItemObject = null;
    }
	
	// Update is called once per frame
	void Update () {
        if (isItemBlock || Vector3.Distance(World.cubeManager.pos, transform.position) > World.drawDistance) return;

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
        Vector2 cubePos = World.Cube.transform.position;

        if(Mathf.Abs(cubePos.x - pos.x) < 1.1f && cubePos.y >= pos.y - 1.1f && cubePos.y <= pos.y - 0.8f) return true;
        
        return false;
    }
}
