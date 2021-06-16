using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerControl : MonoBehaviour
{
    public static Vector2 MouseTarget => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    PointerHandler<IPointerHandler> _itemPointerHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        _itemPointerHandler = new PointerHandler<IPointerHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        var interactableHit = Physics2D.Raycast(MouseTarget, Vector2.zero, 0f, LayerMask.GetMask("Interactable"));
        _itemPointerHandler.HandleSinglePointer(interactableHit);
    }

    // public void RegisterPointerHandler<T>(PointerHandler<T> pointerHandler) where T : IPointerHandler
    // {

    // }
}
