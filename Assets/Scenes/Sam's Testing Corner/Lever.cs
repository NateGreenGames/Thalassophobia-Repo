using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    public Animator anim;
    [SerializeField] FuseBox fuseBox;
    public MeshRenderer lightButtonBacklight;
    Vector3 lastMouseCoordinate = Vector3.zero;
    public bool isInteractable { get; set; }
    private void Start()
    {
        if (!fuseBox)
        {
            GameObject _fuseBox = GameObject.Find("TP_FuseBox");
            if (_fuseBox != null)
            {
                fuseBox = _fuseBox.GetComponent<FuseBox>();
            }
        }
        isInteractable = true;
        anim = GetComponent<Animator>();
    }

    public void OnInteract()
    {
        lastMouseCoordinate = Input.mousePosition;
        StartCoroutine(HoldingDownLever());
    }

    public void OnLookingAt()
    {
        //None
    }

    public void OnInteractHeld()
    {
        //NONE
    }

    public IEnumerator HoldingDownLever()
    {
        if (!FuseBox.fB.isOverloaded)
        {
            lastMouseCoordinate = Input.mousePosition;
            while (Input.GetKey(KeyCode.Mouse0))
            {
                bool buttonIsBeingHeld = true;
                yield return new WaitForEndOfFrame();
                Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
                lastMouseCoordinate = Input.mousePosition;
                if (buttonIsBeingHeld && ((mouseDelta.y < 0)))
                {
                    anim.SetTrigger("LeverDown");
                    fuseBox.Overload();
                    break;
                    //isInteractable = false;
                }
                else
                {
                    buttonIsBeingHeld = false;
                }
            }
        }
        else if (FuseBox.fB.isOverloaded)
        {
            lastMouseCoordinate = Input.mousePosition;
            while (Input.GetKey(KeyCode.Mouse0))
            {
                bool buttonIsBeingHeld = true;
                yield return new WaitForEndOfFrame();
                Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
                lastMouseCoordinate = Input.mousePosition;
                if (buttonIsBeingHeld && ((mouseDelta.y > 0)))
                {
                    anim.SetTrigger("LeverUp");
                    FuseBox.fB.isOverloaded = false;
                    lightButtonBacklight.material.EnableKeyword("_EMISSION");
                    AudioManager.instance.PlaySFX(eSFX.leverPushPull, 0.2f);
                    AudioManager.instance.PlaySFX(eSFX.powerOn, 0.3f);
                    break;
                    //isInteractable = false;
                }
                else
                {
                    buttonIsBeingHeld = false;
                }
            }
        }
    }

    public void OnInteractEnd()
    {
        //do nothing
    }
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetTrigger("Up");
            isInteractable = true;
        }
    }*/
}
