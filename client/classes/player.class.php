<?php


include 'ia.class.php';


Class Player extends IA
{
	const DEAD = -1;
	const KO = 0;
	const OK = 1;
	const MSG = 2;
	const LEVELUP = 3;
	const INVENTORY = 4;
	const SEE = 5;
	const PLACES = 6;
	const WELCOME = 7;
	const MAPSIZE = 8;
	const INCANTATION = 8;

	private $_team = null;
	private $_count = 0;
	private $_socket = null;
	private $_level = null;
	private $_view = array();
	private $_foodNeeds = 7;
	private $_seekingIncantation = false;
	private $_seekingFood = false;
	private $_inventory = array(
							"nourriture" => 10,
							"linemate" => 0,
							"deraumere" => 0,
							"sibur" => 0,
							"mendiane" => 0,
							"phiras" => 0,
							"thystame" => 0,
							"joueur" => 1
						);
	private $_levelNeeds = array(
								2 => array(
									"linemate" => 1,
									"deraumere" => 0,
									"sibur" => 0,
									"mendiane" => 0,
									"phiras" => 0,
									"thystame" => 0,
									"joueur" => 1
								),
								3 => array(
									"linemate" => 1,
									"deraumere" => 1,
									"sibur" => 1,
									"mendiane" => 0,
									"phiras" => 0,
									"thystame" => 0,
									"joueur" => 2
								),
								4 => array(
									"linemate" => 2,
									"deraumere" => 0,
									"sibur" => 1,
									"mendiane" => 0,
									"phiras" => 2,
									"thystame" => 0,
									"joueur" => 2
								),
								5 => array(
									"linemate" => 1,
									"deraumere" => 1,
									"sibur" => 2,
									"mendiane" => 0,
									"phiras" => 1,
									"thystame" => 0,
									"joueur" => 4
								),
								6 => array(
									"linemate" => 1,
									"deraumere" => 2,
									"sibur" => 1,
									"mendiane" => 3,
									"phiras" => 0,
									"thystame" => 0,
									"joueur" => 4
								),
								7 => array(
									"linemate" => 1,
									"deraumere" => 2,
									"sibur" => 3,
									"mendiane" => 0,
									"phiras" => 1,
									"thystame" => 0,
									"joueur" => 6
								),
								8 => array(
									"linemate" => 2,
									"deraumere" => 2,
									"sibur" => 2,
									"mendiane" => 2,
									"phiras" => 2,
									"thystame" => 1,
									"joueur" => 6
								));

	public function		__construct($team, Socket $socket)
	{
		print("---CONSTRUCT---\n");
		$this->_team = $team;
		$this->_socket = $socket;
		$this->_level = 1;

		$res = $this->receive();
		if ($res === self::WELCOME)
		{
			PRINT("---WELCOME IF---\n");
			$this->_socket->send($this->_team);
			do
			{
				PRINT("---RECV LOOP INFOS---\n");
				$res = $this->receive();
			} while($res !== self::PLACES);
			do
			{
				PRINT("---RECV LOOP MAPSIZE---\n");
				$res = $this->receive();
			} while($res !== self::MAPSIZE);
			$this->play();
		}
	}

	private function	play()
	{
		static $done = false;
		$this->_count = 0;

		print("---PLAY---\n");
		$this->inventory(false);

		if ($this->_inventory['nourriture'] >= 10 && !$done)
		{
			$done = true;
			$this->spawn(false);
		}
		$this->remainingPlaces(false);
		$this->defineNeeds();

	}

	private function	findBuddies()
	{
		print("---FIND BUDDIES---\n");
		$this->broadcast("INCANTATION:".$this->getLevel().":".$this->getTeam(), false);
		$this->play();
	}

	private function	parseView($elem)
	{
		print("---PARSE VIEW---\n");
		print("SEEKING FOR ITEM: " .$elem. "\n");
		$tileWeight = 123456789;
		$tile = false;

		foreach ($this->_view as $x => $content)
		{
			if ($x !== 0)
			{
				$n = floor(sqrt($x));
				$y = $n + abs($x - (pow($n, 2) + $n));
				$z = ($x / (pow($n, 2) + $n)) != 1 ? $y + 1 : $y;
			}
			else
				$z = 0;
			$this->_view[$x]['weight'] = $z;
			if (array_key_exists($elem, $this->_view[$x]))
			{
				if ($this->_view[$x]['joueur'] <= 1 && $this->_view[$x]['weight'] < $tileWeight)
				{
					$tile = $x;
					$tileWeight = $this->_view[$x]['weight'];
				}
				/*else if ($elem == "nourriture" && $this->_view[$x]['weight'] < $tileWeight
									&& !array_key_exists("joueur", $this->_view[$x]))
				{
					$tile = $x;
					$tileWeight = $this->_view[$x]['weight'];
				}*/
			}
		}
		return ($tile);
	}

	private function	findPath($x)
	{
		$y = 0;
		$n = floor(sqrt($x));

		for ($i = 0; $i < $n; $i++)
			$this->forward(false, false);

		if ($x < (pow($n, 2) + $n))
		{
			$this->left(false, false);
			$y = abs($x - (pow($n, 2) + $n));
		}
		else if ($x > (pow($n, 2) + $n))
		{
			$this->right(false, false);
			$y = abs($x - (pow($n, 2) + $n));
		}

		for ($i = 0; $i < $y; $i++)
			$this->forward(false, false);
	}

	private function	findNeeds($elem)
	{
		print("---FIND NEEDS---\n");
		$tile = $this->parseView($elem);

		if ($tile === false)
		{
			if (rand(0, 1))
				$this->forward(false, false);
			else
				rand(0, 1) ? $this->left(false, false) : $this->right(false, false);
			$this->play();
		}
		else
		{
			$this->findPath($tile);
			$this->take($elem, false);
		}
		$this->play();
	}

	private function	dropItems()
	{
		print("---DROP ITEMS---\n");
		foreach($this->_inventory as $elem => $val)
		{
			if ($elem !== "joueur" && $elem !== "nourriture" && $this->_inventory[$elem] > 0)
			{
				while ($this->_inventory[$elem] > 0)
					$this->lay($elem, false);
			}
		}
	}

	private function	defineNeeds()
	{
		print("---DEFINE NEEDS---\n");
		$this->see(false);

		$noNeeds = true;
		if ($this->_inventory['nourriture'] > $this->foodNeeds)
		{
			$this->foodNeeds = 7;
			foreach($this->_levelNeeds[$this->_level + 1] as $thing => $needs)
			{
				if ($thing === "joueur" && $this->_view[0][$thing] > $needs)
				{
					$this->expel(false);
				}
				if ($this->_inventory[$thing] + $this->_view[0][$thing] < $needs)
				{
					$noNeeds = false;
					$need = $thing;
					break;
				}
			}
			if ($noNeeds === true)
			{
				$this->dropItems();
				$this->incantation(false);
			}
			else
			{
				if ($thing === "joueur")
					$this->findBuddies();
				else
					$this->findNeeds($need);
			}
		}
		else
		{
			$this->foodNeeds = 15;
			$this->findNeeds("nourriture");
		}
	}

	private function	analyzeMessage()
	{
		print("---ANALYZE MESSAGE---\n");

		$tab = explode(",", $this->_answer);
		$tile = $tab[0];
		$msg = $tab[1];
		foreach(explode(" ", $tile) as $data)
		{
			if (ctype_digit($data))
				$dir = intval($data);
		}

		$tab = explode(":", $msg);
		if (trim($tab[0]) === "INCANTATION")
		{
			if (substr($tab[1], 0, 2) == $this->_level && $tab[2] == $this->getTeam() && !$this->_seekingFood)
			{
				print("---MOVING BROADCAST---\n");
				switch ($dir)
				{
					case 1:
						$this->forward(false, true);
						break;
					case 2:
						$this->forward(false, true);
						$this->left(false, true);
						$this->forward(false, true);
						break;
					case 3:
						$this->left(false, true);
						$this->forward(false, true);
						break;
					case 4:
						$this->left(false, true);
						$this->forward(false, true);
						$this->left(false, true);
						$this->forward(false, true);
						break;
					case 5:
						$this->left(false, true);
						$this->left(false, true);
						$this->forward(false, true);
						break;
					case 6:
						$this->right(false, true);
						$this->forward(false, true);
						$this->right(false, true);
						$this->forward(false, true);
						break;
					case 7:
						$this->right(false, true);
						$this->forward(false, true);
						break;
					case 8:
						$this->forward(false, true);
						$this->right(false, true);
						$this->forward(false, true);
						break;
					case 0:
						$this->_count = 0;
						$this->dropItems();
						$this->_count = 0;
						$this->incantation(true);
					default:
						break;
				};
			}
			return (true);
		}
		return (false);
	}

	private function	handle($res, $function, $parameters, $force = false)
	{
		print("---HANDLE---\n");
		print("---HANDLE RES: " .$res."---\n");


		if ($function != "lvlup")
			$this->_count += 1;
		if ($this->_count >= 5)
		{
			print("---BLOCKED---\n");
			$this->_count = 0;
			$this->play();
		}
		else if ($res === self::DEAD)
			die("A player from team " .$this->_team. " died.\n");
		else if ($res === self::MSG && $this->_inventory['nourriture'] > $this->foodNeeds)
		{
			var_dump($function);
			if ($force || $function == "incantation" || $function == "lvlup" || $this->analyzeMessage() === true)
			{
				if (!$force)
				{
					if ($parameters === null)
						$this->$function(true);
					else
						$this->$function($parameters, true);
				}
				else
					$this->$function(true, true);
			}
		}
		else if ($res === self::INCANTATION)
			$this->lvlup(false);
		else if (!$force && $res === self::KO)
			$this->play();
		else
		{
			if (!$force)
			{
				if ($parameters === null)
					$this->$function(true);
				else
					$this->$function($parameters, true);
			}
			else
				$this->$function(true, true);
		}
		/*
			HANDLE MORE RES
		*/
	}

	private function	isInventory($str)
	{
		if (preg_match('/^{\s*[A-z]+\s\d+,\s/', $str))
			return (true);
		return (false);
	}

	private function	receive()
	{
		static $remains = null;

		if (empty($remains))
		{
			print("----RECV REMAINS EMPTY---\n");
			$this->_answer = $this->_socket->receive();

			$tab = explode("\n", $this->_answer);
			$this->_answer = $tab[0];
			array_shift($tab);
			array_pop($tab);
			$remains = $tab;
			print("----REMAINS---\n");
			print_r($remains);
		}
		else
		{
			print("----RECV WITH REMAINS---\n");
			$this->_answer = $remains[0];
			print("---SHIFTED : " .$remains[0]. "---\n");
			array_shift($remains);
			print("----REMAINS---\n");
			print_r($remains);
		}

		if ($this->_answer === "mort")
			return (self::DEAD);
		else if ($this->_answer === "ko")
			return (self::KO);
		else if ($this->_answer === "ok")
			return (self::OK);
		else if (strpos($this->_answer, "message ") !== false)
			return (self::MSG);
		else if ($this->_answer === "elevation en cours")
			return (self::INCANTATION);
		else if (strpos($this->_answer, "niveau actuel : ") !== false)
			return (self::LEVELUP);
		else if (strpos($this->_answer, "{") === 0)
		{
			if ($this->isInventory($this->_answer))
				return (self::INVENTORY);
			else
				return (self::SEE);
		}
		else if (preg_match('/^\d+$/', $this->_answer))
			return (self::PLACES);
		else if ($this->_answer === "BIENVENUE")
			return (self::WELCOME);
		else if (preg_match('/\d+\s\d+/', $this->_answer))
			return (self::MAPSIZE);
		else
			return (false);
	}

	public function		forward($callback, $force = false)
	{
		print("---FORWARD---\n");
		if (!$callback)
		{
			$this->_count = 0;
			$this->_socket->send("avance");
		}

		$res = $this->receive();
		if ($res !== self::OK)
			$this->handle($res, "forward", null, $force);
	}

	public function		right($callback, $force = false)
	{
		print("---RIGHT---\n");
		if (!$callback)
		{
			$this->_count = 0;
			$this->_socket->send("droite");
		}

		$res = $this->receive();
		if ($res !== self::OK)
			$this->handle($res, "right", null, $force);
	}

	public function		left($callback, $force = false)
	{
		print("---LEFT---\n");
		if (!$callback)
		{
			$this->_count = 0;
			$this->_socket->send("gauche");
		}

		$res = $this->receive();
		if ($res !== self::OK)
			$this->handle($res, "left", null, $force);
	}

	public function		see($callback)
	{
		print("---SEE---\n");
		if (!$callback)
		{
			$this->_count = 0;
			$this->_socket->send("voir");
		}

		$res = $this->receive();
		if ($res === self::SEE)
		{
			$clean = str_replace(array(0 =>"{ ", 1 => "}"), "", $this->_answer);
			$view = explode(", ", $clean);
			$i = 0;
			foreach ($view as $elem)
			{
				$tile = explode(" ", $elem);
				$this->_view[$i] = array_count_values($tile);
				$i++;
			}
			array_pop($this->_view);
		}
		else
			$this->handle($res, "see", null, false);
	}

	public function		inventory($callback)
	{
		print("---INVENTORY---\n");
		if (!$callback)
		{
			$this->_count = 0;
			$this->_socket->send("inventaire");
		}

		$res = $this->receive();
		if ($res == self::INVENTORY)
		{
			$clean = str_replace(array(0 =>"{ ", 1 => "}"), "", $this->_answer);
			$inv = explode(", ", $clean);
			foreach ($inv as $elem)
			{
				$data = explode(" ", $elem);
				if ($data[0] !== "")
					$this->_inventory[$data[0]] = intval($data[1]);
			}
			print("---My inventory---\n");
			var_dump($this->_inventory);
		}
		else
			$this->handle($res, "inventory", null, false);
	}

	public function		take($object, $callback)
	{
		print("---TAKE---\n");
		if (!$callback)
		{
			$this->_count = 0;
			$this->_socket->send("prend " .$object);
		}

		$res = $this->receive();
		if ($res === self::OK)
			$this->_inventory[$object]++;
		else
			$this->handle($res, "take", $object, false);
	}

	public function		lay($object, $callback)
	{
		print("---LAY---\n");
		if (!$callback)
		{
			$this->_count = 0;
			$this->_socket->send("pose " .$object);
		}

		$res = $this->receive();
		if ($res === self::OK)
			$this->_inventory[$object]--;
		else
			$this->handle($res, "lay", $object, false);
	}

	public function		expel($callBack)
	{
		print("---EXPEL---\n");
		if (!$callback)
		{
			$this->_count = 0;
			$this->_socket->send("expulse");
		}

		$res = $this->receive();
		if ($res !== self::OK)
			$this->handle($res, "expel", null, false);
	}

	public function		broadcast($data, $callback)
	{
		print("---BROADCAST---\n");
		if (!$callback)
		{
			$this->_count = 0;
			$this->_socket->send("broadcast " .$data);
		}

		$res = $this->receive();
		if ($res !== self::OK)
			$this->handle($res, "broadcast", $data);
	}

	public function		incantation($callback)
	{
		print("---INCANTATION---\n");
		if (!$callback)
		{
			$this->_count = 0;
			$this->_socket->send("incantation");
		}

		$res = $this->receive();

		if ($res == self::KO)
		{
			$this->play();
		}
		else if ($res !== self::INCANTATION)
			$this->handle($res, "incantation", null, false);
		else if ($res == self::INCANTATION)
			$this->lvlup(false);
	}

	public function lvlup($callback)
	{
		print("---LVL UP---\n");
		if (!$callback)
			$this->_count = 0;

		$res = $this->receive();

		if ($res !== self::LEVELUP)
			$this->handle($res, "lvlup", null, false);

		$len = strlen($this->_answer);
		$pos = strpos($this->_answer, ":") + 1;
		$this->setLevel(intval(substr($this->_answer, $pos, $len)));
		$this->play();
	}

	public function		spawn($callback)
	{
		print("---SPAWN---\n");
		if (!$callback)
		{
			$this->_count = 0;
			$this->_socket->send("fork");
		}

		$res = $this->receive();
		if ($res !== self::OK)
			$this->handle($res, "spawn", null, false);
	}

	public function		fork()
	{
		print("---FORKING---\n");
		$pid = pcntl_fork();
		if ($pid == -1)
			print("pcntl_fork() failed\n");
		else if (!$pid)
			exec("./bin/php main.php -n " .$this->_team. " -p " .$this->_socket->getPort());
	}

	public function		remainingPlaces($callback)
	{
		print("---REMAININGPLACES---\n");
		if (!$callback)
		{
			$this->_count = 0;
			$this->_socket->send("connect_nbr");
		}

		$res = $this->receive();
		if ($res == self::PLACES)
		{
			if (intval($this->_answer) > 0)
				$this->fork();
		}
		else
			$this->handle($res, "remainingPlaces", null, false);
	}

	public function		setTeam($team)
	{
		$this->_team = $team;

		return ($this);
	}

	public function		getTeam()
	{
		return ($this->_team);
	}

	public function		setSocket($socket)
	{
		$this->_socket = $socket;

		return ($this);
	}

	public function		getSocket()
	{
		return ($this->_socket);
	}

	public function		setLevel($level)
	{
		$this->_level = $level;

		return ($this);
	}

	public function		getLevel()
	{
		return ($this->_level);
	}

	public function		setView(array $view)
	{
		$this->_view = $view;

		return ($this);
	}

	public function		getView()
	{
		return ($this->_view);
	}

	public function		setFoodNeeds($foodNeeds)
	{
		$this->_foodNeeds = $foodNeeds;

		return ($this);
	}

	public function		getFoodNeeds()
	{
		return ($this->_foodNeeds);
	}

	public function		setSeekingIncantation($bool)
	{
		$this->_seekingIncantation = $bool;

		return ($this);
	}

	public function		getSeekingIncantation()
	{
		return ($this->_seekingIncantation);
	}

	public function		setSeekingFood($bool)
	{
		$this->_seekingFood = $bool;

		return ($this);
	}

	public function		getSeekingFood()
	{
		return ($this->_seekingFood);
	}

	public function		setInventory(array $inventory)
	{
		$this->_inventory = $inventory;

		return ($this);
	}

	public function		getInventory()
	{
		return ($this->_inventory);
	}

	public function		setLevelNeeds($index, array $levelNeeds)
	{
		$this->_levelNeeds[$index] = $levelNeeds;

		return ($this);
	}

	public function		getLevelNeeds()
	{
		return ($this->_levelNeeds);
	}

	public function __toString()
	{
		return ("Player from team " .$this->_team );
	}
}
