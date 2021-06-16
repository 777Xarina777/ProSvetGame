using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using System.Linq;

public class TimeOfDay : MonoBehaviour
{
    # region States

    public enum States
    {
        Evening,
        Night,
        Morning
    }

    #endregion

    # region Fields

    StateMachine<States, GeneralDriver> _fsm;

    // TODO: delete later
    public static event System.Action<States> onTimeOfDayChange;

    // state change times fields
    [SerializeField] private float stateChangeTimer = 0;
    const float timeToChangeState = 3f;

    // night event system
    private GameObject _nightEventSystem;

    // pointer handlers
    [SerializeField] private Camera _mainCamera;
    private PointerHandler<DialogueInteraction> _dialoguePointerHandler;
    private PointerHandler<InteractableItem> _itemPointerHandler;

    #endregion

    private void Awake()
    {
        _fsm = new StateMachine<States, GeneralDriver>(this);
    }

    private void Start()
    {
        // _dialoguePointerHandler = new PointerHandler<DialogueInteraction>();
        // _itemPointerHandler = new PointerHandler<InteractableItem>();

        _nightEventSystem = GameObject.Find("TimeOfDay/NightEventSystem");
        _nightEventSystem.SetActive(false);
        

        _fsm.ChangeState(States.Evening);
    }

    void Evening_Enter()
    {
        Debug.Log("Evening Enter");
        onTimeOfDayChange?.Invoke(States.Evening);

         // give access to dialogue interaction
            // restrict access to interactable item
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
        onTimeOfDayChange?.Invoke(States.Night);

        
        

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
        onTimeOfDayChange?.Invoke(States.Morning);

        
        
        _nightEventSystem.SetActive(false);
    }

    void Morning_Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _fsm.ChangeState(States.Evening);
        }
    }

    # region State Machine Driver Methods

    private void Update()
    {
        _fsm.Driver.Update.Invoke();
    }

    # endregion

}
