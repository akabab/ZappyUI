<?php

include "classes/socket.class.php";
include "classes/player.class.php";

$opts = getopt("n:p:h:");
$team = null;
$port = null;
$adress = null;

ini_set('memory_limit', '1024M');

function	usage()
{
	print("Usage: ./client -n <team> -p <port> [-h <hostname>]\n");
	print("		-n nom d'equipe\n");
	print("		-p port\n");
	print("		-h nom de la machine (par defaut: localhost)\n");
	exit(-1);
}

if (array_key_exists("n", $opts) && array_key_exists("p", $opts))
{
	if (ctype_digit($opts['p']) && is_string($opts['n']))
	{
		$team = $opts['n'];
		$port = intval($opts['p']);
		if (array_key_exists("h", $opts))
		{
			if (is_string($opts['h']))
				$adress = $opts['h'];
			else
				usage();
		}
		else
			$adress = "localhost";
	}
	else
		usage();
}
else
	usage();

ob_implicit_flush();

$socket = new Socket($adress, $port);
$player = new Player($team, $socket, $argv[0]);
