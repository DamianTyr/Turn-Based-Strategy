using TMPro;
using UnityEngine;

namespace Grid
{
    public class GridDebugObject : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textMeshPro;
        private object _gridObject;

        public virtual void SetGridObject(object gridObject)
        {
            _gridObject = gridObject;
        }

        protected virtual void Update()
        {
            _textMeshPro.text = _gridObject.ToString();
        }
    }
}
