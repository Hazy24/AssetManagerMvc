$(document).ready(function () {

    var selects = $("select");

    $("select").each(function (index, object) {
        selectwithonlyemptyoption = object.length == 1 && $(this).val() == "";
        // alert($(this).val());
        if (object.length == 0) {
            var idVal = $(this).attr('id');
            // alert(idVal);
            var start = idVal.charAt(0);
            var rest = idVal.substr(1, idVal.length - 1);
            var link = start.toLowerCase() + rest + "Link";
            // alert(link);
            changeToText(idVal, link)           
        }
        $(':input:enabled:visible:first').focus();
    })

    //for (var i = 0; i < selects.length; i++) {
    //    var selectwithonlyemptyoption = selects[i].length == 1 && $(selects[i]).val() == "";
    //    if (selects[i].length == 0 || selectwithonlyemptyoption) {
    //        var idVal = selects[i].id.valueOf();
    //        alert(idVal);
    //        var start = idVal.charAt(0);
    //        var rest = idVal.substr(1, idVal.length - 1);
    //        var link = start.toLowerCase() + rest + "Link";
    //        alert(link);
    //        changeToText(idVal, link)            
    //    }
    //    $(':input:enabled:visible:first').focus();
    //}
});