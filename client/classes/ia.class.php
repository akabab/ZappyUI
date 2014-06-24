<?php

include 'automaton.class.php';

Class IA extends Automaton
{
	protected function		process()
	{
		$routine = $this->getRoutines($this->getState());

		foreach ($routines as $function)
			$this->$function();
	}
}
