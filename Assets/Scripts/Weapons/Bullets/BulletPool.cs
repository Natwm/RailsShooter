using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;
    private List<GameObject> bullets;

    void Awake()
    {
        if(instance != null)
            Debug.LogWarning("Multiple instance of same Singleton : WeaponManager");
        instance = this;
        
        bullets = new List<GameObject>();
    }

    public GameObject GetBullet(GameObject prefab)
    {
        GameObject bullet; 

        for(int i = 0; i <  bullets.Count; i++)
        {
            if(bullets[i].gameObject.name.Contains(prefab.name))
            {
                bullet = bullets[i];
                bullet.GetComponent<BulletsBehaviours>().OnPoolExit(this);
                bullets.Remove(bullet);
                return bullet;
            }
        }

        bullet = Instantiate(prefab);
        bullet.transform.SetParent(transform);
        bullet.GetComponent<BulletsBehaviours>().OnPoolExit(this);
        return bullet;
    }

    public void AddBullet(BulletsBehaviours bullet)
    {
        bullets.Add(bullet.gameObject);
    }
}
