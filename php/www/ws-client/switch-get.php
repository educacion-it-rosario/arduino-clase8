<?php
/*
        Federico Pfaffendorf
        yo@federicopfaffendorf.com.ar
        (t) @fpfaffendorf
*/


if (!extension_loaded("soap"))
    dl("php_soap.dll");

ini_set ("soap.wsdl_cache_enabled", "0");

try {
    $host = $_SERVER["HTTP_HOST"];
    $url = "http://" . $host . "/ws/switch.wsdl";
    $c = new SoapClient($url, array('trace'=>1));
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
