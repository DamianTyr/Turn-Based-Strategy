using UnityEngine;
using System;
using UnityEngine.EventSystems;


namespace PlayerInput
{
    public class MouseInputHandler : MonoBehaviour
    {
        [Serializable]
        public struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float raycastRadius = 1f;
        [SerializeField] LayerMask layersToIgnoreForComponent;
        
        static IRaycastable currentRaycastTarget;
        
        private void Update()
        {
            if (InteractWithUI()) return;
            if (InteractWithComponent()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                HandleRaycastTargetChange(null);
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                if (raycastables.Length == 0) HandleRaycastTargetChange(null);

                foreach (IRaycastable raycastable in raycastables)
                {               
                    //TODO below = and different from current Raycastable
                    if (raycastable.HandleRaycast(this,hit))
                    {
                        if (InputManager.Instance.IsMouseButtonDownThisFrame())
                        {
                            raycastable.HandleMouseClick();    
                        }
                        HandleRaycastTargetChange(raycastable);
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }
        
        private void HandleRaycastTargetChange(IRaycastable _raycastable)
        {
            if (_raycastable == null)
            {
                if (currentRaycastTarget == null) return;             
                currentRaycastTarget.HandleRaycastStop();
                currentRaycastTarget = null;
            }
            else
            {   
                if (currentRaycastTarget != null)
                {
                    if (currentRaycastTarget == _raycastable) return;
                    currentRaycastTarget.HandleRaycastStop();
                    currentRaycastTarget = _raycastable;
                }
                else
                {
                    currentRaycastTarget = _raycastable;
                }
            }
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius,Mathf.Infinity, ~layersToIgnoreForComponent);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }
        
        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            return ray;
        }
    }
}
