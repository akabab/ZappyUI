<?php

Class Socket
{
	private $_adress = null;
	private $_port = null;
	private $_socket = null;

	public function		__construct($adress, $port)
	{
		$this->_adress = gethostbyname($adress);
		$this->_port = $port;

		$this->_socket = socket_create(AF_INET, SOCK_STREAM, SOL_TCP);
		if ($this->_socket === false)
			die("socket_create() failed: " .socket_strerror(socket_last_error()). "\n");

		if (socket_connect($this->_socket, $this->_adress, $this->_port) === false)
			die("socket_connect() failed: " .socket_strerror(socket_last_error($this->_socket)). "\n");

		socket_set_block($this->_socket);
	}

	public function		send($data)
	{
		if (socket_send($this->_socket, $data. "\n", strlen($data) + 1, 0) === false)
			die("socket_send() failed: " .socket_strerror(socket_last_error($this->_socket)). "\n");
		else
			print($data. "\n");
	}

	public function		receive()
	{
		if (($data = socket_read($this->_socket, 8192)) !== false)
		{
			print("---DATA : " .$data. "---\n");
			return ($data);
		}
		else
			die("socket_recv() failed: " .socket_strerror(socket_last_error($this->_socket)). "\n");
	}

	public function		close()
	{
		socket_close($this->_socket);
	}

	public function		getSocket()
	{
		return ($this->_socket);
	}

	public function		getPort()
	{
		return ($this->_port);
	}
}
