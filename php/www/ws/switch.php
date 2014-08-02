<?php
/*
        Federico Pfaffendorf
        yo@federicopfaffendorf.com.ar
        (t) @fpfaffendorf
*/

if (!extension_loaded("soap"))
    dl("php_soap.dll");

ini_set ("soap.wsdl_cache_enabled", "0");

$server = new SoapServer ("./switch.wsdl");

/*
  PRE: Recibe en la variable $status un booleano true o false
  indicando el estado del switch.
  POST: Guarda en un archivo local el valor. Retorna true
  o false dependiendo el resultado de la operacion
  EXCEPTION: -
*/

function set ($status){
  if ($status)
      $status = "true";
  else
      $status = "false";

  return file_put_contents ("./switch.status", $status, LOCK_EX);
}

/*
  PRE: -
  POST: Regresa el estado actual del switch, pudiendo ser
  true, false o null en el caso de que el estado no se encuentre
  inicializado.
  EXCEPTION: -
*/

function get ()
{
    $status = file_get_contents ("./switch.status");

    if ($status === "false")
        return (false);
    elseif ($status === "true")
        return (true);
    else
        return (null);

}

$server->AddFunction("set");
$server->AddFunction("get");
$server->handle();

?>
