using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieParts : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Destroy(this.gameObject, 5f);
    }

}
