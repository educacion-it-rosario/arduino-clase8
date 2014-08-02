/*
 * Variable global para llevar cuenta del estado del interruptor
 */
var switchStatus = false;

/*
 * Esta funcion genera un numero aleatorio entre 1 y maximo
 */
function random() {
    return Math.floor((Math.random() * 1000000)+1);
}

/*
 * Funcion que via el web-service cambia el estado de la salida en funcion
 * del interruptor mostrado.
 */
function switchClick(img) {
    var data = {
        status: !switchStatus,
        random: random(),
        port: 13,
    };

    $.get("/cgi-bin/switch-set.py", data)
        .done( switchGetOnData );
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

    if (data.error!=undefined){
        window.alert(data.error);
        return;
    }

    // actualizamos el estado de nuestra variable interna
    switchStatus = data.status;

    // seleccionar del html aquel elemeno que tenga el id "switch"
    img = $("#switch");

    // sino encuentra el switch entonces no podemos hacer nada mas
    if (img.length==0) {
        // avisemos al desarrollador que algo esta mal!
        console.error("No encuentro switch en el DOM")
        return;
    }


    if (switchStatus) {
        console.log("cambiando a on.jpg")
        img.attr("src", "/imgs/on.jpg");
    }
    else {
        console.log("cambiando a off.jpg")
        img.attr("src", "/imgs/off.jpg");
    }
}

/*
 * Funcion que se va a llamar cuando el navegador ya haya terminado de
 * descargar del servidor todos los componentes y este listo para nuestra
 * aplicacion
 */
function onReady() {
    // leer el estado actual del switch asi nos sincronizamos
    $.getJSON("/cgi-bin/switch-get.py",
              {
                  'random': random(),
                  'port': 13
              }
             ).done(switchGetOnData);

    // conectar el switch al manejador de funciones
    $("#switch").bind("click", switchClick);
}


$().ready(onReady);
