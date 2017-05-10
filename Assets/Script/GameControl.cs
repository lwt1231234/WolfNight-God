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
	public int MarkID;
	//预设体
	public GameObject PlayerCard, PlayerName,DeadMark;
	//全局对象
	public GameObject ConfigUI,PlayerUI,PlayerUIConfig,ChoosePlayer,ShowInfo,ShowPlayer,GoNext,PlayerNow,GameStatus,SpeakContent;

	//局部变量
	GameObject[] Player = new GameObject[20],Lovers = new GameObject[5],Deaded= new GameObject[20],SortList = new GameObject[20];
	//string[] PlayerRole = new string[20];
	//string[] RoleList = {"狼人","狼人","狼人","狼人","先知","女巫","守卫","丘比特","猎人","长老","白痴","村民","村民","村民","村民","村民","村民","村民","村民"};
	GameObject jisha,shouhu,dusha,jingzhang,piaosi;
	bool jieyaoyongle,duyaoyongle,shiyongjieyao,shaguozhanglao,diyiye,jingzhangsiwang;
	bool GameReset;
	string jieguo;
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

		Application.targetFrameRate = 30;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)||Input.GetKeyDown (KeyCode.KeypadEnter)) {
			PlayerInfoExit ();
		}
	}

	//-------------------------------------------------------------------------------------------------------
	//--------------------------------游戏中配置玩家信息-----------------------------------------------------
	//点选身份
	public void SetCupid()
	{
		PlayerNow.GetComponent<PlayerCard> ().Role = "丘比特";
		SelectPlayer (PlayerNow);
	}
	public void SetLover()
	{
		PlayerNow.GetComponent<PlayerCard> ().IsLover = !PlayerNow.GetComponent<PlayerCard> ().IsLover;
		int i, j;
		j = 0;
		for (i = 0; i < PlayerNum; i++)
			if (Player [i].GetComponent<PlayerCard> ().IsLover) {
				if (j > 1) {
					GameStatus.GetComponent<Text> ().text += "\n注意当前情侣数量>2";
					return;
				}
				Lovers [j] = Player [i];
				j++;
			}
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
	public void SetGuard()
	{
		PlayerNow.GetComponent<PlayerCard> ().Role = "守卫";
		SelectPlayer (PlayerNow);
	}
	public void SetProphet()
	{
		PlayerNow.GetComponent<PlayerCard> ().Role = "先知";
		SelectPlayer (PlayerNow);
	}
	public void SetHunter()
	{
		PlayerNow.GetComponent<PlayerCard> ().Role = "猎人";
		SelectPlayer (PlayerNow);
	}
	public void SetElder()
	{
		PlayerNow.GetComponent<PlayerCard> ().Role = "长老";
		SelectPlayer (PlayerNow);
	}
	public void SetIdiot()
	{
		PlayerNow.GetComponent<PlayerCard> ().Role = "白痴";
		SelectPlayer (PlayerNow);
	}
	public void SetVillager()
	{
		PlayerNow.GetComponent<PlayerCard> ().Role = "村民";
		SelectPlayer (PlayerNow);
	}
	//点选操作
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
	public void SetProtect()
	{
		if (shouhu != PlayerNow)
			shouhu = PlayerNow;
		else
			shouhu = null;
		SelectPlayer (PlayerNow);
	}
	public void SetPolice()
	{
		if (jingzhang != PlayerNow)
			jingzhang = PlayerNow;
		else
			jingzhang = null;
		SelectPlayer (PlayerNow);
	}
	public void SetVote()
	{
		PlayerUIConfig.SetActive (false);
		PlayerNow.GetComponent<PlayerCard> ().WillDie = "票死";
		piaosi = PlayerNow;
		Invoke ("Stage_baitianjiesuan", 0.0f);
	}
	public void SetAlive()
	{
		PlayerNow.GetComponent<PlayerCard> ().IsAlive = true;
		UpdateAlive();
		SelectPlayer (PlayerNow);
	}
	public void SetDead()
	{
		PlayerNow.GetComponent<PlayerCard> ().IsAlive = false;
		UpdateAlive();
		SelectPlayer (PlayerNow);
	}
	public void SetGod()
	{
		PlayerNow.GetComponent<PlayerCard> ().Role = "上帝";
		SelectPlayer (PlayerNow);
	}
	public void SelectPlayer(GameObject P)
	{
		PlayerNow = P;
		PlayerUIConfig.SetActive (true);
		CanClick = false;
		string str;
		str = P.GetComponent<PlayerCard> ().PlayerID.ToString () + "号玩家：" + P.GetComponent<PlayerCard> ().Name;
		if (P.GetComponent<PlayerCard> ().IsAlive)
			str += "，存活\n";
		else
			str += "，死亡\n";
		if (GameMode == "God") {
			str += "身份：" + P.GetComponent<PlayerCard> ().Role;
			if (P.GetComponent<PlayerCard> ().IsLover)
				str += "、情侣";
			if (P == jingzhang)
				str += "、警长";
			str += "\n";
			if (jisha == P)
				str += "被杀 ";
			if (dusha == P)
				str += "被毒 ";
			if (shouhu == P)
				str += "被守 ";
		}
		if(duyaoyongle)
			GameObject.Find ("PlayerConfig/Poison").GetComponent<Button> ().interactable = false;
		if (P.GetComponent<PlayerCard> ().Role == "猎人"){
			if (P.GetComponent<PlayerCard> ().WillDie == "否" || P.GetComponent<PlayerCard> ().WillDie == "击杀")
				str += "死亡后能使用技能 ";
			else
				str += "死亡后不能使用技能 ";
			}
		GameObject.Find ("PlayerConfig/Text").GetComponent<Text> ().text = str;
		UpdatePlayerCard ();
	}

	void UpdateAlive(){
		int i;
		Vector3 Position;
		for(i=0;i<DeadNum;i++)
			Destroy (Deaded [i]);
		DeadNum = 0;
		for (i = 0; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().IsAlive == false) {
				Position = Player [i].GetComponent<Transform> ().position;
				Deaded [DeadNum] = (GameObject)Instantiate (DeadMark, Position, Quaternion.identity);
				DeadNum++;
			}
		}
	}

	//--------------------------------游戏进度控制--------------------------------------------------------------------
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
		Application.targetFrameRate = 30;
		GameObject.Find ("MainCanvas/GameSpeed").GetComponent<Text> ().text = "30";
		ConfigUI.SetActive (false);
		PlayerUI.SetActive (false);
		PlayerUIConfig.SetActive (true);
		//string[] RoleList = {"狼人","狼人","狼人","狼人","先知","女巫","守卫","丘比特","猎人","长老","白痴","村民","村民","村民","村民","村民","村民","村民","村民"};
		if (Toggles [5] == 0) {//先知
			GameObject.Find ("PlayerConfig/Prophet").GetComponent<Button> ().interactable = false;
		} else {
			GameObject.Find ("PlayerConfig/Prophet").GetComponent<Button> ().interactable = true;
		}
		if (Toggles [6] == 0) {//女巫
			GameObject.Find ("PlayerConfig/Witch").GetComponent<Button> ().interactable = false;
			GameObject.Find ("PlayerConfig/Poison").GetComponent<Button> ().interactable = false;
		} else {
			GameObject.Find ("PlayerConfig/Witch").GetComponent<Button> ().interactable = true;
			GameObject.Find ("PlayerConfig/Poison").GetComponent<Button> ().interactable = true;
		}
		if (Toggles [7] == 0) {//守卫
			GameObject.Find ("PlayerConfig/Guard").GetComponent<Button> ().interactable = false;
			GameObject.Find ("PlayerConfig/Protect").GetComponent<Button> ().interactable = false;
		} else {
			GameObject.Find ("PlayerConfig/Guard").GetComponent<Button> ().interactable = true;
			GameObject.Find ("PlayerConfig/Protect").GetComponent<Button> ().interactable = true;
		}
		if (Toggles [8] == 0) {//丘比特
			GameObject.Find ("PlayerConfig/Cupid").GetComponent<Button> ().interactable = false;
			GameObject.Find ("PlayerConfig/Lover").GetComponent<Button> ().interactable = false;
		} else {
			GameObject.Find ("PlayerConfig/Cupid").GetComponent<Button> ().interactable = true;
			GameObject.Find ("PlayerConfig/Lover").GetComponent<Button> ().interactable = true;
		}
		if (Toggles [9] == 0) {//猎人
			GameObject.Find ("PlayerConfig/Hunter").GetComponent<Button> ().interactable = false;
		} else {
			GameObject.Find ("PlayerConfig/Hunter").GetComponent<Button> ().interactable = true;
		}
		if (Toggles [10] == 0) {//长老
			GameObject.Find ("PlayerConfig/Elder").GetComponent<Button> ().interactable = false;
		} else {
			GameObject.Find ("PlayerConfig/Elder").GetComponent<Button> ().interactable = true;
		}
		if (Toggles [11] == 0) {//白痴
			GameObject.Find ("PlayerConfig/Idiot").GetComponent<Button> ().interactable = false;
		} else {
			GameObject.Find ("PlayerConfig/Idiot").GetComponent<Button> ().interactable = true;
		}


		PlayerUIConfig.SetActive (false);

		GameStatus.GetComponent<Text>().text="游戏开始";
		//初始化游戏参数
		DeadNum =0;
		day = 0;
		Lovers [0] = null;
		Lovers [1] = null;
		jisha = null;
		shouhu = null;
		dusha = null;
		piaosi = null;
		jingzhang = null;
		jieyaoyongle = false;
		duyaoyongle = false;
		shiyongjieyao = false;
		shaguozhanglao = false;
		jingzhangsiwang = false;
		diyiye = true;
		GameReset = false;
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
		UpdatePlayerCard ();
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

		//重置所有天黑阶段标记
		jisha = null;
		dusha = null;
		shiyongjieyao = false;
		shouhu = null;
		for(int i =0;i<PlayerNum;i++)
			Player [i].GetComponent<PlayerCard> ().WillDie = "否";

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
		if(diyiye)
			GameStatus.GetComponent<Text>().text="1.标记狼人\n2.标记被杀目标\n3.平票或标记完成后点击下一步";
		else
			GameStatus.GetComponent<Text>().text="1.无\n2.标记被杀目标\n3.平票或标记完成后点击下一步";
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
		if(diyiye)
			GameStatus.GetComponent<Text>().text="1.标记女巫\n2.选择是否救人";
		else
			GameStatus.GetComponent<Text>().text="1.无\n2.选择是否救人(女巫除第一晚外不能自救)\n3.完成后点下一步";
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
		if(duyaoyongle)
			GameStatus.GetComponent<Text>().text="1.毒药已经用了(摇头)\n2.完成后点击下一步";
		else
			GameStatus.GetComponent<Text>().text="1.标记被毒杀的人\n2.完成后点击下一步";
		CanClick = true;
	}
	//守卫阶段->等上帝点下一步->先知阶段
	void Stage_shouwei(){
		GameStage = "守卫";
		shouhu = null;
		if (Toggles [7] == 0) {
			Invoke ("Stage_xianzhi", 0.0f);
			return;
		}
		SpeakContent.GetComponent<Text> ().text = "1.守卫请睁眼\n2.请选择守护目标\n3.守卫请闭眼";
		if(diyiye)
			GameStatus.GetComponent<Text>().text="1.标记守卫\n2.标记被守护的人\n3.不守护或标记完成后点击下一步";
		else
			GameStatus.GetComponent<Text>().text="1.无\n2.标记被守护的人(不能连续守同一个人)\n3.不守护或标记完成后点击下一步";
		CanClick = true;
	}
	//先知阶段->等上帝点下一步->长老
	void Stage_xianzhi(){
		GameStage = "先知";
		if (Toggles [5] == 0) {
			Invoke ("Stage_zhanglao", 0.0f);
			return;
		}
		SpeakContent.GetComponent<Text> ().text = "1.先知请睁眼\n2.选择查验目标\n3.向上是好人向下是坏人，他是(比手势)\n4.先知请闭眼";
		if(diyiye)
			GameStatus.GetComponent<Text>().text="1.标记先知\n2.点击被查验的人\n3.根据目标身份比手势\n4.完成后点击下一步";
		else
			GameStatus.GetComponent<Text>().text="1.无\n2.点击被查验的人\n3.根据目标身份比手势\n4.完成后点击下一步";
		CanClick = true;
	}
	//长老阶段->上帝点下一步->白痴阶段
	void Stage_zhanglao(){
		GameStage = "长老";
		if (Toggles [10] == 0||diyiye == false) {
			Invoke ("Stage_baichi", 0.0f);
			return;
		}
		SpeakContent.GetComponent<Text> ().text = "1.长老请睁眼\n2.长老请闭眼";
		GameStatus.GetComponent<Text>().text="1.标记长老\n2.完成后点击下一步";
		CanClick = true;
	}
	//白痴阶段->上帝点下一步->将死计算阶段
	void Stage_baichi(){
		GameStage = "白痴";
		if (Toggles [11] == 0||diyiye == false) {
			Invoke ("Stage_willdie", 0.0f);
			return;
		}
		SpeakContent.GetComponent<Text> ().text = "1.白痴请睁眼\n2.白痴请闭眼";
		GameStatus.GetComponent<Text>().text="1.标记白痴\n2.完成后点击下一步";
		CanClick = true;
	}
	//将死计算阶段->猎人阶段
	void Stage_willdie(){
		if ((jisha != null) && (shiyongjieyao == false) && (shouhu != jisha))
			if (jisha.GetComponent<PlayerCard> ().Role == "长老") //杀到长老
				if (shaguozhanglao == false)//第一次杀长老
					shaguozhanglao = true;
				else//第二次杀长老
					jisha.GetComponent<PlayerCard> ().WillDie = "击杀";
			else//没杀到长老
				jisha.GetComponent<PlayerCard> ().WillDie = "击杀";
		if (jisha != null && jisha.GetComponent<PlayerCard> ().WillDie == "击杀" //杀死了
			&& jisha.GetComponent<PlayerCard> ().IsLover == true) {//且是情侣
			GameObject AnotherLover;
			if (Lovers [0] == jisha)
				AnotherLover = Lovers [1];
			else
				AnotherLover = Lovers [0];
			if (AnotherLover.GetComponent<PlayerCard> ().WillDie == "否")
				AnotherLover.GetComponent<PlayerCard> ().WillDie = "殉情";
		}
		if (dusha != null) {
			dusha.GetComponent<PlayerCard> ().WillDie = "毒杀";
			if (dusha.GetComponent<PlayerCard> ().IsLover == true) {//死情侣
				GameObject AnotherLover;
				if (Lovers [0] == dusha)
					AnotherLover = Lovers [1];
				else
					AnotherLover = Lovers [0];
				if (AnotherLover.GetComponent<PlayerCard> ().WillDie == "否")
					AnotherLover.GetComponent<PlayerCard> ().WillDie = "殉情";
			}
		}
		Invoke ("Stage_lieren", 0.0f);
		return;
	}
	//猎人阶段->等上帝点下一步->警长阶段
	void Stage_lieren(){
		GameStage = "猎人";
		if (Toggles [9] == 0) {
			Invoke ("Stage_jingzhang", 0.0f);
			return;
		}
		SpeakContent.GetComponent<Text> ().text = "1.猎人请睁眼\n2.如果你今晚死了，向上能使用技能向下不能使用技能\n3.猎人请闭眼";
		GameStatus.GetComponent<Text>().text="1.标记猎人\n2.点击猎人，根据面板上提示比手势\n3.完成后点击下一步";
		CanClick = true;
	}
	//警长阶段->等上帝点下一步->夜晚结算
	void Stage_jingzhang(){
		GameStage = "警长";
		if (diyiye == false) {
			Invoke ("Stage_yewanjiesuan", 0.0f);
			return;
		}
		SpeakContent.GetComponent<Text> ().text = "1.天亮了，想竞选警长的请举手\n2.票选警长";
		GameStatus.GetComponent<Text>().text="1.竞选警长的发言，顺序顺时针/逆时针轮换\n2.标记警长，完成后点击下一步";
		CanClick = true;
	}
	//夜晚结算->等上帝点下一步->投票阶段
	void Stage_yewanjiesuan(){
		GameStage = "夜晚结算";
		string str;
		str = "";
		int DieNum=0;
		for (int i = 0; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().WillDie != "否") {
				str += Player [i].GetComponent<PlayerCard> ().PlayerID.ToString () + Player [i].GetComponent<PlayerCard> ().Name + " ";
				die (Player [i]);
				DieNum++;
			}
		}
		if (str == "")
			str = "昨晚是平安夜";
		else
			str += "死亡了";
		if (jingzhangsiwang) {
			jingzhangsiwang = false;
			str += "\n警长死亡了，请移交或放弃警徽";
		}
		if (jingzhang != null && jingzhang.GetComponent<PlayerCard> ().IsAlive) {
			if (DieNum > 1 || DieNum == 0)
				str += "\n请警长选择警左还是警右发言";
			else
				str += "\n请警长选择死左还是死右发言";
		} else {
			if(day%2==0)
				str += "\n请从死左开始发言";
			else
				str += "\n请从死右开始发言";
		}
		SpeakContent.GetComponent<Text> ().text = "昨晚"+str;
		GameStatus.GetComponent<Text> ().text = "坐等讨论完毕";
		CanClick = true;
		Application.targetFrameRate = 10;
		GameObject.Find ("MainCanvas/GameSpeed").GetComponent<Text> ().text = "10";
	}
	//投票->点击票死->白天结算
	void Stage_toupiao(){
		GameStage = "投票";
		SpeakContent.GetComponent<Text> ().text = "1.票选狼人，请比数字，有没想好的吗？\n3、2、1，投票";
		GameStatus.GetComponent<Text>().text="1.标记票死的人,2次平票则直接点下一步";
		piaosi = null;
		CanClick = true;
	}
	//白天结算-》等上帝点下一步-》天黑阶段
	void  Stage_baitianjiesuan(){
		GameStage = "白天结算";
		diyiye = false;
		day++;
		string str;
		str="";
		if (piaosi == null) {
			SpeakContent.GetComponent<Text> ().text = "今天是平安日";
			GameStatus.GetComponent<Text>().text="3秒后将自动跳转到下一步";
			GoNext.GetComponent<Button> ().interactable=false;
			Invoke ("Stage_tianhei", 3.0f);
			return;
		}
		piaosi.GetComponent<PlayerCard> ().WillDie = "票死";
		if (piaosi.GetComponent<PlayerCard> ().IsLover) {
			if(Lovers[0]==piaosi)
				Lovers[1].GetComponent<PlayerCard> ().WillDie = "殉情";
			else
				Lovers[0].GetComponent<PlayerCard> ().WillDie = "殉情";
		}
		for (int i = 0; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().WillDie != "否") {
				str += Player [i].GetComponent<PlayerCard> ().PlayerID.ToString () + Player [i].GetComponent<PlayerCard> ().Name + " ";
				die (Player [i]);
			}
		}
		str += "死亡了，请遗言";
		if (jingzhangsiwang) {
			jingzhangsiwang = false;
			str += "\n警长死亡了，请移交或放弃警徽";
			GameStatus.GetComponent<Text> ().text = "1.移交警徽后标记新的警长\n2.完成后点击下一步";
			CanClick = true;
		} else {
			GameStatus.GetComponent<Text>().text="完成后点击下一步";
		}
			
		
		SpeakContent.GetComponent<Text> ().text = str;

	}
	//_------------------------------------------------------------------------------------
	//上帝点下一步
	public void NextStage(){
		GoNext.GetComponent<Button> ().interactable=false;
		PlayerUIConfig.SetActive (false);
		Invoke ("EnableNextStage", 1.0f);
		print (GameStage);
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
		//守卫》先知
		if (GameStage == "守卫") {
			Invoke ("Stage_xianzhi", 0.0f);
			return;
		}
		//先知》长老
		if (GameStage == "先知") {
			Invoke ("Stage_zhanglao", 0.0f);
			return;
		}
		//长老》白痴
		if (GameStage == "长老") {
			Invoke ("Stage_baichi", 0.0f);
			return;
		}
		//白痴》死亡结算
		if (GameStage == "白痴") {
			Invoke ("Stage_willdie", 0.0f);
			return;
		}
		//猎人》竞选警长
		if (GameStage == "猎人") {
			Invoke ("Stage_jingzhang", 0.0f);
			return;
		}
		//竞选警长》公布夜晚结果
		if (GameStage == "警长") {
			Invoke ("Stage_yewanjiesuan", 0.0f);
			return;
		}
		//夜晚结果》投票
		if (GameStage == "夜晚结算") {
			Invoke ("Stage_toupiao", 0.0f);
			return;
		}
		if (GameStage == "投票") {
			Invoke ("Stage_baitianjiesuan", 0.0f);
			return;
		}
		if (GameStage == "白天结算") {
			Invoke ("Stage_tianhei", 0.0f);
			return;
		}
	}


	//-----------------------------------------------------------------------------
	//----------------------------主界面按钮响应
	//添加角色
	public void AddPlayer(){
		Vector3 Position = new Vector3(0, 0, PlayerCardZ);
		Vector3 offset = new Vector3(0, -1.4f, 0);
		Vector3 MarkY = new Vector3(0, 0.0f, 0);
		Player [PlayerNum] = (GameObject)Instantiate (PlayerCard, Position, Quaternion.identity);
		Player [PlayerNum].GetComponent<PlayerCard> ().PlayerID = 0;
		GameObject GN= (GameObject)Instantiate (PlayerName, Position, Quaternion.identity);
		Player [PlayerNum].GetComponent<PlayerCard> ().PlayerName = GN;
		Player [PlayerNum].GetComponent<PlayerCard> ().PlayerMark = (GameObject)Instantiate (PlayerName, Camera.main.WorldToScreenPoint (transform.position + MarkY), Quaternion.identity);
		Player [PlayerNum].GetComponent<PlayerCard> ().PlayerMark.transform.SetParent (GameObject.Find ("MainCanvas").transform);
		Player [PlayerNum].GetComponent<PlayerCard> ().PlayerMark.GetComponent<Text> ().text = "0";
		GN.transform.SetParent (GameObject.Find ("MainCanvas").transform);
		Position+=offset;
		GN.transform.Translate (Camera.main.WorldToScreenPoint (Position));
		PlayerNum++;
	}
	//删除玩家
	public void PlayerDelete(){
		PlayerNow.GetComponent<PlayerCard>().Delete ();
		PlayerUI.SetActive (false);
		Invoke ("UpdatePlayerList", 0.5f);
		CanClick = true;
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
		ConfigUI.SetActive (false);
        int i;
        CanMove = false;
		for (i = 0; i < PlayerNum; i++) {
			Player [i].GetComponent<PlayerCard> ().PlayerMark.GetComponent<Text> ().text = "";
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
    //查看玩家状态
	public void GameCheck(){
		if (ShowPlayer.activeSelf) {
			ShowPlayer.SetActive (false);
		} else {
			ShowPlayer.SetActive (true);
			UpdateShowPlayer ();
		}
	}
	void UpdateShowPlayer(){
		string tmp;
		int i,j=0;

		GameObject.Find ("ShowPlayer/ID").GetComponent<Text> ().text = "";
		GameObject.Find ("ShowPlayer/Name").GetComponent<Text> ().text = "";
		GameObject.Find ("ShowPlayer/Role").GetComponent<Text> ().text = "";
		GameObject.Find ("ShowPlayer/Alive").GetComponent<Text> ().text = "";
		GameObject.Find ("ShowPlayer/Police").GetComponent<Text> ().text = "";
		GameObject.Find ("ShowPlayer/Lover").GetComponent<Text> ().text = "";
		//找狼人
		for (i = 0; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "狼人") {
				SortList [j] = Player [i];
				j++;
			}
		}
		for (i = 0; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "先知") {
				SortList [j] = Player [i];
				j++;
			}
		}
		for (i = 0; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "女巫") {
				SortList [j] = Player [i];
				j++;
			}
		}
		for (i = 0; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "守卫") {
				SortList [j] = Player [i];
				j++;
			}
		}
		for (i = 0; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "长老") {
				SortList [j] = Player [i];
				j++;
			}
		}
		for (i = 0; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "白痴") {
				SortList [j] = Player [i];
				j++;
			}
		}
		for (i = 0; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "猎人") {
				SortList [j] = Player [i];
				j++;
			}
		}
		for (i = 0; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "村民") {
				SortList [j] = Player [i];
				j++;
			}
		}
		for (i = 0; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "丘比特") {
				SortList [j] = Player [i];
				j++;
			}
		}
		for (i = 0; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "未标记") {
				SortList [j] = Player [i];
				j++;
			}
		}
		for (i = 0; i < PlayerNum; i++) {
			if (Player [i].GetComponent<PlayerCard> ().Role == "上帝") {
				SortList [j] = Player [i];
				j++;
			}
		}
		for (i = 0; i < PlayerNum; i++) {
			if (i > 0 && SortList [i].GetComponent<PlayerCard> ().Role != "狼人" && SortList [i - 1].GetComponent<PlayerCard> ().Role == "狼人") {
				GameObject.Find ("ShowPlayer/ID").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Name").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Role").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Alive").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Lover").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Police").GetComponent<Text> ().text += "\n";
			}
			if (i > 0 && SortList [i].GetComponent<PlayerCard> ().Role == "村民" && SortList [i - 1].GetComponent<PlayerCard> ().Role != "村民") {
				GameObject.Find ("ShowPlayer/ID").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Name").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Role").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Alive").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Lover").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Police").GetComponent<Text> ().text += "\n";
			}
			if (i > 0 && SortList [i].GetComponent<PlayerCard> ().Role != "村民" && SortList [i - 1].GetComponent<PlayerCard> ().Role == "村民") {
				GameObject.Find ("ShowPlayer/ID").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Name").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Role").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Alive").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Lover").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Police").GetComponent<Text> ().text += "\n";
			}
			if (i > 0 && SortList [i].GetComponent<PlayerCard> ().Role == "未标记" && SortList [i - 1].GetComponent<PlayerCard> ().Role != "未标记") {
				GameObject.Find ("ShowPlayer/ID").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Name").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Role").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Alive").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Lover").GetComponent<Text> ().text += "\n";
				GameObject.Find ("ShowPlayer/Police").GetComponent<Text> ().text += "\n";
			}
			tmp = SortList [i].GetComponent<PlayerCard> ().PlayerID.ToString() + "\n";
			GameObject.Find ("ShowPlayer/ID").GetComponent<Text> ().text += tmp;
			tmp = SortList [i].GetComponent<PlayerCard> ().Name + "\n";
			GameObject.Find ("ShowPlayer/Name").GetComponent<Text> ().text += tmp;
			tmp = SortList [i].GetComponent<PlayerCard> ().Role + "\n";
			GameObject.Find ("ShowPlayer/Role").GetComponent<Text> ().text += tmp;
			if (SortList [i].GetComponent<PlayerCard> ().IsAlive)
				GameObject.Find ("ShowPlayer/Alive").GetComponent<Text> ().text +="存活\n";
			else
				GameObject.Find ("ShowPlayer/Alive").GetComponent<Text> ().text +="[死亡]\n";
			if (SortList [i] == jingzhang)
				GameObject.Find ("ShowPlayer/Police").GetComponent<Text> ().text += "警长\n";
			else
				GameObject.Find ("ShowPlayer/Police").GetComponent<Text> ().text += "\n";
			if (SortList [i].GetComponent<PlayerCard> ().IsLover)
				GameObject.Find ("ShowPlayer/Lover").GetComponent<Text> ().text += "情侣\n";
			else
				GameObject.Find ("ShowPlayer/Lover").GetComponent<Text> ().text += "\n";
		}
	}
	//切换游戏模式
	public void SwitchMode()
	{
		if (GameMode == "Player")
		{
			GameMode = "God";
			GameObject.Find ("MainCanvas/GameMod").GetComponent<Text> ().text = "上帝模式";
		}
		else if (GameMode == "God")
		{
			GameMode = "Player";
			GameObject.Find ("MainCanvas/GameMod").GetComponent<Text> ().text = "玩家模式";
		}
		UpdatePlayerCard ();
	}

	//-----------------------------------------------------------------------
	//------------------------功能函数
	//玩家死亡
	void die(GameObject p){
		if (p.GetComponent<PlayerCard> ().IsAlive == false)
			return;
		p.GetComponent<PlayerCard> ().IsAlive = false;
		p.GetComponent<PlayerCard> ().WillDie = "否";
		if (jingzhang == p) {
			jingzhangsiwang = true;
		}
		Vector3 Position = p.GetComponent<Transform>().position;
		Deaded[DeadNum]= (GameObject)Instantiate (DeadMark, Position, Quaternion.identity);
		DeadNum++;
	}

    //更新玩家卡片
	void UpdatePlayerCard(){
		int i;
		if (GameMode == "God")
		{
			for (i = 0; i < PlayerNum; i++)
			{
				Player[i].GetComponent<PlayerCard>().PlayerMark.GetComponent<Text>().text 
				= Player[i].GetComponent<PlayerCard>().PlayerID.ToString()
					+"\n"
					+ Player[i].GetComponent<PlayerCard>().Role;
			}
		}
		else if (GameMode == "Player")
		{
			
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

	public void GameStop(){
		int i;
		if (GameReset == false) {
			GameReset = true;
			Invoke ("MarkGameReset", 1.0f);
		} else {
			GameStage = "准备开始";
			SpeakContent.GetComponent<Text>().text="游戏已重置，请准备重新开始";
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
	}

	void MarkGameReset(){
		GameReset = false;
	}

	public void GameExit(){
		Application.Quit ();
	}
		
	void EnableNextStage(){
		GoNext.GetComponent<Button> ().interactable=true;
	}
		
	public void Choose_YES(){
		GameObject.Find ("ChoosePlayer/YES").GetComponent<Button> ().interactable=false;
		GameObject.Find ("ChoosePlayer/NO").GetComponent<Button> ().interactable=true;
	}

	public void Choose_NO(){
		GameObject.Find ("ChoosePlayer/YES").GetComponent<Button> ().interactable=true;
		GameObject.Find ("ChoosePlayer/NO").GetComponent<Button> ().interactable=false;
	}

	public void Full(){
		for (int i = 0; i < PlayerNum; i++){
			if (Player [i].GetComponent<PlayerCard> ().Role == "未标记")
				Player [i].GetComponent<PlayerCard> ().Role = "村民";
		}
		UpdateShowPlayer ();
		UpdatePlayerCard ();
	}
}
