$(document).ready(function () {
    $("#passwordSwitch").click(function (e) {
        e.preventDefault();

        $this = $(this);
        $passwordElement = $("#password-control");

        if ($this.hasClass("fa-unlock")) {
            $passwordElement[0].type = 'password';
            $this.removeClass("fa-unlock")
            $this.addClass("fa-lock")
        } else {
            $passwordElement[0].type = 'text';
            $this.removeClass("fa-lock")
            $this.addClass("fa-unlock")
        }
    })
})