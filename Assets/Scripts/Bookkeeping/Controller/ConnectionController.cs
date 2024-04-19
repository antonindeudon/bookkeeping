using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bookkeeping
{
    public class ConnectionController : MonoBehaviour
    {
        public ConnectionPanel connectionPanel;
        public string errorMessage;
        public float artificialRetryDelay;

        private string message;
        private Action operation;

        private void Awake()
        {
            connectionPanel.onRetryClicked += OnRetryClicked;
        }

        public void TryConnecting(string message, Action operation)
        {
            this.message = message;
            this.operation = operation;
            connectionPanel.gameObject.SetActive(true);
            TryConnecting();
        }

        private void TryConnecting()
        {
            connectionPanel.SetConnecting(message);
            StartCoroutine(TryConnectingCoroutine());
        }

        private IEnumerator TryConnectingCoroutine()
        {
            yield return new WaitForSeconds(artificialRetryDelay);

            try
            {
                operation.Invoke();
                connectionPanel.gameObject.SetActive(false);
            }
            catch(ConnectionException exception)
            {
                Debug.LogError(exception);
                connectionPanel.SetConnectionFailed(errorMessage);
            }
        }

        private void OnRetryClicked()
        {
            TryConnecting();
        }
    }
}