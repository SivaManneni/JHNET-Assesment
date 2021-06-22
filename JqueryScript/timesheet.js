$(document).ready(function () {
    $.noConflict(true);
    $('#btnsubmit').prop("disabled", false);
    $('#loading').hide();

    $("#starttimepicker").datepicker({
        autoclose: true,
        todayHighlight: true,
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        minDate: 0,
        onSelect: function (dateText) {
            var fromDate = $("#starttimepicker").datepicker('getDate');
            if (fromDate != null) {
                fromDate.setDate(fromDate.getDate());
                $("#endtimepicker").datepicker('setDate', fromDate);
                $("#lunchtimepicker").datepicker('setDate', fromDate);
            }
        }
    });

    $("#endtimepicker").datepicker({
        autoclose: true,
        todayHighlight: true,
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        disabled: true
    });

    $("#lunchtimepicker").datepicker({
        autoclose: true,
        todayHighlight: true,
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true,
        disabled: true
    });

    $("#starttimepicker").datepicker('setDate', new Date());
    $("#endtimepicker").datepicker('setDate', new Date());
    $("#lunchtimepicker").datepicker('setDate', new Date());
});


$('#btnsubmit').button().click(function (e) {
    e.preventDefault();
    $('#validationMessage').html('');
    var startDate = $('#starttimepicker').val();
    var selecteduser = $("#ddlUsers").val();
    var selectedproject = $("#ddlProjects").val();
    var startHour = $('#startHour').val();
    var startMinute = $('#startMinute').val();
    var endHour = $('#endHour').val();
    var endMinute = $('#endMinute').val();
    var lunchHour = $('#lunchHour').val();
    var lunchMinute = $('#lunchMinute').val();
    if (startDate == "") {
        $('#validationMessage').append('<br>Please Select Start Time.<br/>')
    }
    if (selecteduser == "" || selecteduser == 0 || selecteduser == null) {
        $('#validationMessage').append('<br>Please select User.<br/>')
    }
    if (selectedproject == "" || selectedproject == 0 || selectedproject == null) {
        $('#validationMessage').append('<br>Please select Project.<br/>')
    }
    if (startHour == "" || startMinute == "") {
        $('#validationMessage').append('<br>Please select Start Hours and Minutes.<br/>')
    }
    else if (lunchHour == "" || lunchMinute == "") {
        $('#validationMessage').append('<br>Please select Lunch Start Hours and Minutes.<br/>')
    }
    else if (endHour == "" || endMinute == "") {
        $('#validationMessage').append('<br>Please select End Hours and Minutes.<br/>')
    }
    else if (!(parseInt(startHour) < parseInt(lunchHour))) {
        $('#validationMessage').append('<br>Lunch start time must be greater than Start Hours.<br/>')
    }
    else if (!(parseInt(lunchHour) < parseInt(endHour))) {
        $('#validationMessage').append('<br>End Hours must be greater than Lunch start time.<br/>')
    }
    if ($.trim($('#validationMessage').html()) != '') {
        $('#ValidationModal').modal('show');
        return false;
    }
    $('#btnsubmit').prop("disabled", true);
    var formData = new FormData($('form')[0]);
    var urlTimeSheetGeneration = $('#UrlTimeSheetGeneration').val();
    var urlViewTimeSheet = $('#UrlViewTimeSheet').val();
    var urlErrorPage = $('#UrlErrorPage').val();
    $('#loading').show();
    $.ajax({
        type: "POST",
        url: urlTimeSheetGeneration,
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.TimesheetId != "" && response.TimesheetId != undefined) {
                $('#loading').hide();
                window.location = urlViewTimeSheet + "?timesheetId=" + response.TimesheetId;
            }
            else {
                window.location = urlErrorPage;
            }
        },
        failure: function (response) {
            window.location = urlErrorPage;
        },
        error: function (response) {
            window.location = urlErrorPage;
        }
    });
});