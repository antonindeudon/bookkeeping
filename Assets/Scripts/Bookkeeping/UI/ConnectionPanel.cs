using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Bookkeeping
{
    public class ConnectionPanel : MonoBehaviour
    {
        public TextMeshProUGUI messageText;
        public string connectionFailedMessage;
        public GameObject buttonsContainer;

        public event Action onRetryClicked;

        public void SetConnecting(string message)
        {
            messageText.text = message;
            buttonsContainer.SetActive(false);
        }

        public void SetConnectionFailed(string errorMessage)
        {
            messageText.text = errorMessage;
            buttonsContainer.SetActive(true);
        }

        public void OnRetryClicked()
        {
            onRetryClicked?.Invoke();
        }
    }
}