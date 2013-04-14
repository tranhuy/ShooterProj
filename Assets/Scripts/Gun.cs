using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour 
{
    public GameObject muzzleFlashPrefab, bulletPrefab, bulletCasePrefab;
    GameObject bullet, bulletCase;
    Transform muzzle;
    public AudioClip gunShot, emptyClip, reload;

    public int ammoCount, ammoCapacity, clipSize;
    public int recoil, damage, accuracy;
    public float reloadSpeed, fireRate, fireRange;
    
    void Start()
    {
        ammoCount = ammoCapacity;
        muzzle = transform.FindChild("Muzzle");
    }

    // Update is called once per frame
    void Update()
    {   
       // Debug.DrawRay(transform.FindChild("Muzzle").position, -transform.up * fireRange, Color.red);      // local up direction of gun is same as global forward direction
    }

    public GUIStyle ammoCounter;
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 200, Screen.height - 30, 200, 30), "Ammo: " + ammoCount + "/" + ammoCapacity, ammoCounter);
    }

    public void Shoot()
    {
        if (ammoCount <= 0)
        {
            AudioSource.PlayClipAtPoint(emptyClip, transform.position);
            return;
        }
        if (transform.position.y >= GameObject.FindGameObjectWithTag("GunFireLevel").transform.position.y)
        {
            AudioSource.PlayClipAtPoint(gunShot, transform.position);
            Instantiate(muzzleFlashPrefab, muzzle.position, Quaternion.identity);   // spawn muzzle flash
            bullet = Instantiate(bulletPrefab, Camera.main.transform.position, muzzle.rotation) as GameObject;
            bulletCase = Instantiate(bulletCasePrefab, muzzle.position, Quaternion.identity) as GameObject;
            bulletCase.GetComponent<BulletCase>().velocity = muzzle.right + Random.insideUnitSphere;
        }
        ammoCount--;
    }

    public void Reload()
    {
        ammoCount = (ammoCount + clipSize <= ammoCapacity) ? ammoCount + clipSize : ammoCapacity;
        AudioSource.PlayClipAtPoint(reload, transform.position);
    }
}
