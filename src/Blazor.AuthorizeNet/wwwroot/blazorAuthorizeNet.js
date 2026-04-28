export function initCommunicator(dotnetRef, mode) {
    if (!window.AuthorizeNetPopup) window.AuthorizeNetPopup = {};
    if (!window.AuthorizeNetIFrame) window.AuthorizeNetIFrame = {};
    if (!AuthorizeNetPopup.options) AuthorizeNetPopup.options = {
        onPopupClosed: null
    };

    AuthorizeNetPopup.closePopup = function () {
        document.getElementById("divAuthorizeNetPopupScreen").style.display = "none";
        document.getElementById("divAuthorizeNetPopup").style.display = "none";
        document.getElementById("iframeAuthorizeNet").src = "empty.html";
        if (AuthorizeNetPopup.options.onPopupClosed) AuthorizeNetPopup.options.onPopupClosed();
    };

    AuthorizeNetPopup.openPopup = function (url) {
        const popup = document.getElementById("divAuthorizeNetPopup");
        const popupScreen = document.getElementById("divAuthorizeNetPopupScreen");
        const ifrm = document.getElementById("iframeAuthorizeNet");
        const form = document.forms["formAuthorizeNetPopup"];
        form.action = url;
        ifrm.style.width = "442px";
        ifrm.style.height = "578px";

        form.submit();

        popup.style.display = "";
        popupScreen.style.display = "";
    };


    AuthorizeNetIFrame.openIFrame = function () {
        const ifrm = document.getElementById("add_payment");
        ifrm.style.display = "";

        const form = document.forms["send_token"];
        form.submit();

        const el = document.getElementById('add_payment');
        const y = el.getBoundingClientRect().top + window.pageYOffset - 50;

        window.scrollTo({ top: y, behavior: 'auto' });
    };

    AuthorizeNetPopup.onReceiveCommunication = async function (querystr) {
        if (mode !== "IFrameLightbox") {
            return;
        }

        const params = parseQueryString(querystr);
        switch (params["action"]) {
            case "successfulSave":
                AuthorizeNetPopup.closePopup();
                break;
            case "cancel":
                await dotnetRef.invokeMethodAsync("HandleCancel", "User cancelled payment");
                AuthorizeNetPopup.closePopup();
                break;
            case "transactResponse":
                const response = params["response"];
                await dotnetRef.invokeMethodAsync("HandleTransactionResponse", response);
                AuthorizeNetPopup.closePopup();
                break;
            case "resizeWindow":
                var w = parseInt(params["width"]);
                var h = parseInt(params["height"]);
                var ifrm = document.getElementById("iframeAuthorizeNet");
                ifrm.style.width = w.toString() + "px";
                ifrm.style.height = h.toString() + "px";
                break;
        }
    };

    AuthorizeNetIFrame.onFrameReceiveCommunication = async function (querystr) {
        if (mode !== "EmbeddedIFrame") {
            return;
        }

        var params = parseQueryString(querystr);
        switch (params["action"]) {
            case "successfulSave":
                var ifrm = document.getElementById("add_payment");
                ifrm.style.display = 'none';
                break;
            case "cancel":
                await dotnetRef.invokeMethodAsync("HandleCancel", "User cancelled payment");
                var ifrm = document.getElementById("add_payment");
                ifrm.style.display = 'none';
                break;
            case "resizeWindow":
                var w = parseInt(params["width"]);
                var h = parseInt(params["height"]);
                var ifrm = document.getElementById("add_payment");
                ifrm.style.width = w.toString() + "px";
                ifrm.style.height = h.toString() + "px";
                break;
            case "transactResponse":
                var ifrm = document.getElementById("add_payment");
                ifrm.style.display = 'none';
                const response = params["response"];
                await dotnetRef.invokeMethodAsync("HandleTransactionResponse", response);
                break;
        }
    };

    function parseQueryString(str) {
        const vars = [];
        const arr = str.split('&');
        let pair;
        for (let i = 0; i < arr.length; i++) {
            pair = arr[i].split('=');
            vars.push(pair[0]);
            vars[pair[0]] = unescape(pair[1]);
        }
        return vars;
    }
}