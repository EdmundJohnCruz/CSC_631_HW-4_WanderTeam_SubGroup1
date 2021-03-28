using UnityEngine;
using Mirror;
using TMPro;
using System;

public class ChatBehavior : NetworkBehaviour
{
    /*Reference variables*/
    [SerializeField] private GameObject chatUI = null;
    [SerializeField] private TMP_Text chatText = null;
    [SerializeField] private TMP_InputField inputField = null;

    private static event Action<string> OnMessage; //Action that passes a string message through OnMessage to give each player have their own chat to handle

    public override void OnStartAuthority() //Called "on start" for objects that are owned by a by separate clients; each client has a unique chat UI!
    {
        chatUI.SetActive(true); //Turns on clients UI

        OnMessage += HandleNewMessage; //Subscribes to a new message being handled
    }

    [ClientCallback] //Clarifies that the functions below is being called back on the client, not the server
    private void OnDestroy()
    {
        if(!hasAuthority) { return; } //Does nothing if there is no authority

        OnMessage -= HandleNewMessage; //Unsubscribe if client has authority
    }
    private void HandleNewMessage(string message) //Deals with new message inputted in chat
    {
        chatText.text += message;
    }

    [Client]// For the client side only
    public void Send(string message)
    {
        /*Two functions below tell client don't do anything if Enter key isnt pressed or string "is null or white space"*/
        if (!Input.GetKeyDown(KeyCode.Return)) { return; }

        if (string.IsNullOrWhiteSpace(message)) { return; }

        CmdSendMessage(message); //Server sends the input field text, which is just the message

        inputField.text = string.Empty;//When message is sent, it clears the input field so that a user can type a new message
    }

    [Command]// For the server side only
    private void CmdSendMessage(string message)
    {
        RpcHandleMessage($"[{connectionToClient.connectionId}]: {message}"); //Send the message but with the clients ID to know who sent it
    }

    [ClientRpc]// For ALL clients, not just one client
    private void RpcHandleMessage(string message)
    {
        OnMessage?.Invoke($"\n{message}"); //Tells all clients to add a new line, then the message
    }
}
