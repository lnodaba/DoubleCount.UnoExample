$(document).ready(function () {

    $("#cancel-button").click(function (e) {
        e.preventDefault();

        const urlParams = new URLSearchParams(window.location.search);
        const returnUrl = urlParams.get('returnUrl');

        window.location.href = window.location.origin + "/Account/Login?returnUrl=" + returnUrl
    })
})