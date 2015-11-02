function changeToText(elementName, linkName) {
    $('#' + elementName).replaceWith($('<input/>', { 'type': 'text', 'class': 'form-control text-box single-line', 'id': elementName, 'name': elementName }));
    $('#' + elementName).focus();
    $('#' + linkName).hide();
}