var switchStatus = false;

function swtichClick(img) {
    x = xmlHttp();

    if (x != false) {
        x.onreadystatechange = function ()  {
            if ((x.readyState == 4) && (x.status == 200))  {
                if (x.responseText.trim().toUpperCase() == "TRUE") {
                    if (switchStatus) {
                        img.src = "./imgs/off.jpg";
                    }
                    else {
                        img.src = "./imgs/on.jpg";
                    }
                    switchStatus = !switchStatus;
                }
            }
        }

        x.open ("GET", "./ws-client/switch-set.php?status=" + (!switchStatus) + "&r=" + Math.random(), true);
        x.send (null);

    }

}

window.onload = function () {

    x = xmlHttp();
    img = document.getElementById ("switch");

    if (x != false) {

        x.onreadystatechange = function () {
            if ((x.readyState == 4) && (x.status == 200)) {
                if (x.responseText.trim().toUpperCase() == "TRUE") {
                    img.src = "./imgs/on.jpg";
                    switchStatus = true;
                }
                else
                {
                    img.src = "./imgs/off.jpg";
                    switchStatus = false;
                }
            }
        }

        x.open ("GET", "./ws-client/switch-get.php?r=" + Math.random(), true);
        x.send (null);
    }
}
