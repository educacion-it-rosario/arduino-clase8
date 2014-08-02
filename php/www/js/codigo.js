/*
 * Variable global para llevar cuenta del estado del interruptor
 */
var switchStatus = false;


/*
 * Funcion que via el web-service cambia el estado de la salida en funcion
 * del interruptor mostrado.
 */
function switchClick(img) {
    var data = {
        status: !switchStatus,
        r: Math.random(),
    };

    $.get("/ws-client/switch-set.php", data)
        .done(
            function( data ) {
                console.debug(data);
            }
        );
}


/*
 * Esta funcion va a ser llamada cuando hayan llegados datos del web
 * service, y nosotros hayamos iniciado un GET al switch-get
 */
function switchGetOnData(data) {
    // variable que usamos para controlar el pulsador mostrado en pantalla
    var img;

    // mostrar por consola el estado de la llamada
    console.log("switchGetOnData", data);

    // seleccionar del html aquel elemeno que tenga el id "switch"
    img = $("#switch");

    // sino encuentra el switch entonces no podemos hacer nada mas
    if (img.length==0) {
        // avisemos al desarrollador que algo esta mal!
        console.error("No encuentro switch en el DOM")
        return;
    }
}

/*
 * Funcion que se va a llamar cuando el navegador ya haya terminado de
 * descargar del servidor todos los componentes y este listo para nuestra
 * aplicacion
 */
function onReady() {
    // leer el estado actual del switch asi nos sincronizamos
    $.get("/ws-client/switch-get.php").done(switchGetOnData);
}


$().ready(onReady);
