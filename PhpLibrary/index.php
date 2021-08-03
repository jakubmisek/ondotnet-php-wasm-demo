<a href="#" onclick="jsf.call('test')" >Click meee</a>
<?php

function test() {
	$GLOBALS['a'] = date("h:i:sa");
}

echo "Hello from PHP, it's $a";