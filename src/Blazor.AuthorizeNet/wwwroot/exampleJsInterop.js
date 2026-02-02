// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function initCommunicator(dotnetRef) {
    if (!window.AuthorizeNetPopup) window.AuthorizeNetPopup = {};
    if (!AuthorizeNetPopup.options) AuthorizeNetPopup.options = {
        onPopupClosed: null
    };

    AuthorizeNetPopup.closePopup = function () {
        document.getElementById("divAuthorizeNetPopupScreen").style.display = "none";
        document.getElementById("divAuthorizeNetPopup").style.display = "none";
        document.getElementById("iframeAuthorizeNet").src = "empty.html";
        document.getElementById("btnOpenAuthorizeNetPopup").disabled = false;
        if (AuthorizeNetPopup.options.onPopupClosed) AuthorizeNetPopup.options.onPopupClosed();
    };


    AuthorizeNetPopup.openPopup = function (url) {
        var popup = document.getElementById("divAuthorizeNetPopup");
        var popupScreen = document.getElementById("divAuthorizeNetPopupScreen");
        var ifrm = document.getElementById("iframeAuthorizeNet");
        var form = document.forms["formAuthorizeNetPopup"];
        form.action = url;
        ifrm.style.width = "442px";
        ifrm.style.height = "578px";

        form.submit();

        popup.style.display = "";
        popupScreen.style.display = "";
        centerPopup();
    };

    AuthorizeNetPopup.onReceiveCommunication = function (querystr) {
        var params = parseQueryString(querystr);
        alert(params["action"]);
        switch (params["action"]) {
            case "successfulSave":
                AuthorizeNetPopup.closePopup();
                break;
            case "cancel":
                dotnetRef.invokeMethodAsync("HandleCancel", "User cancelled payment");
                AuthorizeNetPopup.closePopup();
                break;
            case "transactResponse":
                var response = params["response"];
                dotnetRef.invokeMethodAsync("HandleTransactionResponse", response);
                AuthorizeNetPopup.closePopup();
                break;
            case "resizeWindow":
                var w = parseInt(params["width"]);
                var h = parseInt(params["height"]);
                var ifrm = document.getElementById("iframeAuthorizeNet");
                ifrm.style.width = w.toString() + "px";
                ifrm.style.height = h.toString() + "px";
                centerPopup();
                break;
        }
    };


    function centerPopup() {
        var d = document.getElementById("divAuthorizeNetPopup");
        d.style.left = "50%";
        d.style.top = "50%";
        var left = -Math.floor(d.clientWidth / 2);
        var top = -Math.floor(d.clientHeight / 2);
        d.style.marginLeft = left.toString() + "px";
        d.style.marginTop = top.toString() + "px";
        d.style.zIndex = "2";
        if (d.offsetLeft < 16) {
            d.style.left = "16px";
            d.style.marginLeft = "0px";
        }
        if (d.offsetTop < 16) {
            d.style.top = "16px";
            d.style.marginTop = "0px";
        }
    }

    function parseQueryString(str) {
        var vars = [];
        var arr = str.split('&');
        var pair;
        for (var i = 0; i < arr.length; i++) {
            pair = arr[i].split('=');
            vars.push(pair[0]);
            vars[pair[0]] = unescape(pair[1]);
        }
        return vars;
    }
}