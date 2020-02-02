using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : BaseBehaviour {


    public float shootDelay = 0.5f;
    public GameObject projectilePrefab;
    public Vector2 firePosistion = Vector2.zero;
    private float timeElapsed = 0;

	// Update is called once per frame
	void Update () {
		if(projectilePrefab != null)
        {
            
            bool canFire = Input.GetKey(KeyCode.M);
            Debug.Log("Firing" + timeElapsed);
            if (canFire && timeElapsed > shootDelay)
            {
                
                CreateProjectile(transform.position);
                timeElapsed = 0;
            }
            timeElapsed += Time.deltaTime;
        }
	}

    public void CreateProjectile(Vector2 pos)
    {
        GameObject clone = Instantiate(projectilePrefab, pos, Quaternion.identity) as GameObject;
        clone.transform.localScale = transform.localScale;
        
        
    }
}
