using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameControl : MonoBehaviour {

	//全局静态变量
	int PlayerCardZ = 1;

	//全局变量
	public int PlayerNum = 0,DeadNum;
	int[] Toggles = new int[20];
	public bool CanMove=true, CanMove_hode, CanClick;
	public string GameStage, GameMode;
	public int Lover,MarkID;
	//预设体
	public GameObject PlayerCard, PlayerName,DeadMark;
	//全局对象
	public GameObject ConfigUI,PlayerUI,PlayerUIConfig,ChoosePlayer,ShowInfo,ShowPlayer,GoNext,PlayerNow,GameStatus,SpeakContent;

	//局部变量
	GameObject[] Player = new GameObject[20],Lovers = new GameObject[2],Deaded= new GameObject[20];
	//string[] PlayerRole = new string[20];
	//string[] RoleList = {"狼人","狼人","狼人","狼人","先知","女巫","守卫","丘比特","猎人","长老","村民","村民","村民","村民","村民","村民","村民","村民"};
	GameObject jisha,shouhu,dusha,toupiao,jingzhang;
	bool jieyaoyongle,duyaoyongle,shiyongjieyao,shaguozhanglao,diyiye,jingzhangsiwang,lierensiwang;
	string jieguo,time;
	int day;

	void Start () {
		//初始化游戏界面
		CanClick = true;
		ConfigUI.SetActive (false);
		PlayerUI.SetActive (false);
		ChoosePlayer.SetActive (false);
		ShowInfo.SetActive (false);
		ShowPlayer.SetActive (false);
		GameStage = "准备开始";
		GameMode = "God";
		GameObject.Find ("MainCanvas/NextStage").GetComponent<Button> ().interactable=false;
	}




	//--------------------------------游戏中配置玩家信息
	public void SetCupid()
	{
		PlayerNow.GetComponent<PlayerCard> ().Role = "丘比特";
		SelectPlayer (PlayerNow);
	}
	public void SetLover()
	{
		PlayerNow.GetComponent<PlayerCard> ().IsLover = !PlayerNow.GetComponent<PlayerCard> ().IsLover;
		SelectPlayer (PlayerNow);
	}
	public void SetWolf()
	{
		PlayerNow.GetComponent<PlayerCard> ().Role = "狼人";
		SelectPlayer (PlayerNow);
	}
	public void SetWitch()
	{
		PlayerNow.GetComponent<PlayerCard> ().Role = "女巫";
		SelectPlayer (PlayerNow);
	}
	public void SetKill()
	{
		if (jisha != PlayerNow)
			jisha = PlayerNow;
		else
			jisha = null;
		SelectPlayer (PlayerNow);
	}
	public void SetPoison()
	{
		if (dusha != PlayerNow)
			dusha = PlayerNow;
		else
			dusha = null;
		SelectPlayer (PlayerNow);
	}

	public void SelectPlayer(GameObject P)
	{
		PlayerUIConfig.SetActive (true);
		CanClick = false;
		string str;
		str = P.GetComponent<PlayerCard> ().PlayerID.ToString () + "号玩家：" + P.GetComponent<PlayerCard> ().Name;
		if (P.GetComponent<PlayerCard> ().IsAlive)
			str += "，存活\n";
		else
			str += "，死亡\n";
		str+= "身份："+P.GetComponent<PlayerCard>().Role;
		if (P.GetComponent<PlayerCard> ().IsLover)
			str += "、情侣";
		str += "\n";
		if (jisha == P)
			str += "被击杀";
		if (dusha == P)
			str += "被毒杀";
		GameObject.Find ("PlayerConfig/Text").GetComponent<Text> ().text = str;
	}




	//--------------------------------游戏进度控制
	//游戏开始->天黑阶段
	public void GameStart(){
		if (GameStage != "准备开始")
			return;
//		if (PlayerNum < 1) {
//			GameStatus.GetComponent<Text>().text="压根没玩家啊！";
//			return;
//		}
//		if (Toggles [0] > PlayerNum) {
//			GameStatus.GetComponent<Text>().text="角色数量大于玩家数量";
//			return;
//		}
//		if (Toggles [0] < PlayerNum) {
//			GameStatus.GetComponent<Text>().text="角色数量小于玩家数量";
//			return;
//		}
		ConfigUI.SetActive (false);
		PlayerUI.SetActive (false);
		PlayerUIConfig.SetActive (true);
		if (Toggles [8] == 0) {//有丘比特
			GameObject.Find ("PlayerConfig/Cupid").GetComponent<Button> ().interactable = false;
			GameObject.Find ("PlayerConfig/Lover").GetComponent<Button> ().interactable = false;
		} else {
			GameObject.Find ("PlayerConfig/Cupid").GetComponent<Button> ().interactable = true;
			GameObject.Find ("PlayerConfig/Lover").GetComponent<Button> ().interactable = true;
		}
		if (Toggles [6] == 0) {//有丘比特
			GameObject.Find ("PlayerConfig/Witch").GetComponent<Button> ().interactable = false;
		} else {
			GameObject.Find ("PlayerConfig/Witch").GetComponent<Button> ().interactable = true;
		}
		PlayerUIConfig.SetActive (false);

		GameStatus.GetComponent<Text>().text="游戏开始";
		//初始化游戏参数
		DeadNum =0;
		day = 0;
		Lover = 0;
		Lovers [0] = null;
		Lovers [1] = null;
		jisha = null;
		shouhu = null;
		dusha = null;
		toupiao = null;
		jingzhang = null;
		jieyaoyongle = false;
		duyaoyongle = false;
		shiyongjieyao = false;
		shaguozhanglao = false;
		jingzhangsiwang = false;
		lierensiwang = false;
		time = "";
		diyiye = true;
		//		//根据配置获得角色列表
		//		int i, j;
		//		j = 0;
		//		for (i = 1; i <= 18; i++) {
		//			if (Toggles [i] == 1) {
		//				PlayerRole [j] = RoleList [i-1];
		//				j++;
		//			}
		//		}
		//		//随机给每个玩家分配角色
		//		int rand;
		//		for(i=0;i<PlayerNum;i++){
		//			rand = Random.Range (0, PlayerNum - i);
		//			Player[i].GetComponent<PlayerCard> ().Role = PlayerRole [rand];
		//			Player [i].GetComponent<PlayerCard> ().IsAlive = true;
		//			if (PlayerRole [rand] == "女巫")
		//				nvwu = Player [i];
		//			for(j=rand;j<PlayerNum-1;j++){
		//				PlayerRole[j]=PlayerRole[j+1];
		//			}
		//		}
		//		GameStatus.GetComponent<Text>().text="请点击自己的卡牌查看身份";
		//		GameStage = "查看身份";
		//		CanClick = true;
		//重置每个玩家身份为未标记
		int i;
		for (i = 0; i < PlayerNum; i++) {
			Player [i].GetComponent<PlayerCard> ().Role = "未标记";
			Player [i].GetComponent<PlayerCard> ().IsAlive = true;
			Player [i].GetComponent<PlayerCard> ().IsLover = false;
		}
		
		GameObject.Find ("MainCanvas/Mark").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/Gamestart").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/GameExit").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/Addplayer").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/GameConfigButton").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/NextStage").GetComponent<Button> ().interactable=true;
		//		SortByRole ();
		Stage_tianhei ();
	}

	//天黑阶段->丘比特阶段
	void Stage_tianhei(){
		GameStage = "天黑";
		SpeakContent.GetComponent<Text>().text="天黑请闭眼";
		GameStatus.GetComponent<Text>().text="3秒后自动转到下个阶段";
		CanClick = true;
		GoNext.GetComponent<Button> ().interactable=false;
		Invoke ("EnableNextStage", 3.0f);
		Invoke ("Stage_qiubite", 3.0f);
	}

	//丘比特阶段->等上帝点下一步->情侣阶段
	void Stage_qiubite(){
		GameStage = "丘比特";
		if (Toggles [8] == 0||diyiye==false) {
			Invoke ("Stage_langren", 0.0f);
			return;
		}
		SpeakContent.GetComponent<Text>().text="1.丘比特请睁眼\n2.请选择情侣\n3.丘比特请闭眼";
		GameStatus.GetComponent<Text>().text="1.标记丘比特\n2.标记情侣\n3.完成后点击下一步";
		CanClick = true;
		GoNext.GetComponent<Button> ().interactable=true;
	}

	//情侣阶段->等上帝点下一步->狼人阶段
	void Stage_qinglv(){
		GameStage = "情侣";
		SpeakContent.GetComponent<Text>().text="1.请伸手\n2.被碰到的情侣请睁眼\n3.情侣请闭眼";
		GameStatus.GetComponent<Text>().text="1.绕场一周，触碰情侣\n2.等情侣互相确认\n3.完成后点击下一步";
	}

	//狼人阶段->等上帝点下一步->女巫阶段
	void Stage_langren(){
		GameStage = "狼人";
		SpeakContent.GetComponent<Text>().text="1.狼人请睁眼\n2.狼人请确定杀人目标\n3.狼人请闭眼";
		GameStatus.GetComponent<Text>().text="1.标记狼人(第一夜)\n2.标记被杀目标\n3.平票或标记完成后点击下一步";
		jisha = null;
		CanClick = true;
	}
	//女巫解药->等上帝点下一步->女巫毒药
	void Stage_nvwu_jieyao(){
		string str;
		GameStage = "女巫解药";
		if (Toggles [6] == 0) {
			Invoke ("Stage_shouwei", 0.0f);
			return;
		}
		str = "1.女巫请睁眼\n2.";
		if (jisha != null) {
			str += jisha.GetComponent<PlayerCard> ().PlayerID.ToString () + jisha.GetComponent<PlayerCard> ().Name + "(比手势)被杀了，是否救他\n";
		} else
			str += "无人被杀(摇头)，是否救他";
		SpeakContent.GetComponent<Text>().text= str;
		GameStatus.GetComponent<Text>().text="1.标记女巫(第一夜)\n2.选择是否救人(女巫除第一晚外不能自救)";
		ChoosePlayer.SetActive (true);
		GameObject.Find ("ChoosePlayer/Text").GetComponent<Text>().text= "使用解药？";
		GameObject.Find ("ChoosePlayer/YES").GetComponent<Button> ().interactable=true;
		GameObject.Find ("ChoosePlayer/NO").GetComponent<Button> ().interactable=false;
		//解药没了
		if (jieyaoyongle == true||jisha==null) {
			GameObject.Find ("ChoosePlayer/YES").GetComponent<Button> ().interactable=false;
		}
		CanClick = true;
		shiyongjieyao = false;

	}
	//女巫毒药->等上帝点下一步->守卫阶段
	void Stage_nvwu_duyao(){
		GameStage = "女巫毒药";
		dusha = null;
		CanClick = true;
		if (Toggles [6] == 0) {
			Invoke ("Stage_shouwei", 0.0f);
			return;
		}
		SpeakContent.GetComponent<Text>().text="1.女巫请选择毒杀目标\n2.女巫请闭眼";
		GameStatus.GetComponent<Text>().text="1.标记被毒杀的人\n2.完成后点击下一步";
		CanClick = true;
	}
	//守卫阶段->等上帝点下一步->先知阶段
	void Stage_shouwei(){
		GameStage = "守卫";
		if (Toggles [7] == 0) {
			MoveOn ();
			return;
		}
		GameStatus.GetComponent<Text> ().text = "守卫请睁眼，选择守护目标";
		CanClick = true;
	}



	//上帝点下一步
	public void NextStage(){
		GoNext.GetComponent<Button> ().interactable=false;
		PlayerUIConfig.SetActive (false);
		Invoke ("EnableNextStage", 1.0f);
		CanClick = false;
		//丘比特》情侣
		if (GameStage == "丘比特") {
			Invoke ("Stage_qinglv", 0.0f);
			return;
		}
		//情侣》狼人
		if (GameStage == "情侣") {
			Invoke ("Stage_langren", 0.0f);
			return;
		}
		//狼人》女巫
		if (GameStage == "狼人") {
			Invoke ("Stage_nvwu_jieyao", 0.0f);
			return;
		}
		//女巫解药》女巫毒药
		if (GameStage == "女巫解药") {
			if (GameObject.Find ("ChoosePlayer/NO").GetComponent<Button> ().interactable == true) {
				shiyongjieyao = true;
				jieyaoyongle = true;
			}
			Invoke ("Stage_nvwu_duyao", 0.0f);
			ChoosePlayer.SetActive (false);
			return;
		}
		//女巫毒药》守卫
		if (GameStage == "女巫毒药") {
			if (dusha != null) {
				duyaoyongle = true;
				PlayerUIConfig.SetActive (true);
				GameObject.Find ("PlayerConfig/Poison").GetComponent<Button> ().interactable = false;
				PlayerUIConfig.SetActive (false);
			}
			Invoke ("Stage_shouwei", 0.0f);
			return;
		}

		if (GameStage == "守卫") {
			shouhu = null;
			GameStatus.GetComponent<Text>().text="守卫请闭眼";
			CanClick = false;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 2.0f);
			return;
		}
		if (GameStage == "先知") {
			GameStatus.GetComponent<Text>().text="先知请闭眼";
			CanClick = false;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 2.0f);
			return;
		}
		if (GameStage == "警长") {
			GameStatus.GetComponent<Text>().text="警长平票，无警长";
			jingzhang = null;
			CanClick = false;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 2.0f);
			return;
		}
		if (GameStage == "夜晚结算") {
			GoNext.GetComponent<Button> ().interactable=false;
			CanClick = false;
			MoveOn ();
			return;
		}
		if (GameStage == "讨论") {
			GoNext.GetComponent<Button> ().interactable=false;
			MoveOn ();
			return;
		}
		if (GameStage == "选狼人") {
			toupiao = null;
			GoNext.GetComponent<Button> ().interactable=false;
			MoveOn ();
			return;
		}
		if (GameStage == "白天结算") {
			ShowInfo.SetActive (false);
			if (jingzhangsiwang||lierensiwang) {
				MoveOn ();
			} else {
				GameStatus.GetComponent<Text> ().text = "天黑请闭眼";
				GoNext.GetComponent<Button> ().interactable=false;
				Invoke ("MoveOn", 3.0f);
			}

			return;
		}
		if (GameStage == "转移警长") {
			GameStatus.GetComponent<Text> ().text = "警徽被撕，无警长";
			jingzhang = null;
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 3.0f);
			return;
		}
		if (GameStage == "猎人") {
			GameStatus.GetComponent<Text> ().text = "猎人放弃";
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("MoveOn", 2.0f);
			return;
		}
	}

	//-------------------------------游戏界面控制




	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)||Input.GetKeyDown (KeyCode.KeypadEnter)) {
			PlayerInfoExit ();
		}
	}
	//添加角色
	public void AddPlayer(){
		Vector3 Position = new Vector3(0, 0, PlayerCardZ);
		Vector3 offset = new Vector3(0, -0.5f, 0);
		Player [PlayerNum] = (GameObject)Instantiate (PlayerCard, Position, Quaternion.identity);
		Player [PlayerNum].GetComponent<PlayerCard> ().PlayerID = 0;
		GameObject GN= (GameObject)Instantiate (PlayerName, Position, Quaternion.identity);
		Player [PlayerNum].GetComponent<PlayerCard> ().PlayerName = GN;
		Player [PlayerNum].GetComponent<PlayerCard> ().PlayerMark = null;
		GN.transform.SetParent (GameObject.Find ("MainCanvas").transform);
		Position+=offset;
		GN.transform.Translate (Camera.main.WorldToScreenPoint (Position));
		PlayerNum++;
	}
	//更新角色列表，去除空节点
	public void UpdatePlayerList(){
		int i, j;
		for (i = 0; i < PlayerNum; i++) {
			if (Player [i] == null) {
				print ("null!");
				for (j = i; j < PlayerNum - 1; j++)
					Player [j] = Player [j + 1];
				PlayerNum--;
				for (j = 0; j < PlayerNum; j++)
					print (Player [j].GetComponent<PlayerCard> ().PlayerID);
			}
		}
	}
	//移动角色卡
	public void MoveCard(bool ison){
		CanMove = ison;
	}
	//给角色编号
	public void MarkCard(){
        CanMove_hode = CanMove;
        int i;
        CanMove = false;
		for (i = 0; i < PlayerNum; i++) {
			Destroy (Player [i].GetComponent<PlayerCard> ().PlayerMark);
			Player [i].GetComponent<PlayerCard> ().PlayerID = 0;
		}
		GameStage = "标记";
		GameStatus.GetComponent<Text>().text="请按顺序点击玩家为其编号";
		CanClick = true;
		MarkID = 1;
		GameObject.Find ("MainCanvas/Addplayer").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/GameConfigButton").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/Gamestart").GetComponent<Button> ().interactable=false;
		GameObject.Find ("MainCanvas/GameStop").GetComponent<Button> ().interactable=false;
	}

	//配置
	public void GameConfig(){
		if (ConfigUI.activeSelf) {
			ConfigUI.SetActive (false);
			CanClick = true;
		} else {
			ConfigUI.SetActive (true);
			CanClick = false;
		}
	}
    //查看玩家幸存状态
	public void GameCheck(){
		if (ShowPlayer.activeSelf) {
			ShowPlayer.SetActive (false);
		} else {
			ShowPlayer.SetActive (true);
			string tmp;
			int i;
			GameObject.Find ("ShowPlayer/ID").GetComponent<Text> ().text = "";
			GameObject.Find ("ShowPlayer/Name").GetComponent<Text> ().text = "";
			GameObject.Find ("ShowPlayer/Role").GetComponent<Text> ().text = "";
			GameObject.Find ("ShowPlayer/Alive").GetComponent<Text> ().text = "";
			GameObject.Find ("ShowPlayer/Police").GetComponent<Text> ().text = "";
			GameObject.Find ("ShowPlayer/Lover").GetComponent<Text> ().text = "";
			for (i = 0; i < PlayerNum; i++) {
				tmp = Player [i].GetComponent<PlayerCard> ().PlayerID.ToString() + "\n";
				GameObject.Find ("ShowPlayer/ID").GetComponent<Text> ().text += tmp;
				tmp = Player [i].GetComponent<PlayerCard> ().Name + "\n";
				GameObject.Find ("ShowPlayer/Name").GetComponent<Text> ().text += tmp;
				tmp = Player [i].GetComponent<PlayerCard> ().Role + "\n";
				GameObject.Find ("ShowPlayer/Role").GetComponent<Text> ().text += tmp;
				if (Player [i].GetComponent<PlayerCard> ().IsAlive)
					GameObject.Find ("ShowPlayer/Alive").GetComponent<Text> ().text +="存活\n";
				else
					GameObject.Find ("ShowPlayer/Alive").GetComponent<Text> ().text +="死亡\n";
				if (Player [i] == jingzhang)
					GameObject.Find ("ShowPlayer/Police").GetComponent<Text> ().text += "警长\n";
				else
					GameObject.Find ("ShowPlayer/Police").GetComponent<Text> ().text += "\n";
				if (Player [i] == Lovers [0] || Player [i] == Lovers [1])
					GameObject.Find ("ShowPlayer/Lover").GetComponent<Text> ().text += "情侣\n";
				else
					GameObject.Find ("ShowPlayer/Lover").GetComponent<Text> ().text += "\n";
			}
		}
	}
    //切换游戏模式
    public void SwitchMode()
    {
        int i;
        if (GameMode == "Player")
        {
            GameMode = "God";
			GameObject.Find ("MainCanvas/GameMod").GetComponent<Text> ().text = "上帝模式";
            for (i = 0; i < PlayerNum; i++)
            {
                Player[i].GetComponent<PlayerCard>().PlayerMark.GetComponent<Text>().text 
                    = Player[i].GetComponent<PlayerCard>().PlayerID.ToString()
                    +" "
                    + Player[i].GetComponent<PlayerCard>().Role;
            }
        }
        else if (GameMode == "God")
        {
            GameMode = "Player";
			GameObject.Find ("MainCanvas/GameMod").GetComponent<Text> ().text = "玩家模式";
            for (i = 0; i < PlayerNum; i++)
            {
                Player[i].GetComponent<PlayerCard>().PlayerMark.GetComponent<Text>().text = Player[i].GetComponent<PlayerCard>().PlayerID.ToString();
            }
        }
        
    }


    //玩家名字输入
    public void PlayerInputName(string Name){
		PlayerNow.GetComponent<PlayerCard>().InputName (Name);
	}
	//删除玩家
	public void PlayerDelete(){
		PlayerNow.GetComponent<PlayerCard>().Delete ();
		Invoke ("UpdatePlayerList", 0.5f);
	}

	//角色信息配置完成
	public void PlayerInfoExit(){
		PlayerUI.SetActive (false);
		CanClick = true;
	}
	//角色游戏中配置完成
	public void PlayerConfigExit(){
		PlayerUIConfig.SetActive (false);
		CanClick = true;
	}

	//角色选择响应
	public void ToggleControl(int ID,bool ison){
		if (ison) {
			Toggles [0]++;
			Toggles [ID] = 1;
		} else {
			Toggles [0]--;
			Toggles [ID] = 0;
		}
	}
	//开始

	void SortByRole(){
		int i,j;
		GameObject tmp;
		j = 0;
		for (i = j; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "狼人") {
				tmp = Player [i];
				Player [i] = Player [j];
				Player [j] = tmp;
				j++;
			}
		}
		for (i = j; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "女巫") {
				tmp = Player [i];
				Player [i] = Player [j];
				Player [j] = tmp;
				j++;
				break;
			}
		}
		for (i = j; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "守卫") {
				tmp = Player [i];
				Player [i] = Player [j];
				Player [j] = tmp;
				j++;
				break;
			}
		}
		for (i = j; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "先知") {
				tmp = Player [i];
				Player [i] = Player [j];
				Player [j] = tmp;
				j++;
				break;
			}
		}
		for (i = j; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "猎人") {
				tmp = Player [i];
				Player [i] = Player [j];
				Player [j] = tmp;
				j++;
				break;
			}
		}
		for (i = j; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "长老") {
				tmp = Player [i];
				Player [i] = Player [j];
				Player [j] = tmp;
				j++;
				break;
			}
		}
		for (i = j; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "丘比特") {
				tmp = Player [i];
				Player [i] = Player [j];
				Player [j] = tmp;
				j++;
				break;
			}
		}
	}

	public void GameStop(){
		int i;
		GameStage = "准备开始";
		GameStatus.GetComponent<Text>().text="游戏已重置，请准备重新开始";
		for (i = 0; i < DeadNum; i++)
			Destroy (Deaded [i]);
		GameObject.Find ("MainCanvas/Mark").GetComponent<Button> ().interactable=true;
		GameObject.Find ("MainCanvas/Gamestart").GetComponent<Button> ().interactable=true;
		GameObject.Find ("MainCanvas/GameExit").GetComponent<Button> ().interactable=true;
		GameObject.Find ("MainCanvas/Addplayer").GetComponent<Button> ().interactable=true;
		GameObject.Find ("MainCanvas/GameConfigButton").GetComponent<Button> ().interactable=true;
		Start ();
	}

	public void GameExit(){
		Application.Quit ();
	}



	void EnableNextStage(){
		GoNext.GetComponent<Button> ().interactable=true;
	}

	void MoveOn(){
		Invoke ("EnableNextStage", 1.0f);
		if (GameStage == "查看身份") {
			Stage_qiubite ();
			return;
		}
		if (GameStage == "丘比特") {
			Stage_langren ();
			return;
		}
		if (GameStage == "情侣") {
			Stage_langren ();
			return;
		}
		if (GameStage == "狼人") {
			Stage_nvwu_jieyao ();
			return;
		}
		if (GameStage == "女巫解药") {
			Stage_nvwu_duyao ();
			return;
		}
		if (GameStage == "女巫毒药") {
			Stage_shouwei ();
			return;
		}
		if (GameStage == "守卫") {
			Stage_xianzhi ();
			return;
		}
		if (GameStage == "先知") {
			if (diyiye) {
				diyiye = false;
				Stage_jingzhang ();
			}
			else {
				yewanjiesuan ();
			}
			return;
		}
		if (GameStage == "警长") {
			yewanjiesuan ();
			return;
		}
		if (GameStage == "夜晚结算") {
			if (lierensiwang) {
				time = "晚上";
				lierendie ();
			} else {
				if (jingzhangsiwang) {
					time = "晚上";
					jingzhangdie ();

				} else {
					Stage_taolun ();
				}
			}
			return;
		}
		if (GameStage == "转移警长") {
			if (time == "晚上") {
				Stage_taolun ();
				return;
			}
			if (time == "白天") {
				GameStatus.GetComponent<Text>().text="天黑请闭眼";
				GoNext.GetComponent<Button> ().interactable=false;
				Invoke ("Stage_langren", 3.0f);
				return;
			}
		}
		if (GameStage == "猎人") {
			ShowInfo.SetActive (true);
			GameObject.Find ("ShowInfo/Text").GetComponent<Text> ().text = jieguo;
			if (time == "晚上") {
				GameStage = "夜晚结算";
				return;
			}
			if (time == "白天") {
				GameStage = "白天结算";
				return;
			}
			return;
		}
		if (GameStage == "讨论") {
			Stage_xuanlangren ();
			return;
		}
		if (GameStage == "选狼人") {
			baitianjiesuan ();
			return;
		}
		if (GameStage == "白天结算") {
			if (lierensiwang) {
				time = "白天";
				lierendie ();
			} else {
				if (jingzhangsiwang) {
					time = "白天";
					jingzhangdie ();

				} else {
					Stage_langren ();
				}
			}
			return;
		}
	}

	public void Choose_YES(){
		GameObject.Find ("ChoosePlayer/YES").GetComponent<Button> ().interactable=false;
		GameObject.Find ("ChoosePlayer/NO").GetComponent<Button> ().interactable=true;
	}

	public void Choose_NO(){
		GameObject.Find ("ChoosePlayer/YES").GetComponent<Button> ().interactable=true;
		GameObject.Find ("ChoosePlayer/NO").GetComponent<Button> ().interactable=false;
	}



//	void ShowLovers(){
//		string name1, name2, role1, role2,ID1,ID2;
//		Invoke ("EnableNextStage", 1.0f);
//		GameStage = "情侣";
//		if (Toggles [8] == 0||diyiye==false) {
//			MoveOn ();
//			return;
//		}
//		name1 = Lovers [0].GetComponent<PlayerCard> ().Name;
//		name2 = Lovers [1].GetComponent<PlayerCard> ().Name;
//		role1 = Lovers [0].GetComponent<PlayerCard> ().Role;
//		role2 = Lovers [1].GetComponent<PlayerCard> ().Role;
//		ID1 = Lovers [0].GetComponent<PlayerCard> ().PlayerID.ToString ();
//		ID2 = Lovers [1].GetComponent<PlayerCard> ().PlayerID.ToString ();
//		GameStatus.GetComponent<Text>().text="请情侣互看身份";
//		ShowInfo.SetActive (true);
//		GameObject.Find ("ShowInfo/Text").GetComponent<Text> ().text = ID1 + name1 + "和" + ID2 + name2 + "连为了情侣\n" + ID1 + name1 + "的身份是" + role1 + "\n" + ID2 + name2 + "的身份是" + role2;
//	}

	void Stage_xianzhi(){
		GameStage = "先知";
		if (Toggles [5] == 0) {
			MoveOn ();
			return;
		}
		GameStatus.GetComponent<Text> ().text = "先知请睁眼，选择查验目标";
		CanClick = true;
	}

	void Stage_jingzhang(){
		GameStage = "警长";
		GameStatus.GetComponent<Text> ().text = "天亮了，请竞选警长";
		CanClick = true;
	}

	void yewanjiesuan(){
		GameStage = "夜晚结算";
		day++;
		jieguo = "";
		diyiye = false;
		if (dusha != null) {
			die (dusha, "毒杀");
			dusha = null;
		}
		if (jisha != null) {
			//有杀人
			if (shouhu != jisha) {
				//没守对
				if (shiyongjieyao) {
					//用解药了
					jieyaoyongle = true;
					shiyongjieyao = false;
				} else {
					//没用解药
					if (jisha.GetComponent<PlayerCard> ().Role == "长老" && shaguozhanglao == false) {
						//杀到长老第一条命
						shaguozhanglao = true;
					}
					else{
						die(jisha,"击杀");
					}
				}
			}
		}
		GameStatus.GetComponent<Text> ().text = "天亮了,昨晚结果为";
		ShowInfo.SetActive (true);
		if (jieguo == "")
			jieguo = "平安夜";
		GameObject.Find ("ShowInfo/Text").GetComponent<Text> ().text = jieguo;
	}

	void jingzhangdie(){
		jingzhangsiwang = false;
		GameStage ="转移警长";
		ShowInfo.SetActive (false);
		GameStatus.GetComponent<Text> ().text = "警长死亡，请移交警徽";
		CanClick = true;
	}

	void lierendie(){
		lierensiwang = false;
		GameStage ="猎人";
		ShowInfo.SetActive (false);
		GameStatus.GetComponent<Text> ().text = "猎人死亡，选择枪杀对象";
		CanClick = true;
	}

	void Stage_taolun(){
		GameStage ="讨论";
		ShowInfo.SetActive (false);
		if (day % 2 == 1) {
			GameStatus.GetComponent<Text> ().text = "请从警长/死者右手开始发言";
		} else {
			GameStatus.GetComponent<Text> ().text = "请从警长/死者左手开始发言";
		}
	}

	void Stage_xuanlangren(){
		GameStage = "选狼人";
		GameStatus.GetComponent<Text> ().text = "票选狼人,2次平票直接点下一步";
		CanClick = true;
	}

	void baitianjiesuan(){
		GameStage = "白天结算";
		ShowInfo.SetActive (true);
		jieguo = "";
		if (toupiao == null) {
			jieguo = "平安日";
		} else {
			die(toupiao,"处决");
		}
		GameObject.Find ("ShowInfo/Text").GetComponent<Text> ().text = jieguo;
	}

	void die(GameObject p,string way){
		if (p.GetComponent<PlayerCard> ().IsAlive == false)
			return;
		p.GetComponent<PlayerCard> ().IsAlive = false;
		if (jingzhang == p) {
			jieguo += "(警长)";
			jingzhangsiwang = true;
		}
		if (p.GetComponent<PlayerCard> ().Role == "猎人") {
			jieguo += "(猎人)";
			lierensiwang = true;
		}
		jieguo+=p.GetComponent<PlayerCard> ().PlayerID.ToString()+p.GetComponent<PlayerCard> ().Name+"死于"+way+"\n";
		Vector3 Position = p.GetComponent<Transform>().position;
		Deaded[DeadNum]= (GameObject)Instantiate (DeadMark, Position, Quaternion.identity);
		DeadNum++;
		if (Lovers [0] == p) {
			die (Lovers [1], "殉情");
		}
		if (Lovers [1] == p) {
			die (Lovers [0], "殉情");
		}
	}
}
