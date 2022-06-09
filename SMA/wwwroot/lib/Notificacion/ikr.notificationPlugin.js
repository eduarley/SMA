(function ($) {
    // ****** SE APLICA EL DISENO CSS******

    $.fn.designSetup = function (options) {

        var defaultSettings = $.extend({
            BeforeSeenColor: "#2E467C",
            AfterSeenColor: "#ccc"
        }, options);

        $(".ikrNoti_Button").css({
            "background": defaultSettings.BeforeSeenColor
        });
        var parentId = $(this).attr("id");

        if ($.trim(parentId) != "" && parentId.length > 0) {
            $("#" + parentId).append("<div class='ikrNoti_Counter'></div>" +
                "<div class='ikrNoti_Button'><center><i class='fa-solid fa-bell' style='color: white;'></i></center></div>" +
                "<div class='ikrNotifications'>" +
                "<h3>Notificaciones (<span class='notiCounterOnHead'>0</span>)</h3>" +
                "<div class='ikrNotificationItems'>" +
                "</div>" +
                "<div class='ikrSeeAll'><a href='#'>Ver Todas</a></div>" +
                "</div>");

            $('#' + parentId + ' .ikrNoti_Counter')
                .css({ opacity: 0 })
                .text(0)
                .css({ top: '-10px' })
                .animate({ top: '-2px', opacity: 1 }, 500);

            $('#' + parentId + ' .ikrNoti_Button').click(function () {
                $('#' + parentId + ' .ikrNotifications').fadeToggle('fast', 'linear', function () {
                    if ($('#' + parentId + ' .ikrNotifications').is(':hidden')) {
                        $('#' + parentId + ' .ikrNoti_Button').css('background-color', defaultSettings.AfterSeenColor);
                    }
                    else $('#' + parentId + ' .ikrNoti_Button').css('background-color', defaultSettings.BeforeSeenColor);
                });
                $('#' + parentId + ' .ikrNoti_Counter').fadeOut('slow');
                return false;
            });
            $(document).click(function () {
                $('#' + parentId + ' .ikrNotifications').hide();
                if ($('#' + parentId + ' .ikrNoti_Counter').is(':hidden')) {
                    $('#' + parentId + ' .ikrNoti_Button').css('background-color', defaultSettings.AfterSeenColor);
                }
            });
            $('#' + parentId + ' .ikrNotifications').click(function () {
                return false;
            });

            $("#" + parentId).css({
                position: "relative"
            });
        }
    };




    // ****** Cargar contenido ******

    $.fn.cargarNotificaciones = function (options) {
        var defaultSettings = $.extend({
            NotificationList: [],
            ControllerName: "",
            ActionName: ""
        }, options);

        var parentId = $(this).attr("id");
        if ($.trim(parentId) != "" && parentId.length > 0) {
            $("#" + parentId + " .ikrNotifications .ikrSeeAll").click(function () {
                window.open('./../../' + defaultSettings.ControllerName + '/' + defaultSettings.ActionName + '', '_blank');
            });


            //----Contador de notificaciones sin leer
            var totalUnReadNoti = defaultSettings.NotificationList.filter(x => x.visto == false).length;
            $('#' + parentId + ' .ikrNoti_Counter').text(totalUnReadNoti);
            $('#' + parentId + ' .notiCounterOnHead').text(totalUnReadNoti);
            //---------

            if (defaultSettings.NotificationList.length > 0) {
                $.map(defaultSettings.NotificationList, function (item) {
                    
                    var className = item.visto ? "" : " ikrSingleNotiDivUnReadColor";
                    //var sNotiFromPropName = $.trim(defaultSettings.NotiFromPropName) == "" ? "" : item[ikrLowerFirstLetter(defaultSettings.NotiFromPropName)];
                    $("#" + parentId + " .ikrNotificationItems").append("<div class='ikrSingleNotiDiv" + className + "' notiId=" + item.idNotificacion + ">" +
                        //"<h4 class='ikrNotiFromPropName'>" + item.idEmisor + "</h4>" +
                        "<h5 class='ikrNotificationTitle'>" + item.titulo + "</h5>" +
                        "<div class='ikrNotificationBody'>" + item.descripcion + "</div>" +
                        "<div class='ikrNofiCreatedDate'>" + item.fecha + "</div>" +
                        "</div>");


                    $("#" + parentId + " .ikrNotificationItems .ikrSingleNotiDiv[notiId=" + item.idNotificacion + "]").click(function () {
                        var url = "./../../Notificacion/Details?id=" + item.idNotificacion;
                        debugger;
                        window.location.href = url;
                    });
                });
            }
        }

    };

}(jQuery));


