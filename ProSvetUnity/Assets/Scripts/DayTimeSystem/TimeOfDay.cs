﻿using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using System.Linq;

public class TimeOfDay : MonoBehaviour
{
    public enum States
    {
        Evening,
        Night,
        Morning
    }

    StateMachine<States, GeneralDriver> _fsm;

    public static event System.Action<States, TimeOfDay> onTimeOfDayChange; 

    [SerializeField] private float stateChangeTimer = 0;

    const float timeToChangeState = 3f;

    private GameObject _nightEventSystem;

    private DialoguePointerHandler _dialoguePointerHandler;

    private ItemPointerHandler _itemPointerHandler;


    private void Awake()
    {
        _fsm = new StateMachine<States, GeneralDriver>(this);
    }

    private void Start()
    {
        _dialoguePointerHandler = Camera.main.GetComponent<DialoguePointerHandler>();
        _itemPointerHandler = Camera.main.GetComponent<ItemPointerHandler>();

        _nightEventSystem = transform.Find("NightEventSystem").gameObject;
        _nightEventSystem.SetActive(false);


        _fsm.ChangeState(States.Evening);
    }

    void Evening_Enter()
    {
        Debug.Log("Evening Enter");
        onTimeOfDayChange?.Invoke(States.Evening, this);

        Helpers.TogglePointerHandler(_dialoguePointerHandler, true); // give access to dialogue interaction
        Helpers.TogglePointerHandler(_itemPointerHandler, false);    // restrict access to interactable item
    }

    void Evening_Update()
    {
        // if all dialogs have been read, then go to another state (later)
        if ( DialogueManager.allChecked)
        {
            stateChangeTimer = timeToChangeState;
            DialogueManager.allChecked = false;
        }

        if (stateChangeTimer > 0)  stateChangeTimer -= Time.fixedDeltaTime;
        else if (stateChangeTimer < 0)
        {
            Debug.Log("ChangeStateToNight");
            
            stateChangeTimer = 0;       // reset fuild
            DialogueManager.allChecked = false;
            
            _fsm.ChangeState(States.Night);
        }
        
        if (Input.GetKeyDown(KeyCode.X))        // mock while
        {
            _fsm.ChangeState(States.Night);
        }
        
    }

    void Night_Enter()
    {
        Debug.Log("Enter Night");
        onTimeOfDayChange?.Invoke(States.Night, this);

        Helpers.TogglePointerHandler(_dialoguePointerHandler, false);
        Helpers.TogglePointerHandler(_itemPointerHandler, true);
        _nightEventSystem.SetActive(true);
    }

    void Night_Update()
    {
        // if all scriptable events has been passed then go to next phase
        if (Input.GetKeyDown(KeyCode.C))
        {
            _fsm.ChangeState(States.Morning);
        }
    }

    void Morning_Enter()
    {
        Debug.Log("Enter Morning");
        onTimeOfDayChange?.Invoke(States.Morning, this);

        Helpers.TogglePointerHandler(_dialoguePointerHandler, false);
        Helpers.TogglePointerHandler(_itemPointerHandler, false);
        _nightEventSystem.SetActive(false);
    }

    void Morning_Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _fsm.ChangeState(States.Evening);
        }
    }

    // ---------- Class methods ----------
    private void Update()
    {
        _fsm.Driver.Update.Invoke();
    }

    // private void FixedUpdate()
    // {
    //     _fsm.Driver.FixedUpdate.Invoke();
    // }

}