using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    [SerializeField] bool isSelectedLeft = true;

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void SelectedLeft()
    {
        this.isSelectedLeft = true;
    }
    public void SelectedRight()
    {
        this.isSelectedLeft = false;
    }
    public bool IsSelectedLeft()
    {
        return this.isSelectedLeft;
    }
}