//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/InventoryInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InventoryInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InventoryInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InventoryInput"",
    ""maps"": [
        {
            ""name"": ""Inventory"",
            ""id"": ""77f7c36b-618a-454d-8ffd-5258f1d0dc9d"",
            ""actions"": [
                {
                    ""name"": ""Open_Close"",
                    ""type"": ""Button"",
                    ""id"": ""bf61ae88-eb77-4623-b3be-0f1ec8a78f93"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""792cf822-2871-4d79-97a7-7d0bb90832a8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fb390202-6659-4fe7-b9c3-2b2db93f392e"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Open_Close"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7699b523-4f0f-4d80-8ca5-150aaef97a80"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""802696f4-764b-4afa-91f0-d89d7485e5f9"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8db7a757-7719-4edb-8048-6dc1f5ffdffa"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Test"",
            ""id"": ""0432bc3a-26c7-424f-8ed3-704363574479"",
            ""actions"": [
                {
                    ""name"": ""Test1"",
                    ""type"": ""Button"",
                    ""id"": ""5796d393-be76-47a8-8f33-baf8fb88c65d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test2"",
                    ""type"": ""Button"",
                    ""id"": ""e4a2a8e3-8e1e-4259-8d3e-a888a7aed36b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test3"",
                    ""type"": ""Button"",
                    ""id"": ""a120a88c-b085-430c-9978-21b3df7cb85e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test4"",
                    ""type"": ""Button"",
                    ""id"": ""d3f4e0ec-a1e2-465d-8af5-51ffa6b8dd9d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""eb385af7-9513-431b-bda1-bee9df437f8e"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a1934d1c-d7f0-4282-8b16-f659c6ff0eb7"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""314f6626-8716-4f87-894a-cdfae561d975"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""268c0ada-6bba-4298-b396-2fec77503c3d"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Ingame"",
            ""id"": ""19320963-5d11-438b-888c-5e61a0fd5793"",
            ""actions"": [
                {
                    ""name"": ""QuickSlot4"",
                    ""type"": ""Button"",
                    ""id"": ""b19d2b18-729f-4aa7-8631-1fa5253bf3d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""QuickSlot3"",
                    ""type"": ""Button"",
                    ""id"": ""5c5e51aa-29e7-404f-aea0-4719a7cd83f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""QuickSlot2"",
                    ""type"": ""Button"",
                    ""id"": ""7ec15c4b-3e61-414b-87f2-e05bcea0b007"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""QuickSlot1"",
                    ""type"": ""Button"",
                    ""id"": ""22c0ba9a-a86b-4ce1-9e2e-a392b7390ab4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MapToggle"",
                    ""type"": ""Button"",
                    ""id"": ""00297565-51e7-45cd-9d38-4b0190bb928b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4f7545f3-9edd-418d-9100-381363ffe110"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSlot1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""acd8ac2c-65ed-4e40-a116-ffa953ec002b"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSlot2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38d02a0e-7a76-4861-b86f-35abfdf4487d"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSlot3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9317021e-6f48-4bff-996f-138a41e70b1f"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MapToggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""412ad50b-c116-4ff0-872b-6376029636fb"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSlot4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""New control scheme"",
            ""bindingGroup"": ""New control scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Inventory
        m_Inventory = asset.FindActionMap("Inventory", throwIfNotFound: true);
        m_Inventory_Open_Close = m_Inventory.FindAction("Open_Close", throwIfNotFound: true);
        m_Inventory_Click = m_Inventory.FindAction("Click", throwIfNotFound: true);
        // Test
        m_Test = asset.FindActionMap("Test", throwIfNotFound: true);
        m_Test_Test1 = m_Test.FindAction("Test1", throwIfNotFound: true);
        m_Test_Test2 = m_Test.FindAction("Test2", throwIfNotFound: true);
        m_Test_Test3 = m_Test.FindAction("Test3", throwIfNotFound: true);
        m_Test_Test4 = m_Test.FindAction("Test4", throwIfNotFound: true);
        // Ingame
        m_Ingame = asset.FindActionMap("Ingame", throwIfNotFound: true);
        m_Ingame_QuickSlot4 = m_Ingame.FindAction("QuickSlot4", throwIfNotFound: true);
        m_Ingame_QuickSlot3 = m_Ingame.FindAction("QuickSlot3", throwIfNotFound: true);
        m_Ingame_QuickSlot2 = m_Ingame.FindAction("QuickSlot2", throwIfNotFound: true);
        m_Ingame_QuickSlot1 = m_Ingame.FindAction("QuickSlot1", throwIfNotFound: true);
        m_Ingame_MapToggle = m_Ingame.FindAction("MapToggle", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Inventory
    private readonly InputActionMap m_Inventory;
    private List<IInventoryActions> m_InventoryActionsCallbackInterfaces = new List<IInventoryActions>();
    private readonly InputAction m_Inventory_Open_Close;
    private readonly InputAction m_Inventory_Click;
    public struct InventoryActions
    {
        private @InventoryInput m_Wrapper;
        public InventoryActions(@InventoryInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Open_Close => m_Wrapper.m_Inventory_Open_Close;
        public InputAction @Click => m_Wrapper.m_Inventory_Click;
        public InputActionMap Get() { return m_Wrapper.m_Inventory; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InventoryActions set) { return set.Get(); }
        public void AddCallbacks(IInventoryActions instance)
        {
            if (instance == null || m_Wrapper.m_InventoryActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_InventoryActionsCallbackInterfaces.Add(instance);
            @Open_Close.started += instance.OnOpen_Close;
            @Open_Close.performed += instance.OnOpen_Close;
            @Open_Close.canceled += instance.OnOpen_Close;
            @Click.started += instance.OnClick;
            @Click.performed += instance.OnClick;
            @Click.canceled += instance.OnClick;
        }

        private void UnregisterCallbacks(IInventoryActions instance)
        {
            @Open_Close.started -= instance.OnOpen_Close;
            @Open_Close.performed -= instance.OnOpen_Close;
            @Open_Close.canceled -= instance.OnOpen_Close;
            @Click.started -= instance.OnClick;
            @Click.performed -= instance.OnClick;
            @Click.canceled -= instance.OnClick;
        }

        public void RemoveCallbacks(IInventoryActions instance)
        {
            if (m_Wrapper.m_InventoryActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IInventoryActions instance)
        {
            foreach (var item in m_Wrapper.m_InventoryActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_InventoryActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public InventoryActions @Inventory => new InventoryActions(this);

    // Test
    private readonly InputActionMap m_Test;
    private List<ITestActions> m_TestActionsCallbackInterfaces = new List<ITestActions>();
    private readonly InputAction m_Test_Test1;
    private readonly InputAction m_Test_Test2;
    private readonly InputAction m_Test_Test3;
    private readonly InputAction m_Test_Test4;
    public struct TestActions
    {
        private @InventoryInput m_Wrapper;
        public TestActions(@InventoryInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Test1 => m_Wrapper.m_Test_Test1;
        public InputAction @Test2 => m_Wrapper.m_Test_Test2;
        public InputAction @Test3 => m_Wrapper.m_Test_Test3;
        public InputAction @Test4 => m_Wrapper.m_Test_Test4;
        public InputActionMap Get() { return m_Wrapper.m_Test; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TestActions set) { return set.Get(); }
        public void AddCallbacks(ITestActions instance)
        {
            if (instance == null || m_Wrapper.m_TestActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_TestActionsCallbackInterfaces.Add(instance);
            @Test1.started += instance.OnTest1;
            @Test1.performed += instance.OnTest1;
            @Test1.canceled += instance.OnTest1;
            @Test2.started += instance.OnTest2;
            @Test2.performed += instance.OnTest2;
            @Test2.canceled += instance.OnTest2;
            @Test3.started += instance.OnTest3;
            @Test3.performed += instance.OnTest3;
            @Test3.canceled += instance.OnTest3;
            @Test4.started += instance.OnTest4;
            @Test4.performed += instance.OnTest4;
            @Test4.canceled += instance.OnTest4;
        }

        private void UnregisterCallbacks(ITestActions instance)
        {
            @Test1.started -= instance.OnTest1;
            @Test1.performed -= instance.OnTest1;
            @Test1.canceled -= instance.OnTest1;
            @Test2.started -= instance.OnTest2;
            @Test2.performed -= instance.OnTest2;
            @Test2.canceled -= instance.OnTest2;
            @Test3.started -= instance.OnTest3;
            @Test3.performed -= instance.OnTest3;
            @Test3.canceled -= instance.OnTest3;
            @Test4.started -= instance.OnTest4;
            @Test4.performed -= instance.OnTest4;
            @Test4.canceled -= instance.OnTest4;
        }

        public void RemoveCallbacks(ITestActions instance)
        {
            if (m_Wrapper.m_TestActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ITestActions instance)
        {
            foreach (var item in m_Wrapper.m_TestActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_TestActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public TestActions @Test => new TestActions(this);

    // Ingame
    private readonly InputActionMap m_Ingame;
    private List<IIngameActions> m_IngameActionsCallbackInterfaces = new List<IIngameActions>();
    private readonly InputAction m_Ingame_QuickSlot4;
    private readonly InputAction m_Ingame_QuickSlot3;
    private readonly InputAction m_Ingame_QuickSlot2;
    private readonly InputAction m_Ingame_QuickSlot1;
    private readonly InputAction m_Ingame_MapToggle;
    public struct IngameActions
    {
        private @InventoryInput m_Wrapper;
        public IngameActions(@InventoryInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @QuickSlot4 => m_Wrapper.m_Ingame_QuickSlot4;
        public InputAction @QuickSlot3 => m_Wrapper.m_Ingame_QuickSlot3;
        public InputAction @QuickSlot2 => m_Wrapper.m_Ingame_QuickSlot2;
        public InputAction @QuickSlot1 => m_Wrapper.m_Ingame_QuickSlot1;
        public InputAction @MapToggle => m_Wrapper.m_Ingame_MapToggle;
        public InputActionMap Get() { return m_Wrapper.m_Ingame; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(IngameActions set) { return set.Get(); }
        public void AddCallbacks(IIngameActions instance)
        {
            if (instance == null || m_Wrapper.m_IngameActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_IngameActionsCallbackInterfaces.Add(instance);
            @QuickSlot4.started += instance.OnQuickSlot4;
            @QuickSlot4.performed += instance.OnQuickSlot4;
            @QuickSlot4.canceled += instance.OnQuickSlot4;
            @QuickSlot3.started += instance.OnQuickSlot3;
            @QuickSlot3.performed += instance.OnQuickSlot3;
            @QuickSlot3.canceled += instance.OnQuickSlot3;
            @QuickSlot2.started += instance.OnQuickSlot2;
            @QuickSlot2.performed += instance.OnQuickSlot2;
            @QuickSlot2.canceled += instance.OnQuickSlot2;
            @QuickSlot1.started += instance.OnQuickSlot1;
            @QuickSlot1.performed += instance.OnQuickSlot1;
            @QuickSlot1.canceled += instance.OnQuickSlot1;
            @MapToggle.started += instance.OnMapToggle;
            @MapToggle.performed += instance.OnMapToggle;
            @MapToggle.canceled += instance.OnMapToggle;
        }

        private void UnregisterCallbacks(IIngameActions instance)
        {
            @QuickSlot4.started -= instance.OnQuickSlot4;
            @QuickSlot4.performed -= instance.OnQuickSlot4;
            @QuickSlot4.canceled -= instance.OnQuickSlot4;
            @QuickSlot3.started -= instance.OnQuickSlot3;
            @QuickSlot3.performed -= instance.OnQuickSlot3;
            @QuickSlot3.canceled -= instance.OnQuickSlot3;
            @QuickSlot2.started -= instance.OnQuickSlot2;
            @QuickSlot2.performed -= instance.OnQuickSlot2;
            @QuickSlot2.canceled -= instance.OnQuickSlot2;
            @QuickSlot1.started -= instance.OnQuickSlot1;
            @QuickSlot1.performed -= instance.OnQuickSlot1;
            @QuickSlot1.canceled -= instance.OnQuickSlot1;
            @MapToggle.started -= instance.OnMapToggle;
            @MapToggle.performed -= instance.OnMapToggle;
            @MapToggle.canceled -= instance.OnMapToggle;
        }

        public void RemoveCallbacks(IIngameActions instance)
        {
            if (m_Wrapper.m_IngameActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IIngameActions instance)
        {
            foreach (var item in m_Wrapper.m_IngameActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_IngameActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public IngameActions @Ingame => new IngameActions(this);
    private int m_NewcontrolschemeSchemeIndex = -1;
    public InputControlScheme NewcontrolschemeScheme
    {
        get
        {
            if (m_NewcontrolschemeSchemeIndex == -1) m_NewcontrolschemeSchemeIndex = asset.FindControlSchemeIndex("New control scheme");
            return asset.controlSchemes[m_NewcontrolschemeSchemeIndex];
        }
    }
    public interface IInventoryActions
    {
        void OnOpen_Close(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
    }
    public interface ITestActions
    {
        void OnTest1(InputAction.CallbackContext context);
        void OnTest2(InputAction.CallbackContext context);
        void OnTest3(InputAction.CallbackContext context);
        void OnTest4(InputAction.CallbackContext context);
    }
    public interface IIngameActions
    {
        void OnQuickSlot4(InputAction.CallbackContext context);
        void OnQuickSlot3(InputAction.CallbackContext context);
        void OnQuickSlot2(InputAction.CallbackContext context);
        void OnQuickSlot1(InputAction.CallbackContext context);
        void OnMapToggle(InputAction.CallbackContext context);
    }
}
