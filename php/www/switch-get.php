<?php
/*
        Federico Pfaffendorf
        yo@federicopfaffendorf.com.ar
        (t) @fpfaffendorf
*/

try {
    $c = new SoapClient("../ws/switch.wsdl");
    $s = $c->get ();

    if ($s === true) {
        echo ("true");
    } elseif ($s === false) {
        echo ("false");
    } else {
        echo ("null");
    }

} catch (SoapFault $e) {
    echo ($e);
}

?>