// GENERATED AUTOMATICALLY FROM 'Assets/InputSystem/Controles.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace InControlActions
{
    public class @ControlActions : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @ControlActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controles"",
    ""maps"": [
        {
            ""name"": ""Menu"",
            ""id"": ""c45bf314-f589-4410-a1f9-77de0e2c9c37"",
            ""actions"": [
                {
                    ""name"": ""Dpad"",
                    ""type"": ""Value"",
                    ""id"": ""112d6033-f7f9-47c1-ba99-08ccfed8b8ae"",
                    ""expectedControlType"": ""Dpad"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left Stick"",
                    ""type"": ""Value"",
                    ""id"": ""dd27b1b6-02c9-40a1-8d89-d4ac06c39835"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right Stick"",
                    ""type"": ""Value"",
                    ""id"": ""aa67b21a-f3a1-48f8-ad92-75d95ae5adfa"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Buttons"",
                    ""type"": ""Button"",
                    ""id"": ""51c25bcb-57cf-4f71-aee1-2f31e361403f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ExitButton"",
                    ""type"": ""Button"",
                    ""id"": ""de0068fc-5f65-402b-be4c-2cd40b764119"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MovementMouse"",
                    ""type"": ""Value"",
                    ""id"": ""5425875d-a4d1-4302-bde2-0006ca68515c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NorteButton"",
                    ""type"": ""Button"",
                    ""id"": ""3db8905a-88ae-4eec-98fc-71c1b8b84846"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WestButton"",
                    ""type"": ""Button"",
                    ""id"": ""1fb89e83-5d80-415a-9a0c-fc3a027af173"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4712d864-8bc7-4ccc-af76-a479fd95b7fb"",
                    ""path"": ""<WebGLGamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dpad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""5c098065-10fa-4574-988d-9bf303c35efb"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dpad"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b89323d2-8a04-4971-a64b-98babe55862d"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dpad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""dffd89a2-c439-4efc-af86-b2a78b9b6b02"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dpad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""bc8b51d6-32e3-431c-abf8-3c8fcd6e0aa4"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dpad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bad46e2c-2088-4842-a2b3-5827f623f654"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dpad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""af148e89-748f-427b-8fad-d935d83560e7"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a3fcc770-a598-427f-9f15-f767a4d6859b"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40f7558a-d8ec-4696-b85c-dd055e16485c"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Buttons"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cdfefd0e-3a01-4838-8c55-1d512410429a"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ExitButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f5258ef0-9def-4096-a816-6b45bec9d9b5"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementMouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d9fe3fe1-b93e-40bf-9c2f-2003911cfc7a"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NorteButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d3023d43-7bd5-499b-9f2b-4648f8f3d674"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WestButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Menu
            m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
            m_Menu_Dpad = m_Menu.FindAction("Dpad", throwIfNotFound: true);
            m_Menu_LeftStick = m_Menu.FindAction("Left Stick", throwIfNotFound: true);
            m_Menu_RightStick = m_Menu.FindAction("Right Stick", throwIfNotFound: true);
            m_Menu_Buttons = m_Menu.FindAction("Buttons", throwIfNotFound: true);
            m_Menu_ExitButton = m_Menu.FindAction("ExitButton", throwIfNotFound: true);
            m_Menu_MovementMouse = m_Menu.FindAction("MovementMouse", throwIfNotFound: true);
            m_Menu_NorteButton = m_Menu.FindAction("NorteButton", throwIfNotFound: true);
            m_Menu_WestButton = m_Menu.FindAction("WestButton", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Menu
        private readonly InputActionMap m_Menu;
        private IMenuActions m_MenuActionsCallbackInterface;
        private readonly InputAction m_Menu_Dpad;
        private readonly InputAction m_Menu_LeftStick;
        private readonly InputAction m_Menu_RightStick;
        private readonly InputAction m_Menu_Buttons;
        private readonly InputAction m_Menu_ExitButton;
        private readonly InputAction m_Menu_MovementMouse;
        private readonly InputAction m_Menu_NorteButton;
        private readonly InputAction m_Menu_WestButton;
        public struct MenuActions
        {
            private @ControlActions m_Wrapper;
            public MenuActions(@ControlActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Dpad => m_Wrapper.m_Menu_Dpad;
            public InputAction @LeftStick => m_Wrapper.m_Menu_LeftStick;
            public InputAction @RightStick => m_Wrapper.m_Menu_RightStick;
            public InputAction @Buttons => m_Wrapper.m_Menu_Buttons;
            public InputAction @ExitButton => m_Wrapper.m_Menu_ExitButton;
            public InputAction @MovementMouse => m_Wrapper.m_Menu_MovementMouse;
            public InputAction @NorteButton => m_Wrapper.m_Menu_NorteButton;
            public InputAction @WestButton => m_Wrapper.m_Menu_WestButton;
            public InputActionMap Get() { return m_Wrapper.m_Menu; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
            public void SetCallbacks(IMenuActions instance)
            {
                if (m_Wrapper.m_MenuActionsCallbackInterface != null)
                {
                    @Dpad.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnDpad;
                    @Dpad.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnDpad;
                    @Dpad.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnDpad;
                    @LeftStick.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeftStick;
                    @LeftStick.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeftStick;
                    @LeftStick.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeftStick;
                    @RightStick.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnRightStick;
                    @RightStick.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnRightStick;
                    @RightStick.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnRightStick;
                    @Buttons.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnButtons;
                    @Buttons.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnButtons;
                    @Buttons.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnButtons;
                    @ExitButton.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnExitButton;
                    @ExitButton.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnExitButton;
                    @ExitButton.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnExitButton;
                    @MovementMouse.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMovementMouse;
                    @MovementMouse.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMovementMouse;
                    @MovementMouse.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMovementMouse;
                    @NorteButton.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnNorteButton;
                    @NorteButton.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnNorteButton;
                    @NorteButton.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnNorteButton;
                    @WestButton.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnWestButton;
                    @WestButton.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnWestButton;
                    @WestButton.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnWestButton;
                }
                m_Wrapper.m_MenuActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Dpad.started += instance.OnDpad;
                    @Dpad.performed += instance.OnDpad;
                    @Dpad.canceled += instance.OnDpad;
                    @LeftStick.started += instance.OnLeftStick;
                    @LeftStick.performed += instance.OnLeftStick;
                    @LeftStick.canceled += instance.OnLeftStick;
                    @RightStick.started += instance.OnRightStick;
                    @RightStick.performed += instance.OnRightStick;
                    @RightStick.canceled += instance.OnRightStick;
                    @Buttons.started += instance.OnButtons;
                    @Buttons.performed += instance.OnButtons;
                    @Buttons.canceled += instance.OnButtons;
                    @ExitButton.started += instance.OnExitButton;
                    @ExitButton.performed += instance.OnExitButton;
                    @ExitButton.canceled += instance.OnExitButton;
                    @MovementMouse.started += instance.OnMovementMouse;
                    @MovementMouse.performed += instance.OnMovementMouse;
                    @MovementMouse.canceled += instance.OnMovementMouse;
                    @NorteButton.started += instance.OnNorteButton;
                    @NorteButton.performed += instance.OnNorteButton;
                    @NorteButton.canceled += instance.OnNorteButton;
                    @WestButton.started += instance.OnWestButton;
                    @WestButton.performed += instance.OnWestButton;
                    @WestButton.canceled += instance.OnWestButton;
                }
            }
        }
        public MenuActions @Menu => new MenuActions(this);
        public interface IMenuActions
        {
            void OnDpad(InputAction.CallbackContext context);
            void OnLeftStick(InputAction.CallbackContext context);
            void OnRightStick(InputAction.CallbackContext context);
            void OnButtons(InputAction.CallbackContext context);
            void OnExitButton(InputAction.CallbackContext context);
            void OnMovementMouse(InputAction.CallbackContext context);
            void OnNorteButton(InputAction.CallbackContext context);
            void OnWestButton(InputAction.CallbackContext context);
        }
    }
}
