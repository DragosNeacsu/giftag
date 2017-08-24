$(".destination-autocomplete").each(function () {
    $(this).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: currentCulture + "/skyscanner/getdestination?keyword=" + request.term,
                success: function (data) {
                    response(JSON.parse(data));
                }
            });
        },
        appendTo: $(this).parent(),
        minLength: 3,
        focus: function (event, ui) {
            $(this).val(ui.item.PlaceName + " (" + ui.item.PlaceId.replace("-sky", "") + ")");
            return false;
        },
        select: function (event, ui) {
            $(this).val(ui.item.PlaceName + " (" + ui.item.PlaceId.replace("-sky", "") + ")");
            return false;
        }
    }).data("ui-autocomplete")._renderItem = function (ul, item) {
        return $("<li>")
            .data("ui-autocomplete-item", item)
            .append("<div class='autocomplete-item'><span class='autocomplete-image'></span> " + item.PlaceName + " (" + item.PlaceId.replace("-sky", "") + ")" + "<br><span class='countryName'>" + item.CountryName + "</span></div>")
            .appendTo(ul);
    };
});


$("#airline").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: currentCulture + "/skyscanner/GetAirlines?keyword=" + request.term,
            success: function (data) {
                response(data);
            }
        });
    },
    appendTo: $(this).parent(),
    minLength: 3,
    focus: function (event, ui) {
        $(this).val(ui.item.airlineName);
        return false;
    },
    select: function (event, ui) {
        $(this).val(ui.item.airlineName);
        $('#airlineLogo').val(ui.item.airlineCode);
        return false;
    }
}).data("ui-autocomplete")._renderItem = function (ul, item) {
    return $("<li>")
        .data("ui-autocomplete-item", item)
        .append("<div class='autocomplete-item'><img src='Content/Airlines/" + item.airlineCode + "' height='20'/> " + item.airlineName + "</div>")
        .appendTo(ul);
};

$(function () {
    $('.selectpicker').selectpicker();
});

$('.selectpicker').on('change', function () {
    var selected = $(this).find("option:selected").val();
    window.location.href = selected;
});