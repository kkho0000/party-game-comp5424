using UnityEngine;
using Unity.Netcode;
public class GameManager : MonoBehaviour
{
private void Update()
{
if (Input.GetKeyDown(KeyCode.O))
{
NetworkManager.Singleton.StartHost();
}
if (Input.GetKeyDown(KeyCode.P))
{
NetworkManager.Singleton.StartClient();
}
}
}
