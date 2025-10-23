using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michael
{
    public class PlayerController : Singleton<PlayerController>
    {
        PlayerControls controls;
        public PlayerControls Controls => controls;
        void OnEnable()
        {
            if(controls == null) controls = new PlayerControls();
            controls.Enable();
        }
        void OnDisable()
        {
            controls.Disable();
        }
        void Start()
        {
        
        }

    }
}
