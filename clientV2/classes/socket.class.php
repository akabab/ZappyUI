<?php

Class Socket
{
	private $_address = null;
	private $_port = null;
	private $_socket = null;

	public function		__construct($address, $port)
	{
		$this->setAddress(gethostbyname($address))
		->setPort($port)
		->setSocket(socket_create(AF_INET, SOCK_STREAM, SOL_TCP));

		if ($this->_socket === false)
			die("socket_create() failed: " .socket_strerror(socket_last_error()). "\n");

		if (socket_connect($this->_socket, $this->_address, $this->_port) === false)
			die("socket_connect() failed: " .socket_strerror(socket_last_error($this->_socket)). "\n");

		socket_set_block($this->_socket);
	}

	public function		send($data)
	{
		if (socket_send($this->_socket, $data. "\n", strlen($data) + 1, 0) === false)
			die("socket_send() failed: " .socket_strerror(socket_last_error($this->_socket)). "\n");
	}

	public function		receive()
	{
		if (($data = socket_read($this->_socket, 8192)) === false)
			die("socket_recv() failed: " .socket_strerror(socket_last_error($this->_socket)). "\n");
		else
			return ($data);
	}

	public function		close()
	{
		socket_close($this->_socket);
	}

	public function		setAddress($address)
	{
		$this->_address = $address;

		return ($this);
	}

	public function getAddress()
	{
		return ($this->_address);
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

	public function		setPort($port)
	{
		$this->_port = $port;

		return ($this);
	}

	public function		getPort()
	{
		return ($this->_port);
	}
}
