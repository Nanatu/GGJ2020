using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    List<GameObject> currentCollisions = new List<GameObject>();
    int faceDir;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        faceDir = this.transform.parent.GetComponent<Controller2D>().collisions.faceDir;
        if (col.gameObject.tag == "Enemy" && !currentCollisions.Contains(col.gameObject))
        {
            currentCollisions.Add(col.gameObject);
            col.gameObject.SendMessage("TakeDamage", 1);
            // col.gameObject.SendMessage("HitMove", new Vector2(-5.0f, 0.0f));
            Debug.Log("Adding" + col.gameObject.tag + "to List");

            if (gameObject.name == "LightAttack")
            {
                col.gameObject.SendMessage("HitMove", new Vector2(faceDir * 25.0f, 0.0f));
                //SoundManagerScript.PlaySound("mubAttack");
            }
        }

    }

    void ResetCollisionList()
    {
        //Debug.Log("Clearing Collision List");
        currentCollisions.Clear();
    }
}
