<?php
/*
        Federico Pfaffendorf
        yo@federicopfaffendorf.com.ar
        (t) @fpfaffendorf
*/

try {
    $c = new SoapClient("../ws/switch.wsdl");
    $s = false;
    if (strtoupper (trim ($_GET ["status"])) == "TRUE") {
        $s = true;
    }
    if ($c->set ($s)) {
        echo ("true");
    } else {
        echo ("false");
    }

} catch (SoapFault $e) {
  echo ($e);
}

?>