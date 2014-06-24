<?php

class Automaton
{
	const LURKING = 10;
	const FOOD = 11;
	const INCANTATION = 12;

	protected $_initialState = self::LURKING;
	protected $_currentState;
	protected $_previousState;
	protected $_routines;

	public function		__construct()
	{
		$this->setPreviousState(null);
		$this->setCurrentState($this->getInitialState());
		$this->setRoutines(null, array(
			self::LURKING => array(
			),
			self::FOOD => array(
			),
			self::INCANTATION => array(
			)));
	}

	public function		setCurrentState($state)
	{
		$this->setPreviousState($this->getCurrentState());
		$this->_currentState = $state;

		return ($this);
	}

	public function		getCurrentState()
	{
		return ($this->_currentState);
	}

	public function		setPreviousState($state)
	{
		$this->_previousState = $state;

		return ($this);
	}

	public function		getPreviousState()
	{
		return ($this->_previousState);
	}

	public function		setRoutines($index, array $routine)
	{
		if ($index !== null)
			$this->_routines[$index] = $routine;
		else
			$this->_routines = $routine;

		return ($this);
	}

	public function getRoutines($index)
	{
		if ($index !== null)
			return ($this->_routines[$index]);
		else
			return ($this->_routines);
	}
}
