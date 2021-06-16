using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointerHandler<T> where T : IPointerHandler
{
    private IPointerHandler _currentClickable, _previousClickable;

    public void HandleSinglePointer(RaycastHit2D rayHitObj)
    {
        if (rayHitObj && rayHitObj.collider.TryGetComponent(out T item))                // if hover any IClickable
        {
            _previousClickable = _currentClickable;                                     // keep previous item
            _currentClickable = item;                                                   // set new current item

            if (_previousClickable != _currentClickable)
                item.OnPointerEnter();


            if (Input.GetMouseButtonDown(0))
            {
                item.OnPointerButtonClick();
            }

            // if (Input.GetMouseButton(0))
            // {
            //     item.OnPointerButtonHold();
            // }
        }
        else if (_currentClickable != null)                                             // if stop hover any IClickable
        {
            _currentClickable.OnPointerExit();
            _currentClickable = null;
        }

        // в том случае если хит продолжается, но объекты стоят вплотную 
        if (rayHitObj && _previousClickable != null && _previousClickable != _currentClickable)
        {
            _previousClickable.OnPointerExit();
            _previousClickable = null;
        }
    }
}
