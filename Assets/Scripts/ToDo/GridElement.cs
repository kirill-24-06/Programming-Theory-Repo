using UnityEngine;

namespace Match3
{
    public class GridElement : MonoBehaviour
    {
        [SerializeField] private Element _element;

        public Element ElementType => _element;
    }
}
