

function ChangeOpenClose(ticketNr, open, close) {
    var selector = $("#btn_" + ticketNr);
    var selectedBtn = selector[0];
    var btnText = selectedBtn.innerText;

    selectedBtn.innerText = btnText === open.value ? close.value : open.value;
}

function CloseCollapsed(wrapper) {
    var selector = wrapper === 'ticket' ? $('.tickets_content') : $('.reply_content');
    selector.find(".show").slideToggle("medium", "swing", function () {
        selector.find(".show").attr("style", "");
        selector.find(".show").removeClass('show');
    });
}
