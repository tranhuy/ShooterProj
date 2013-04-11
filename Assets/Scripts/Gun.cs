using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour 
{
    public GameObject muzzleFlashPrefab, bulletPrefab;
    GameObject bullet;
    Transform muzzle;
    public AudioClip gunShot, emptyClip, reload;

    public int ammoCount, ammoCapacity, clipSize;
    public int recoil, damage;
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
        GUI.Label(new Rect(Screen.width - 120, Screen.height - 50, 120, 50), ammoCount + "/" + ammoCapacity, ammoCounter);
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
        }
        ammoCount--;
    }

    public void Reload()
    {
        ammoCount = (ammoCount + clipSize <= ammoCapacity) ? ammoCount + clipSize : ammoCapacity;
        AudioSource.PlayClipAtPoint(reload, transform.position);
    }
}
