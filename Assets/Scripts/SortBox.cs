using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortBox : MonoBehaviour
{
    MasterSorter masterSorter;

    public SortItem.SortItemType itemType;
    public GameObject star;
    // Start is called before the first frame update
    void Start()
    {
        masterSorter = FindObjectOfType<MasterSorter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<SortItem>().itemType == itemType)
        {
            masterSorter.AddScore(1);
            Instantiate(star, collision.transform.position + Vector3.down, Quaternion.identity);
        }
            
    }
}
