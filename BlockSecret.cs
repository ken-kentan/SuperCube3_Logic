using UnityEngine;
using System.Collections;

public class BlockSecret : MonoBehaviour {
    
    public bool Magnet, Aqua, PlusJump, BigPoint, Point;
    public bool isItemBlock;

    private Collider thisCollider;
    private float posY;

    // Use this for initialization
    void Start () {
        thisCollider = GetComponent<Collider>();
        posY = transform.position.y - 1.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (isItemBlock) return;

	    if(CubeManager.posY <= posY && CubeManager.speedY > 0) thisCollider.isTrigger = false;
        else                        thisCollider.isTrigger = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Cube") return;

        World.audioSource.PlayOneShot(World.findItemSE);

        GetComponent<Renderer>().material = World.materialBlockSecret;

        if (Magnet)        Instantiate(World.Magnet, transform.position + new Vector3(0, 1.27f, 0), transform.rotation);
        else if (Aqua)     Instantiate(World.Aqua, transform.position + new Vector3(0, 1.27f, 0), transform.rotation);
        else if (PlusJump) Instantiate(World.PlusJump, transform.position + new Vector3(0, 1.27f, 0), transform.rotation);
        else if (BigPoint) Instantiate(World.BigPoint, transform.position + new Vector3(0, 1.27f, 0), transform.rotation);
        else if (Point)    Instantiate(World.Point, transform.position + new Vector3(0, 1.27f, 0), transform.rotation);

        if (!isItemBlock)
        {
            GameDataManager.SecretBlock++;
            GameDataManager.SaveSecret();
        }

        Destroy(this);
    }
}
