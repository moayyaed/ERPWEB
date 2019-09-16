function OpenInNewTabWinBrowser(url) {
        
    var win = window.open(url,'_blank');
    win.focus();
}

function OpenInBrowser(url) {
    location.href = url;
}

function GetPathServer() {
    //var location = window.location.origin + "/FixedERP"; /*produccion*/
    var location = window.location.origin; /*local*/

    return location;
}

$(document).ready(function () {
    $(".form").keypress(function (e) {//Para deshabilitar el uso de la tecla "Enter"
        if (e.which == 13) {
            return false;
        }
    });
});