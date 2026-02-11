export function initCommunicator(dotnetRef) {
    if (!window.AuthorizeNetPopup) window.AuthorizeNetPopup = {};
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

    AuthorizeNetPopup.onReceiveCommunication = async function (querystr) {
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
                console.log(response);
                const transactionDetail = response ? JSON.parse(response) : null;
                await dotnetRef.invokeMethodAsync("HandleTransactionResponse", transactionDetail);
                AuthorizeNetPopup.closePopup();
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