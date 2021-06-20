﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public static class Helpers
{
    public static void ToggleGameObjectsCollection(List<GameObject> interactables, bool state)
        => interactables.ForEach(io => io.SetActive(state));

    public static void ToggleInteractableItemComponents(List<InteractableItem> items, bool state)
        => items.ForEach(i => i.enabled = state);

    public static void TogglePointerHandler(PointerHandler pointerHandler, bool state) =>
        pointerHandler.enabled = state;

    public static bool Reached(Transform current, Transform target) =>
        Vector2.Distance(current.position, target.position) < 1e-2;

    public static T EventArgsConvert<T>(EventArgs e) where T : EventArgs
    {
        if (e.GetType() == typeof(T))       // if extentable to inherited class
        {
            var re = e as T;
            return re;
        }

        throw new ArgumentException("Try to pass wrong argument type to EventArgsConvert Method." +
            $" {typeof(T)} class doesn't inherit from EventArgs class");
    }

    // public static void WalkAround()
    // {
    //     timer -= Time.fixedDeltaTime;

    //     if (timer < 0)
    //     {
    //         direction = -direction;
    //         timer = changeTime;
    //     }

    //     Vector2 objPos = _rb2d.position;
    //     objPos.x += _speed * Time.fixedDeltaTime * direction;
    //     _rb2d.MovePosition(objPos);
    // }

}
