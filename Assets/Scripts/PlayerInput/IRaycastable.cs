using UnityEngine;

namespace PlayerInput
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool HandleRaycast(MouseInputHandler callingController, RaycastHit hit);

        void HandleRaycastStop();

        void HandleMouseClick();
    }
}