using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Modify : MonoBehaviour
{

    Vector2 rot;
	public GameObject Explosion_Block;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
			int force = 50;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            {
				List<Vector3> prefab_pos = Terrain.Explosion(hit, new BlockAir());

				if (prefab_pos!= null){
					for(int i=0;i<prefab_pos.Count;i++){
						GameObject temp = Instantiate(Explosion_Block,prefab_pos[i],Explosion_Block.transform.rotation) as GameObject;
						temp.GetComponent<Rigidbody>().AddForce(
							new Vector3(
							transform.forward.x*force,
							transform.forward.y*force,
							transform.forward.z*force
							));
					}
				}
            }
        }

//        rot = new Vector2(
//            rot.x + Input.GetAxis("Mouse X") * 3,
//            rot.y + Input.GetAxis("Mouse Y") * 3);
//
//        transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
//        transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);
//
//        transform.position += transform.forward * 3 * Input.GetAxis("Vertical");
//        transform.position += transform.right * 3 * Input.GetAxis("Horizontal");
    }
}