using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace GalaxyGridiron
{
    public class SplashMenuUIController : MonoBehaviour
    {
        [SerializeField] int _delayInMilliseconds = 1000;
        [SerializeField] UnityEvent _mainEvent;

        private void Start()
        {
            Display();
        }

        private void Display()
        {
            StartCoroutine(DisplayCoroutine());
        }

        IEnumerator DisplayCoroutine()
        {
            yield return new WaitForSeconds(_delayInMilliseconds / 1000);
            _mainEvent?.Invoke();
        }
    }
}
