using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using System;
using Newtonsoft.Json;

public class Manager : MonoBehaviour {


    public GameObject player;
    public SocketIOComponent socket;
    public Button conectar;
    public float id;

    public GameObject otherPlayersTemplate;

    private List<Player> otherPlayers = new List<Player>();

    public bool sincronizando = false;
	// Use this for initialization
	void Start () {


        socket.On("register", OnRegister);
        socket.On("sync", OnSync);
        //socket.On("open", OnConnect);
        conectar.onClick.AddListener(conectarSocket);
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            Formatting = Newtonsoft.Json.Formatting.Indented,
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        };

    }
    private void OnSync(SocketIOEvent obj)
    {
        print("sincronizando");
        print(obj.name);
        print(obj.data);
        var list = obj.data["players"].ToString();
        List<PlayerData> players = JsonConvert.DeserializeObject<List<PlayerData>>(list);
        print("lista de players");
        foreach (PlayerData p in players)
        {
            if (p.id != id)
            {
                updatePlayerOnList(p);
            }
        }
    }

    private void updatePlayerOnList(PlayerData p)
    {
        print("atualizando a lista de jogadores");
        bool locate = false;
        foreach (Player player in otherPlayers)
        {
            print("entrou o foreach");
            if (player.id == p.id)
            {
                print("Achou o jogador");
                player.gameObject.transform.position = p.position;
                player.gameObject.transform.rotation = p.rotate;
                locate = true;
            }
        }
        if (!locate)
        {
            print("criou o objeto");
            Player player = new Player();
            player.gameObject = (GameObject) Instantiate(otherPlayersTemplate,p.position,p.rotate);
            player.id = p.id;
            otherPlayers.Add(player);
        }
    }

    void conectarSocket()
    {
        socket.Connect();
    }

    void OnConnect(SocketIOEvent obj)
    {
        if(socket.IsConnected && socket.sid != null)
        {
            print("conectado");
            //socket.Emit("register");
        }
    }

    void OnRegister(SocketIOEvent obj)
    {
        id = obj.data["id"].f;
        print("registrado");
        print(id);
    }

    void updatePlayerOnSocket()
    {
        if(id == 0)
        {
            return;
        }
        PlayerData pd = new PlayerData();
        pd.id = id;
        pd.position = player.GetComponent<Rigidbody>().transform.position;
        pd.rotate = player.GetComponent<Rigidbody>().transform.rotation;
        JSONObject json = new JSONObject(JsonConvert.SerializeObject(pd));
        socket.Emit("updatePlayer", json);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(socket.IsConnected)
        {
            if (!sincronizando)
            {
                sincronizando = true;
                print("invoke");
                InvokeRepeating("updatePlayerOnSocket", 0f, 0.03f);
            }
        }else
        {
            sincronizando = false;
            CancelInvoke("updatePlayerOnSocket");
        }
    }

    class PlayerData
    {
        public Vector3 position { get; set; }
        public Quaternion rotate { get; set; }
        public float id;
    }

    class Player
    {
        public GameObject gameObject{ get; set; }
        public float id { get; set; }
    }
}
